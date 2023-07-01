using System.Collections.Generic;


namespace WORLDGAMEDEVELOPMENT
{
    public sealed class Controllers : IAwake, IInitialization, IExecute, IFixedExecute, ILateExecute, ICleanup
    {

        #region Fields

        private readonly List<IAwake> _awakeControllers;
        private readonly List<IInitialization> _initializeControllers;
        private readonly List<IExecute> _executeControllers;
        private readonly List<IFixedExecute> _fixedControllers;
        private readonly List<ILateExecute> _lateControllers;
        private readonly List<ICleanup> _cleanupControllers;

        #endregion


        #region ClassLifeCycles

        public Controllers()
        {
            _awakeControllers = new List<IAwake>();
            _initializeControllers = new List<IInitialization>();
            _executeControllers = new List<IExecute>();
            _fixedControllers = new List<IFixedExecute>();
            _lateControllers = new List<ILateExecute>();
            _cleanupControllers = new List<ICleanup>();
        }

        #endregion


        #region Methods

        public Controllers Add(IController controller)
        {
            if (controller is IAwake awake)
            {
                _awakeControllers.Add(awake);
            }
            if (controller is IInitialization initialization)
            {
                _initializeControllers.Add(initialization);
            }
            if (controller is IExecute execute)
            {
                _executeControllers.Add(execute);
            }
            if (controller is IFixedExecute fixedExecute)
            {
                _fixedControllers.Add(fixedExecute);
            }
            if (controller is ILateExecute lateExecute)
            {
                _lateControllers.Add(lateExecute);
            }
            if (controller is ICleanup cleanup)
            {
                _cleanupControllers.Add(cleanup);
            }

            return this;
        } 

        #endregion


        #region Awake

        public void Awake()
        {
            for (int index = 0; index < _awakeControllers.Count; index++)
            {
                _awakeControllers[index].Awake();
            }
        }

        #endregion


        #region Initialize

        public void Initialize()
        {
            for (int index = 0; index < _initializeControllers.Count; index++)
            {
                _initializeControllers[index].Initialize();
            }
        }

        #endregion


        #region Execute

        public void Execute(float deltaTime)
        {
            for (int index = 0; index < _executeControllers.Count; index++)
            {
                _executeControllers[index].Execute(deltaTime);
            }
        }

        #endregion


        #region FixedExecute

        public void FixedExecute(float fixedDeltaTime)
        {
            for (int index = 0; index < _fixedControllers.Count; index++)
            {
                _fixedControllers[index].FixedExecute(fixedDeltaTime);
            }
        }
        #endregion


        #region LateExecute

        public void LateExecute(float fixedDeltaTime)
        {
            for (int index = 0; index < _lateControllers.Count; index++)
            {
                _lateControllers[index].LateExecute(fixedDeltaTime);
            }
        }

        #endregion


        #region Cleanup

        public void Cleanup()
        {
            for (int index = 0; index < _cleanupControllers.Count; index++)
            {
                _cleanupControllers[index].Cleanup();
            }
        }

        #endregion
    }
}