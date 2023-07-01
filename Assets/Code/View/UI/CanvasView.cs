using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    public sealed class CanvasView : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}