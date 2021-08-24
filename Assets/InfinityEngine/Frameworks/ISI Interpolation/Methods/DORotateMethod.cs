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
    /// Rotates smoothly the gameobject
    /// </summary>
    [AddComponentMenu("InfinityEngine/Interpolations/DORotateMethod")]
    public class DORotateMethod : InterpolationMethod
    {
        [Accordion("Parameters")]
        [SerializeField]
        private Space space;

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

        [Accordion("Parameters")]
        [Message("The rotation mode", MessageTypes.Info)]
        [SerializeField]
        private RotationModes rotationMode;

        private Vector3 rotationBeforePreview;

        public override void Invoke()
        {
            base.Invoke();

            interpolable = space == Space.Self ? transform.DORotateLocal(Quaternion.Euler(from), Quaternion.Euler(to), duration)
                                               : transform.DORotate(Quaternion.Euler(from), Quaternion.Euler(to), duration);

            interpolable.SetCached().SetRotationMode(rotationMode);


            rotationBeforePreview = space == Space.Self ? transform.localRotation.eulerAngles : transform.rotation.eulerAngles;
            StartInterpolation();
        }

        protected override void OnAfterPreview()
        {
            base.OnAfterPreview();
            if (isStartedInEditMode)
            {
                if(space == Space.Self)
                {
                    transform.localRotation = Quaternion.Euler(rotationBeforePreview);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(rotationBeforePreview);
                }
            }
        }

        private void SetFromAsGameObject()
        {
            from = space == Space.Self ? transform.localRotation.eulerAngles : transform.rotation.eulerAngles;
        }

        private void SetToAsGameObject()
        {
            to = space == Space.Self ? transform.localRotation.eulerAngles : transform.rotation.eulerAngles;
        }

    }
}