using System;
using UnityEngine;
using UnityEngine.EventSystems;


namespace WORLDGAMEDEVELOPMENT
{
    public sealed class ImageView : MonoBehaviour, IPointerClickHandler
    {
        public int ImageIndex;
        public event Action<int> OnClick;

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke(ImageIndex);
        }
    }
}