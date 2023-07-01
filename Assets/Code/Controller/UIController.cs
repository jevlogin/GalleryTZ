using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;


namespace WORLDGAMEDEVELOPMENT
{
    public sealed class UIController : ICleanup, IExecute, ILateExecute
    {
        #region Fields

        public event Action<int> LoadNextImageBatch;

        private UIInitialization _uiInitialization;
        private UIModel _model;
        private SCeneController _sceneController;
        private DeviceOrientationType _currentDeviceOrientation;

        private int _currentIndexScene;
        private int _lastVisibleIndexElements;
        private int _endIndex;
        private int _startIndex;
        private bool _isLoading;
        private bool _isAllLoading;
        private int _transmittedImageIndex;

        #endregion


        #region ClassLifeCicles

        public UIController(UIInitialization uiInitialization, SCeneController sceneController)
        {
            _uiInitialization = uiInitialization;
            _sceneController = sceneController;
            _model = _uiInitialization.UIModel;
            _sceneController.CurrentSceneIndex.Subscribe(OnSceneIndexChanged);
        }

        #endregion


        #region Methods

        private void OnSceneIndexChanged(int sceneIndex)
        {
            _currentIndexScene = _sceneController.CurrentSceneIndex.Value;
            SwitchPanel(_currentIndexScene);
        }

        private void SwitchPanel(int currentIndexScene)
        {
            switch (currentIndexScene)
            {
                case 0:
                    #region Canvas

                    if (_model.UIComponents.CanvasView == null)
                    {
                        CreateCanvas();
                    }
                    else
                    {
                        if (!_model.UIComponents.CanvasView.gameObject.activeSelf)
                        {
                            _model.UIComponents.CanvasView.gameObject.SetActive(true);
                        }
                    }

                    #endregion


                    #region PanelMenu

                    if (_model.UIComponents.PanelMenuUIView == null)
                    {
                        CreatePanelMenu();
                    }
                    else
                    {
                        if (!_model.UIComponents.PanelMenuUIView.isActiveAndEnabled)
                        {
                            _model.UIComponents.PanelMenuUIView.gameObject.SetActive(true);
                        }
                    }

                    #endregion


                    #region LoadingPanel

                    if (_model.UIComponents.PanelLoadingUIView == null)
                    {
                        CreateLoadingPanel();
                    }

                    #endregion

                    break;

                case 1:
                    _model.UIComponents.PanelMenuUIView.gameObject.SetActive(false);
                    _model.UIComponents.PanelLoadingUIView.gameObject.SetActive(false);
                    _model.UIComponents.PanelImageUIView?.gameObject.SetActive(false);

                    #region CreateGalleryPanel

                    if (_model.UIComponents.PanelGalleryUIView == null)
                    {
                        CreateGalleryPanel();
                    }
                    else
                    {
                        _model.UIComponents.PanelGalleryUIView.gameObject.SetActive(true);
                    }

                    #endregion

                    break;

                case 2:
                    if (_model.UIComponents.PanelImageUIView == null)
                    {
                        CreatePanelImageUIView();
                        _model.UIComponents.PanelLoadingUIView.transform.SetSiblingIndex(_model.UIComponents.CanvasView.transform.childCount);
                    }
                    else
                    {
                        _model.UIComponents.PanelImageUIView.Image.texture
                            = _model.UIComponents.PanelGalleryUIView.Images[_transmittedImageIndex].texture;

                        _model.UIComponents.PanelImageUIView.gameObject.SetActive(true);
                    }

                    _model.UIComponents.PanelGalleryUIView.gameObject.SetActive(false);
                    _model.UIComponents.PanelLoadingUIView.gameObject.SetActive(false);

                    break;
            }
        }

        private void SwitchPanelIndex(int currentIndexScene)
        {
            switch (currentIndexScene)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    foreach (var ipanel in _model.UIComponents.ListPanelViews)
                    {
                        if (ipanel is UIPanelImageView panelImageView)
                        {
                            panelImageView.enabled = true;
                        }
                        if (ipanel is UIPanelLoadingView panelLoadingView)
                        {
                            panelLoadingView.enabled = true;
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private async void CreateGalleryPanel()
        {
            var galleryViewPanel = Object.Instantiate(_model.UIStruct.UIPanelGalleryPrefab,
                                        _model.UIComponents.CanvasView.transform).GetComponent<UIPanelGalleryView>();
            galleryViewPanel.name = _model.UIStruct.UIPanelGalleryPrefab.name;
            _model.UIComponents.PanelGalleryUIView = galleryViewPanel;
            _model.UIComponents.PanelGalleryUIView.UrlBase = _model.UISettings.UrlBase;
            _model.UIComponents.PanelGalleryUIView.ImagePrefix = _model.UISettings.ImagePrefix;
            _model.UIComponents.PanelGalleryUIView.CountImages = _model.UISettings.CountImages;
            _model.UIComponents.PanelGalleryUIView.Images = new RawImage[_model.UIComponents.PanelGalleryUIView.CountImages];

            _model.UIComponents.PanelGalleryUIView.ScrollRect.onValueChanged.AddListener(DebugScroll);

            _startIndex = 0;
            _endIndex = 0;
            _isAllLoading = false;

            LoadNextImageBatch += async (startIndex) =>
            {
                await LoadNextImageBatchAsync(startIndex);
            };

            await LoadNextImageBatchAsync(_startIndex);
        }

        private async Task LoadNextImageBatchAsync(int startIndex)
        {
            _isLoading = true;

            _startIndex = startIndex + _endIndex;

            _endIndex = Mathf.Max(0, _startIndex + 10);
            if (_endIndex > _model.UIComponents.PanelGalleryUIView.CountImages)
            {
                _endIndex = _model.UIComponents.PanelGalleryUIView.CountImages;
                _isAllLoading = true;
            }

            for (int i = _startIndex + 1; i <= _endIndex; i++)
            {
                var imageUrl = _model.UIComponents.PanelGalleryUIView.UrlBase + i
                                + _model.UIComponents.PanelGalleryUIView.ImagePrefix;

                var currentIndex = i - 1;
                CreateRawImageInGameObjects(currentIndex);
                await GetImage(imageUrl, currentIndex);
            }
            _isLoading = false;
        }

        private async void DebugScroll(Vector2 position)
        {
            if (!_isLoading && !_isAllLoading)
            {
                var visibleRect = _model.UIComponents.PanelGalleryUIView.ScrollRect.content.rect.height * position.y;

                var preSpacing = (_model.UIComponents.PanelGalleryUIView.Top + _model.UIComponents.PanelGalleryUIView.Bottom)
                    / _model.UIComponents.PanelGalleryUIView.CountImages / _model.UIComponents.PanelGalleryUIView.ConstraintCount
                    + _model.UIComponents.PanelGalleryUIView.SpassingY;

                _lastVisibleIndexElements = (int)(visibleRect / (_model.UIComponents.PanelGalleryUIView.ImageHeight + preSpacing));

                if (_lastVisibleIndexElements <= 0)
                {
                    await LoadNextImageBatchAsync(_lastVisibleIndexElements);
                }
            }
        }


        private async Task GetImage(string url, int index)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
            var requestOperation = www.SendWebRequest();

            while (!requestOperation.isDone)
            {
                await Task.Yield();
            }

            if (www.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(www);
                _model.UIComponents.PanelGalleryUIView.Images[index].texture = texture;
            }
            else
            {
                Debug.LogError("Failed to load image: " + www.error);
            }
        }

        private void CreateRawImageInGameObjects(int index)
        {
            //TODO надо будет сделать постоянные ссылки в изображениях.
            //TODO ImagePrefab заменить на ImageView избавиться от лишних проверок TryGetComponent
            var rawImageGO = Object.Instantiate(_model.UISettings.ImagePrefab,
                                _model.UIComponents.PanelGalleryUIView.ScrollRect.content);

            if (rawImageGO.TryGetComponent<ImageView>(out var rawImageView))
            {
                rawImageView.ImageIndex = index;
                rawImageView.OnClick += RaImageView_OnClick;
            }

            _model.UIComponents.PanelGalleryUIView.Images[index] = rawImageGO;
        }

        private async void RaImageView_OnClick(int imageIndex)
        {
            _transmittedImageIndex = imageIndex;
            await LoadNextLevel();
            _model.UIComponents.PanelImageUIView.Image.texture
                = _model.UIComponents.PanelGalleryUIView.Images[_transmittedImageIndex].texture;
        }

        private void CreatePanelImageUIView()
        {
            if (_model.UIComponents.PanelImageUIView == null)
            {
                var panelImageView = Object.Instantiate(_model.UIStruct.UIPanelImageViewPrefab,
                    _model.UIComponents.CanvasView.transform).GetComponent<UIPanelImageView>();
                panelImageView.name = _model.UIStruct.UIPanelImageViewPrefab.name;
                _model.UIComponents.PanelImageUIView = panelImageView;

                _model.UIComponents.PanelImageUIView.ButtonBackScene.onClick.AddListener(LoadBackLevel);
            }
        }

        private void CreateLoadingPanel()
        {
            UIPanelLoadingView panelLoading = Object.Instantiate(_model.UIStruct.UIPanelLoadingPrefab,
                                                        _model.UIComponents.CanvasView.transform)
                                                            .GetComponent<UIPanelLoadingView>();
            panelLoading.name = _model.UIStruct.UIPanelLoadingPrefab.name;
            _model.UIComponents.PanelLoadingUIView = panelLoading;

            _model.UIComponents.ListPanelViews.Add(panelLoading);
        }

        private void CreatePanelMenu()
        {
            var panelMenu = Object.Instantiate(_model.UIStruct.UIPanelMenuPrefab,
                                                    _model.UIComponents.CanvasView.transform)
                                                        .GetComponent<UIPanelMenuView>();
            panelMenu.name = _model.UIStruct.UIPanelMenuPrefab.name;
            _model.UIComponents.PanelMenuUIView = panelMenu;

            _model.UIComponents.ListPanelViews.Add(panelMenu);

            _model.UIComponents.PanelMenuUIView.StartGallery.onClick.AddListener(async () => await LoadNextLevel());
        }

        private void CreateCanvas()
        {
            var canvasSpawn = Object.Instantiate(_model.UIStruct.CanvasPrefab).GetComponent<CanvasView>();
            canvasSpawn.name = _model.UIStruct.CanvasPrefab.name;
            _model.UIComponents.CanvasView = canvasSpawn;
        }

        private async Task LoadNextLevel()
        {
            await LoadLevel(LevelType.Next);
        }

        private async void LoadBackLevel()
        {
            await LoadLevel(LevelType.Back);
        }

        private async Task LoadLevel(LevelType levelType)
        {
            _model.UIComponents.PanelLoadingUIView.gameObject.SetActive(true);
            _model.UIComponents.PanelGalleryUIView?.gameObject.SetActive(false);

            switch (levelType)
            {
                case LevelType.Back:
                    await LoadSceneAsync(-1);
                    break;
                case LevelType.Next:
                    await LoadSceneAsync(1);
                    break;
            }
        }

        private async Task LoadSceneAsync(int stepLevel)
        {
            var asyncLoad = SceneManager.LoadSceneAsync(_currentIndexScene + stepLevel);

            asyncLoad.allowSceneActivation = false;

            while (!asyncLoad.isDone)
            {
                _model.UIComponents.PanelLoadingUIView.SliderBarLoading.value = asyncLoad.progress;

                if (asyncLoad.progress >= 0.9f && !asyncLoad.allowSceneActivation)
                {
                    await Task.Delay(Random.Range(2000, 3000));
                    asyncLoad.allowSceneActivation = true;
                    _sceneController.CurrentSceneIndex.Value += stepLevel;
                }
                await Task.Delay(400);
            }
        }

        #endregion


        #region ICleanup

        public void Cleanup()
        {
            LoadNextImageBatch = null;
            _model.UIComponents.PanelGalleryUIView?.ScrollRect.onValueChanged.RemoveListener(DebugScroll);
            _model.UIComponents.PanelMenuUIView?.StartGallery.onClick.RemoveListener(async () => await LoadNextLevel());
            _model.UIComponents.PanelImageUIView?.ButtonBackScene.onClick.RemoveListener(LoadBackLevel);
        }

        #endregion


        #region IExecute

        public void Execute(float deltaTime)
        {
            //Debug.Log($"_currentDeviceOrientation = {_currentDeviceOrientation}");
            if (Input.deviceOrientation == DeviceOrientation.Portrait ||
                Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown)
            {
                _currentDeviceOrientation = DeviceOrientationType.Portrait;
            }
            else if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft ||
                     Input.deviceOrientation == DeviceOrientation.LandscapeRight)
            {
                _currentDeviceOrientation = DeviceOrientationType.Landscape;
            }
        }

        #endregion


        #region ILateExecute

        public void LateExecute(float fixedDeltaTime)
        {
            if (_currentIndexScene == 2)
            {
                if (_currentDeviceOrientation == DeviceOrientationType.Portrait)
                {
                    Screen.orientation = ScreenOrientation.Portrait;
                }
                else if (_currentDeviceOrientation == DeviceOrientationType.Landscape)
                {
                    Screen.orientation = ScreenOrientation.LandscapeLeft;
                }
            }
        }

        #endregion
    }
}