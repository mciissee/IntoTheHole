/*************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Infinity Interactive Unity Asset Store catalog: http://u3d.as/riS	                                                                *
*************************************************************************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using System;

namespace InfinityEngine.Interpolations
{


    /// <summary>
    ///   Provides static functions to display notifications on screen   
    /// </summary>
    public class QuickNotification : MonoBehaviour
    {

        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private CanvasGroup canvasGroup;

        [SerializeField] private Text label;
        [SerializeField] private Outline outline;

        /// <summary>
        /// Gets or sets the main color of the text display by the Notification object
        /// </summary>
        public Color TextColor
        {
            get => label.color;
            set => label.color = value;
        }

        /// <summary>
        /// Gets or sets the outline color of the text display by the Notification object
        /// </summary>
        public Color OutlineColor
        {
            get => outline.effectColor;
            set => outline.effectColor = value;
        }

        /// <summary>
        /// Gets or sets the Interpolable object used to animate the QuickNotification.
        /// </summary>
        public Interpolable Interpolation { get; set; }

        /// <summary>
        /// The RectTransform component of the GameObject
        /// </summary>
        public RectTransform RectTransform { get => rectTransform; private set => rectTransform = value; }

        /// <summary>
        /// The CanvasGround component of the GameObject
        /// </summary>
        public CanvasGroup CanvasGroup { get => canvasGroup; private set => canvasGroup = value; }

        private void Awake()
        {
            rectTransform = rectTransform ?? GetComponent<RectTransform>();
            canvasGroup = canvasGroup ?? GetComponent<CanvasGroup>();

            if (rectTransform == null || canvasGroup == null)
            {
                throw new NullReferenceException("The Gameobject must have a RectTransform and CanvasGroup component");
            }
        }

        /// <summary>
        /// Show the notification on screen.
        /// </summary>
        /// <param name="message">The message of the notification</param>
        /// <param name="position">The position of the notification on the screen</param>
        public void ShowNotification(string message, Vector3 position)
        {
            label.text = message;
            rectTransform.anchoredPosition3D = QuickNotificationManager.ToCanvasPoint(position);
            Interpolation.Start();
        }

    }
}