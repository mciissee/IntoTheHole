/************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS                                                                                      *
*************************************************************************************************************************************/

using InfinityEngine.Extensions;

namespace InfinityEngine.Interpolations
{

    internal class ColorAnim : AnimCreator
    {
        private readonly Anim anim;

        public ColorAnim(Anim anim)
        {
            this.anim = anim;
        }

        public void Reset()
        {
            var from = anim.ColorValue.starts;

            switch (anim.AnimatableComponent)
            {
                case AnimatableComponents.MeshRenderer:
                    anim.meshRendererReferenceValue.sharedMaterial.color = from;
                    break;
                case AnimatableComponents.Image:
                    anim.imageReferenceValue.color = from;
                    break;
                case AnimatableComponents.Text:
                    anim.textReferenceValue.color = from;
                    break;
                case AnimatableComponents.SpriteRenderer:
                    anim.spriteRendererReferenceValue.color = from;
                    break;
                case AnimatableComponents.Camera:
                    anim.cameraReferenceValue.backgroundColor = from;
                    break;
            }
        }

        public void Create()
        {
            Interpolable newInterpolable = null;
            var from = anim.ColorValue.starts;
            var to = anim.ColorValue.ends;
            switch (anim.AnimatableComponent)
            {
                case AnimatableComponents.MeshRenderer:
                    newInterpolable = anim.meshRendererReferenceValue.sharedMaterial.DOColor(from, to, anim.Duration);
                    break;
                case AnimatableComponents.Image:
                    newInterpolable = anim.imageReferenceValue.DOColor(from, to, anim.Duration);
                    break;
                case AnimatableComponents.Text:
                    newInterpolable = anim.textReferenceValue.DOColor(from, to, anim.Duration);
                    break;
                case AnimatableComponents.SpriteRenderer:
                    newInterpolable = anim.spriteRendererReferenceValue.DOColor(from, to, anim.Duration);
                    break;
                case AnimatableComponents.Camera:
                    newInterpolable = anim.cameraReferenceValue.DOColor(from, to, anim.Duration);
                    break;
            }

            anim.interpolable = newInterpolable;
        }

        public bool IsValid()
        {
            if (anim.AnimatableComponent == AnimatableComponents.MeshRenderer)
            {
                return anim.meshRendererReferenceValue.sharedMaterial != null && anim.meshRendererReferenceValue.sharedMaterial.HasProperty("_Color");
            }

            return anim.AnimatableComponent == AnimatableComponents.Image
                        || anim.AnimatableComponent == AnimatableComponents.SpriteRenderer
                        || anim.AnimatableComponent == AnimatableComponents.Text;
        }

    }

}