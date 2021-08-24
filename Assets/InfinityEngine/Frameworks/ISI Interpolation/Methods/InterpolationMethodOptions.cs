/************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Infinity Interactive Unity Asset Store catalog: http://u3d.as/riS	                                                                *
************************************************************************************************************************************/

using System;
using InfinityEngine.Attributes;

namespace InfinityEngine.Interpolations
{
    [Serializable]
    [CustomDrawer]
    public class InterpolationMethodOptions
    {
        [Message("If sets to true, the interpolation will be paused when the gameobject is not visible", MessageTypes.Info)]
        public bool disableOnHide;

        [Message("If sets to true, the interpolation will be paused when the application is on pause (Time.timeScale == 0)", MessageTypes.Info)]
        public bool disableOnPause;

        [Message("If sets to true, the gameobject will be deactivated at the end of the interpolation", MessageTypes.Info)]
        public bool hideGameObjectAtEnd;

        [Message("The options of the interpolation", MessageTypes.Info)]
        public InterpolationOptions options;

        public void ApplyOptionsToInterpolable(Interpolable interpolable)
        {
            interpolable.SetDisableOnHide(disableOnHide);
            interpolable.SetDisableOnPause(disableOnPause);
            interpolable.SetOptions(options);
        }

    }
}