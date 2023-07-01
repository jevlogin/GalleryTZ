using UnityEngine;
using UnityEngine.UI;


namespace WORLDGAMEDEVELOPMENT
{
    public sealed class UIPanelMenuView : UIPanelView
    {
        [SerializeField] private Button _startGallery;

        public Button StartGallery => _startGallery;
    }
}