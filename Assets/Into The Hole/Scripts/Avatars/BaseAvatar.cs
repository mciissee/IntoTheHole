/************************************************************************************************************************************													   *
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS	                                                                *
*************************************************************************************************************************************/
namespace IntoTheHole
{
    using UnityEngine;

    /// <summary>
    ///   Base class for all avatar
    /// </summary>
    public abstract class BaseAvatar : MonoBehaviour
    {

        private Controler controler;
        private BaseItem baseItem;

        void OnEnable()
        {
            controler = controler ?? FindObjectOfType<Controler>();

            controler.RegisterEvent(GameEvent.Start, OnGameStart);
            controler.RegisterEvent(GameEvent.End, OnGameOver);
        }


        /// <summary>
        /// Called when this component is disable
        /// </summary>
        protected virtual void OnDisable()
        {
            controler.UnRegisterEvent(GameEvent.Start, OnGameStart);
            controler.UnRegisterEvent(GameEvent.End, OnGameOver);
        }

        private void OnTriggerEnter(Collider c)
        {
            if (c.CompareTag("Item"))
            {
                baseItem = c.transform.parent.parent.GetComponent<BaseItem>();
                baseItem.OnCollideWithTheAvatar();
            }
        }

        /// <summary>
        /// React to game start
        /// </summary>
        public abstract void OnGameStart();

        /// <summary>
        /// React to gameover
        /// </summary>
        public abstract void OnGameOver();

    }
}