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
    [DontDrawInspectorIf("HasNoFadableComponent", "The GameObject must have a fadable component [CanvasGroup | Image | Text | SpriteRenderer]")]
    [AddComponentMenu("InfinityEngine/Interpolations/DOFadeMethod")]
    public class DOFadeMethod : InterpolationMethod
    {
        [Accordion("Parameters")]
        [Popup(nameof(GetComponentTypes), PopupValueTypes.Method)]
        [SerializeField]
        private string componentType;

        [Accordion("Parameters")]
        [Message("The starts alpha of the component to fade", MessageTypes.Info)]
        [InspectorButton("SetFromAsGameObject", "Current", 60, 20, false, InspectorButtonLocations.Right)]
        [SerializeField]
        [Range(0, 1)]
        private float from;

        [Accordion("Parameters")]
        [Message("The ends alpha of the component to fade", MessageTypes.Info)]
        [InspectorButton(nameof(SetToAsGameObject), "Current", 60, 20, false, InspectorButtonLocations.Right)]
        [SerializeField]
        [Range(0, 1)]
        private float to;

        [HideInInspector]
        [SerializeField]
        private CanvasGroup canvasGroup;

        [HideInInspector]
        [SerializeField]
        private Image image;

        [HideInInspector]
        [SerializeField]
        private Text text;

        [HideInInspector]
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        private StringBuilder componentTypes;

        private float fadeBeforePreview;

        public override void Invoke()
        {
            base.Invoke();

            fadeBeforePreview = GetFade();
            switch (componentType)
            {
                case "Text":
                    interpolable = text.DOFade(from, to, duration).SetCached();
                    break;
                case "Image":
                    interpolable = image.DOFade(from, to, duration).SetCached();
                    break;
                case "CanvasGroup":
                    interpolable = canvasGroup.DOFade(from, to, duration).SetCached();
                    break;
                case "SpriteRenderer":
                    interpolable = spriteRenderer.DOFade(from, to, duration).SetCached();
                    break;
            }
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
                        text.SetAlpha(fadeBeforePreview);
                        break;
                    case "Image":
                        image.SetAlpha(fadeBeforePreview);
                        break;
                    case "CanvasGroup":
                        canvasGroup.alpha = fadeBeforePreview;
                        break;
                    case "SpriteRenderer":
                        spriteRenderer.SetAlpha(fadeBeforePreview);
                        break;
                }
            }
        }

        private void SetFromAsGameObject()
        {
            var fade = GetFade();
            if (System.Math.Abs(fade - -1) > double.Epsilon)
            {
                from = fade;
            }
        }

        private void SetToAsGameObject()
        {
            var fade = GetFade();
            if (System.Math.Abs(fade - -1) > double.Epsilon)
            {
                to = fade;
            }
        }

        private float GetFade()
        {
            switch (componentType)
            {
                case "Text":
                    return text.color.a;
                case "Image":
                    return image.color.a;
                case "CanvasGroup":
                    return canvasGroup.alpha;
                case "SpriteRenderer":
                    return spriteRenderer.color.a;
            }
            return -1;
        }

        private string GetComponentTypes()
        {
            componentTypes = componentTypes ?? new StringBuilder();
            componentTypes.Length = 0;

            if (canvasGroup != null && canvasGroup.gameObject != gameObject)
                canvasGroup = null;

            if (image != null && image.gameObject != gameObject)
                image = null;

            if (text != null && text.gameObject != gameObject)
                text = null;

            if (spriteRenderer != null && spriteRenderer.gameObject != gameObject)
                spriteRenderer = null;

            if (HasComponent(ref canvasGroup))
                componentTypes.Append("CanvasGroup,");

            if (HasComponent(ref image))
                componentTypes.Append("Image,");

            if (HasComponent(ref text))
                componentTypes.Append("Text,");

            if (HasComponent(ref spriteRenderer))
                componentTypes.Append("SpriteRenderer");

            return componentTypes.ToString();
        }

        private bool HasNoFadableComponent()
        {
            return string.IsNullOrEmpty(GetComponentTypes());
        }

    }

}