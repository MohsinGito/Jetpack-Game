using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameSession
{
    public class GameSession : Singleton<GameSession>
    {

        #region Private Attributes

        private static long startTime;
        private List<GameState> gamePausedList;

        private static event Action onGameStartEvents;
        private static event Action onGameEndStartEvents;
        private static event Action<int> onGameCoinsChanged;
        private static event Action<int> onGameScoresChanged;

        #endregion

        #region Public Properties

        public static long StartTime
        {
            get { return startTime; }
        }

        public static long CurrenTimeSincePlayed
        {
            get { return GetCurrentTimeInSeconds(); }
        }

        public static float GameFPS
        {
            get { return GetCurrentFPS(); }
        }

        public static Action<int> OnGameCoinsChanged
        {
            set { onGameCoinsChanged += value; }
            get { return onGameCoinsChanged; }
        }

        public static Action<int> OnGameScoresChanged
        {
            set { onGameScoresChanged += value; }
            get { return onGameScoresChanged; }
        }

        public static Action OnGameStart
        {
            set { onGameStartEvents += value; }
            get { return onGameStartEvents; }
        }

        public static Action OnGameEnd
        {
            set { onGameEndStartEvents += value; }
            get { return onGameEndStartEvents; }
        }

        #endregion

        #region Unity Methods

        private void Start()
        {
            startTime = GetCurrentTimeInSeconds();
        }

        private void OnApplicationFocus(bool focus)
        {
            NotifyGameStateChanged(!focus);
        }

        #endregion

        #region Private Methods

        private static long GetCurrentTimeInSeconds()
        {
            var newTime = new DateTimeOffset(DateTime.UtcNow);
            return newTime.ToUnixTimeSeconds();
        }

        private void NotifyGameStateChanged(bool focus)
        {
            gamePausedList = FindObjectsOfType<GameState>().ToList();
            foreach (GameState gamePaused in gamePausedList)
            {
                gamePaused.GameStateChanged(focus);
            }
        }

        private static float GetCurrentFPS()
        {
            return Time.frameCount / Time.time;
        }

        #endregion

    }

    public abstract class GameState : MonoBehaviour
    {
        public abstract void GameStateChanged(bool isPaused);
    }
}