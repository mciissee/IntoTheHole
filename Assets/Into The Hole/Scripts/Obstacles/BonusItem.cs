/************************************************************************************************************************************													   *
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS	                                                                *
*************************************************************************************************************************************/
namespace IntoTheHole
{
    using InfinityEngine;
    using InfinityEngine.ResourceManagement;
    using UnityEngine;

    /// <summary>
    ///     This is the component attached to bonus Items
    /// </summary>
    public class BonusItem : BaseItem
    {

        /// <summary>
        /// Invoked the player hit the bonus item.
        /// </summary>
        public override void OnCollideWithTheAvatar()
        {
            SoundManager.PlayEffect(R.audioclip.SoundPowerUp);
            controler.BonusCount++;
            PoolManager.Push(gameObject);
        }
    }
}