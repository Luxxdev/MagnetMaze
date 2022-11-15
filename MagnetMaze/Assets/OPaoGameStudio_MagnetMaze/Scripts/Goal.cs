using UnityEngine;
using UnityEngine.SceneManagement;
using LoLSDK;

namespace OPaoGameStudio_MagnetMaze
{
    public class Goal : MonoBehaviour
    {
        [SerializeField] private int nextScene;
        // [SerializeField] private int progressCount;

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Singleton.Instance.gameData.completedLevels = nextScene;
                LOLSDK.Instance.SaveState(Singleton.Instance.gameData);
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
