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

namespace InfinityEngine.Interpolations
{
    /// <summary>
    /// Changes smoothly the fill amount of an Image component
    /// </summary>
    [DontDrawInspectorIf("IsMissingImageComponent", "The component is valid only on a GameObject with the component Image")]
    [AddComponentMenu("InfinityEngine/Interpolations/DOFillAmountMethod")]
    public class DOFillAmount : InterpolationMethod
    {

        [Accordion("Parameters")]
        [Message("The starts fillAmount of the image", MessageTypes.Info)]
        [InspectorButton("SetFromAsGameObject", "Current", 60, 20, false, InspectorButtonLocations.Right)]
        [SerializeField]
        [Range(0, 1)]
        private float from;

        [Accordion("Parameters")]
        [Message("The ends fillAmount of the image", MessageTypes.Info)]
        [InspectorButton("SetToAsGameObject", "Current", 60, 20, false, InspectorButtonLocations.Right)]
        [SerializeField]
        [Range(0, 1)]
        private float to;

        private float fillAmountBeforePreview;

        [HideInInspector]
        [SerializeField]
        private Image image;

        public override void Invoke()
        {
            base.Invoke();

            if(image == null)
            {
                image = GetComponent<Image>();
                if (image == null)
                {
                    Debug.LogError("The GameObject must have an Image component", gameObject);
                    return;
                }
            }
  

            fillAmountBeforePreview = image.fillAmount;
            interpolable = image.DOFillAmount(from, to, duration).SetCached();
            StartInterpolation();
        }

        protected override void OnAfterPreview()
        {
            base.OnAfterPreview();
            if (isStartedInEditMode)
            {
                image.fillAmount = fillAmountBeforePreview;
            }
        }

        private void SetFromAsGameObject()
        {
            from = image.fillAmount;
        }

        private void SetToAsGameObject()
        {
            to = image.fillAmount;
        }

        private bool IsMissingImageComponent()
        {
            if(image == null)
            {
                image = GetComponent<Image>();
            }
            else if(image.gameObject != gameObject)
            {
                image = null;
            }
            return image == null;
        }

    }
}
