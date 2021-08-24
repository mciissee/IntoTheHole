/*************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Infinity Interactive Unity Asset Store catalog: http://u3d.as/riS	                                                                *
*************************************************************************************************************************************/
using System;
using UnityEngine;
using InfinityEngine.Extensions;
using InfinityEngine.ResourceManagement;
using System.Linq;
using InfinityEngine.Attributes;

namespace InfinityEngine.Interpolations
{

    /// <summary>
    ///   Manages <see cref="QuickNotification"/> objects.
    /// </summary>
    /// <remarks>
    ///   This class is not a singleton because we want to change the canvas linked to 
    ///   the notification objects between scenes
    /// </remarks>
    [OverrideInspectorAttribute]
    public class QuickNotificationManager : MonoBehaviour
    {
        private static QuickNotificationManager Instance;

        [SerializeField] private GameObject[] templates;
        [SerializeField] private RectTransform canvasRectTransform;
        [SerializeField] private Camera _camera;

        private void Awake()
        {
            Instance = this;
        }

        /// <summary>
        /// Changes the given world position to a point inside the canvas attached to this GameObject.
        /// </summary>
        /// <param name="position">The world position</param>
        /// <returns>The point corresponding to the given world position in the canvas</returns>
        public static Vector3 ToCanvasPoint(Vector3 position)
        {
            position.z = 0;
            var screenPoint = RectTransformUtility.WorldToScreenPoint(Instance._camera, position);
            return screenPoint - Instance.canvasRectTransform.sizeDelta / 2;
        }

        /// <summary>
        /// Creates new QuickNotification which wiil appears with a scale animation.
        /// </summary>
        /// <param name="templateName">The name of the QuickNotification template to use</param>
        /// <param name="options">The options to apply to the notification</param>
        /// <param name="scaleFactor">The final scale (<c>Vector2.one</c>) of the notification object will be multiplied by this value</param>
        /// <returns>New QuickNotification object</returns>
        public static QuickNotification CreateScalableNotication(string templateName, NotificationOptions options, float scaleFactor = 1)
        {
            var prefab = Instance.templates.FirstOrDefault(t => t.name == templateName);
            var template = prefab.FromPool<QuickNotification>();
            template.transform.localScale = Vector3.zero;
            template.transform.SetParent(Instance.canvasRectTransform);

            void onComplete(Interpolable arg) => Infinity.After(options.CloseDelay, template.gameObject.ToPool);
            template.Interpolation = template.transform.DOScale(Vector3.one * scaleFactor, options.EntryDuration)
                .OnComplete(onComplete);

            if (options.InterpolationOptions != null)
            {
                template.Interpolation.SetOptions(options.InterpolationOptions);
            }
            return template;

        }

        /// <summary>
        /// Creates new QuickNotification which will appears with a fade animation
        /// </summary>
        /// <param name="templateName">The name of the QuickNotification template to use</param>
        /// <param name="options">The options to apply to the notification</param>
        /// <returns>New QuickNotification object</returns>
        public static QuickNotification CreateFadableNotification(string templateName, NotificationOptions options)
        {
            var prefab = Instance.templates.FirstOrDefault(t => t.name == templateName);
            var template = prefab.FromPool<QuickNotification>();
            template.transform.SetParent(Instance.canvasRectTransform);
            template.transform.localScale = Vector3.one;

            template.CanvasGroup.alpha = 0.0f;

            void onComplete(Interpolable arg) => Infinity.After(options.CloseDelay, template.gameObject.ToPool);
            template.Interpolation = template.CanvasGroup.DOFadeIn(options.EntryDuration).OnComplete(onComplete);

            if (options.InterpolationOptions != null)
            {
                template.Interpolation.SetOptions(options.InterpolationOptions);
            }

            return template;
        }

        /// <summary>
        /// Deactivates all visible notifications
        /// </summary>
        public static void ClearAll()
        {
            foreach (var item in Instance.templates)
            {
                PoolManager.ResetPoolWithTag(item.name);
            }

        }
    }
}