/************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS	                                                                                    *
*************************************************************************************************************************************/

#pragma warning disable RECS0020 // Delegate subtraction has unpredictable result

using UnityEngine;
using System;
using InfinityEngine;
using InfinityEngine.Extensions;
using InfinityEngine.Localization;
using InfinityEngine.Serialization;
using InfinityEngine.Attributes;

namespace IntoTheHole
{
    /// <summary>
    /// Controler component of MVC design pattern
    /// </summary>
    [OverrideInspector]
    public class Controler : MonoBehaviour
    {

        #region Fields
        [InfinityHeader]
        [SerializeField]
        private bool __help__; // don't changes the name

        [Accordion("Players")]
        [Message("Avatars that the player can use to play", MessageTypes.Info)]
        [SerializeField]
        private Player[] players;

        [Accordion("Pipe Material Animation")]
        [Message("The material used by the pipe objects (the color will be animated)", MessageTypes.Info)]
        [SerializeField]
        private Material pipeMaterial;

        [Accordion("Pipe Material Animation")]
        [Message("The time interval between the color exchange of the material", MessageTypes.Info)]
        [SerializeField]
        private float switchInterval;

        [Accordion("Pipe Material Animation")]
        [Message("The time taken to change the color of the material", MessageTypes.Info)]
        [SerializeField]
        private float switchDuration;

        [Accordion("Pipe Material Animation")]
        [Message("The different colors that the material can take", MessageTypes.Info)]
        [SerializeField]
        private Color[] backgroundColors;

        private Player model;
        private View view;

        private Action GameStartedEvent;
        private Action GameEndedEvent;

        private int currentPlayer;

        #endregion Fields

        #region Unity

        void Start()
        {

            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Application.targetFrameRate = 60;
            currentPlayer = 0;

            model = players[currentPlayer];
            view = FindObjectOfType<View>();
            view.SetModel(model);

            if (backgroundColors == null || backgroundColors.Length == 0)
            {
                pipeMaterial.DORandomColor(switchInterval, switchDuration).Start();
            }
            else
            {
                pipeMaterial.DOPickColor(backgroundColors, switchInterval, switchDuration).Start();
            }

            // register the callback only after the initialization of ISILocalization plugin.
            Infinity.When(() => ISILocalization.IsInitialized, () => BackButtonManager.SetActionExit(R3.strings.PRESS_TO_QUIT));
        }

        void OnApplicationQuit()
        {
            pipeMaterial.color = Color.white;
        }

        #endregion Unity

        #region Game Events

        /// <summary>
        /// Registers new Game Event 
        /// </summary>
        /// <param name="evt">Event type</param>
        /// <param name="action">Action to do when the event is trigerred</param>
        public void RegisterEvent(GameEvent evt, Action action)
        {
            switch (evt)
            {
                case GameEvent.Start:
                    GameStartedEvent += action;
                    break;
                case GameEvent.End:
                    GameEndedEvent += action;
                    break;
            }
        }

        /// <summary>
        /// Removes an existing Game Event 
        /// </summary>
        /// <param name="evt">Event type</param>
        /// <param name="action">Action to remove</param>
        public void UnRegisterEvent(GameEvent evt, Action action)
        {
            switch (evt)
            {
                case GameEvent.Start:
                    GameStartedEvent -= action;
                    break;
                case GameEvent.End:
                    GameEndedEvent -= action;
                    break;
            }

        }

        /// <summary>
        /// On Start callback.
        /// When this function is called, <see cref="GameEvent.Start"/> will be raised and every registered action will be invoked
        /// </summary>
        public void OnStart()
        {
            view.HideHome();
            Tutorial.Show(R2.bools.IsShowTutorial.Key, TutorialCallback);
        }

        /// <summary>
        /// On Start callback.
        /// When this function is called, <see cref="GameEvent.End"/> will be raised and every registered action will be invoked
        /// </summary>
        public void OnEnd()
        {
            GameEndedEvent?.Invoke();
        }

        #endregion Game Events

        #region GamePlay

        private void TutorialCallback()
        {
            GameStartedEvent?.Invoke();
            if (!SoundManager.IsPlayingMusic)
            {
                SoundManager.PlayMusic();
            }
        }

        public void PlayMusic()
        {
            SoundManager.PlayMusic();
        }
        /// <summary>
        /// The number of bonus of the player.
        /// </summary>
        public int BonusCount
        {
            get => model.BonusCount;
            set => model.BonusCount = value;
        }

        /// <summary>
        /// Changes the player avatar and updates the model of <see cref="View"/>.
        /// </summary>
        public void ChangePlayer()
        {
            model.gameObject.SetActive(false);
            currentPlayer++;
            if (currentPlayer >= players.Length)
            {
                currentPlayer = 0;
            }
            model = players[currentPlayer];
            model.gameObject.SetActive(true);
            view.SetModel(model);
        }

        /// <summary>
        /// Enable touch move mode
        /// </summary>
        public bool EnableTouchMove { set => model.EnableTouchMove = value; }

        /// <summary>
        /// Toggle accelerometer move mode.
        /// </summary>
        public bool EnableAccelerometer { set => model.EnableAccelerometer = value; }

        #endregion GamePlay

    }
}