/************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS                                                                                      *
*************************************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

[assembly: InternalsVisibleTo("Assembly-CSharp-Editor")]
namespace InfinityEngine.Interpolations
{
    /// <summary>
    /// Components allowing to creates powerful animation sequences.
    /// </summary>
    [AddComponentMenu("InfinityEngine/Interpolations/Sequencer")]
    public class Sequencer : MonoBehaviour
    {

        #region Fields
        [SerializeField] internal bool m_optionsFoldout;
        [SerializeField] internal bool m_playOnStart;
        [SerializeField] internal bool m_disabledOnHide = true;
        [SerializeField] internal bool m_disabledOnPause = true;
        [SerializeField] internal string m_sequenceToPlay;
        [SerializeField] internal List<AnimSequence> m_sequences;
        [SerializeField] internal Sequencer m_copiedSequence;
        #endregion Fields

        #region Properties 

        /// <summary>
        /// Gets the animation sequences
        /// </summary>
        public List<AnimSequence> Sequences => m_sequences ?? (m_sequences = new List<AnimSequence>());

        /// <summary>
        /// Gets a value indicating wheter any sequence is playing.
        /// </summary>
        public bool IsPlaying => Sequences.Any(seq => seq.IsPlaying);

        /// <summary>
	    /// Gets a value indicating whether the sequences should be paused when the application is on pause (Time.timeScale set to 0)
	    /// </summary>
	    public bool IsDisabledOnPause
        {
            get => m_disabledOnPause;
            set => m_disabledOnPause = value;
        }

        /// <summary>
        /// Gets a value indicating whether the sequences should be paused when the GameObject linked to it is not visible
        /// </summary>
        public bool IsDisabledOnHide
        {
            get => m_disabledOnHide;
            set => m_disabledOnHide = value;
        }

        internal Sequencer Copy
        {
            get => m_copiedSequence;
            set => m_copiedSequence = value;
        }

        #endregion Properties

        #region Methods

        private void Start()
        {
            if (m_playOnStart)
            {
                PlaySequenceWithName(m_sequenceToPlay);
            }
        }

        /// <summary>
        /// Plays the animation sequence with the given name
        /// </summary>
        /// <param name="sequenceName">The name of the sequence</param>
        public void PlaySequenceWithName(string sequenceName)
        {
            var sequence = Sequences.FirstOrDefault(seq => seq.Name == sequenceName);
            if (sequence != null)
            {
                StopAllSequences();
                sequence.Start(IsDisabledOnHide, IsDisabledOnPause);
            }
            else
            {
                Debug.LogFormat(gameObject, "There is no AnimSequence identified by '{0}' in the sequencer", sequenceName);
            }
        }

        /// <summary>
        /// Stops All animations
        /// </summary>
        public void StopAllSequences()
        {
            foreach (var seq in Sequences)
            {
                seq.Stop();
            }
        }

        /// <summary>
        /// Deletes the animation sequence with the given name
        /// </summary>
        /// <param name="sequenceName">The name of the sequence</param>
        public void DeleteSequenceWithName(string sequenceName)
        {
            var sequence = Sequences.FirstOrDefault(seq => seq.Name == sequenceName);
            if (sequence != null)
            {
                Sequences.Remove(sequence);
            }
            else
            {
                Debug.LogFormat(gameObject, "There is no AnimSequence identified by '{0}' in the sequencer", sequenceName);
            }
        }

        /// <summary>
        /// Stops the animation sequence with the given name
        /// </summary>
        /// <param name="sequenceName">The name of the sequence</param>
        public void StopSequenceWithName(string sequenceName)
        {
            var sequence = Sequences.FirstOrDefault(seq => seq.Name == sequenceName);
            if (sequence != null)
            {
                sequence.Stop();
            }
            else
            {
                Debug.LogFormat(gameObject, "There is no AnimSequence identified by '{0}' in the sequencer", sequenceName);
            }
        }

        internal bool IsValidAnimAtIndexInSequence(string sequenceName, int index)
        {
            var sequence = Sequences.FirstOrDefault(seq => seq.Name == sequenceName);
            if (sequence != null)
            {
                return sequence.IsValidAnimationAt(index);
            }
            return false;
        }

        internal void SetAsCopied()
        {
            if (m_copiedSequence == null)
            {
                return;
            }
            this.m_disabledOnHide = m_copiedSequence.m_disabledOnHide;
            this.m_disabledOnPause = m_copiedSequence.m_disabledOnPause;
            this.m_sequences = new List<AnimSequence>();
            AnimSequence newSequence;
            for (int i = 0; i < m_copiedSequence.Sequences.Count; i++)
            {
                newSequence = new AnimSequence(transform);
                newSequence.SetAs(m_copiedSequence.Sequences[i]);
                this.m_sequences.Add(newSequence);
            }
        }

        internal void DeleteAnimAtIndexInSequence(string sequenceName, int index)
        {
            var sequence = Sequences.FirstOrDefault(seq => seq.Name == sequenceName);
            if (sequence != null)
            {
                sequence.RemoveAnimationAt(index);
            }
            else
            {
                Debug.LogFormat(gameObject, "There is no AnimSequence identified by '{0}' in the sequencer", sequenceName);
            }
        }

        internal void AddAnimationInSequence(string sequenceName, AnimationTypes animType)
        {
            var sequence = Sequences.FirstOrDefault(seq => seq.Name == sequenceName);
            if (sequence != null)
            {
                sequence.AddAnimation(animType);
            }
            else
            {
                Debug.LogFormat(gameObject, "There is no AnimSequence identified by '{0}' in the sequencer", sequenceName);
            }
        }

        internal void LoadSequenceAtPathIntoSequence(string sequenceName, string path)
        {
            var sequence = Sequences.FirstOrDefault(seq => seq.Name == sequenceName);
            if (sequence != null)
            {
                sequence.Load(path);
            }
            else
            {
                Debug.LogFormat(gameObject, "There is no AnimSequence identified by '{0}' in the sequencer", sequenceName);
            }
        }

        internal void SaveSequenceAtPath(string sequenceName, string path)
        {
            var sequence = Sequences.FirstOrDefault(seq => seq.Name == sequenceName);
            if (sequence != null)
            {
                sequence.Save(path);
            }
            else
            {
                Debug.LogFormat(gameObject, "There is no AnimSequence identified by '{0}' in the sequencer", sequenceName);
            }
        }

        internal void AutoSetFromValueOfAnimAtIndexInSequence(string sequenceName, int index)
        {
            var sequence = Sequences.FirstOrDefault(seq => seq.Name == sequenceName);
            if (sequence != null)
            {
                sequence.AutoSetFromValueOfAnimAt(index);
            }
            else
            {
                Debug.LogFormat(gameObject, "There is no AnimSequence identified by '{0}' in the sequencer", sequenceName);
            }
        }

        internal void AutoSetToValueOfAnimAtIndexInSequence(string sequenceName, int index)
        {
            var sequence = Sequences.FirstOrDefault(seq => seq.Name == sequenceName);
            if (sequence != null)
            {
                sequence.AutoSetToValueOfAnimAt(index);
            }
            else
            {
                Debug.LogFormat(gameObject, "There is no AnimSequence identified by '{0}' in the sequencer", sequenceName);
            }
        }

        internal void ValidateSequenceTransforms()
        {
            for (int i = 0; i < Sequences.Count; i++)
            {
                Sequences[i].Transform = transform;
            }
        }

        internal static bool CanMultiEdit(Sequencer first, Sequencer second)
        {
            var sequenceCount = first.Sequences.Count;
            if (sequenceCount != second.Sequences.Count)
            {
                return false;
            }
            AnimSequence A;
            AnimSequence B;
            for (int i = 0; i < first.Sequences.Count; i++)
            {
                A = first.Sequences[i];
                B = second.Sequences[i];

                if (!AnimSequence.CanMultiEdit(A, B))
                {
                    return false;
                }

            }
            return true;
        }

        #endregion Methods
    }

}