/************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS                                                                                      *
*************************************************************************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using InfinityEngine.Extensions;

namespace InfinityEngine.Interpolations { 
  
    internal class FadeAnim : AnimCreator
    {
        private Anim anim;

        public FadeAnim(Anim anim)
        {
            this.anim = anim;
        }

        public void Reset()
        {
            var from = anim.FloatValue.starts;
            switch (anim.AnimatableComponent)
            {
                case AnimatableComponents.CanvasGroup:
                    anim.Transform.GetComponent<CanvasGroup>().alpha = from;
                    break;
                case AnimatableComponents.Image:
                    anim.Transform.GetComponent<Image>().SetAlpha(from);
                    break;
                case AnimatableComponents.Text:
                    anim.Transform.GetComponent<Text>().SetAlpha(from);
                    break;
                case AnimatableComponents.SpriteRenderer:
                    anim.Transform.GetComponent<SpriteRenderer>().SetAlpha(from);
                    break;
            }
        }

        public void Create()
        {
            Interpolable newInterpolable = null;
            var from = anim.FloatValue.starts;
            var to = anim.FloatValue.ends;
            switch (anim.AnimatableComponent)
            {
                case AnimatableComponents.CanvasGroup:
                    newInterpolable = anim.canvasGroupReferenceValue.DOFade(from, to, anim.Duration);
                    break;
                case AnimatableComponents.Image:
                    newInterpolable = anim.imageReferenceValue.DOFade(from, to, anim.Duration);
                    break;
                case AnimatableComponents.Text:
                    newInterpolable = anim.textReferenceValue.DOFade(from, to, anim.Duration);
                    break;
                case AnimatableComponents.SpriteRenderer:
                    newInterpolable = anim.spriteRendererReferenceValue.DOFade(from, to, anim.Duration);
                    break;
                default:
                    return;
            }
            anim.interpolable = newInterpolable;
        }

        public bool IsValid()
        {
            return anim.AnimatableComponent == AnimatableComponents.CanvasGroup
                        || anim.AnimatableComponent == AnimatableComponents.Image
                        || anim.AnimatableComponent == AnimatableComponents.SpriteRenderer
                        || anim.AnimatableComponent == AnimatableComponents.Text;
        }

    }

}