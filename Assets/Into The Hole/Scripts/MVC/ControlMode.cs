/************************************************************************************************************************************
* Developed by Mamadou Cisse and Jasper Flick                                                                                       *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS	                                                                                    *
*************************************************************************************************************************************/
namespace IntoTheHole
{
    /// <summary>
    /// Game movements mode
    /// </summary>
    public enum ControlMode
    {
        /// <summary>
        /// Move by touching the screen
        /// </summary>
        Touch,

        /// <summary>
        /// Move thanks to the accelerometer
        /// </summary>
        Accelerometer,

        /// <summary>
        /// Move thanks to the accelerometer and by touching the screen
        /// </summary>
        Both
    }
}
