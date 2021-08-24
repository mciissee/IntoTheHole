/*************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS	                                                                *
*************************************************************************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using System;
using InfinityEngine.Utils;
using System.Collections.Generic;
using InfinityEngine.Extensions;
using InfinityEngine.Serialization;
using InfinityEngine.Interpolations;
using InfinityEngine.Attributes;

namespace InfinityEngine
{
    /// <summary>
    ///   Tutorial manager. 
    /// </summary>
    [OverrideInspector]
    [RequireComponent(typeof(CanvasGroup), typeof(RectTransform))]
    public class Tutorial : MonoBehaviour
    {

        #region Fields
        [InfinityHeader]
        [SerializeField]
        private bool __help__;

        [Accordion("Options")]
        [Message("The tutorial will be displayed only is the value of the preference identified by this value is sets to false\n" +
            "Go to the menu 'Infinity Engine/VP to creates a key is this  popup is empty'", MessageTypes.Info)]
        [Popup(R2.bools.Names, PopupValueTypes.String)]
        [SerializeField]
        private string conditionPrefKey;

        [Accordion("Options")]
        [Message("The ease function used when the canvas is opened or closed", MessageTypes.Info)]

        [SerializeField]
        private EaseTypes easeType;

        [Accordion("UI")]
        [Message("The button used to manages the tutorials.", MessageTypes.Info)]
        [SerializeField]
        private Button nextButton;

        [Message("The button used to close the canvas.", MessageTypes.Info)]
        [Accordion("UI")]
        [SerializeField]
        private Button closeButton;

        [Accordion("UI")]
        [Message("An indicator (such as a hand icon) to move each time the next button is clicked.", MessageTypes.Info)]
        [SerializeField]
        private RectTransform indicator;

        [Accordion("Tasks")]
        [Message("Each time the button next is clicked, the task at the index (click count) will be displayed.\n\n" +
            "If the toggle 'Enable Indicator' is active, the Indicator specified in the area 'UI' will be moved to 'Indicator Position'", MessageTypes.Info)]
        [SerializeField]
        private Task[] listTutorials;

        private int current;
        private Action callback;
        private RectTransform rectTransform;
        private CanvasGroup canvasGroup;
        private static List<Tutorial> Instances;

        #endregion Fields

        #region Unity

        void Awake()
        {
            Instances = Instances ?? new List<Tutorial>();
            if (!Instances.Contains(this))
                Instances.Add(this);
        }

        void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            rectTransform = GetComponent<RectTransform>();
            current = 0;

            if (nextButton != null)
                nextButton.onClick.AddListener(Next);

            if (closeButton != null)
                closeButton.onClick.AddListener(Close);

            gameObject.SetActive(false);
        }

        #endregion Unity

        #region Others

        private void ShowCurrent()
        {
            if (indicator != null && listTutorials[current].EnableIndicator)
            {
                indicator.gameObject.SetActive(true);
                indicator.DOMoveAnchor3D(listTutorials[current].Position, 1).SetEase(EaseTypes.BackOut).Start();
            }
            else
            {
                indicator.gameObject.SetActive(false);
            }

            listTutorials[current].Label.gameObject.SetActive(true);
        }

        /// <summary>
        /// Close the tutorial window
        /// </summary>
        public void Close()
        {
            rectTransform.DOScale(Vector3.one, Vector3.zero, 1).SetEase(easeType).Start();
            canvasGroup.DOFadeOut(.5f).OnComplete((arg) =>
            {
                VP.SetBool(conditionPrefKey, true);
                VP.Save();
                callback?.Invoke();
            }).Start();

        }

        /// <summary>
        /// Display next step
        /// </summary>
        public void Next()
        {
            listTutorials[current].Label.gameObject.SetActive(false);
            current++;
            if (current >= listTutorials.Length)
            {
                Close();
            }
            else
            {
                ShowCurrent();
            }
        }

        /// <summary>
        ///Display the tutorial identified by the given name if it has not yet been displayed at least once.
        /// </summary>
        /// <param name="name">The name of the tutorial</param>
        /// <param name="callback">Action invoked at the of the tutorial </param>
        public static void Show(string name, Action callback)
        {
            var tutorial = Find(name);
            if (tutorial == null)
            {
                callback.Invoke();
                return;
            }
            if (VP.GetBool(tutorial.conditionPrefKey))
            {
                callback.Invoke();
                return;
            }

            if (tutorial.listTutorials.Length == 0)
            {
                Debugger.Log($"There is 0 tutorial to show for {name}", tutorial.gameObject);
                callback.Invoke();
                return;
            }


            tutorial.callback = callback;
            tutorial.rectTransform.localScale = Vector3.zero;
            tutorial.gameObject.SetActive(true);

            tutorial.rectTransform.DOScale(Vector3.zero, Vector3.one, 1).SetEase(tutorial.easeType).Start();
            tutorial.canvasGroup.DOFadeIn(1.5f).SetEase(tutorial.easeType).Start();
            tutorial.ShowCurrent();
        }

        private static Tutorial Find(string name)
        {
            var tutorial = Instances.Find(tut => tut.conditionPrefKey == name);
            if (tutorial == null)
            {
                Debugger.LogError($"There is no tutorial with the name {name}");
                return null;
            }
            return tutorial;
        }

        #endregion Others

        /// <summary>
        /// Tutorial task
        /// </summary>
        [Serializable]
        public class Task
        {
            [SerializeField]
            private Text label;

            [SerializeField]
            private bool enableIndicator;

            [SerializeField]
            private Vector3 indicatorPosition;

            /// <summary>
            /// The label that display the tutorial message
            /// </summary>
            public Text Label => label;

            /// <summary>
            /// Is an indicator is displayed in the current step?
            /// </summary>
            public bool EnableIndicator => enableIndicator;

            /// <summary>
            /// The position of the indicator
            /// </summary>
            public Vector3 Position => indicatorPosition;
        }
    }
}