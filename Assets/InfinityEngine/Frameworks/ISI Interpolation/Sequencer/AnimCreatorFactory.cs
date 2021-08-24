/************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS                                                                                      *
*************************************************************************************************************************************/

namespace InfinityEngine.Interpolations
{
    internal static class AnimCreatorFactory
    {
        public static AnimCreator Create(Anim anim)
        {
            switch (anim.AnimationType)
            {
                case AnimationTypes.Translation:
                    return new TranslationAnim(anim);
                case AnimationTypes.Rotation:
                    return new RotationAnim(anim);
                case AnimationTypes.Scale:
                    return new ScaleAnim(anim);
                case AnimationTypes.Fade:
                    return new FadeAnim(anim);
                case AnimationTypes.Color:
                    return new ColorAnim(anim);
                default:
                    return null;
            }
        }

    }
}
