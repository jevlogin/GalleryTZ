using UnityEngine.EventSystems;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal class EventSystemController : IAwake
    {
        #region Fields

        private EventSystem _eventSystem;

        #endregion


        #region Properties

        public EventSystem EventSystem { get => _eventSystem; private set => _eventSystem = value; }

        #endregion


        #region IAwake

        public void Awake()
        {
            EventSystem = Object.FindObjectOfType<EventSystem>();

            if (EventSystem == null)
            {
                GameObject eventSystemObject = new GameObject("EventSystem");
                EventSystem = eventSystemObject.AddComponent<EventSystem>();
                EventSystem.gameObject.AddComponent<StandaloneInputModule>();
            }
            Object.DontDestroyOnLoad(EventSystem.gameObject);
        }

        #endregion
    }
}