using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OPaoGameStudio_MagnetMaze
{
    public class Singleton : MonoBehaviour
    {
        [System.Serializable]
        public class GameData
        {
            public int storedDialogs = 0;
            public int completedLevels = 0;
            public int playerProgress = 0;
        }
        public GameData gameData = new GameData();
        public static Singleton Instance;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
            DontDestroyOnLoad(gameObject);
        }

        public GameData GetGameData()
        {
            return gameData;
        }
        public int GetPlayerProgress()
        {
            return gameData.playerProgress;
        }
        public void SetPlayerProgress(int value)
        {
            gameData.playerProgress = value;
        }
        public int GetPlayerCompletedLevels()
        {
            return gameData.playerProgress;
        }
        public void SetPlayerCompletedLevels(int value)
        {
            gameData.completedLevels = value;

        }
    }
}