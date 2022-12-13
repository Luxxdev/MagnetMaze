using UnityEngine;
using UnityEngine.SceneManagement;
using LoLSDK;

namespace OPaoGameStudio_MagnetMaze
{
    public class Goal : MonoBehaviour
    {
        [SerializeField] private int nextScene;
        public bool isCompleted = false;
        private bool reload = false;
        public int levelToLoad;
        public Animator animator;
        // [SerializeField] private int progressCount;

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Singleton.Instance.gameData.seenDialogs = 0;
                if (isCompleted)
                {
                    Singleton.Instance.gameData.playerProgress = 12;
                    LOLSDK.Instance.SubmitProgress(0, 12, 12);
                }
                if (nextScene > 5)
                {
                    Singleton.Instance.gameData.completedLevels = 5;
                }
                else
                {
                    Singleton.Instance.gameData.completedLevels = nextScene;
                    LOLSDK.Instance.SaveState(Singleton.Instance.gameData);
                }
                LoadNextScene(nextScene);
            }
        }

        public void LoadNextScene(int scene)
        {
            levelToLoad = scene;
            animator.SetTrigger("FadeOut");

            // if (!isCompleted)
            //     SceneManager.LoadScene($"LEVEL_{scene}"); //LoadScene("LEVEL_" + scene.ToString());
            // else
            // {
            //     SceneManager.LoadScene(scene);
            // }
            return;
        }
        public void ReloadScene()
        {
            reload = true;
            levelToLoad = SceneManager.GetActiveScene().buildIndex - 2;
            animator.SetTrigger("FadeOut");

            // Scene actualScene = SceneManager.GetActiveScene();
            // SceneManager.LoadScene(actualScene.name);
            // return;
        }

        public void onFadeComplete()
        {
            if (!isCompleted || reload)
                SceneManager.LoadScene($"LEVEL_{levelToLoad}");

            else
                SceneManager.LoadScene(levelToLoad);

        }
    }
}
