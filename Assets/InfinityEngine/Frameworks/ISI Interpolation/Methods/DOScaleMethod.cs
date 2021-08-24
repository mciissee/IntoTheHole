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
    /// Scales smoothly the gameobject
    /// </summary>
    [AddComponentMenu("InfinityEngine/Interpolations/DOScaleMethod")]
    public class DOScaleMethod : InterpolationMethod
    {

        [Accordion("Parameters")]
        [Message("The starts scale of the gameobject", MessageTypes.Info)]
        [InspectorButton("SetFromAsGameObject", "Current", 60, 20, false, InspectorButtonLocations.Right)]
        [SerializeField]
        private Vector3 from;

        [Accordion("Parameters")]
        [Message("The ends scale of the gameobject", MessageTypes.Info)]
        [InspectorButton("SetToAsGameObject", "Current", 60, 20, false, InspectorButtonLocations.Right)]
        [SerializeField]
        private Vector3 to;

        private Vector3 scaleBeforePreview;


        public override void Invoke()
        {
            base.Invoke();
            interpolable = transform.DOScale(from, to, duration).SetCached();
            scaleBeforePreview = transform.localScale;
            StartInterpolation();
        }

        protected override void OnAfterPreview()
        {
            base.OnAfterPreview();
            if (isStartedInEditMode)
            {
                transform.localScale = scaleBeforePreview;
            }
        }

        private void SetFromAsGameObject()
        {
            from = transform.localScale;
        }

        private void SetToAsGameObject()
        {
            to = transform.localScale;
        }

    }
}