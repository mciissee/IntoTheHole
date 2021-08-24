/************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS	                                                                                    *
*************************************************************************************************************************************/
using UnityEngine;
using System;
using UnityEngine.UI;
using InfinityEngine;
using InfinityEngine.Serialization;
using InfinityEngine.Interpolations;
using InfinityEngine.DesignPatterns;
using InfinityEngine.Localization;
using InfinityEngine.Attributes;

namespace IntoTheHole
{
    /// <summary>
    /// View component of MVC Design pattern.
    /// This component represents contain only user interface elements.
    /// </summary>
    [OverrideInspector]
    public class View : MonoBehaviour, Observer
    {

        #region Fields
        [Accordion("Menu Animators")]
        [SerializeField]
        private DOScaleMethod homeUI;
        [Accordion("Menu Animators")]
        [SerializeField]
        private DOScaleMethod hudUI;
        [Accordion("Menu Animators")]
        [SerializeField]
        private DOScaleMethod endGameUI;

        [Accordion("Setting")]
        [SerializeField]
        private Toggle touchToogle;
        [Accordion("Setting")]
        [SerializeField]
        private Toggle accelerometerToogle;

        [Accordion("HUD")]
        [SerializeField]
        private Text velocityLabel;

        [Accordion("HUD")]
        [SerializeField]
        private Text distanceLabel;

        [Accordion("GameOver")]
        [SerializeField]
        private Text scoreLabel;

        [Accordion("GameOver")]
        [SerializeField]
        private Text bestScoreLabel;

        [Accordion("GameOver")]
        [SerializeField]
        private Text totalScoreLabel;

        [Accordion("GameOver")]
        [SerializeField]
        private Text recordLabel;

        private Controler controler;
        private Player model;

        #endregion Fields

        #region Unity

        /// <summary>
        /// Registers this <see cref="OnStart"/> and <see cref="OnEnd"/> callback's to <see cref="Controler.GameStartedEvent"/>
        /// and <see cref="Controler.GameEndedEvent"/>
        /// </summary>
        void OnEnable()
        {
            controler = controler ?? FindObjectOfType<Controler>();
            controler.RegisterEvent(GameEvent.Start, OnStart);
            controler.RegisterEvent(GameEvent.End, OnEnd);
        }

        /// <summary>
        /// Removes this <see cref="OnStart"/> and <see cref="OnEnd"/> callback's to <see cref="Controler.GameStartedEvent"/>
        /// and <see cref="Controler.GameEndedEvent"/>
        /// </summary>
        void OnDisable()
        {
            controler.UnRegisterEvent(GameEvent.Start, OnStart);
            controler.UnRegisterEvent(GameEvent.End, OnEnd);
        }

        #endregion Unity

        #region Game Events

        /// <summary>
        /// This method is called when <see cref="Controler.OnStart"/> method is invoked
        /// </summary>
        void OnStart()
        {
            hudUI.Invoke();
        }

        /// <summary>
        /// This method is called when <see cref="Controler.OnEnd"/> method is invoked
        /// </summary>
        void OnEnd()
        {
            // close the hud menu
            hudUI.InvokeReverse();

            var distance = (int)(model.Distance * 10f);

            R2.integers.Score.Value = distance;
            R2.integers.TotalScore.Value += distance;

            var isBestScore = ScoreManager.SetIFBestScore(R2.integers.BestScore.Key, distance);

            recordLabel.gameObject.SetActive(isBestScore);

            scoreLabel.text = R3.strings.SCORE.Format(distance);
            bestScoreLabel.text = R3.strings.BEST.Format(R2.integers.BestScore.Value);
            totalScoreLabel.text = R3.strings.TOTAL.Format(R2.integers.TotalScore.Value);

            VP.Save();
            endGameUI.Invoke();
        }

        #endregion Game Events

        #region Gameplay

        /// <summary>
        /// Hides home menu
        /// </summary>
        public void HideHome()
        {
            if (homeUI.isActiveAndEnabled)
            {
                homeUI.InvokeReverse();
            }
        }

        #endregion Gameplay

        #region MVC

        /// <summary>
        /// Called when the model <see cref="Player"/> has changed
        /// </summary>
        /// <param name="obj">The model</param>
        public void OnChanged(object obj)
        {
            model = obj as Player;
            velocityLabel.text = string.Format("{0} m/s", Math.Round(model.Velocity * 10f, 2));
            distanceLabel.text = string.Format("{0} m", (int)(model.Distance * 10f));
            if (model.ModeMoveChanged)
            {
                touchToogle.isOn = model.EnableTouchMove;
                accelerometerToogle.isOn = model.EnableAccelerometer;
            }
        }

        /// <summary>
        /// Changes the model that is linked to this view.
        /// </summary>
        /// <param name="model">The new model</param>
        public void SetModel(Player model)
        {
            if (!model.HasObserver(this))
            {
                model.AddObserver(this);
            }
            this.model = model;
        }

        #endregion MVC

    }

}