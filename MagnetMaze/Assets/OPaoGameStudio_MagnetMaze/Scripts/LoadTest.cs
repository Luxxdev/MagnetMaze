using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SimpleJSON;
using TMPro;
using LoLSDK;

namespace OPaoGameStudio_MagnetMaze
{
    public class LoadTest : MonoBehaviour
    {
        JSONNode _langNode;
        [SerializeField] private Button newGameButton, continueButton;

        void Start()
        {
            Helper.StateButtonInitialize<Singleton.GameData>(newGameButton, continueButton, (Singleton.GameData saveData) =>
            {
                if (saveData != null)
                {
                    Singleton.Instance.gameData = saveData;
                    LoadNextScene(saveData.completedLevels);
                }
                else
                {
                    LoadNextScene(0);
                }
            }
            );
            TextDisplayUpdate();
        }

        void TextDisplayUpdate()
        {
            newGameButton.transform.GetChild(0).GetComponent<TMP_Text>().text = SharedState.LanguageDefs["newGame"];
            continueButton.transform.GetChild(0).GetComponent<TMP_Text>().text = SharedState.LanguageDefs["continue"];
        }
        public void LoadNextScene(int scene)
        {
            SceneManager.LoadScene($"LEVEL_{scene}");
            return;
        }
    }
}