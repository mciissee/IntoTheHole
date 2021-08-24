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
    ///   Base class for all item in a <see cref="Pipe"/>
    /// </summary>
    public abstract class BaseItem : MonoBehaviour
    {

        #region Fields

        private Transform rotater;

        /// <summary>
        /// Reference to the instance <see cref="Controler"/> class.
        /// </summary>
        protected Controler controler;


        /// <summary>
        /// The prefab of the item
        /// </summary>
        [SerializeField]
        protected GameObject prefab;

        #endregion Fields

        /// <summary>
        /// Called when the item is generated
        /// </summary>
        protected virtual void Start()
        {
            controler = controler ?? FindObjectOfType<Controler>();
        }

        /// <summary>
        /// Initializes the item
        /// </summary>
        public virtual void OnInit() { }

        /// <summary>
        /// Called when the player collids an item
        /// </summary>
        /// <param name="avatar">The avatar</param>
        public abstract void OnCollideWithTheAvatar();

        /// <summary>
        /// Generate items into the given <paramref name="pipe"/>
        /// </summary>
        /// <param name="pipe"></param>
        /// <param name="curveRotation"></param>
        /// <param name="ringRotation"></param>
        public void Position(Pipe pipe, float curveRotation, float ringRotation)
        {
            rotater = rotater ?? transform.GetChild(0);

            transform.SetParent(pipe.transform, false);
            transform.localRotation = Quaternion.Euler(0f, 0f, -curveRotation);
            rotater.localPosition = new Vector3(0f, pipe.CurveRadius);
            rotater.localRotation = Quaternion.Euler(ringRotation, 0f, 0f);
            OnInit();
        }
    }


}