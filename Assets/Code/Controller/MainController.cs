using System.IO;
using Unity.VisualScripting;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    public sealed class MainController : MonoBehaviour
    {
        #region Fields

        [SerializeField] private string _dataPath;

        private Data _data;
        private Camera _camera;
        private Controllers _controllers;
        private SCeneController _sceneController;

        #endregion


        #region UnityMethods

        private void Awake()
        {
            #region Data

            _data = Resources.Load<Data>(Path.Combine(ManagerPath.DATA, ManagerPath.DATA));
            if (_data == null) _data = Resources.Load<Data>(_dataPath);

            #endregion

            _controllers = new Controllers();

            _sceneController = gameObject.GetOrAddComponent<SCeneController>();

            #region EventSystemInitialize

            var eventSystemController = new EventSystemController();
            _controllers.Add(eventSystemController);

            #endregion


            #region Camera

            if (_camera == null)
                _camera = Camera.main;

            #endregion


            #region UIController

            var uiFactory = new UIFactory(_data.UIData);
            var uiInitialization = new UIInitialization(uiFactory.GetOrCreateUIModel());
            var uiController = new UIController(uiInitialization, _sceneController);
            _controllers.Add(uiController);

            #endregion

            _controllers.Awake();
        }
        

        private void Start()
        {
            _controllers.Initialize();

            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            _controllers.Execute(Time.deltaTime);
        }

        private void LateUpdate()
        {
            _controllers.LateExecute(Time.deltaTime);
        }

        private void OnDestroy()
        {
            _controllers.Cleanup();
        }

        #endregion
    }
}