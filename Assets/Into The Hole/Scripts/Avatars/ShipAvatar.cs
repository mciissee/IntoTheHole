/************************************************************************************************************************************													   *
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS	                                                                *
*************************************************************************************************************************************/
namespace IntoTheHole
{
    using UnityEngine;
    using System.Collections;
    using System;

    /// <summary>
    /// Component for Ship avatar.
    /// </summary>
    public class ShipAvatar : BaseAvatar
    {

        [SerializeField]
        private GameObject shape;

        private void Start()
        {
            shape.SetActive(false);
        }

        /// <summary>
        /// React to game starts event.
        /// </summary>
        public override void OnGameStart()
        {
            shape.SetActive(true);
            gameObject.SetActive(true);
        }

        /// <summary>
        /// React to game ends event.
        /// </summary>
        public override void OnGameOver()
        {
            shape.SetActive(false);
        }
    }
}