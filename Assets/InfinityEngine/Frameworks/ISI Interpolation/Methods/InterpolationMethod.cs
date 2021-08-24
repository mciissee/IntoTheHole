/************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Infinity Interactive Unity Asset Store catalog: http://u3d.as/riS	                                                                *
************************************************************************************************************************************/

using UnityEngine;
using InfinityEngine.Attributes;
using InfinityEngine.Utils;

namespace InfinityEngine.Interpolations
{

    /// <summary>
    /// Base class of all components used to provides a access to an extension method of <see cref="InfinityEngine.Extensions"/> method in design time.
    /// </summary>
    [OverrideInspector]
    [AddComponentMenu("InfinityEngine/Interpolations/InterpolationMethod")]
    public abstract class InterpolationMethod : MonoBehaviour
    {

        #region Fields

        [HideInInspector]
        [SerializeField]
        private InterpolationMethodChooser chooser;

        private InterpolationMethodCallbacks callbacks;
        private InterpolationMethodOptions options;

        private bool isReverseMode;

        #region Heading

        [InfinityHeader("https://infinity-engine-f6f33.firebaseapp.com/#ISI-Interpolation")]
        [SerializeField]
        protected bool __help__;

        [DrawOrder(-1)]
        [Message("The name used by the function Invoke(string) of the component InterpolationMethodChooser to identify the component", MessageTypes.Info)]
        [VisibleIf(nameof(HasChooserComponent), MemberTypes.Method)]
        [SerializeField]
        protected string _name;

        [DrawOrder(-1)]
        [Message("Invoke the function when the game starts", MessageTypes.Info)]
        [SerializeField]
        protected bool playOnStart;

        #endregion Heading

        #region Options

        [Accordion("Options")]
        [SerializeField]
        protected InterpolationMethodOptions normalStateOptions;

        [Accordion("Options")]
        [SerializeField]
        protected InterpolationMethodOptions reverseStateOptions;

        #endregion Options

        #region Callbacks

        [Accordion("Callbacks")]
        [SerializeField]
        protected InterpolationMethodCallbacks normalStatecallbacks;

        [Accordion("Callbacks")]
        [SerializeField]
        protected InterpolationMethodCallbacks reverseStateCallbacks;

        #endregion Callbacks

        [Accordion("Parameters")]
        [Message("The duration of the interpolation", MessageTypes.Info)]
        [MessageIfEquals(0f, "The interpolation will not start if the duration is sets to 0", MessageTypes.Warning)]
        [SerializeField]
        protected float duration;


        [InspectorButton(nameof(StopInvoke), "Stop " + FA.stop, 18, 18)]
        [InspectorButton(nameof(PreviewReverse), "Play Reverse " + FA.backward, 18, 18)]
        [InspectorButton(nameof(Invoke), "Play " + FA.play, 18, 18)]
        [SerializeField]
        internal DecoratorField buttons;

        [VisibleIf(nameof(IsPlaying), MemberTypes.Property)]
        [ProgressBar]
        [SerializeField]
        protected float progress;

        protected bool isStartedInEditMode;
        protected Interpolable interpolable;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets a value indicating whether the interpolation is creates by the interpolation is started
        /// </summary>
        public bool IsStarted => interpolable != null && interpolable.IsStarted;

        /// <summary>
        /// Gets a value indicating whether the interpolation is creates by the interpolation is playing
        /// </summary>
        public bool IsPlaying => interpolable != null && interpolable.IsPlaying;

        /// <summary>
        /// Gets the name of given to the method in the inspector
        /// </summary>
        public string Name => _name;

        /// <summary>
        /// Gets or sets the options of the animation
        /// </summary>
        public InterpolationOptions Options { get { return options.options; } set { options.options = value; } }

        #endregion Properties

        #region Methods

        protected virtual void OnValidate()
        {
            duration = Mathf.Max(0, duration);
        }

        private void Start()
        {
            if (playOnStart)
            {
                Invoke();
            }
        }

        /// <summary>
        /// Invokes the function
        /// </summary>
        public virtual void Invoke()
        {
            StopInvoke();
            isStartedInEditMode = false;
            interpolable = null;
            callbacks = isReverseMode ? reverseStateCallbacks : normalStatecallbacks;
            options = isReverseMode ? reverseStateOptions : normalStateOptions;
        }

        /// <summary>
        /// Stops the function
        /// </summary>
        public virtual void StopInvoke()
        {
            if (IsStarted)
            {
                interpolable.Terminate();
            }
        }

        /// <summary>
        /// Invokes the reversed version of the animation
        /// </summary>
        public void InvokeReverse()
        {
            isReverseMode = true;
            Invoke();
            isReverseMode = false;
        }

        internal void PreviewReverse()
        {
            InvokeReverse();
        }

        /// <summary>
        /// This method is linked to <see cref="StopInvoke"/> and the name should not change because it is used
        /// by an editor script to know if it should draw the button 'Stop Loop'.
        /// </summary>
        /// <returns><c>true</c> if the button should be drawed</returns>
        internal bool __StopInvoke__()
        {
            return IsPlaying && interpolable.RepeatCount != 0;
        }

        private void UpdateProgress()
        {
            progress = interpolable.CompletedPercent;
        }

        /// <summary>
        /// Called after the end of the interpolation in editor mode
        /// </summary>
        protected virtual void OnAfterPreview()
        {
            progress = 0;
        }

        internal void StartInterpolation()
        {
            if (interpolable != null)
            {
                options.ApplyOptionsToInterpolable(interpolable);

                if (isReverseMode)
                {
                    interpolable.Reverse();
                }

                isStartedInEditMode = !Application.isPlaying;

                AddListeners();
                gameObject.SetActive(true);
                interpolable.Start();
            }
        }

        internal void AddListeners()
        {
            if (interpolable != null)
            {
                interpolable.OnUpdate(arg => progress = arg.CompletedPercent);
                if (!Application.isPlaying)
                {
                    interpolable.OnTerminate(arg => OnAfterPreview());
                    callbacks.ApplyCallbacksToInterpolable(interpolable);
                }

                if (options.hideGameObjectAtEnd)
                {
                    interpolable.OnTerminate(arg => gameObject.SetActive(false));
                }
            }
        }

        internal bool HasComponent<T>(ref T value) where T : Component
        {
            if (value != null && value.transform != transform)
            {
                value = null;
            }

            value = value ?? GetComponent<T>();
            return value != null;
        }

        internal bool HasChooserComponent()
        {
            return HasComponent(ref chooser);
        }

        #endregion Methods

    }
}