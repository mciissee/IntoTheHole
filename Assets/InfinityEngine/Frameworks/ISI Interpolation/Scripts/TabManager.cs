/*************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Infinity Interactive Unity Asset Store catalog: http://u3d.as/riS	                                                                *
*************************************************************************************************************************************/

using UnityEngine;
using InfinityEngine.Extensions;
using UnityEngine.UI;
using InfinityEngine.Attributes;

namespace InfinityEngine.Interpolations
{
    /// <summary>
    /// Tab manager class
    /// </summary>
    [OverrideInspector]
    public class TabManager : MonoBehaviour
    {

        /// <summary>   
        /// Represents a simple tab
        /// </summary>
        [System.Serializable]
        public class Tab
        {

            private bool mEnable;

            /// <summary>   
            /// The button used to close the tab . 
            /// </summary>
            public Button button;

            /// <summary>   
            /// The sequencer component of the menu.
            /// </summary>
            public Sequencer sequencer;


            /// <summary>   
            /// Gets or sets a value indicating whether this menu is enabled. 
            /// </summary>
            public bool Enable
            {
                get { return mEnable; }
                set
                {
                    mEnable = value;
                    if (value)
                    {
                        sequencer.PlaySequenceWithName("Open");
                    }
                    else
                    {
                        sequencer.PlaySequenceWithName("Close");
                    }
                }
            }

        }

        [SerializeField]
        private Tab[] tabs;
        private Tab current;

        void Start()
        {

            tabs.ForEach(menu =>
            {
                menu.button.onClick.AddListener(() =>
                {
                    if (current != menu)
                    {
                        current.Enable = false;
                        menu.Enable = true;
                        current = menu;
                    }

                });

            });

            current = tabs[0];
            current.sequencer.gameObject.SetActive(true);
        }

    }
}