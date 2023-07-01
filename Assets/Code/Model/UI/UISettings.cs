using UnityEngine.UI;


namespace WORLDGAMEDEVELOPMENT
{
    [System.Serializable]
    public sealed class UISettings
    {
        #region Fields

        public RawImage ImagePrefab { get; set; }

        #endregion


        #region Properties

        public string UrlBase { get; set; }
        public string ImagePrefix { get; set; }
        public int CountImages { get; set; }

        #endregion
    }
}
