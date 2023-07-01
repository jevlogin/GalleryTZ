namespace WORLDGAMEDEVELOPMENT
{
    public sealed class UIInitialization
    {
        #region Fields

        private readonly UIModel _uIModel;

        #endregion


        #region Properties

        public UIModel UIModel => _uIModel;

        #endregion


        #region ClassLifeCycles

        public UIInitialization(UIModel uIModel)
        {
            _uIModel = uIModel;
        }

        #endregion
    }
}