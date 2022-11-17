using UnityEngine;
using UnityEngine.SceneManagement;
using LoLSDK;

namespace OPaoGameStudio_MagnetMaze
{
    public class Goal : MonoBehaviour
    {
        [SerializeField] private int nextScene;
        public bool isCompleted = false;
        // [SerializeField] private int progressCount;

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (isCompleted)
                {
                    Singleton.Instance.gameData.playerProgress = 12;
                    LOLSDK.Instance.SubmitProgress(0, 12, 12);
                }
                Singleton.Instance.gameData.completedLevels = nextScene;
                LOLSDK.Instance.SaveState(Singleton.Instance.gameData);
                LoadNextScene(nextScene);
            }
        }

        public void LoadNextScene(int scene)
        {
            if (!isCompleted)
                SceneManager.LoadScene($"LEVEL_{scene}"); //LoadScene("LEVEL_" + scene.ToString());
            else
                SceneManager.LoadScene(scene);
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
