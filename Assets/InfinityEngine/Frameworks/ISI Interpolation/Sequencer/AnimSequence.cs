#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.

/************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS                                                                                      *
*************************************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using UnityEngine;

[assembly: InternalsVisibleTo("Assembly-CSharp-Editor")]
namespace InfinityEngine.Interpolations
{


    /// <summary>
    /// Represents a sequence of animations.
    /// </summary>
    [Serializable]
    public class AnimSequence
    {

        #region Fields


        public const string FileExtension = "isiseq";

        #region Used In Inspector Script

        [SerializeField] internal bool m_foldout;
        [SerializeField] internal bool m_timelineFoldout;
        [SerializeField] internal bool m_selectedAnimFoldout;
        [SerializeField] internal int m_selectedAnim;
        [SerializeField] internal bool m_isOnLoad;
        [SerializeField] internal string m_draggedFilePath;
        [SerializeField] internal UnityEngine.Object m_draggedObject;


        #endregion Used In Inspector Script
        [SerializeField] internal string m_name;
        [SerializeField] internal float m_duration;
        [SerializeField] internal bool m_isStopped;
        [SerializeField] internal bool m_disableAtEnd = true;

        [SerializeField] internal Transform m_transform;
        [SerializeField] internal List<Anim> m_animations;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets the name of the sequence
        /// </summary>
        public string Name => m_name;

        /// <summary>
        /// Gets or sets the animations of the sequence
        /// </summary>
        internal List<Anim> Animations { get => m_animations; set => m_animations = value; }

        /// <summary>
        /// The Transform linked to the animation sequence
        /// </summary>
        public Transform Transform { get => m_transform; set => m_transform = value; }

        /// <summary>
        /// Gets the duration of the sequence
        /// </summary>
        public float Duration => m_duration;

        /// <summary>
        /// Gets a value indicating whether is animation is playing.
        /// </summary>
        public bool IsPlaying => m_animations.Any(anim => anim != null && anim.IsPlaying);

        #endregion Properties

        internal AnimSequence() => m_animations = m_animations ?? new List<Anim>();


        /// <summary>
        /// Creates new AnimSequence and link it to the given Transform
        /// </summary>
        /// <param name="transform">The Transform linked to this Animation</param>
        internal AnimSequence(Transform transform)
        {
            m_transform = transform;
            m_animations = new List<Anim>();
        }

        /// <summary>
        /// Adds new animation of the given type if it not added.
        /// </summary>
        /// <param name="type">Animation type</param>
        internal void AddAnimation(AnimationTypes type)
        {
            m_animations = m_animations ?? new List<Anim>();
            var newAnim = new Anim(type, Transform);
            newAnim.time = new MinMax() { min = 0, max = Duration };
            m_animations.Add(newAnim);
        }

        /// <summary>
        /// Removes the animation at the given index.
        /// </summary>
        /// <param name="index">The index of the animation</param>
        internal void RemoveAnimationAt(int index) => Animations.RemoveAt(index);


        /// <summary>
        /// Checks if there is an animation of the given type
        /// </summary>
        /// <param name="type">The type of the animation</param>
        /// <returns>return <c>true</c> if the given animation is added <c>false</c> otherwise</returns>
        internal bool HasAnimation(AnimationTypes type) => Animations.Any(anim => anim.AnimationType == type);

        /// <summary>
        /// Checks if the animation at the given index in the sequence is valid
        /// </summary>
        /// <param name="index">The index of the animation</param>
        /// <returns>A value indicating whether the animation is valid or not</returns>
        internal bool IsValidAnimationAt(int index)
        {
            try
            {
                if (Animations[index].transformReferenceValue == null)
                {
                    Animations[index].transformReferenceValue = Transform;
                }
                return Animations[index].IsValid();
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Starts the available and valid animations of the sequence.
        /// </summary>
        /// <param name="disableOnHide">Disable this sequence when this GameObject linked to it is hidden</param>
        /// <param name="disableOnPause">Disable this sequence when Unity Time.timeScale is set to 0</param>
        public void Start(bool disableOnHide, bool disableOnPause)
        {
            if (m_animations == null || m_animations.Count == 0)
                return;

            Stop();

            var isPreviewMode = !Application.isPlaying;
            if (isPreviewMode)
            {
                OnBeforePreview();
            }

            m_isStopped = false;

            Anim anim;
            for (int i = 0; i < Animations.Count; i++)
            {
                anim = Animations[i];
                anim.DisableOnHide = disableOnHide;
                anim.DisableOnPause = disableOnPause;
                if (!anim.Start())
                {
                    Debug.LogErrorFormat(Transform, "The animation at the index {0} of the sequence {1} is not valid", i, Name);
                }
            }

            if (!isPreviewMode)
            {
                Infinity.AfterRealTime(m_duration, () =>
                {
                    // It is possible that the sequence has been stopped at this moment.
                    if (!m_isStopped)
                    {
                        HideGameObjectIfShouldHideAtEnd();
                        m_isStopped = true;
                    }
                });
            }

        }

        /// <summary>
        /// Sets the animation sequence as the given.
        /// </summary>
        /// <param name="other">AnimSequence object to copy</param>
        public void SetAs(AnimSequence other)
        {
            m_name = other.m_name;
            m_draggedFilePath = other.m_draggedFilePath;
            m_draggedObject = other.m_draggedObject;
            m_animations = new List<Anim>(other.m_animations.Count);
            m_duration = other.m_duration;
            Anim newAnim;
            foreach (var anim in other.m_animations)
            {
                if (anim.Transform != other.Transform)
                {
                    m_animations.Add(anim.Clone());
                }
                else
                {
                    newAnim = new Anim(anim.AnimationType, Transform);
                    newAnim.Copy(anim);
                    m_animations.Add(newAnim);
                }

            }

        }

        /// <summary>
        /// Save this AnimSequence at the given path.
        /// </summary>
        /// <param name="path">Save path</param>
        internal void Save(string path)
        {
            var doc = new XmlDocument();

            var docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);
            var root = doc.CreateElement("root");

            foreach (var anim in Animations)
            {
                root.AppendChild(anim.ToXml(doc));
            }

            doc.AppendChild(root);

            using (var stream = new StreamWriter(new FileStream(path, FileMode.Create), Encoding.UTF8))
            {
                doc.Save(stream);
                stream.Close();
            }
        }

        /// <summary>
        /// Load an AnimSequence placed at Application.datapath + the given path
        /// </summary>
        /// <param name="path">the path relative to Assets folder.</param>
        /// <returns><c>true</c> if the file is loaded <c>false</c> otherwise</returns>
        internal bool Load(string path)
        {
            if (string.IsNullOrEmpty(path) || !path.EndsWith(FileExtension, StringComparison.Ordinal))
            {
                Debug.LogError($"The file {path} is not a valid animation sequence file", m_transform);
                return false;
            }

            path = path.Replace("Assets/", string.Empty).Replace("Assets\\", string.Empty);
            path = Path.Combine(Application.dataPath, path);
            if (!File.Exists(path))
            {
                Debug.LogError($"The file '{path}' does not exist", m_transform);
                return false;
            }

            var doc = new XmlDocument();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                doc.Load(stream);
                stream.Close();
            }

            var root = doc["root"];
            var childs = root.ChildNodes;
            var count = childs.Count;
            Anim anim;
            Animations.Clear();
            for (int i = 0; i < count; i++)
            {
                anim = Anim.FromXml(childs[i]);
                anim.Transform = Transform;
                Animations.Add(anim);
            }
            return true;
        }

        /// <summary>
        /// Stop all animations
        /// </summary>
        public void Stop()
        {
            if (IsPlaying)
            {
                m_isStopped = true;
                var count = Animations.Count;
                for (int i = 0; i < count; i++)
                {
                    Animations[i].Stop();
                }
            }
        }

        /// <summary>
        /// Resets the animation sequence
        /// </summary>
        internal void Reset()
        {
            if (Animations == null)
            {
                return;
            }
            var count = Animations.Count;
            for (int i = 0; i < count; i++)
            {
                Animations[i].Reset();
            }
        }

        private void HideGameObjectIfShouldHideAtEnd()
        {

            if (m_transform != null && m_disableAtEnd)
            {
                m_transform.gameObject.SetActive(false);
            }
        }

        internal void OnBeforePreview()
        {
            foreach (var anim in Animations)
            {
                anim.OnBeforePreview();
            }
        }

        internal void AutoSetFromValueOfAnimAt(int index)
        {
            Animations[index].AutoSetFromValue();
        }

        internal void AutoSetToValueOfAnimAt(int index)
        {
            Animations[index].AutoSetToValue();
        }

        internal static bool CanMultiEdit(AnimSequence first, AnimSequence second)
        {
            var count = first.Animations.Count;
            if (count != second.Animations.Count)
            {
                return false;
            }
            for (int i = 0; i < count; i++)
            {
                if (first.Animations[i].AnimationType != second.Animations[i].AnimationType)
                {
                    return false;
                }
            }
            return true;
        }
    }

}