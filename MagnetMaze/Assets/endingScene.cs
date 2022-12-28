using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using LoLSDK;

namespace OPaoGameStudio_MagnetMaze
{
    public class endingScene : MonoBehaviour
    {
        [SerializeField] private Button finishButton;
        [SerializeField] private Button bonusButton;
        [SerializeField] private TMPro.TextMeshProUGUI endText;
        // Start is called before the first frame update
        void Start()
        {
            TextDisplayUpdate();
        }

        // Update is called once per frame
        void TextDisplayUpdate()
        {
            if (Singleton.Instance.gameData.completedLevels == 6)
            {
                bonusButton.gameObject.SetActive(false);
                endText.GetComponent<TMP_Text>().text = SharedState.LanguageDefs["completedText2"];
                LOLSDK.Instance.SpeakText("completedText2");

            }
            else
            {
                endText.GetComponent<TMP_Text>().text = SharedState.LanguageDefs["completedText1"];
                LOLSDK.Instance.SpeakText("completedText1");
            }


            bonusButton.transform.GetChild(0).GetComponent<TMP_Text>().text = SharedState.LanguageDefs["bonus"];
            finishButton.transform.GetChild(0).GetComponent<TMP_Text>().text = SharedState.LanguageDefs["finish"];


        }
    }
}
