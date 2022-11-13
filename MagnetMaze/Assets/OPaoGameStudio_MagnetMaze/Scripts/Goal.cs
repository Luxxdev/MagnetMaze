using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using LoLSDK;

namespace OPaoGameStudio_MagnetMaze
{
    public class Goal : MonoBehaviour
    {
        [SerializeField] private int nextScene;
        [SerializeField] private int progressCount;
        [SerializeField] GameData gameData;

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                gameData.completedLevels = nextScene;
                gameData.completedLevels = progressCount;
                LOLSDK.Instance.SaveState(gameData);
                LOLSDK.Instance.SubmitProgress(0, progressCount, 15);
                LoadNextScene(nextScene);
            }
        }

        public void LoadNextScene(int scene)
        {
            SceneManager.LoadScene($"LEVEL_{scene}"); //LoadScene("LEVEL_" + scene.ToString());
            return;
        }
        public void ReloadScene()
        {
            Scene actualScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(actualScene.name);
            return;
        }
    }
}
