using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SimpleJSON;
using TMPro;
using LoLSDK;

namespace OPaoGameStudio_MagnetMaze
{
    [System.Serializable]
    public class GameData
    {
        public int completedLevels = 0;
        public int playerProgress = 0;
    }
    public class LoadTest : MonoBehaviour
    {
        public GameData gameData;
        public Goal goal;
        JSONNode _langNode;
        [SerializeField] private Button newGameButton, continueButton;
        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("Starting game");
            Helper.StateButtonInitialize<GameData>(newGameButton, continueButton, (GameData saveData) =>
            {
                if (saveData != null)
                {
                    Debug.Log("Sorry continue hasn't been implementeed yet! Starting a New game instead");
                    // LoadNextScene(saveData.completedLevels);
                    LoadNextScene(saveData.completedLevels);
                }
                else
                {
                    Debug.Log("New game!");
                    LoadNextScene(0);
                }
            }
            );
            Debug.Log("pos chamar helper");
        }

        // Update is called once per frame
        void Update()
        {

        }
        private void OnLoad(GameData loadedGameData)
        {
            Debug.Log("Loading");
            if (loadedGameData != null)
            {
                gameData = loadedGameData;
                //continueButton.onClick.AddListener(gameData.completedLevels => goal.LoadNextScene);
            }
            newGameButton.onClick.AddListener(() =>
            {
                Debug.Log("calling Initial Scene");
                LoadNextScene(0);
            });
            // Initially set the text displays since the lang node should be populated.
            TextDisplayUpdate();
        }

        string GetText(string key)
        {
            string value = _langNode?[key];
            return value ?? "--missing--";
        }

        void TextDisplayUpdate()
        {
            newGameButton.transform.GetChild(0).GetComponent<TMP_Text>().text = GetText("newGame");
            continueButton.transform.GetChild(0).GetComponent<TMP_Text>().text = GetText("continue");
        }
        public void LoadNextScene(int scene)
        {
            SceneManager.LoadScene($"LEVEL_{scene}"); //LoadScene("LEVEL_" + scene.ToString());
            return;
        }
    }
}