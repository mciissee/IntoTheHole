#pragma warning disable 0414
/************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS                                                                                      *
*************************************************************************************************************************************/

using UnityEngine;
using InfinityEngine.Utils;
using System;
using System.Xml;
using UnityEngine.Events;
using UnityEngine.UI;
using InfinityEngine.Extensions;

namespace InfinityEngine.Interpolations
{


    /// <summary>
    /// Base class for all animation  classes.
    /// </summary>
    [Serializable]
    public class Anim
    {

        #region Fields

        #region Used in inspector script

        [SerializeField] internal int m_foldout;
        [SerializeField] internal float m_completedPercent = -1;
        [SerializeField] internal float m_onAfterPreviewFloatValue;
        [SerializeField] internal Vector3 m_onAfterPreviewVector3Value;
        [SerializeField] internal Color m_onAfterPreviewColorValue;

        #endregion Used in inspector script

        [SerializeField] internal MinMax time;
        [SerializeField] internal FloatValue m_floatValue;
        [SerializeField] internal Vector3Value m_vector3Value;
        [SerializeField] internal ColorValue m_colorValue;


        [SerializeField] internal int m_repeat;
        [SerializeField] internal float m_repeatInterval;
        [SerializeField] internal bool m_disableOnHide;
        [SerializeField] internal bool m_disableOnPause;

        [SerializeField] internal AnimationCurve m_motion;
        [SerializeField] internal AudioClip m_onStartSound;
        [SerializeField] internal AudioClip m_onCompleteSound;

        [SerializeField] internal AnimationTypes m_animationType;
        [SerializeField] internal LoopTypes m_loopType;
        [SerializeField] internal EaseTypes m_ease;
        [SerializeField] internal AnimatableComponents m_animatableComponent;
        [SerializeField] internal RotationModes m_rotationMode;

        [SerializeField] internal UnityEvent m_onStartCallback;
        [SerializeField] internal UnityEvent m_onUpdateCallback;
        [SerializeField] internal UnityEvent m_onCompleteCallback;
        [SerializeField] internal UnityEvent m_onTerminateCallback;

        [SerializeField] internal Transform transformReferenceValue;
        [SerializeField] internal RectTransform rectTransformReferenceValue;
        [SerializeField] internal MeshRenderer meshRendererReferenceValue;
        [SerializeField] internal Camera cameraReferenceValue;
        [SerializeField] internal Image imageReferenceValue;
        [SerializeField] internal Text textReferenceValue;
        [SerializeField] internal CanvasGroup canvasGroupReferenceValue;
        [SerializeField] internal SpriteRenderer spriteRendererReferenceValue;

        /// <summary>
        /// Gets the interpolable object used to play the animation
        /// </summary>
        internal Interpolable interpolable;

        internal AnimCreator animCreator;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets the type of the animation
        /// </summary>
        public AnimationTypes AnimationType
        {
            get { return m_animationType; }
            protected set { m_animationType = value; }
        }

        /// <summary>
        /// Gets or sets the duration of the animation
        /// </summary>
        public float Duration
        {
            get { return time.max - time.min; }
        }

        /// <summary>
        /// Gets or sets the starts delay of the animation
        /// </summary>
        public float Delay
        {
            get { return time.min; }
        }

        /// <summary>
        /// Gets or sets the number of repetition of the animation (-1 for infinite loop)
        /// </summary>
        public int Repeat
        {
            get { return m_repeat; }
            private set { m_repeat = Mathf.Clamp(value, -1, value); }
        }

        /// <summary>
        /// Gets or sets the repeat interval of the animation
        /// </summary>
        public float RepeatInterval
        {
            get { return m_repeatInterval; }
            private set { m_repeatInterval = Mathf.Clamp(value, 0, value); }
        }

        /// <summary>
        /// Gets or sets the type of loop of the animation
        /// </summary>
        public LoopTypes LoopType
        {
            get { return m_loopType; }
            private set { m_loopType = value; }
        }

        /// <summary>
        /// Gets or sets the to use with the animation ease to use
        /// </summary>
        /// <remarks>
        /// You must specify the value of <see cref="Motion"/> in the case of <see cref="EaseTypes.Custom"/>
        /// </remarks>
        public EaseTypes Ease
        {
            get { return m_ease; }
            private set { m_ease = value; }
        }

        /// <summary>
        /// Gets or sets the AnimationCurve to use for the animation. this is necesary only if current value of <see cref="Ease"/> is  EaseType.Custom
        /// </summary>
        public AnimationCurve Motion
        {
            get { return m_motion; }
            private set { m_motion = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the animation should be disabled when the GameObject linked to it is not visible
        /// </summary>
        public bool DisableOnHide
        {
            get { return m_disableOnHide; }
            set { m_disableOnHide = value; }
        }

        /// <summary>
        ///  Gets or sets a value indicating whether the animation should be disabled when the application is paused
        /// </summary>
        public bool DisableOnPause
        {
            get { return m_disableOnPause; }
            set { m_disableOnPause = value; }
        }

        /// <summary>
        /// Gets or sets the type of the fadable comp of the animation in the case of fade animation
        /// </summary>
        public AnimatableComponents AnimatableComponent
        {
            get { return m_animatableComponent; }
            private set { m_animatableComponent = value; }
        }

        /// <summary>
        /// Gets or sets the rotation mode of the animation in the case of rotation animation
        /// </summary>
        public RotationModes RotationMode
        {
            get { return m_rotationMode; }
            set { m_rotationMode = value; }
        }

        /// <summary>
        /// Gets or sets the sound to play at the starts of the animation
        /// </summary>
        public AudioClip OnStartSound
        {
            get { return m_onStartSound; }
            private set { m_onStartSound = value; }
        }

        /// <summary>
        /// Gets or sets the sound to play at the end of the animation
        /// </summary>
        public AudioClip OnCompleteSound
        {
            get { return m_onCompleteSound; }
            private set { m_onCompleteSound = value; }
        }

        /// <summary>
        /// Gets the completed percent of the animation
        /// </summary>
        public float CompletingPercent
        {
            get
            {
                if (IsPlaying)
                {
                    return interpolable.CompletedPercent;
                }
                return 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the animation is playing
        /// </summary>
        public bool IsPlaying
        {
            get
            {
                return interpolable != null && interpolable.IsPlaying;
            }
        }

        internal FloatValue FloatValue
        {
            get { return m_floatValue; }
            set { m_floatValue = value; }
        }

        internal Vector3Value Vector3Value
        {
            get { return m_vector3Value; }
            set { m_vector3Value = value; }
        }

        internal ColorValue ColorValue
        {
            get { return m_colorValue; }
            set { m_colorValue = value; }
        }

        internal bool HasRectTransform
        {
            get { return RectTransform != null; }
        }

        internal bool HasTransform
        {
            get
            {
                if (Transform == null)
                {
                    Debugger.LogError("There is no Transform attached to the animation");
                    return false;
                }
                return true;
            }
        }

        internal Transform Transform
        {
            get { return transformReferenceValue; }
            set { transformReferenceValue = value; }
        }

        internal RectTransform RectTransform
        {
            get
            {
                if (transformReferenceValue == null)
                {
                    return null;
                }

                if (rectTransformReferenceValue == null)
                {
                    rectTransformReferenceValue = transformReferenceValue.GetComponent<RectTransform>();
                }
                return rectTransformReferenceValue;
            }

        }

        #endregion Properties

        private Anim()
        {

        }

        public Anim(AnimationTypes type, Transform transform)
        {
            this.AnimationType = type;
            this.transformReferenceValue = transform;
        }

        #region Methods

        /// <summary>
        /// Starts the animation
        /// </summary>
        public bool Start()
        {
            if (Transform == null)
            {
                return false;
            }

            if (animCreator == null)
            {
                animCreator = AnimCreatorFactory.Create(this);
            }

            if (!animCreator.IsValid())
            {
                return false;
            }

            animCreator.Create();

            interpolable.SetCached()
           .SetStartDelay(Delay)
           .SetRepeat(Repeat, LoopType)
           .SetRepeatInterval(RepeatInterval)
           .SetDisableOnHide(DisableOnHide)
           .SetDisableOnPause(DisableOnPause)
           .SetRotationMode(RotationMode)
           .OnStart(arg => PlayOnStartSound())
           .OnTerminate(arg => PlayOnCompleteSound());
            if (Ease == EaseTypes.Custom)
            {
                interpolable.SetEase(Motion);
            }
            else
            {
                interpolable.SetEase(Ease);
            }

            transformReferenceValue.SetGameObjectActive(true);

            interpolable.OnUpdate(arg =>
            {
                m_completedPercent = arg.CompletedPercent;
            });

            interpolable.OnTerminate(arg =>
            {
                m_completedPercent = -1;
                if (!Application.isPlaying)
                {
                    OnAfterPreview();
                }
            });

            if (Application.isPlaying)
            {
                if (m_onStartCallback != null)
                {
                    interpolable.OnStart(arg => m_onStartCallback.Invoke());
                }

                if (m_onUpdateCallback != null)
                {
                    interpolable.OnUpdate(arg => m_onUpdateCallback.Invoke());
                }

                if (m_onCompleteCallback != null)
                {
                    interpolable.OnComplete(arg => m_onCompleteCallback.Invoke());
                }

                if (m_onTerminateCallback != null)
                {
                    interpolable.OnTerminate(arg => m_onTerminateCallback.Invoke());
                }
            }

            interpolable.Start();
            return true;
        }

        /// <summary>
        /// Resets the animation
        /// </summary>
        public void Reset()
        {
            if (animCreator != null)
            {
                animCreator.Reset();
            }
        }

        /// <summary>
        /// Sets this animation as the given in the parameter of the function.
        /// </summary>
        /// <param name="other">Anim to copy</param>
        public void Copy(Anim other)
        {
            switch (m_animationType)
            {
                case AnimationTypes.Translation:
                case AnimationTypes.Rotation:
                case AnimationTypes.Scale:
                    Vector3Value = new Vector3Value(other.Vector3Value);
                    break;
                case AnimationTypes.Fade:
                    FloatValue = new FloatValue(other.FloatValue);
                    break;
                case AnimationTypes.Color:
                    ColorValue = new ColorValue(other.ColorValue);
                    break;
            }

            time = other.time;
            m_disableOnHide = other.m_disableOnHide;
            m_disableOnPause = other.m_disableOnPause;
            m_repeat = other.m_repeat;
            m_repeatInterval = other.m_repeatInterval;
            m_ease = other.m_ease;
            m_motion = other.m_motion;
            m_animatableComponent = other.m_animatableComponent;
            m_loopType = other.m_loopType;
            m_onStartSound = other.m_onStartSound;
            m_onCompleteSound = other.m_onCompleteSound;
        }

        /// <summary>
        /// Creates an exact clone of the animation
        /// </summary>
        /// <returns>A clone of the animation</returns>
        public Anim Clone()
        {
            var anim = new Anim(AnimationType, Transform);
            anim.Copy(this);
            return anim;
        }

        /// <summary>
        /// Stops the animation if it is playing.
        /// </summary>
        public void Stop()
        {
            if (interpolable != null)
            {
                interpolable.Terminate();
                m_completedPercent = -1;
            }
        }

        private void PlayOnStartSound()
        {
            if (m_onStartSound != null)
            {
                SoundManager.PlayEffect(m_onStartSound);
            }
        }

        private void PlayOnCompleteSound()
        {
            if (OnCompleteSound != null)
            {
                SoundManager.PlayEffect(OnCompleteSound);
            }
        }

        private float GetFade()
        {
            switch (AnimatableComponent)
            {
                case AnimatableComponents.CanvasGroup:
                    if (HasComponent(ref canvasGroupReferenceValue))
                    {
                        return canvasGroupReferenceValue.alpha;
                    }
                    break;
                case AnimatableComponents.Text:
                    if (HasComponent(ref textReferenceValue))
                    {
                        return textReferenceValue.color.a;
                    }
                    break;
                case AnimatableComponents.Image:
                    if (HasComponent(ref imageReferenceValue))
                    {
                        return imageReferenceValue.color.a;
                    }
                    break;
                case AnimatableComponents.SpriteRenderer:
                    if (HasComponent(ref spriteRendererReferenceValue))
                    {
                        return spriteRendererReferenceValue.color.a;
                    }
                    break;
                default:
                    return 0;

            }
            return 0;
        }

        private Color GetColor()
        {
            switch (AnimatableComponent)
            {
                case AnimatableComponents.MeshRenderer:
                    if (HasComponent(ref meshRendererReferenceValue))
                    {
                        if (meshRendererReferenceValue.sharedMaterial == null || !meshRendererReferenceValue.sharedMaterial.HasProperty("_Color"))
                        {
                            return Color.black;
                        }
                        return meshRendererReferenceValue.sharedMaterial.color;
                    }
                    break;
                case AnimatableComponents.Text:
                    if (HasComponent(ref textReferenceValue))
                    {
                        return textReferenceValue.color;
                    }
                    break;
                case AnimatableComponents.Image:
                    if (HasComponent(ref imageReferenceValue))
                    {
                        return imageReferenceValue.color;
                    }
                    break;
                case AnimatableComponents.SpriteRenderer:
                    if (HasComponent(ref spriteRendererReferenceValue))
                    {
                        return spriteRendererReferenceValue.color;
                    }
                    break;
                default:
                    return Color.black;

            }
            return Color.black;
        }

        private void OnAfterPreview()
        {
            switch (AnimationType)
            {
                case AnimationTypes.Translation:
                    if (HasRectTransform)
                    {
                        rectTransformReferenceValue.anchoredPosition3D = m_onAfterPreviewVector3Value;
                    }
                    else
                    {
                        transformReferenceValue.localPosition = m_onAfterPreviewVector3Value;
                    }
                    break;
                case AnimationTypes.Rotation:
                    transformReferenceValue.localRotation = Quaternion.Euler(m_onAfterPreviewVector3Value);
                    break;
                case AnimationTypes.Scale:
                    transformReferenceValue.localScale = m_onAfterPreviewVector3Value;
                    break;
                case AnimationTypes.Fade:
                    OnAfterFadeAnimPreview();
                    break;
                case AnimationTypes.Color:
                    OnAfterColorAnimPreview();
                    break;
                default:
                    break;
            }
        }

        internal void OnBeforePreview()
        {
            switch (AnimationType)
            {
                case AnimationTypes.Translation:
                    if (HasRectTransform)
                    {
                        m_onAfterPreviewVector3Value = rectTransformReferenceValue.anchoredPosition3D;
                    }
                    else
                    {
                        m_onAfterPreviewVector3Value = transformReferenceValue.localPosition;
                    }
                    break;
                case AnimationTypes.Rotation:
                    m_onAfterPreviewVector3Value = transformReferenceValue.localRotation.eulerAngles;
                    break;
                case AnimationTypes.Scale:
                    m_onAfterPreviewVector3Value = transformReferenceValue.localScale;
                    break;
                case AnimationTypes.Fade:
                    OnBeforeFadeAnimPreview();
                    break;
                case AnimationTypes.Color:
                    OnBeforeColorAnimPreview();
                    break;
                default:
                    break;
            }
        }

        private void OnAfterFadeAnimPreview()
        {
            switch (AnimatableComponent)
            {
                case AnimatableComponents.CanvasGroup:
                    canvasGroupReferenceValue.alpha = m_onAfterPreviewFloatValue;
                    break;
                case AnimatableComponents.Text:
                    textReferenceValue.SetAlpha(m_onAfterPreviewFloatValue);
                    break;
                case AnimatableComponents.Image:
                    imageReferenceValue.SetAlpha(m_onAfterPreviewFloatValue);
                    break;
                case AnimatableComponents.SpriteRenderer:
                    spriteRendererReferenceValue.SetAlpha(m_onAfterPreviewFloatValue);
                    break;
            }
        }

        private void OnAfterColorAnimPreview()
        {
            switch (AnimatableComponent)
            {
                case AnimatableComponents.MeshRenderer:
                    meshRendererReferenceValue.sharedMaterial.color = m_onAfterPreviewColorValue;
                    break;
                case AnimatableComponents.Text:
                    textReferenceValue.color = m_onAfterPreviewColorValue;
                    break;
                case AnimatableComponents.Image:
                    imageReferenceValue.color = m_onAfterPreviewColorValue;
                    break;
                case AnimatableComponents.SpriteRenderer:
                    spriteRendererReferenceValue.color = m_onAfterPreviewColorValue;
                    break;
                case AnimatableComponents.Camera:
                    cameraReferenceValue.backgroundColor = m_onAfterPreviewColorValue;
                    break;
            }
        }

        private void OnBeforeFadeAnimPreview()
        {
            if (IsValid())
            {
                switch (AnimatableComponent)
                {
                    case AnimatableComponents.CanvasGroup:
                        m_onAfterPreviewFloatValue = canvasGroupReferenceValue.alpha;
                        break;
                    case AnimatableComponents.Text:
                        m_onAfterPreviewFloatValue = textReferenceValue.color.a;
                        break;
                    case AnimatableComponents.Image:
                        m_onAfterPreviewFloatValue = imageReferenceValue.color.a;
                        break;
                    case AnimatableComponents.SpriteRenderer:
                        m_onAfterPreviewFloatValue = spriteRendererReferenceValue.color.a;
                        break;
                }
            }
        }

        private void OnBeforeColorAnimPreview()
        {
            if (IsValid())
            {

                switch (AnimatableComponent)
                {
                    case AnimatableComponents.MeshRenderer:
                        m_onAfterPreviewColorValue = meshRendererReferenceValue.sharedMaterial.color;
                        break;
                    case AnimatableComponents.Text:
                        m_onAfterPreviewColorValue = textReferenceValue.color;
                        break;
                    case AnimatableComponents.Image:
                        m_onAfterPreviewColorValue = imageReferenceValue.color;
                        break;
                    case AnimatableComponents.SpriteRenderer:
                        m_onAfterPreviewColorValue = spriteRendererReferenceValue.color;
                        break;
                    case AnimatableComponents.Camera:
                        m_onAfterPreviewColorValue = cameraReferenceValue.backgroundColor;
                        break;
                }

            }
        }

        internal XmlNode ToXml(XmlDocument doc)
        {
            var root = doc.CreateElement("Anim");

            var typeNode = doc.CreateElement("Type");
            typeNode.AppendChild(doc.CreateTextNode(AnimationType.ToString()));
            root.AppendChild(typeNode);

            var delayNode = doc.CreateElement("delay");
            delayNode.AppendChild(doc.CreateTextNode(Delay.ToString()));
            root.AppendChild(delayNode);

            var durationNode = doc.CreateElement("duration");
            durationNode.AppendChild(doc.CreateTextNode(Duration.ToString()));
            root.AppendChild(durationNode);

            var repeatNode = doc.CreateElement("repeat");
            repeatNode.AppendChild(doc.CreateTextNode(Repeat.ToString()));
            root.AppendChild(repeatNode);

            var repeatIntervalNode = doc.CreateElement("repeatInterval");
            repeatIntervalNode.AppendChild(doc.CreateTextNode(RepeatInterval.ToString()));
            root.AppendChild(repeatIntervalNode);

            var loopNode = doc.CreateElement("loopType");
            loopNode.AppendChild(doc.CreateTextNode(LoopType.ToString()));
            root.AppendChild(loopNode);


            var easeNode = doc.CreateElement("ease");
            easeNode.AppendChild(doc.CreateTextNode(Ease.ToString()));
            root.AppendChild(easeNode);

            var fadableNode = doc.CreateElement("FadableComponentType");
            fadableNode.AppendChild(doc.CreateTextNode(AnimatableComponent.ToString()));
            root.AppendChild(fadableNode);

            var rotationNode = doc.CreateElement("rotationMode");
            rotationNode.AppendChild(doc.CreateTextNode(RotationMode.ToString()));
            root.AppendChild(rotationNode);

            if (Motion != null && Motion.keys.Length > 0)
            {
                var motionNode = doc.CreateElement("AnimationCurve");

                XmlNode keyNode;
                XmlNode timeNode;
                XmlNode valueNode;

                XmlNode inTangNode;
                XmlNode outTangNode;

                var keys = Motion.keys;
                foreach (var frame in keys)
                {
                    keyNode = doc.CreateElement("keyframe");
                    timeNode = doc.CreateElement("time");
                    valueNode = doc.CreateElement("value");
                    inTangNode = doc.CreateElement("inTangent");
                    outTangNode = doc.CreateElement("outTangent");

                    timeNode.AppendChild(doc.CreateTextNode(frame.time.ToString()));
                    valueNode.AppendChild(doc.CreateTextNode(frame.value.ToString()));

                    inTangNode.AppendChild(doc.CreateTextNode(frame.inTangent.ToString()));
                    outTangNode.AppendChild(doc.CreateTextNode(frame.outTangent.ToString()));

                    keyNode.AppendChild(timeNode);
                    keyNode.AppendChild(valueNode);
                    keyNode.AppendChild(inTangNode);
                    keyNode.AppendChild(outTangNode);

                    motionNode.AppendChild(keyNode);

                }

                root.AppendChild(motionNode);
            }
            switch (AnimationType)
            {
                case AnimationTypes.Translation:
                case AnimationTypes.Rotation:
                case AnimationTypes.Scale:
                    Vector3Value.ToXml(root);
                    break;
                case AnimationTypes.Fade:
                    FloatValue.ToXml(root);
                    break;
                case AnimationTypes.Color:
                    ColorValue.ToXml(root);
                    break;
            }
            return root;
        }

        internal static Anim FromXml(XmlNode root)
        {
            Anim anim;
            float delay;
            int repeat;
            float repeatInterval;
            float duration;
            LoopTypes loopType;
            EaseTypes ease;
            AnimatableComponents fadableType = AnimatableComponents.None;
            RotationModes rotationMode;
            AnimationTypes animType;
            AnimationCurve motion = null;
            MinMax range = new MinMax();

            try
            {
                var type = root["Type"];
                animType = (AnimationTypes)Enum.Parse(typeof(AnimationTypes), type.FirstChild.Value);

                var delayNode = root["delay"];
                delay = float.Parse(delayNode.FirstChild.Value);

                var durationNode = root["duration"];
                duration = float.Parse(durationNode.FirstChild.Value);

                range.min = delay;
                range.max = delay + duration;


                var repeatNode = root["repeat"];
                repeat = int.Parse(repeatNode.FirstChild.Value);

                var repeatIntervalNode = root["repeatInterval"];
                repeatInterval = float.Parse(repeatIntervalNode.FirstChild.Value);

                var loopNode = root["loopType"];
                loopType = (LoopTypes)Enum.Parse(typeof(LoopTypes), loopNode.FirstChild.Value);

                var easeNode = root["ease"];
                ease = (EaseTypes)Enum.Parse(typeof(EaseTypes), easeNode.FirstChild.Value);


                var fadableNode = root["FadableComponentType"];
                fadableType = (AnimatableComponents)Enum.Parse(typeof(AnimatableComponents), fadableNode.FirstChild.Value);

                var rotationNode = root["rotationMode"];
                rotationMode = (RotationModes)Enum.Parse(typeof(RotationModes), rotationNode.FirstChild.Value);

                var motionNode = root["AnimationCurve"];

                if (motionNode != null)
                {

                    motion = new AnimationCurve();

                    XmlNode timeNode;
                    XmlNode valueNode;

                    XmlNode inTangNode;
                    XmlNode outTangNode;
                    XmlNodeList arrayNode = motionNode.ChildNodes;

                    float time;
                    float value;
                    float inTang;
                    float outTang;

                    foreach (XmlNode node in arrayNode)
                    {
                        timeNode = node["time"];
                        valueNode = node["value"];
                        inTangNode = node["inTangent"];
                        outTangNode = node["outTangent"];

                        time = float.Parse(timeNode.FirstChild.Value);
                        value = float.Parse(valueNode.FirstChild.Value);
                        inTang = float.Parse(inTangNode.FirstChild.Value);
                        outTang = float.Parse(outTangNode.FirstChild.Value);

                        motion.AddKey(new Keyframe(time, value, inTang, outTang));
                    }
                }

            }
            catch (Exception ex)
            {

                Debug.LogError("Failed to the load the animation : \n" + ex.StackTrace);
                return null;
            }

            anim = new Anim();
            anim.time = range;
            anim.m_animationType = animType;
            anim.m_repeat = repeat;
            anim.m_repeatInterval = repeatInterval;
            anim.m_loopType = loopType;
            anim.m_ease = ease;
            anim.m_motion = motion;
            anim.m_animatableComponent = fadableType;
            anim.m_rotationMode = rotationMode;
            switch (animType)
            {
                case AnimationTypes.Translation:
                case AnimationTypes.Rotation:
                case AnimationTypes.Scale:
                    anim.Vector3Value = Vector3Value.FromXml(root);
                    break;
                case AnimationTypes.Fade:
                    anim.FloatValue = FloatValue.FromXml(root);
                    break;
                case AnimationTypes.Color:
                    anim.ColorValue = ColorValue.FromXml(root);
                    break;
            }
            return anim;
        }

        internal void AutoSetFromValue()
        {
            if (transformReferenceValue == null)
            {
                return;
            }
            switch (AnimationType)
            {
                case AnimationTypes.Translation:

                    m_vector3Value.starts = HasRectTransform ? rectTransformReferenceValue.anchoredPosition3D : transformReferenceValue.localPosition;
                    break;
                case AnimationTypes.Rotation:
                    m_vector3Value.starts = transformReferenceValue.localRotation.eulerAngles;
                    break;
                case AnimationTypes.Scale:
                    m_vector3Value.starts = transformReferenceValue.localScale;
                    break;
                case AnimationTypes.Fade:
                    m_floatValue.starts = GetFade();
                    break;
                case AnimationTypes.Color:
                    m_colorValue.starts = GetColor();
                    break;
                default:
                    break;
            }
        }

        internal void AutoSetToValue()
        {
            if (transformReferenceValue == null)
            {
                return;
            }
            switch (AnimationType)
            {
                case AnimationTypes.Translation:
                    m_vector3Value.ends = HasRectTransform ? rectTransformReferenceValue.anchoredPosition3D : transformReferenceValue.localPosition;
                    break;
                case AnimationTypes.Rotation:
                    m_vector3Value.ends = transformReferenceValue.localRotation.eulerAngles;
                    break;
                case AnimationTypes.Scale:
                    m_vector3Value.ends = transformReferenceValue.localScale;
                    break;
                case AnimationTypes.Fade:
                    m_floatValue.ends = GetFade();
                    break;
                case AnimationTypes.Color:
                    m_colorValue.ends = GetColor();
                    break;
                default:
                    break;
            }
        }

        internal bool IsValid()
        {
            if (transformReferenceValue == null)
            {
                return false;
            }
            switch (AnimatableComponent)
            {
                case AnimatableComponents.None:
                    return false;
                case AnimatableComponents.Transform:
                    return true;
                case AnimatableComponents.RectTransform:
                    return HasComponent(ref rectTransformReferenceValue);
                case AnimatableComponents.CanvasGroup:
                    return HasComponent(ref canvasGroupReferenceValue);
                case AnimatableComponents.Camera:
                    return HasComponent(ref cameraReferenceValue);
                case AnimatableComponents.Text:
                    return HasComponent(ref textReferenceValue);
                case AnimatableComponents.Image:
                    return HasComponent(ref imageReferenceValue);
                case AnimatableComponents.SpriteRenderer:
                    return HasComponent(ref spriteRendererReferenceValue);
                case AnimatableComponents.MeshRenderer:
                    return HasComponent(ref meshRendererReferenceValue)
                        && meshRendererReferenceValue.sharedMaterial != null
                        && meshRendererReferenceValue.sharedMaterial.HasProperty("_Color");
                    ;
                default:
                    break;
            }

            return true;
        }

        internal bool HasComponent<T>(ref T value) where T : Component
        {
            if (value != null && value.transform != Transform)
            {
                value = null;
            }
            if (value == null)
            {
                value = Transform.GetComponent<T>();
            }
            return value != null;
        }

        #endregion Methods

    }

}