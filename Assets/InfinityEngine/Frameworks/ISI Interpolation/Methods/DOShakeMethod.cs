/************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Infinity Interactive Unity Asset Store catalog: http://u3d.as/riS	                                                                *
************************************************************************************************************************************/

using UnityEngine;
using InfinityEngine.Attributes;
using InfinityEngine.Extensions;
using UnityEngine.UI;
using System.Text;

namespace InfinityEngine.Interpolations
{
    /// <summary>
    /// Fades smoothly the gameobject
    /// </summary>
    [AddComponentMenu("InfinityEngine/Interpolations/DOShakeMethod")]
    public class DOShakeMethod : InterpolationMethod
    {


        [Accordion("Parameters")]
        [Popup("Position,Scale,Rotation", PopupValueTypes.String)]
        [SerializeField]
        private string shakeType;

        [Accordion("Parameters")]
        [Message("The axis to shake (for example (1, 0, 0) will shake only the x component of the scale, position or rotation)", MessageTypes.Info)]
        [SerializeField]
        private Vector3 axis;

        [Accordion("Parameters")]
        [Message("Control the radius of the shake", MessageTypes.Info)]
        [SerializeField]
        private float amount;


        private Vector3 valueBeforePreview;

        protected override void OnValidate()
        {
            base.OnValidate();
            if(axis.x != 0 && axis.x != 1)
            {
                axis.x = 0;
            }
            if (axis.y != 0 && axis.y != 1)
            {
                axis.y = 0;
            }
            if (axis.z != 0 && axis.z != 1)
            {
                axis.z = 0;
            }
        }

        public override void Invoke()
        {
            base.Invoke();
       
            switch (shakeType)
            {
                case "Position":
                    interpolable = transform.DOShakePosition(axis, amount, duration).SetCached();
                    break;
                case "Scale":
                    interpolable = transform.DOShakeScale(axis, amount, duration).SetCached();
                    break;
                case "Rotation":
                    interpolable = transform.DOShakeRotation(axis, amount, duration).SetCached();
                    break;
            }
            valueBeforePreview = GetValueBeforePreview();
            StartInterpolation();
        }

        protected override void OnAfterPreview()
        {
            base.OnAfterPreview();
            if (isStartedInEditMode)
            {
                switch (shakeType)
                {
                    case "Position":
                        transform.localPosition = valueBeforePreview;
                        break;
                    case "Scale":
                        transform.localScale = valueBeforePreview;
                        break;
                    case "Rotation":
                        transform.localRotation = Quaternion.Euler(valueBeforePreview);
                        break;
                }
            }
        }

        private Vector3 GetValueBeforePreview()
        {
            switch (shakeType)
            {
                case "Position":
                    return transform.localPosition;
                case "Scale":
                    return transform.localScale;
                case "Rotation":
                    return transform.localRotation.eulerAngles;
            }
            return Vector3.one;
        }

    }

}