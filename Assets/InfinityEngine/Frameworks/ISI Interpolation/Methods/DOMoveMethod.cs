/************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Infinity Interactive Unity Asset Store catalog: http://u3d.as/riS	                                                                *
************************************************************************************************************************************/

using UnityEngine;
using InfinityEngine.Attributes;
using InfinityEngine.Extensions;

namespace InfinityEngine.Interpolations
{
    /// <summary>
    /// Translates smoothly the gameobject
    /// </summary>
    [AddComponentMenu("InfinityEngine/Interpolations/DOMoveMethod")]
    public class DOMoveMethod : InterpolationMethod
    {

        [VisibleIfAttribute("HasNotRectTransformComponent", MemberTypes.Method)]
        [Accordion("Parameters")]
        [SerializeField]
        private Space space;

        [Accordion("Parameters")]
        [Message("The starts position of the gameobject", MessageTypes.Info)]
        [InspectorButton("SetFromAsGameObject", "Current", 60, 20, false, InspectorButtonLocations.Right)]
        [SerializeField]
        private Vector3 from;

        [Accordion("Parameters")]
        [Message("The ends position of the gameobject", MessageTypes.Info)]
        [InspectorButton("SetToAsGameObject", "Current", 60, 20, false, InspectorButtonLocations.Right)]
        [SerializeField]
        private Vector3 to;


        [HideInInspector]
        [SerializeField]
        private RectTransform rectTransform;

        private Vector3 positionBeforePreview;


        public override void Invoke()
        {
            base.Invoke();

            if (rectTransform == null)
            {
                rectTransform = GetComponent<RectTransform>();
            }


            if(rectTransform == null)
            {
                if (space == Space.World)
                {
                    interpolable = transform.DOMove(from, to, duration).SetCached();
                }
                else
                {
                    interpolable = transform.DOMoveLocal(from, to, duration).SetCached();
                }
            }
            else
            {
                interpolable = rectTransform.DOMoveAnchor3D(from, to, duration).SetCached();
            }

            positionBeforePreview = rectTransform == null ?  (space == Space.World ? transform.position : transform.localPosition) : rectTransform.anchoredPosition3D;
            StartInterpolation();
        }


        protected override void OnAfterPreview()
        {
            base.OnAfterPreview();
            if (isStartedInEditMode)
            {
                if(rectTransform == null)
                {
                    if(space == Space.World)
                    {
                        transform.position = positionBeforePreview;
                    }
                    else
                    {
                        transform.localPosition = positionBeforePreview;
                    }
                }
                else
                {
                    rectTransform.anchoredPosition3D = positionBeforePreview;
                }
              
            }
        }

        private bool HasNotRectTransformComponent()
        {
            if(rectTransform == null)
            {
                rectTransform = GetComponent<RectTransform>();
            }
            else if (rectTransform.gameObject != gameObject)
            {
                rectTransform = null;
            }

            return rectTransform == null;
        }

        private void SetFromAsGameObject()
        {
            if (rectTransform == null)
            {
                rectTransform = GetComponent<RectTransform>();
            }
            if (rectTransform == null)
            {
                from = space == Space.World ? transform.position : transform.localPosition;
            }
            else
            {
                from = rectTransform.anchoredPosition3D;
            }
        }

        private void SetToAsGameObject()
        {
            if (rectTransform == null)
            {
                rectTransform = GetComponent<RectTransform>();
            }
            if (rectTransform == null)
            {
                to = space == Space.World ? transform.position : transform.localPosition;
            }
            else
            {
                to = rectTransform.anchoredPosition3D;
            }
        }

    }

}