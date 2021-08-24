/************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS	                                                                                    *
*************************************************************************************************************************************/
namespace IntoTheHole
{
    using UnityEngine;

    /// <summary>
    ///    Component for Particle System avatar
    /// </summary>
    public class ParticleAvatar : BaseAvatar
    {

        #region Fields

        [SerializeField]
        private ParticleSystem shape;
        [SerializeField]
        private ParticleSystem trail;
        [SerializeField]
        private ParticleSystem burst;
        [SerializeField]
        private float deathCountdown = -1f;

        #endregion Fields

        #region Unity

        private void Start()
        {
            Reset(false);
        }

        private void Update()
        {
            if (deathCountdown >= 0f)
                deathCountdown -= Time.deltaTime;
        }

        #endregion Unity

        #region Game Events

        /// <summary>
        /// Reacts to game starts event
        /// </summary>
        public override void OnGameStart()
        {
            Reset(true);
        }

        /// <summary>
        /// Reacts to game ends event
        /// </summary>
        public override void OnGameOver()
        {
            if (deathCountdown < 0f)
            {
                Reset(false);
            }
        }

        #endregion Game Events

        /// <summary>
        /// Re active player avatar shape.
        /// </summary>
        private void Reset(bool active = true)
        {
            if (active)
            {
                deathCountdown = -1f;
            }
            else
            {
                burst.Emit(burst.main.maxParticles);
                deathCountdown = burst.main.startLifetime.constant;
            }
            shape.gameObject.SetActive(active);
            trail.gameObject.SetActive(active);
            burst.gameObject.SetActive(active);
        }

    }
}