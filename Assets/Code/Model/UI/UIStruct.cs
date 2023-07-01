using System;
using UnityEngine;
using UnityEngine.UI;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    public struct UIStruct
    {
        #region Fields

        [SerializeField] private GameObject _canvasPrefab;
        [SerializeField] private UIPanelView _uiPanelMenuPrefab;
        [SerializeField] private UIPanelView _uiPanelLoadingPrefab;
        [SerializeField] private UIPanelView _uiPanelGalleryPrefab;
        [SerializeField] private UIPanelView _uiPanelImageViewPrefab;
        [SerializeField] private RawImage _imagePrefab;

        [SerializeField] private string _urlBase;
        [SerializeField] private string _imagePrefix;
        [SerializeField] private int _countImages;

        #endregion


        #region Properties

        public readonly GameObject CanvasPrefab => _canvasPrefab;
        public readonly UIPanelView UIPanelMenuPrefab => _uiPanelMenuPrefab;
        public readonly UIPanelView UIPanelLoadingPrefab => _uiPanelLoadingPrefab;
        public readonly UIPanelView UIPanelGalleryPrefab => _uiPanelGalleryPrefab;
        public readonly UIPanelView UIPanelImageViewPrefab => _uiPanelImageViewPrefab;
        public readonly RawImage ImagePrefab => _imagePrefab;

        public readonly string UrlBase => _urlBase;
        public readonly string ImagePrefix => _imagePrefix;
        public readonly int CountImages => _countImages;

        #endregion
    }
}