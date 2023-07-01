using UnityEngine;
using UniRx;
using UnityEngine.SceneManagement;


namespace WORLDGAMEDEVELOPMENT
{
    public sealed class SCeneController : MonoBehaviour
    {
        #region Fields

        private ReactiveProperty<int> _currentSceneIndex = new ReactiveProperty<int>(0);

        #endregion


        #region Properties

        public IReactiveProperty<int> CurrentSceneIndex => _currentSceneIndex;

        #endregion


        #region UnityMethods

        private void Awake()
        {
            _currentSceneIndex.Value = SceneManager.GetActiveScene().buildIndex;
        }

        #endregion
    }
}