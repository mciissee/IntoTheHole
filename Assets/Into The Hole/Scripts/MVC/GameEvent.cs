/************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS	                                                                                    *
*************************************************************************************************************************************/

#pragma warning disable RECS0020 // Delegate subtraction has unpredictable result

using UnityEngine;

namespace IntoTheHole
{
    /// <summary>
    /// Game events
    /// </summary>
    [SerializeField]
    public enum GameEvent
    {
        /// <summary>
        /// Game starts event
        /// </summary>
        Start,

        /// <summary>
        /// Game ends event
        /// </summary>
        End,
    }
}