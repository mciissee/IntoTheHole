/************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS                                                                                      *
*************************************************************************************************************************************/

using UnityEngine;
using System;
using InfinityEngine.Extensions;

namespace InfinityEngine.Interpolations
{
    internal class ScaleAnim : AnimCreator
    {
        private Anim anim;

        public ScaleAnim(Anim anim)
        {
            this.anim = anim;
        }

        public void Reset()
        {
            if (!anim.HasTransform)
            {
                return;
            }

            anim.Transform.localScale = anim.Vector3Value.starts;
        }

        public void Create()
        {
            var from = anim.Vector3Value.starts;
            var to = anim.Vector3Value.ends;

            anim.interpolable = anim.Transform.DOScale(from, to, anim.Duration);
        }

        public bool IsValid()
        {
            return anim.Transform != null;
        }

    }

}