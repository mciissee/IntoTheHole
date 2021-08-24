/************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS								                                                        *
*************************************************************************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using InfinityEngine.Interpolations;
using InfinityEngine.Utils;

namespace InfinityEditor
{

    /// <summary>
    /// Editor script of <see cref="Sequencer"/> script
    /// </summary>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Sequencer))]
    public partial class SequencerInspector : Editor
    {

        #region Fields
        private const string HelpUrl = "https://infinity-engine-f6f33.firebaseapp.com/#ISI-Interpolation";
        private static GUIContent DisableOnHideContent;
        private static GUIContent DisableOnPauseContent;
        private static GUIContent SetAsContent;
        private static GUIContent PlayOnStartContent;
        private static GUIContent SequenceToPlayContent;

        private Sequencer script;
        private SerializedProperty sequences;
        private List<SequenceDrawer> drawers;
        private SerializedProperty disableOnHide;
        private SerializedProperty disableOnPause;
        private SerializedProperty setAsOther;
        private SerializedProperty playOnStart;
        private SerializedProperty sequenceToPlay;
        private SerializedProperty optionsFoldout;
        private SimpleAccordion headerAccordion;

        private bool showHelp;
        private bool isInitialized;
        #endregion Fields

        #region Methods

        private void OnEnable()
        {
            script = (Sequencer)target;

            CheckState();

            EditorApplication.playModeStateChanged += PlayModeChanged;
            EditorApplication.update += Repaint;
        }

        private void OnDisable()
        {
            EditorApplication.update -= Repaint;
            EditorApplication.playModeStateChanged -= PlayModeChanged;
        }

        private void OnDestroy()
        {
            if (!Application.isPlaying)
            {
                script.StopAllSequences();
            }
        }

        public override void OnInspectorGUI()
        {
            if (!CanMultiEdit())
            {
                EditorGUILayout.HelpBox("The Selected Sequencer Components Cannot Be Multi Edited !", MessageType.Error);
                return;
            }
            serializedObject.Update();
            ApplyToAllTargets(sequencer => sequencer.ValidateSequenceTransforms());
            CheckState();

            showHelp = EditorUtils.DrawHeader(showHelp, HelpUrl);

            EditorUtils.ShowMessage(Strings.SequencerHelp, MessageType.Info, showHelp);


            headerAccordion.OnGUI();

            GUILayout.Space(10);

            var drawersCount = drawers.Count;

            for (int i = 0; i < drawersCount; i++)
            {
                if (sequences == null)
                {
                    isInitialized = false;
                    CheckState();
                }
                drawers[i].serializedProperty = sequences.GetArrayElementAtIndex(i);
                drawers[i].Draw(showHelp);
                GUILayout.Space(10);
            }

            var centeredRect = EditorUtils.GetCenteredRect(80, 30);
            if (DrawerHelper.FAButton(centeredRect, FA.plus_circle, FAOption.FontSize(24)))
            {
                sequences.arraySize++;
                var newSequence = sequences.GetArrayElementAtIndex(sequences.arraySize - 1);
                newSequence.FindPropertyRelative(nameof(AnimSequence.m_transform)).objectReferenceValue = script.transform;
                newSequence.FindPropertyRelative(nameof(AnimSequence.m_duration)).floatValue = 1.0f;
                drawers.Add(new SequenceDrawer(newSequence));

            }
            SaveSerializedObject();
        }

        private void CheckState()
        {
            if (!isInitialized)
            {

                DisableOnHideContent = new GUIContent(Strings.DisableOnHide);
                DisableOnPauseContent = new GUIContent(Strings.DisableOnPause);
                PlayOnStartContent = new GUIContent(Strings.PlayOnStart);
                SequenceToPlayContent = new GUIContent(Strings.SequenceToPlay);
                SetAsContent = new GUIContent(Strings.SetAsOther);

                optionsFoldout = serializedObject.FindProperty(nameof(Sequencer.m_optionsFoldout));
                disableOnHide = serializedObject.FindProperty(nameof(Sequencer.m_disabledOnHide));
                disableOnPause = serializedObject.FindProperty(nameof(Sequencer.m_disabledOnPause));
                setAsOther = serializedObject.FindProperty(nameof(Sequencer.m_copiedSequence));
                playOnStart = serializedObject.FindProperty(nameof(Sequencer.m_playOnStart));
                sequenceToPlay = serializedObject.FindProperty(nameof(Sequencer.m_sequenceToPlay));
                sequences = serializedObject.FindProperty(nameof(Sequencer.m_sequences));

                drawers = new List<SequenceDrawer>();
                SerializedProperty property;
                for (int i = 0; i < sequences.arraySize; i++)
                {
                    property = sequences.GetArrayElementAtIndex(i);
                    drawers.Add(new SequenceDrawer(property));
                }

                headerAccordion = new SimpleAccordion(Strings.Options, DrawOptions);
                headerAccordion.IsExpanded = optionsFoldout.boolValue;


                headerAccordion.drawHeaderCallback = () => SimpleAccordion.DrawDefaultAccordionHeader(headerAccordion, FA.gear);
                headerAccordion.expandStateChangeCallback = acc => optionsFoldout.boolValue = acc.IsExpanded;
                headerAccordion.drawCallback = DrawOptions;

                isInitialized = true;
            }

            foreach (var drawer in drawers)
            {
                drawer.onDeleteSequenceCallback = OnDeleteSequence;
                drawer.onSaveCallback = OnSave;
                drawer.onLoadCallback = OnLoad;
                drawer.onPreviewCallback = OnPreview;
                drawer.onStopPreviewCallback = OnStopPreview;
                drawer.onValidateAnimCallback = OnValidateAnim;
                drawer.onClickAutoSetFromValueCallback = OnClickAutoSetFromValue;
                drawer.onClickAutoSetToValueCallback = OnClickAutoSetToValue;
            }
        }

        private void Reset()
        {
            drawers = null;
            sequences = null;
            isInitialized = false;
            SaveSerializedObject();
            serializedObject.Update();
            CheckState();
        }

        private void DrawOptions()
        {
            GUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(setAsOther, SetAsContent);
            if (setAsOther.objectReferenceValue != null)
            {
                if (GUILayout.Button(Strings.Confirm))
                {
                    ApplyToAllTargets(sequencer =>
                    {
                        sequencer.SetAsCopied();
                    });

                    Reset();
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorUtils.ShowMessage(Strings.SetAsOtherHelp, MessageType.Info, showHelp);

            EditorGUILayout.PropertyField(disableOnHide, DisableOnHideContent);
            EditorUtils.ShowMessage(Strings.DisableOnHideHelp, MessageType.Info, showHelp);
            EditorGUILayout.PropertyField(disableOnPause, DisableOnPauseContent);
            EditorUtils.ShowMessage(Strings.DisableOnPauseHelp, MessageType.Info, showHelp);

            EditorGUILayout.PropertyField(playOnStart, PlayOnStartContent);
            if (playOnStart.boolValue)
            {
                EditorGUILayout.PropertyField(sequenceToPlay, SequenceToPlayContent);
                EditorUtils.ShowMessage(Strings.SequenceToPlayHelp, MessageType.Info, showHelp);

            }
        }

        private void OnDeleteSequence(string sequenceName)
        {
            ApplyToAllTargets(sequencer => sequencer.DeleteSequenceWithName(sequenceName));

            for (int i = 0; i < drawers.Count; i++)
            {
                if (drawers[i].sequenceName.stringValue == sequenceName)
                {
                    drawers.RemoveAt(i);
                    return;
                }
            }
        }

        private void OnSave(string sequenceName, string path)
        {
            path = path.Replace(Application.dataPath, "Assets");
            script.SaveSequenceAtPath(sequenceName, path);
            AssetDatabase.Refresh();
        }

        private void OnLoad(string sequenceName, string path)
        {
            path = path.Replace(Application.dataPath, string.Empty);
            ApplyToAllTargets(sequencer => sequencer.LoadSequenceAtPathIntoSequence(sequenceName, path));
        }

        private void OnPreview(string sequenceName)
        {
            script.PlaySequenceWithName(sequenceName);
        }

        private void OnStopPreview(string sequenceName)
        {
            script.StopSequenceWithName(sequenceName);
        }

        private bool OnValidateAnim(string sequenceName, int index)
        {
            return script.IsValidAnimAtIndexInSequence(sequenceName, index);
        }

        private void OnClickAutoSetFromValue(string sequenceName, int index)
        {
            ApplyToAllTargets(sequencer => sequencer.AutoSetFromValueOfAnimAtIndexInSequence(sequenceName, index));
        }

        private void OnClickAutoSetToValue(string sequenceName, int index)
        {
            ApplyToAllTargets(sequencer => sequencer.AutoSetToValueOfAnimAtIndexInSequence(sequenceName, index));
        }

        private void SaveSerializedObject()
        {
            if (GUI.changed)
            {
                var len = targets.Length;
                for (var i = 0; i < len; i++)
                {
                    EditorUtility.SetDirty(targets[i]);
                }
            }
            serializedObject.ApplyModifiedProperties();
        }

        private void ApplyToAllTargets(Action<Sequencer> action)
        {
            var len = targets.Length;
            for (var i = 0; i < len; i++)
            {
                action.Invoke(targets[i] as Sequencer);
            }
        }

        private bool CanMultiEdit()
        {
            Sequencer seq;
            var sequenceCount = script.Sequences.Count;
            foreach (var item in targets)
            {
                if (item == target)
                {
                    continue;
                }
                seq = item as Sequencer;
                if (!Sequencer.CanMultiEdit(seq, script))
                {
                    return false;
                }
            }
            return true;
        }

        private void PlayModeChanged(PlayModeStateChange stateChange)
        {
            script.StopAllSequences();
        }

        #endregion Methods

    }

}