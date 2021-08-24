/************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS                                                                                      *
*************************************************************************************************************************************/

using UnityEngine;
using InfinityEngine.Extensions;

namespace InfinityEngine.Interpolations
{

    internal class TranslationAnim : AnimCreator
    {
        private Anim anim;

        public TranslationAnim(Anim anim)
        {
            this.anim = anim;
        }

        public void Reset()
        {
            var rectTransform = anim.RectTransform;
            if (rectTransform)
            {
                rectTransform.anchoredPosition3D = anim.Vector3Value.starts;
            }
            else
            {
                anim.Transform.position = anim.Vector3Value.starts;
            }   
        }

        public  void Create()
        {
            if (!anim.HasTransform)
                return;

            var from = anim.Vector3Value.starts;
            var to = anim.Vector3Value.ends;
            anim.interpolable = anim.HasRectTransform  ? anim.RectTransform.DOMoveAnchor3D(from, to, anim.Duration)  : anim.Transform.DOMoveLocal(from, to, anim.Duration);
        }
        
        public bool IsValid()
        {
            return anim.Transform != null;
        }

    }
}