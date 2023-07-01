namespace WORLDGAMEDEVELOPMENT
{
    public sealed class UIModel
    {
        #region Fields

        public UIStruct UIStruct;
        public UIComponents UIComponents;
        public UISettings UISettings;

        #endregion


        #region ClassLifeCycles

        public UIModel(UIStruct uiStruct, UIComponents uiComponents, UISettings uiSettings)
        {
            UIStruct = uiStruct;
            UIComponents = uiComponents;
            UISettings = uiSettings;
        } 

        #endregion
    }
}