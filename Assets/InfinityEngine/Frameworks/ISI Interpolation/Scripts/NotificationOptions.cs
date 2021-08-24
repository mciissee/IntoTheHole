/*************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Infinity Interactive Unity Asset Store catalog: http://u3d.as/riS	                                                                *
*************************************************************************************************************************************/
using System;

namespace InfinityEngine.Interpolations
{


    /// <summary>
    /// Options to apply to a <see cref="QuickNotification"/> object.
    /// </summary>
    [Serializable]
    public class NotificationOptions
    {

        /// <summary>
        /// Gets or sets the options to apply to the interpolation created by the <see cref="QuickNotification"/> object
        /// when it appears on the screen
        /// </summary>
        public InterpolationOptions InterpolationOptions { get; set; }

        /// <summary>
        /// Gets or sets the time taken by the <see cref="QuickNotification"/> to appears on the screen.
        /// </summary>
        public float EntryDuration { get; set; }

        /// <summary>
        /// Gets or sets the time to wait before close the <see cref="QuickNotification"/> object.
        /// </summary>
        /// <remarks>
        /// This time is not linked to <see cref="EntryDuration"/>
        /// </remarks>
        public float CloseDelay { get; set; }
    }
}