/************************************************************************************************************************************													   *
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS	                                                                *
*************************************************************************************************************************************/
namespace IntoTheHole
{
    using System;
    using UnityEngine;

    /// <summary>
    ///   Represents a Trap Item 
    /// </summary>
    public class TrapItem : BaseItem
    {
        /// <summary>
        /// Invoked when the player hit the trap
        /// </summary>
        public override void OnCollideWithTheAvatar() => controler.OnEnd();
    }
}