using UnityEngine;
using UnityEngine.UI;


namespace WORLDGAMEDEVELOPMENT
{
    public sealed class UIPanelLoadingView : UIPanelView
    {
        [SerializeField] Slider _sliderBarLoading;

        public Slider SliderBarLoading => _sliderBarLoading;
    }
}