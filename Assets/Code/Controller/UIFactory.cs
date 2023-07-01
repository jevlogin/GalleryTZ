using System.Collections.Generic;

namespace WORLDGAMEDEVELOPMENT
{
    public sealed class UIFactory
    {
        #region Fields

        private readonly UIData _uIData;
        private UIModel _uiModel;

        #endregion

        
        #region ClassLifeCycles
        
        public UIFactory(UIData uIData)
        {
            _uIData = uIData;
        }

        #endregion

        
        #region Methods
        
        internal UIModel GetOrCreateUIModel()
        {
            if (_uiModel == null)
            {
                var uiStruct = _uIData.UIStruct;
                var uiComponents = new UIComponents() { ListPanelViews = new List<IPanelUIView>() };
                var uiSettings = new UISettings
                {
                    UrlBase = uiStruct.UrlBase,
                    ImagePrefix = uiStruct.ImagePrefix,
                    CountImages = uiStruct.CountImages,
                    ImagePrefab = uiStruct.ImagePrefab,
                };

                _uiModel = new UIModel(uiStruct, uiComponents, uiSettings);
            }

            return _uiModel;
        } 

        #endregion
    }
}