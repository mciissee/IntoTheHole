#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.


/************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS								                                                        *
*************************************************************************************************************************************/

using System;
using System.IO;
using System.Linq;
using InfinityEngine.Extensions;
using InfinityEngine.Interpolations;
using InfinityEngine.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static InfinityEditor.SequencerInspector;

namespace InfinityEditor
{

    /// <summary>
    /// Drawer class of <see cref="AnimSequence"/> class.<br/>
    /// </summary>
    [Serializable]
    public class SequenceDrawer
    {

        #region Fields
        private static AnimationTypes[] AnimationTypesEnumValues =
            Enum.GetNames(typeof(AnimationTypes))
                .Select(animation => (AnimationTypes)Enum.Parse(typeof(AnimationTypes), animation))
                .ToArray();

        private static GUIContent SequenceHeadingHelp;
        private static GUIContent NameContent;

        private static GUIContent OnStartContent;
        private static GUIContent OnUpdateContent;
        private static GUIContent OnCompleteContent;
        private static GUIContent OnTerminateContent;

        private static GUIContent DurationContent;
        private static GUIContent DelayContent;
        private static GUIContent EaseContent;
        private static GUIContent AnimatedComponentContent;
        private static GUIContent RepeatContent;
        private static GUIContent IntervalContent;
        private static GUIContent RepeatTypeContent;
        private static GUIContent FromContent;
        private static GUIContent ToContent;
        private static GUIContent StartSoundContent;
        private static GUIContent CompleteSoundContent;
        private static GUIContent DisableAtEndContent;
        private static GUIContent RotationModeContent;

        internal Action<string> onDeleteSequenceCallback;
        internal Action<string, string> onSaveCallback;
        internal Action<string, string> onLoadCallback;
        internal Action<string> onPreviewCallback;
        internal Action onSetOuAsInCallback;
        internal Action<string> onStopPreviewCallback;
        internal Action<string, int> onClickAutoSetFromValueCallback;
        internal Action<string, int> onClickAutoSetToValueCallback;
        internal Action<string, AnimationTypes, bool> onClickUseCurrent;
        internal Func<string, int, bool> onValidateAnimCallback;

        private int fadableComponentCount;
        private int colorableComponentCount;
        private bool showHelp;

        internal SerializedProperty serializedProperty;
        internal SerializedProperty animations;
        internal SerializedProperty isExpanded;
        internal SerializedProperty sequenceName;

        private SerializedProperty sequenceDuration;
        private SerializedProperty disableGameObjectAtEnd;
        private SerializedProperty selectedAnimationIndex;
        private SerializedProperty selectedAnimFoldout;

        private int newSelectedAnimationIndex;

        private SimpleAccordion accordion;
        private SimpleAccordion timelineAccordion;
        private SimpleAccordion selectedAnimAccordion;

        private GUIStyle timelineRangeStyle;
        private bool isAddedNewAnim;
        private bool isDeletedAnim;
        private int deletedIndex;
        private AnimationTypes addedAnimType;

        private int animationsCount;
        private int lastAnimCount;
        #endregion Fields

        public SequenceDrawer(SerializedProperty serializedProperty)
        {
            this.serializedProperty = serializedProperty;
        }

        private void FindSerializedProperties()
        {
            if (sequenceName == null)
            {
                sequenceName = serializedProperty.FindPropertyRelative(nameof(AnimSequence.m_name));
                sequenceDuration = serializedProperty.FindPropertyRelative(nameof(AnimSequence.m_duration));
                animations = serializedProperty.FindPropertyRelative(nameof(AnimSequence.m_animations));
                isExpanded = serializedProperty.FindPropertyRelative(nameof(AnimSequence.m_foldout));
                selectedAnimationIndex = serializedProperty.FindPropertyRelative(nameof(AnimSequence.m_selectedAnim));
                disableGameObjectAtEnd = serializedProperty.FindPropertyRelative(nameof(AnimSequence.m_disableAtEnd));
                selectedAnimFoldout = serializedProperty.FindPropertyRelative(nameof(AnimSequence.m_selectedAnimFoldout));
            }
        }

        private void CheckGuiContents()
        {
            if (NameContent == null)
            {
                NameContent = new GUIContent(Strings.Name);
                SequenceHeadingHelp = new GUIContent(Strings.SequenceHeadingHelp);
                DurationContent = new GUIContent(Strings.Duration);
                DelayContent = new GUIContent(Strings.Delay);
                EaseContent = new GUIContent(Strings.Ease);
                AnimatedComponentContent = new GUIContent(Strings.ComponentToAnimate);
                RepeatContent = new GUIContent(Strings.RepeatCount);
                IntervalContent = new GUIContent(Strings.RepeatInterval);
                RepeatTypeContent = new GUIContent(Strings.RepeatType);
                FromContent = new GUIContent(Strings.From);
                ToContent = new GUIContent(Strings.To);
                StartSoundContent = new GUIContent(Strings.OnStartSound);
                CompleteSoundContent = new GUIContent(Strings.OnCompleteSound);
                DisableAtEndContent = new GUIContent(Strings.DisableGameObjectAtEnd);
                RotationModeContent = new GUIContent(Strings.RotationMode);

                OnStartContent = new GUIContent(Strings.OnStartCallback);
                OnUpdateContent = new GUIContent(Strings.OnUpdateCallback);
                OnCompleteContent = new GUIContent(Strings.OnCompleteCallback);
                OnTerminateContent = new GUIContent(Strings.OnTerminateCallback);
            }
        }

        private void CheckAccordion()
        {
            if (accordion == null)
            {
                accordion = new SimpleAccordion();
                accordion.IsExpanded = isExpanded.boolValue;
            }

            accordion.Title = sequenceName.stringValue;
            accordion.expandStateChangeCallback = (acc) =>
            {
                isExpanded.boolValue = !isExpanded.boolValue;

            };
            accordion.drawHeaderCallback = DrawAccordionHeader;
            accordion.drawCallback = DrawAccordionBody;

        }

        private Rect DrawAccordionHeader()
        {
            var headerRect = SimpleAccordion.DrawDefaultAccordionHeader(accordion, 16);
            var playBtnRect = new Rect(headerRect.width - 35, headerRect.y, 16, 16);
            if (DrawerHelper.FAButton(playBtnRect, FA.play_circle))
            {
                onPreviewCallback(sequenceName.stringValue);
            }

            var stopBtnRect = new Rect(headerRect.width - 16, headerRect.y, 16, 16);
            if (DrawerHelper.FAButton(stopBtnRect, FA.stop_circle))
            {
                onStopPreviewCallback(sequenceName.stringValue);
            }
            var deleteBtnRect = new Rect(headerRect.width, headerRect.y, 16, 16);
            if (DrawerHelper.FAButton(deleteBtnRect, FA.remove))
            {
                onDeleteSequenceCallback(sequenceName.stringValue);
            }

            return headerRect;
        }

        private void DrawAccordionBody()
        {
            GUILayout.Space(5);

            DrawHeader();

            GUILayout.Space(5);

            ValidateAnimationTimes();

            DrawSelectedAnimation();

            GUILayout.Space(15);
            if (!string.IsNullOrEmpty(sequenceName.stringValue) && sequenceDuration.floatValue > 0)
            {
                DrawSaveAndLoadArea();
            }
        }

        public void Draw(bool showHelp)
        {

            this.showHelp = showHelp;

            FindSerializedProperties();
            lastAnimCount = animations.arraySize;
            animationsCount = animations.arraySize;

            CheckGuiContents();

            CheckAccordion();

            accordion.OnGUI();

            if (isDeletedAnim)
            {
                animations.DeleteArrayElementAtIndex(deletedIndex);
                isDeletedAnim = false;
            }
            if (isAddedNewAnim)
            {
                animations.arraySize++;
                animations.GetArrayElementAtIndex(animationsCount)
                          .FindPropertyRelative(nameof(Anim.m_animationType))
                          .enumValueIndex = (int)addedAnimType;

                isAddedNewAnim = false;
            }

            if (selectedAnimationIndex != null)
            {
                selectedAnimationIndex.intValue = newSelectedAnimationIndex;
            }

        }

        private void DrawHeader()
        {
            EditorGUILayout.PropertyField(sequenceName, NameContent);
            EditorUtils.ShowMessage(Strings.NameWarning, MessageType.Error, string.IsNullOrEmpty(sequenceName.stringValue));
            if (string.IsNullOrEmpty(sequenceName.stringValue))
            {
                return;
            }

            sequenceDuration.floatValue = EditorGUILayout.DelayedFloatField(DurationContent, sequenceDuration.floatValue);
            sequenceDuration.floatValue = Mathf.Max(0, sequenceDuration.floatValue);
            EditorUtils.ShowMessage(Strings.DurationWarning, MessageType.Error, Math.Abs(sequenceDuration.floatValue) < double.Epsilon);
            if (Math.Abs(sequenceDuration.floatValue) < double.Epsilon)
            {
                selectedAnimationIndex = null;
                return;
            }

            EditorUtils.ShowMessage(Strings.SequenceHeadingHelp, MessageType.Info, showHelp);
            EditorGUILayout.PropertyField(disableGameObjectAtEnd, DisableAtEndContent);
            EditorUtils.ShowMessage(Strings.DisableGameObjectAtEndHelp, MessageType.Info, showHelp);
            var timelineFoldount = serializedProperty.FindPropertyRelative(nameof(AnimSequence.m_timelineFoldout));
            if (timelineAccordion == null)
            {
                timelineAccordion = new SimpleAccordion();
                timelineAccordion.IsExpanded = timelineFoldount.boolValue;
            }
            timelineAccordion.expandStateChangeCallback = (acc) => timelineFoldount.boolValue = acc.IsExpanded;
            if (animationsCount == 0)
            {
                if (timelineAccordion.IsExpanded)
                {
                    timelineAccordion.IsExpanded = false;
                }
            }
            timelineAccordion.Title = Strings.Timeline;
            timelineAccordion.drawHeaderCallback = DrawTimelineHeader;
            timelineAccordion.drawCallback = DrawTimeline;
            timelineAccordion.OnGUI();
        }

        private Rect DrawTimelineHeader()
        {
            var rect = SimpleAccordion.DrawDefaultAccordionHeader(timelineAccordion, 14, 10);
            if (DrawerHelper.FAButton(new Rect(rect.width, rect.y, 32, rect.height), FA.plus))
            {
                var genericMenu = new GenericMenu();
                foreach (var type in AnimationTypesEnumValues)
                {
                    genericMenu.AddItem(new GUIContent(type.ToString()), false, () =>
                    {
                        isAddedNewAnim = true;
                        addedAnimType = type;
                    });
                }
                genericMenu.ShowAsContext();
            }
            return rect;
        }

        private void DrawTimeline()
        {
            selectedAnimationIndex = serializedProperty.FindPropertyRelative(nameof(AnimSequence.m_selectedAnim));

            newSelectedAnimationIndex = selectedAnimationIndex.intValue;

            if (timelineRangeStyle == null)
            {
                timelineRangeStyle = new GUIStyle();
                timelineRangeStyle.fontSize = 8;
                timelineRangeStyle.alignment = TextAnchor.MiddleCenter;
            }

            var timeLineRect = GUILayoutUtility.GetRect(0, 0, GUILayout.ExpandWidth(true), GUILayout.Height(28 * animationsCount));
            GUI.Box(timeLineRect, GUIContent.none);

            AnimationTypes animationType;
            SerializedProperty currentAnimTime;
            SerializedProperty currentAnim;
            SerializedProperty currentAnimMinTime;
            SerializedProperty currentAnimMaxTime;
            Rect selectedAnimRect;
            Rect animRect;

            var lastBackgroundColor = GUI.backgroundColor;
            var totalWidth = timeLineRect.width;
            var duration = sequenceDuration.floatValue;
            var currentAnimProgress = 0f;
            var isValidAnim = false;
            var animTimelineText = string.Empty;
            var x = 0.0f;
            var y = timeLineRect.y;
            var w = 0.0f;
            var h = 20.0f;

            for (int i = 0; i < animationsCount; i++)
            {
                isValidAnim = onValidateAnimCallback.Invoke(sequenceName.stringValue, i);
                currentAnim = animations.GetArrayElementAtIndex(i);
                currentAnimTime = currentAnim.FindPropertyRelative(nameof(Anim.time));
                currentAnimMinTime = currentAnimTime.FindPropertyRelative("min");
                currentAnimMaxTime = currentAnimTime.FindPropertyRelative("max");

                if (Math.Abs(currentAnimMinTime.floatValue - currentAnimMaxTime.floatValue) < double.Epsilon && Math.Abs(currentAnimMaxTime.floatValue) < double.Epsilon)
                {
                    currentAnimMaxTime.floatValue = duration;
                }

                x = ((currentAnimMinTime.floatValue * totalWidth) / duration) + timeLineRect.x + 2;
                w = ((currentAnimMaxTime.floatValue * totalWidth) / duration) - x + timeLineRect.x;

                animationType = (AnimationTypes)currentAnim.FindPropertyRelative(nameof(Anim.m_animationType)).enumValueIndex;
                currentAnimProgress = currentAnim.FindPropertyRelative(nameof(Anim.m_completedPercent)).floatValue;

                animRect = new Rect(x, y, w, h);
                EditorGUI.ProgressBar(animRect, currentAnimProgress <= 0 ? 0 : currentAnimProgress, string.Empty);

                GUI.Label(animRect, GetAnimationTimelineTitle(animationType));
                animRect.y += animRect.height;
                animRect.height = 8;

                if (isValidAnim)
                {
                    animTimelineText = string.Format("{0} > {1}", currentAnimMinTime.floatValue.ToString("0.##"), currentAnimMaxTime.floatValue.ToString("0.##"));
                    GUI.Box(animRect, animTimelineText, timelineRangeStyle);
                }
                else
                {
                    DrawerHelper.FAIcon(animRect, FA.info, FAOption.TextColor(Color.red), FAOption.FontSize(8));
                }

                if (DrawerHelper.FAButton(new Rect(timeLineRect.width + 12, y, 8, h), FA.remove, FAOption.TextColor(Color.red), FAOption.FontSize(8)))
                {
                    isDeletedAnim = true;
                    deletedIndex = i;
                    continue;
                }

                selectedAnimRect = new Rect(timeLineRect.x, y, timeLineRect.width, h);
                if (selectedAnimationIndex.intValue == i)
                {
                    DrawerHelper.FAIcon(new Rect(timeLineRect.x - 12, y, 8, h), FA.arrow_right, FAOption.FontSize(8));

                }
                if (IsClickedIn(selectedAnimRect))
                {
                    newSelectedAnimationIndex = i;
                }

                y += h + 8;
            }
        }

        private void ValidateAnimationTimes()
        {
            var sequenceTime = sequenceDuration.floatValue;

            var newStartsTime = 0f;
            var newEndsTime = 0f;

            SerializedProperty animTime;
            SerializedProperty animStartsTime;
            SerializedProperty animEndsTime;
            SerializedProperty anim;
            for (int i = 0; i < animationsCount; i++)
            {
                anim = animations.GetArrayElementAtIndex(i);
                onValidateAnimCallback.Invoke(sequenceName.stringValue, i);
                animTime = anim.FindPropertyRelative(nameof(Anim.time));
                animStartsTime = animTime.FindPropertyRelative("min");
                animEndsTime = animTime.FindPropertyRelative("max");
                newStartsTime = animStartsTime.floatValue;
                newEndsTime = animEndsTime.floatValue;
                if (Math.Abs(newStartsTime - newEndsTime) < double.Epsilon && Math.Abs(newEndsTime) < double.Epsilon)
                {
                    newEndsTime = sequenceTime;
                    animEndsTime.floatValue = newEndsTime;
                }
                if (sequenceTime < newEndsTime)
                {
                    newEndsTime = sequenceTime;
                    animEndsTime.floatValue = newEndsTime;
                }
            }
        }
        private void DrawSelectedAnimation()
        {
            if (selectedAnimationIndex == null)
            {
                return;
            }

            if (selectedAnimationIndex.intValue >= animationsCount)
            {
                selectedAnimationIndex.intValue = 0;
            }

            if (selectedAnimAccordion == null)
            {
                selectedAnimAccordion = new SimpleAccordion();
                selectedAnimAccordion.Title = Strings.SelectedAnim;
            }
            selectedAnimAccordion.drawHeaderCallback = () => SimpleAccordion.DrawDefaultAccordionHeader(selectedAnimAccordion, 14, 8);
            if (animationsCount == 0)
            {
                if (selectedAnimAccordion.IsExpanded)
                {
                    selectedAnimAccordion.IsExpanded = false;
                    selectedAnimFoldout.boolValue = false;
                }
            }
            else
            {
                if (serializedProperty.isAnimated != selectedAnimFoldout.boolValue)
                {
                    selectedAnimAccordion.IsExpanded = selectedAnimFoldout.boolValue;
                }
            }
            selectedAnimAccordion.expandStateChangeCallback = acc => selectedAnimFoldout.boolValue = acc.IsExpanded;

            selectedAnimAccordion.drawCallback = () =>
            {
                if (animationsCount == 0)
                {
                    return;
                }
                var anim = animations.GetArrayElementAtIndex(selectedAnimationIndex.intValue);

                DrawAnimation(anim);
            };

            selectedAnimAccordion.OnGUI();
        }

        private void DrawAnimation(SerializedProperty property)
        {
            var animationType = (AnimationTypes)property.FindPropertyRelative(nameof(Anim.m_animationType)).enumValueIndex;
            var easeType = property.FindPropertyRelative(nameof(Anim.m_ease));
            var motion = property.FindPropertyRelative(nameof(Anim.m_motion));

            var onStartSound = property.FindPropertyRelative(nameof(Anim.m_onStartSound));
            var onCompleteSound = property.FindPropertyRelative(nameof(Anim.m_onCompleteSound));

            GUILayout.Space(5);

            // Transform Field
            var animTransformReference = property.FindPropertyRelative(nameof(Anim.transformReferenceValue));
            EditorGUILayout.PropertyField(animTransformReference, new GUIContent("Transform"));
            var transform = animTransformReference.objectReferenceValue as Transform;
            if (transform == null)
            {
                EditorGUILayout.HelpBox(Strings.AnimMissingTransform, MessageType.Error);
                return;
            }

            GUILayout.Space(5);

            // Animated Component Field
            var animatableComponent = property.FindPropertyRelative(nameof(Anim.m_animatableComponent));
            EditorGUILayout.PropertyField(animatableComponent, AnimatedComponentContent);
            EditorUtils.ShowMessage(Strings.AnimatedComponentHelp, MessageType.Info, showHelp);

            SerializedProperty animatedValue = null;
            switch (animationType)
            {
                case AnimationTypes.Translation:
                case AnimationTypes.Rotation:
                case AnimationTypes.Scale:
                    animatableComponent.enumValueIndex = (int)AnimatableComponents.Transform;
                    animatedValue = property.FindPropertyRelative(nameof(Anim.m_vector3Value));
                    break;

                case AnimationTypes.Fade:
                    animatedValue = property.FindPropertyRelative(nameof(Anim.m_floatValue));
                    var fadableType = (AnimatableComponents)animatableComponent.enumValueIndex;
                    if (!IsFadableType(fadableType) || !HasComponent(transform, fadableType))
                    {
                        fadableType = FindFisrtFadableComponent(transform);
                    }

                    animatableComponent.enumValueIndex = (int)fadableType;
                    EditorUtils.ShowMessage(Strings.AnimMissingFadable, MessageType.Error, fadableType == AnimatableComponents.None);
                    EditorUtils.ShowMessage(string.Format(Strings.FadeWarning, fadableComponentCount), MessageType.Warning, fadableComponentCount > 1);

                    break;
                case AnimationTypes.Color:
                    animatedValue = property.FindPropertyRelative(nameof(Anim.m_colorValue));
                    var colorableType = (AnimatableComponents)animatableComponent.enumValueIndex;
                    if (!IsColorableType(colorableType) || !HasComponent(transform, colorableType))
                    {
                        colorableType = FindFistColorableComponent(transform);
                    }

                    animatableComponent.enumValueIndex = (int)colorableType;
                    EditorUtils.ShowMessage(Strings.MissingColorable, MessageType.Error, colorableType == AnimatableComponents.None);
                    EditorUtils.ShowMessage(string.Format(Strings.ColorWarning, fadableComponentCount), MessageType.Warning, colorableComponentCount > 1);

                    if (colorableType == AnimatableComponents.MeshRenderer && !onValidateAnimCallback(sequenceName.stringValue, selectedAnimationIndex.intValue))
                    {
                        EditorGUILayout.HelpBox(Strings.MeshRendererMissingMaterial, MessageType.Error);
                    }
                    break;
            }

            // Time Field
            var time = property.FindPropertyRelative(nameof(Anim.time));
            var startsTime = time.FindPropertyRelative("min");
            var endsTime = time.FindPropertyRelative("max");
            var newStartsTime = startsTime.floatValue;
            var newEndsTime = endsTime.floatValue;
            var maxTime = sequenceDuration.floatValue;
            var position = GUILayoutUtility.GetRect(0, 40, GUILayout.ExpandWidth(true));
            position.width -= 10;

            if (maxTime > 0)
            {
                var xDivision = position.width * 0.33f;
                var yDivision = position.height * 0.5f;
                EditorGUI.LabelField(new Rect(position.x + 5, position.y, xDivision, yDivision), Strings.Time);

                EditorGUI.LabelField(new Rect(position.x + 7, position.y + yDivision, position.width, yDivision), "0");
                EditorGUI.LabelField(new Rect(position.x + position.width - 20f, position.y + yDivision, position.width, yDivision), maxTime.ToString("0.##"));

                EditorGUI.MinMaxSlider(new Rect(position.x + 24f, position.y + yDivision, position.width - 48f, yDivision), ref newStartsTime, ref newEndsTime, 0, maxTime);

                EditorGUI.LabelField(new Rect(position.x + xDivision - 15, position.y, xDivision, yDivision), "From : ");

                newStartsTime = Mathf.Clamp(EditorGUI.FloatField(new Rect(position.x + xDivision + 30, position.y, xDivision - 30, yDivision), newStartsTime), 0, newEndsTime);
                EditorGUI.LabelField(new Rect(position.x + xDivision * 2f, position.y, xDivision, yDivision), "To : ");
                newEndsTime = Mathf.Clamp(EditorGUI.FloatField(new Rect(position.x + xDivision * 2f + 24, position.y, xDivision - 24, yDivision), newEndsTime), newStartsTime, maxTime);
            }

            startsTime.floatValue = newStartsTime;
            endsTime.floatValue = newEndsTime;
            EditorUtils.ShowMessage(Strings.TimeHelp, MessageType.Info, showHelp);

            GUILayout.Space(5);

            // From & To values Field
            var startsVal = animatedValue.FindPropertyRelative("starts");
            var endsVal = animatedValue.FindPropertyRelative("ends");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(startsVal, FromContent);

            if (GUILayout.Button(Strings.Current, GUILayout.Width(60)))
            {
                onClickAutoSetFromValueCallback(sequenceName.stringValue, selectedAnimationIndex.intValue);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(endsVal, ToContent);
            if (GUILayout.Button(Strings.Current, GUILayout.Width(60)))
            {
                onClickAutoSetToValueCallback(sequenceName.stringValue, selectedAnimationIndex.intValue);
            }
            EditorGUILayout.EndHorizontal();
            EditorUtils.ShowMessage(Strings.AnimValueHelp, MessageType.Info, showHelp);

            GUILayout.Space(5);

            //EaseType Field
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(easeType, EaseContent);
            if (((EaseTypes)easeType.enumValueIndex) == EaseTypes.Custom)
            {
                EditorGUILayout.PropertyField(motion, GUIContent.none);
            }
            EditorGUILayout.EndHorizontal();

            EditorUtils.ShowMessage(Strings.AnimEaseHelp, MessageType.Info, showHelp);

            //Rotation Mode Field
            if (animationType == AnimationTypes.Rotation)
            {
                var rotationMode = property.FindPropertyRelative(nameof(Anim.m_rotationMode));
                EditorGUILayout.PropertyField(rotationMode, RotationModeContent);
                EditorUtils.ShowMessage(Strings.RotationModeHelp, MessageType.Info, showHelp);
            }


            var repeat = property.FindPropertyRelative(nameof(Anim.m_repeat));
            var interval = property.FindPropertyRelative(nameof(Anim.m_repeatInterval));
            var loopType = property.FindPropertyRelative(nameof(Anim.m_loopType));

            EditorGUILayout.PropertyField(loopType, RepeatTypeContent);
            EditorGUILayout.PropertyField(repeat, RepeatContent);
            EditorGUILayout.PropertyField(interval, IntervalContent);

            EditorUtils.ShowMessage(Strings.AnimLoopHelp, MessageType.Info, showHelp);

            repeat.intValue = Mathf.Clamp(repeat.intValue, -1, repeat.intValue);
            interval.floatValue = Mathf.Clamp(interval.floatValue, 0.0f, interval.floatValue);

            GUILayout.Space(5);

            var onStart = property.FindPropertyRelative(nameof(Anim.m_onStartCallback));
            var onUpdate = property.FindPropertyRelative(nameof(Anim.m_onUpdateCallback));
            var onComplete = property.FindPropertyRelative(nameof(Anim.m_onCompleteCallback));
            var onTerminate = property.FindPropertyRelative(nameof(Anim.m_onTerminateCallback));
            EditorGUILayout.PropertyField(onStart, OnStartContent);
            EditorGUILayout.PropertyField(onUpdate, OnUpdateContent);
            EditorGUILayout.PropertyField(onComplete, OnCompleteContent);
            EditorGUILayout.PropertyField(onTerminate, OnTerminateContent);
            EditorUtils.ShowMessage(Strings.CallbackHelp, MessageType.Info, showHelp);

            GUILayout.Space(5);
            EditorGUILayout.PropertyField(onStartSound, StartSoundContent);
            EditorGUILayout.PropertyField(onCompleteSound, CompleteSoundContent);

            GUILayout.Space(5);
            EditorUtils.ShowMessage(Strings.SoundHelp, MessageType.Info, showHelp);
        }

        private void DrawSaveAndLoadArea()
        {
            var isOnLoad = serializedProperty.FindPropertyRelative(nameof(AnimSequence.m_isOnLoad));

            EditorGUILayout.BeginHorizontal();
            if (!isOnLoad.boolValue && DrawerHelper.FAButton(FA.save))
            {
                var path = EditorUtility.SaveFilePanel(Strings.Save, "Assets/", "", AnimSequence.FileExtension);
                onSaveCallback.Invoke(sequenceName.stringValue, path);
            }

            GUILayout.Space(10);

            if (!isOnLoad.boolValue && DrawerHelper.FAButton(FA.refresh))
            {
                isOnLoad.boolValue = true;
            }
            EditorGUILayout.EndHorizontal();

            if (isOnLoad.boolValue)
            {
                EditorGUILayout.BeginHorizontal();

                var path = serializedProperty.FindPropertyRelative(nameof(AnimSequence.m_draggedFilePath));
                var draggedObject = serializedProperty.FindPropertyRelative(nameof(AnimSequence.m_draggedObject));
                var hasError = !IsValidPath(path.stringValue);
                if (hasError)
                {
                    path.stringValue = null;
                    draggedObject.objectReferenceValue = null;
                }
                if (EditorUtils.Drop(200, 20, hasError ? Strings.DragAndDrop : Path.GetFileNameWithoutExtension(path.stringValue), Color.cyan))
                {
                    hasError = !IsValidPath(DragAndDrop.paths[0]);
                    if (!hasError)
                    {
                        path.stringValue = DragAndDrop.paths[0];
                        draggedObject.objectReferenceValue = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path.stringValue);
                    }
                }
                if (Event.current.type == EventType.MouseDown && GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
                {
                    EditorGUIUtility.PingObject(draggedObject.objectReferenceValue);
                }
                if (GUILayout.Button(Strings.Confirm))
                {
                    if (!hasError)
                    {
                        onLoadCallback.Invoke(sequenceName.stringValue, path.stringValue);
                    }
                    else
                    {
                        Debug.Log("There is no .isiseq file!");
                    }
                }

                if (GUILayout.Button(Strings.Cancel))
                {
                    isOnLoad.boolValue = false;
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        private bool IsValidPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }
            if (!File.Exists(path))
            {
                return false;
            }
            if (Path.GetExtension(path).Replace(".", "") != AnimSequence.FileExtension)
            {
                return false;
            }
            var fileName = Path.GetFileNameWithoutExtension(path);

            return CodeGenerationUtils.IsIdentifier(fileName);
        }

        private bool IsClickedIn(Rect r)
        {
            var e = Event.current;
            return (e.type == EventType.MouseDown && r.Contains(e.mousePosition));
        }

        private bool HasComponent(Transform transform, AnimatableComponents type)
        {
            switch (type)
            {
                case AnimatableComponents.Camera:
                    return transform.GetComponent<Camera>() != null;
                case AnimatableComponents.Transform:
                    return transform != null;
                case AnimatableComponents.RectTransform:
                    return transform.GetComponent<RectTransform>() != null;
                case AnimatableComponents.CanvasGroup:
                    return transform.GetComponent<CanvasGroup>() != null;
                case AnimatableComponents.Text:
                    return transform.GetComponent<Text>() != null;
                case AnimatableComponents.Image:
                    return transform.GetComponent<Image>() != null;
                case AnimatableComponents.SpriteRenderer:
                    return transform.GetComponent<SpriteRenderer>() != null;
                case AnimatableComponents.MeshRenderer:
                    return transform.GetComponent<MeshRenderer>() != null;
                default:
                    return false;
            }

        }

        private string GetAnimationTimelineTitle(AnimationTypes type)
        {
            switch (type)
            {
                case AnimationTypes.Translation:
                    return "T";
                case AnimationTypes.Rotation:
                    return "R";
                case AnimationTypes.Scale:
                    return "S";
                case AnimationTypes.Fade:
                    return "F";
                case AnimationTypes.Color:
                    return "C";
                default:
                    return type.ToString();
            }
        }

        /// <summary>
        /// Finds the first 'fadable' component in the GameObject of the transform which is attached to the given Transform. 
        /// If there is more than one fadable component, the component with the highest priority is returned and if there is 0 component,
        /// this function return <see cref="AnimatableComponents.None"/> <br/>
        /// The priority is <br/>
        ///    - CanvasGroup<br/>
        ///    - SpriteRenderer<br/>
        ///    - Image<br/>
        ///    - Text<br/>
        /// </summary>
        /// <param name="transform">The Transform linked to the sequence</param>
        /// <returns>First fadable component of the Transform</returns>
        private AnimatableComponents FindFisrtFadableComponent(Transform transform)
        {
            var result = AnimatableComponents.None;
            fadableComponentCount = 0;

            if (transform.GetComponent<Text>() != null)
            {
                result = AnimatableComponents.Text;
                fadableComponentCount++;
            }

            if (transform.GetComponent<Image>() != null)
            {
                result = AnimatableComponents.Image;
                fadableComponentCount++;
            }

            if (transform.GetComponent<SpriteRenderer>() != null)
            {
                result = AnimatableComponents.SpriteRenderer;
                fadableComponentCount++;
            }

            if (transform.GetComponent<CanvasGroup>() != null)
            {
                result = AnimatableComponents.CanvasGroup;
                fadableComponentCount++;
            }
            return result;
        }

        /// <summary>
        /// Finds the first 'Colorable' component in the GameObject of the transform which is attached to the given Transform. 
        /// If there is more than one colorable component, the component with the highest priority is returned and if there is 0 component,
        /// this function return <see cref="AnimatableComponents.None"/> <br/>
        /// The priority is <br/>
        ///    - Camera<br/>
        ///    - MeshRenderer<br/>
        ///    - SpriteRenderer<br/>
        ///    - Image<br/>
        ///    - Text<br/>
        /// </summary>
        /// <param name="transform">The Transform linked to the sequence</param>
        /// <returns>First colorable component of the Transform</returns>
        private AnimatableComponents FindFistColorableComponent(Transform transform)
        {
            var result = AnimatableComponents.None;
            colorableComponentCount = 0;
            if (transform.GetComponent<Text>() != null)
            {
                result = AnimatableComponents.Text;
                colorableComponentCount++;
            }
            if (transform.GetComponent<Image>() != null)
            {
                result = AnimatableComponents.Image;
                colorableComponentCount++;
            }
            if (transform.GetComponent<SpriteRenderer>() != null)
            {
                result = AnimatableComponents.SpriteRenderer;
                colorableComponentCount++;
            }
            if (transform.GetComponent<MeshRenderer>() != null)
            {
                result = AnimatableComponents.MeshRenderer;
                colorableComponentCount++;
            }
            if (transform.GetComponent<Camera>() != null)
            {
                result = AnimatableComponents.Camera;
                colorableComponentCount++;
            }
            return result;
        }

        private bool IsFadableType(AnimatableComponents type)
        {
            switch (type)
            {
                case AnimatableComponents.CanvasGroup:
                case AnimatableComponents.Text:
                case AnimatableComponents.Image:
                case AnimatableComponents.SpriteRenderer:
                    return true;
                default:
                    return false;
            }
        }

        private bool IsColorableType(AnimatableComponents type)
        {
            switch (type)
            {
                case AnimatableComponents.MeshRenderer:
                case AnimatableComponents.Text:
                case AnimatableComponents.Image:
                case AnimatableComponents.SpriteRenderer:
                case AnimatableComponents.Camera:
                    return true;
                default:
                    return false;
            }
        }
    }

}