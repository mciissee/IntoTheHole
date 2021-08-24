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
using System.Linq;

namespace InfinityEngine.Interpolations
{
    /// <summary>
    /// Fades smoothly the choosen fadable component in the gamobject
    /// </summary>
    [DontDrawInspectorIf("HasNotFadableComponent", "The GameObject must have a fadable component [Image | Text | SpriteRenderer | Camera | MeshRenderer]")]
    [AddComponentMenu("InfinityEngine/Interpolations/DOColorMethod")]
    public class DOColorMethod : InterpolationMethod
    {

        #region Fields

        [Accordion("Parameters")]
        [Message("The MeshRenderer of the GameObject must have a material", MessageTypes.Error, "HasNotMaterialInMeshRenderer", MemberTypes.Method)]
        [Message("The type of the component to animate", MessageTypes.Info)]
        [Popup("GetComponentTypes", PopupValueTypes.Method)]
        [SerializeField]
        private string componentType;

        [Accordion("Parameters")]
        [Message("Use an array of color and pick a random color from the array each time there is a changement of color", MessageTypes.Info)]
        [SerializeField]
        private bool useColorArray;

        [Accordion("Parameters")]
        [Message("Use a random color each time there is a changement of color", MessageTypes.Info)]
        [SerializeField]
        private bool useRandomColors;

        [Accordion("Parameters")]
        [Message("The starts color of the component to animate", MessageTypes.Info)]
        [InspectorButton("SetFromAsGameObject", "Current", 60, 20, false, InspectorButtonLocations.Right)]
        [VisibleIfAttribute("IsSingleColorMode", MemberTypes.Method)]
        [SerializeField]
        private Color from;

        [Accordion("Parameters")]
        [Message("The ends color of the component to animate", MessageTypes.Info)]
        [InspectorButton("SetToAsGameObject", "Current", 60, 20, false, InspectorButtonLocations.Right)]
        [VisibleIfAttribute("IsSingleColorMode", MemberTypes.Method)]
        [SerializeField]
        private Color to;


        [Accordion("Parameters")]
        [Message("The different colors that the component can take during the animation", MessageTypes.Info)]
        [Message("You must specify the colors by clicking the button '+'", MessageTypes.Warning, "IsUsingArrayAndColorArrayIsEmpty", MemberTypes.Method)]
        [VisibleIfAttribute("useColorArray", MemberTypes.Field)]
        [SerializeField]
        private Color[] colorArray;

        [Accordion("Parameters")]
        [Message("The interval of time between the switch of the colors", MessageTypes.Info)]
        [VisibleIfAttribute("IsSingleColorMode", MemberTypes.Method, true)]
        [SerializeField]
        private float changeInterval;

        #region Components

        [HideInInspector]
        [SerializeField]
        private MeshRenderer meshRendrer;

        [HideInInspector]
        [SerializeField]
        private Image image;

        [HideInInspector]
        [SerializeField]
        private Text text;

        [HideInInspector]
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        [HideInInspector]
        [SerializeField]
        private Camera mCamera;

        #endregion Components

        private StringBuilder componentTypes;

        private Color colorBeforePreview;

        #endregion Fields

        protected override void OnValidate()
        {
            base.OnValidate();
            useColorArray = useRandomColors ? false : useColorArray;
            useRandomColors = useColorArray ? false : useRandomColors;
            if (!IsSingleColorMode())
            {
                Options.RepeatInterval = changeInterval;
            }
        }

        public override void Invoke()
        {
            base.Invoke();

            switch (componentType)
            {
                case "Text":
                    AnimateText();
                    break;
                case "Image":
                    AnimateImage();
                    break;
                case "MeshRenderer":
                    AnimateMeshRenderer();
                    break;
                case "SpriteRenderer":
                    AnimateSpriteRenderer();
                    break;
                case "Camera":
                    AnimateCamera();                   
                    break;
            }

            colorBeforePreview = GetColor();
            StartInterpolation();
        }

        protected override void OnAfterPreview()
        {
            base.OnAfterPreview();
            if (isStartedInEditMode)
            {
                switch (componentType)
                {
                    case "Text":
                        text.color = colorBeforePreview;
                        break;
                    case "Image":
                        image.color = colorBeforePreview;
                        break;
                    case "MeshRenderer":
                        if (meshRendrer.sharedMaterial != null)
                        {
                            meshRendrer.sharedMaterial.color = colorBeforePreview;
                        }
                        break;
                    case "SpriteRenderer":
                        spriteRenderer.color = colorBeforePreview;
                        break;
                    case "Camera":
                        mCamera.backgroundColor = colorBeforePreview;
                        break;
                }
            }
        }

        private void AnimateMeshRenderer()
        {
            if(meshRendrer == null)
            {
                Debug.LogError("Impossible to starts the interpolation because there is no MeshRenderer component in the GameObject", gameObject);
                return;
            }
            if (useColorArray)
            {
                interpolable = text.DOPickColor(colorArray, changeInterval, duration).SetCached();
            }
            else if (useRandomColors)
            {
                interpolable = meshRendrer.sharedMaterial.DORandomColor(changeInterval, duration).SetCached();
            }
            else if (meshRendrer.sharedMaterial != null)
            {
                interpolable = meshRendrer.sharedMaterial.DOColor(from, to, duration).SetCached();

            }
            else
            {
                Debug.LogError("Impossible to starts the interpolation because there is no material in the MeshRenderer component ", meshRendrer);
            }
        }

        private void AnimateSpriteRenderer()
        {
            if (spriteRenderer == null)
            {
                Debug.LogError("Impossible to starts the interpolation because there is no SpriteRenderer component in the GameObject", gameObject);
                return;
            }
            if (useColorArray)
            {
                interpolable = spriteRenderer.DOPickColor(colorArray, changeInterval, duration).SetCached();
            }
            else if (useRandomColors)
            {
                interpolable = spriteRenderer.DORandomColor(changeInterval, duration).SetCached();
            }
            else
            {
                interpolable = spriteRenderer.DOColor(from, to, duration).SetCached();

            }
        }

        private void AnimateImage()
        {
            if (image == null)
            {
                Debug.LogError("Impossible to starts the interpolation because there is no Image component in the GameObject", gameObject);
                return;
            }
            if (useColorArray)
            {
                interpolable = image.DOPickColor(colorArray, changeInterval, duration).SetCached();
            }
            else if (useRandomColors)
            {
                interpolable = image.DORandomColor(changeInterval, duration).SetCached();
            }
            else
            {
                interpolable = image.DOColor(from, to, duration).SetCached();
            }
        }

        private void AnimateCamera()
        {
            if (mCamera == null)
            {
                Debug.LogError("Impossible to starts the interpolation because there is no Camera component in the GameObject", gameObject);
                return;
            }
            if (useColorArray)
            {
                interpolable = mCamera.DOPickColor(colorArray, changeInterval, duration).SetCached();
            }
            else if (useRandomColors)
            {
                interpolable = mCamera.DORandomColor(changeInterval, duration).SetCached();
            }
            else
            {
                interpolable = mCamera.DOColor(from, to, duration).SetCached();

            }
        }

        private void AnimateText()
        {
            if (mCamera == null)
            {
                Debug.LogError("Impossible to starts the interpolation because there is no Text component in the GameObject", gameObject);
                return;
            }
            if (useColorArray)
            {
                interpolable = text.DOPickColor(colorArray, changeInterval, duration).SetCached();
            }
            else if (useRandomColors)
            {
                interpolable = text.DORandomColor(changeInterval, duration).SetCached();
            }
            else
            {
                interpolable = text.DOColor(from, to, duration).SetCached();

            }
        }

        private void SetFromAsGameObject()
        {
            var fade = GetColor();
            from = fade;
            
        }

        private void SetToAsGameObject()
        {
            var fade = GetColor();
            to = fade;        
        }

        private Color GetColor()
        {
            switch (componentType)
            {
                case "Text":
                    return text.color;

                case "Image":
                    return image.color;

                case "MeshRenderer":
                    if (meshRendrer.sharedMaterial != null)
                    {
                        return meshRendrer.sharedMaterial.color;
                    }
                    break;

                case "SpriteRenderer":
                    return spriteRenderer.color;

                case "Camera":
                    return mCamera.backgroundColor;
            }
            return Color.black;
        }

        private string GetComponentTypes()
        {
            if (componentTypes == null)
            {
                componentTypes = new StringBuilder();
            }

            componentTypes.Length = 0;

            if (HasComponent(ref meshRendrer))
            {
                componentTypes.Append("MeshRenderer,");
            }

            if (HasComponent(ref image))
            {
                componentTypes.Append("Image,");
            }

            if (HasComponent(ref text))
            {
                componentTypes.Append("Text,");
            }

            if (HasComponent(ref spriteRenderer))
            {
                componentTypes.Append("SpriteRenderer");
            }

            if (HasComponent(ref mCamera))
            {
                componentTypes.Append("Camera");
            }

            return componentTypes.ToString();
        }

        private bool HasNotMaterialInMeshRenderer()
        {
            return componentType == "MeshRenderer" && meshRendrer.sharedMaterial == null;
        }

        private bool HasNotFadableComponent()
        {
            return string.IsNullOrEmpty(GetComponentTypes());
        }

        private bool IsSingleColorMode()
        {
            return !useColorArray && !useRandomColors;
        }

        private bool IsUsingArrayAndColorArrayIsEmpty()
        {
            return useColorArray && (colorArray == null || !colorArray.Any());
        }
    }
}