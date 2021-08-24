/************************************************************************************************************************************
* Developed by Mamadou Cisse and Jasper Flick                                                                                       *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS	                                                                                    *
*************************************************************************************************************************************/
using InfinityEngine.Localization;
namespace IntoTheHole
{
    using InfinityEngine;
    using InfinityEngine.DesignPatterns;
    using InfinityEngine.ResourceManagement;
    using InfinityEngine.Serialization;
    using UnityEngine;

    /// <summary>
    /// Represents the player and the model component of MVC design pattern.
    /// </summary>
    public class Player : Observable
    {

        #region Fields

        [SerializeField]
        private float startVelocity = 1.0f;
        [SerializeField]
        private float maxVelocity = 4.0f;
        [SerializeField]
        private float rotationVelocity;
        [SerializeField]
        private float acceleration = 0.025f;


        private ControlMode controlMode = ControlMode.Touch;
        private float velocity;
        private float distanceTraveled;
        private int bonusCount;
        private float deltaToRotation;
        private float systemRotation;
        private float worldRotation;
        private float avatarRotation;

        private Controler controler;
        private PipeManager pipeManager;
        private Transform world;
        private Transform rotater;
        private Pipe currentPipe;

        private bool IsStarted = false;
        private bool enableTouchMove;
        private bool enableAccelerometerMove;
        private bool moveModeChanged;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the velocity of the player.
        /// </summary>
        /// <remarks>
        ///     The function <see cref="View.OnChanged(object)"/> is invoked when you changes this value.
        /// </remarks>
        public float Velocity
        {
            get => velocity;

            private set
            {
                velocity = value;
                NotifyObservers();
            }
        }

        /// <summary>
        /// Gets or sets the distance covered by the player.
        /// </summary>
        /// <remarks>
        ///     The function <see cref="View.OnChanged(object)"/> is invoked when you changes this value.
        /// </remarks>
        public float Distance
        {
            get => distanceTraveled;

            private set
            {
                distanceTraveled = value;
                NotifyObservers();
            }
        }

        /// <summary>
        /// Gets or sets the number of bonues of the player.
        /// </summary>
        /// <remarks>
        ///     The function <see cref="View.OnChanged(object)"/> is invoked when you changes this value.
        /// </remarks>
        public int BonusCount
        {
            get => bonusCount;

            set
            {
                bonusCount = value;
                NotifyObservers();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating if the player can move by touching the screen
        /// </summary>
        /// <remarks>
        ///     The function <see cref="View.OnChanged(object)"/> is invoked when you changes this value.
        /// </remarks>
        public bool EnableTouchMove
        {
            get => enableTouchMove;
            set
            {
                enableTouchMove = value;
                VP.SetBool(R2.bools.EnableTouchMode.Key, value);
                ModeMoveChanged = true;
                VP.Save();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating if the player can move by using the accelerometer
        /// </summary>
        /// <remarks>
        ///     The function <see cref="View.OnChanged(object)"/> is invoked when you changes this value.
        /// </remarks>
        public bool EnableAccelerometer
        {
            get => enableAccelerometerMove;
            set
            {
                enableAccelerometerMove = value;
                VP.SetBool(R2.bools.EnableAccelerometer.Key, value);
                ModeMoveChanged = true;
                VP.Save();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating if the current movement mode has changed.
        /// </summary>
        /// <remarks>
        ///     The function <see cref="View.OnChanged(object)"/> is invoked when you changes this value.
        /// </remarks>
        public bool ModeMoveChanged
        {
            get => moveModeChanged;
            set
            {
                moveModeChanged = value;
                NotifyObservers();
                moveModeChanged = false;
            }
        }

        #endregion Properties

        #region Methods

        #region Unity

        void OnEnable()
        {
            controler = controler ?? FindObjectOfType<Controler>();
            controler.RegisterEvent(GameEvent.Start, OnStart);
            controler.RegisterEvent(GameEvent.End, OnEnd);
        }

        void OnDisable()
        {
            controler.UnRegisterEvent(GameEvent.Start, OnStart);
            controler.UnRegisterEvent(GameEvent.End, OnEnd);
        }

        private void Start()
        {

            pipeManager = FindObjectOfType<PipeManager>();
            world = pipeManager.transform.parent;
            rotater = transform.GetChild(0);

            ResetStatistics();

            currentPipe = pipeManager.SetupFirstPipe();
            SetupCurrentPipe();

            EnableAccelerometer = VP.GetBool(R2.bools.EnableAccelerometer.Key, EnableAccelerometer);
            EnableTouchMove = VP.GetBool(R2.bools.EnableTouchMode.Key, EnableTouchMove);
        }

        private void Update()
        {

            if (IsStarted && velocity < maxVelocity)
                Velocity += acceleration * Time.deltaTime;

            float delta = velocity * Time.deltaTime;
            Distance += delta;
            systemRotation += delta * deltaToRotation;

            if (systemRotation >= currentPipe.CurveAngle)
            {
                delta = (systemRotation - currentPipe.CurveAngle) / deltaToRotation;
                currentPipe = pipeManager.SetupNextPipe();
                SetupCurrentPipe();
                systemRotation = delta * deltaToRotation;
            }

            pipeManager.transform.localRotation = Quaternion.Euler(0f, 0f, systemRotation);

            if (IsStarted)
                UpdateAvatarRotation();

        }

        #endregion Unity

        #region Game Events

        /// <summary>
        /// Function invoked by the instance of <see cref="Controler"/> class when the function <see cref="Controler.OnStart"/> is called.
        /// </summary>
        public void OnStart()
        {
            IsStarted = true;
            ResetStatistics();
        }

        /// <summary>
        /// Function invoked by the instance of <see cref="Controler"/> class when the function <see cref="Controler.OnEnd"/> is called.
        /// </summary>
        public void OnEnd()
        {
            if (!IsStarted)
            {
                return;
            }
            SoundManager.PlayEffect(R.audioclip.SoundFail);
            IsStarted = false;
        }

        #endregion Game Events

        private void ResetStatistics()
        {
            distanceTraveled = 0f;
            avatarRotation = 0f;
            systemRotation = 0f;
            worldRotation = 0f;
            velocity = startVelocity;
            bonusCount = 0;

            if (enableAccelerometerMove && enableTouchMove)
            {
                controlMode = ControlMode.Both;
            }
            else if (EnableTouchMove)
            {
                controlMode = ControlMode.Touch;
            }
            else if (EnableAccelerometer)
            {
                controlMode = ControlMode.Accelerometer;
            }
            else
            {
                controlMode = ControlMode.Touch;
            }

        }

        private void UpdateAvatarRotation()
        {
            var rotationInput = 0f;
            if (Application.isMobilePlatform)
            {
                switch (controlMode)
                {
                    case ControlMode.Touch:
                        if (Input.touchCount == 1)
                        {
                            if (Input.GetTouch(0).position.x < Screen.width * 0.5f)
                            {
                                rotationInput = -1f;
                            }
                            else
                            {
                                rotationInput = 1f;
                            }
                        }
                        break;
                    case ControlMode.Accelerometer:
                        rotationInput = Input.acceleration.x;
                        break;
                    case ControlMode.Both:
                        rotationInput = Input.acceleration.x;
                        if (Input.touchCount == 1)
                        {
                            if (Input.GetTouch(0).position.x < Screen.width * 0.5f)
                            {
                                rotationInput = -1f;
                            }
                            else
                            {
                                rotationInput = 1f;
                            }
                        }
                        break;
                }
            }
            else
            {
                rotationInput = Input.GetAxis("Horizontal");
            }

            avatarRotation += rotationVelocity * Time.deltaTime * rotationInput;
            if (avatarRotation < 0f)
            {
                avatarRotation += 360f;
            }
            else if (avatarRotation >= 360f)
            {
                avatarRotation -= 360f;
            }
            rotater.localRotation = Quaternion.Euler(avatarRotation, 0f, 0f);
        }

        private void SetupCurrentPipe()
        {
            deltaToRotation = 360f / (2f * Mathf.PI * currentPipe.CurveRadius);
            worldRotation += currentPipe.RelativeRotation;
            if (worldRotation < 0f)
            {
                worldRotation += 360f;
            }
            else if (worldRotation >= 360f)
            {
                worldRotation -= 360f;
            }
            world.localRotation = Quaternion.Euler(worldRotation, 0f, 0f);
        }

        #endregion Methods

    }

}