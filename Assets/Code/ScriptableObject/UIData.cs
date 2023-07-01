using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [CreateAssetMenu(fileName = "UIData", menuName = "Data/UIData", order = 51)]
    public sealed class UIData : ScriptableObject
    {
        #region Fields

        [SerializeField] private UIStruct _uiStruct;
        [SerializeField] private UIComponents _uiComponents;
        [SerializeField] private UISettings _uISettings;

        #endregion


        #region Properties

        public UIStruct UIStruct => _uiStruct;
        public UIComponents UIComponents => _uiComponents;
        public UISettings UISettings => _uISettings;

        #endregion
    }
}