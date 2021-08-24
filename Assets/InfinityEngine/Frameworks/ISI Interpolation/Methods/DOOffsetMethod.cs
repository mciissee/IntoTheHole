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
    [DontDrawInspectorIf("IsMissingImageComponent", "The component is valid only on a GameObject with the component MeshRenderer with a valid material")]
    [AddComponentMenu("InfinityEngine/Interpolations/DOOffsetMethod")]
    public class DOOffsetMethod : InterpolationMethod
    {

        [Accordion("Parameters")]
        [Message("The direction of the offset", MessageTypes.Info)]
        [SerializeField]
        private Vector2 direction;

        private Vector2 offsetBeforePreview;

        [HideInInspector]
        [SerializeField]
        private MeshRenderer meshRenderer;

        public override void Invoke()
        {
            base.Invoke();

            if (meshRenderer == null)
            {
                meshRenderer = GetComponent<MeshRenderer>();
                if (meshRenderer == null)
                {
                    Debug.LogError("The GameObject must have an MeshRenderer component", gameObject);
                    return;
                }
            }

            if(meshRenderer.sharedMaterial == null)
            {
                Debug.LogError("The GameObject must have a MeshRenderer component with a valid material", gameObject);
                return;
            }

            interpolable = meshRenderer.sharedMaterial.DOOffset(direction, duration).SetCached();
            offsetBeforePreview = meshRenderer.sharedMaterial.mainTextureOffset;
            StartInterpolation();
        }

        protected override void OnAfterPreview()
        {
            base.OnAfterPreview();
            if (isStartedInEditMode)
            {
                meshRenderer.sharedMaterial.mainTextureOffset = offsetBeforePreview;
            }
        }

        private bool IsMissingImageComponent()
        {
            if (meshRenderer == null)
            {
                meshRenderer = GetComponent<MeshRenderer>();
            }
            if (meshRenderer != null && meshRenderer.gameObject != gameObject)
            {
                meshRenderer = null;
            }
            return meshRenderer == null || meshRenderer.sharedMaterial == null;
        }

    }
}
