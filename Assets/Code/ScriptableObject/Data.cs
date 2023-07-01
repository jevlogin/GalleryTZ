using System.IO;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    [CreateAssetMenu(fileName = "Data", menuName = "Data/Data", order = 51)]
    public sealed class Data : ScriptableObject
    {
        #region Fields

        [SerializeField] private string _uiDataPath;

        private UIData _uiData;

        #endregion


        #region UIData

        public UIData UIData

        {
            get
            {
                if (_uiData == null)
                {
                    _uiData = Resources.Load<UIData>(Path.Combine(ManagerPath.DATA, ManagerPath.UI, nameof(UIData)));
                }
                if (_uiData == null)
                {
                    _uiData = Resources.Load<UIData>(_uiDataPath);
                }
                return _uiData;
            }
        }

        #endregion
    }
}