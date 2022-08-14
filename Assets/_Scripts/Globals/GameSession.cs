using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameControllers
{
    public class GameSession : Singleton<GameSession>
    {

        #region Private Attributes

        private static long startTime;
        private static List<GameState> gameStateScripts;

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

        #region Public Methods

        public static void CacheScripts()
        {
            gameStateScripts = FindObjectsOfType<GameState>().ToList();
        }

        public static void StartGame()
        {
            foreach (GameState gamePaused in gameStateScripts)
            {
                gamePaused.OnGameStart();
            }
        }

        public static void EndGame()
        {
            foreach (GameState gamePaused in gameStateScripts)
            {
                gamePaused.OnGameEnd();
            }
        }

        public static void PauseGame()
        {
            foreach (GameState gamePaused in gameStateScripts)
            {
                gamePaused.OnGamePause();
            }
        }

        public static void ResumeGame()
        {
            foreach (GameState gamePaused in gameStateScripts)
            {
                gamePaused.OnGameResume();
            }
        }

        public static void PlayerDied()
        {
            foreach (GameState gamePaused in gameStateScripts)
            {
                gamePaused.OnPlayerDied();
            }
        }

        public static void ClearCache()
        {
            onGameStartEvents = null;
            OnGameScoresChanged = null;
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
            gameStateScripts = FindObjectsOfType<GameState>().ToList();
            foreach (GameState gamePaused in gameStateScripts)
            {
                gamePaused.GameFocusChanged(focus);
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
        public virtual void GameFocusChanged(bool isPaused) { }
        public virtual void OnGameStart() { }
        public virtual void OnGameEnd() { }
        public virtual void OnGamePause() { }
        public virtual void OnGameResume() { }
        public virtual void OnPlayerDied() { }
    }
}