/************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS	                                                                                    *
*************************************************************************************************************************************/

namespace IntoTheHole
{
    using InfinityEngine;
    using UnityEngine;
    using Random = UnityEngine.Random;

    /// <summary>
    ///   Represents a Rotatif Trap Item 
    /// </summary>
    public class RotatifTrap : BaseItem
    {
        /// <summary>
        /// The probability to generates rotate the item
        /// </summary>
        [Range(0, 1)]
        public float rotationProbability;
        /// <summary>
        /// Min rotation speed of the item
        /// </summary>
        public float minRotationSpeed;
        /// <summary>
        /// Max roation speed of the item
        /// </summary>
        public float maxRotationSpeed;

        private AutoRotate rotationScript;

        private float rotationSpeed;

        /// <summary>
        /// Initializes the item.
        /// </summary>
        public override void OnInit()
        {
            base.OnInit();
            rotationScript = rotationScript ?? transform.GetChild(0).GetComponent<AutoRotate>();

            rotationScript.gameObject.SetActive((Random.value <= rotationProbability));

            rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);

            rotationScript.speed = ((int)(Random.value * 100)) % 2 == 0 ? rotationSpeed : -rotationSpeed;

            rotationScript.rotate = true;
        }

        /// <summary>
        /// Called when the player it the item
        /// </summary>
        public override void OnCollideWithTheAvatar() => controler.OnEnd();
    }
}