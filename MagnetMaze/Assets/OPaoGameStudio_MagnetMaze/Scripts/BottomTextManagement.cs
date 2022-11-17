using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using LoLSDK;

namespace OPaoGameStudio_MagnetMaze
{
    public class BottomTextManagement : MonoBehaviour
    {
        public TextAsset[] textJSONList;
        public TextAsset[] revisionTextsJSONList;
        public bool isDialogOpen = false;
        private string dialogueID;
        public string failMessage;
        public Image charExpression;
        public Image explanationImage;
        public bool isRevisionDialog = false;
        public Image darkenPanel;
        public Button retryBTN, nextBTN, yesBTN, noBTN;
        public TMPro.TextMeshProUGUI energyAlert;
        public AudioManager AUM;
        private int dialogCounter = 0;
        protected int phraseIndex = 0;
        protected float transTime = 0.5f;
        [SerializeField] protected RectTransform panelTransform;
        private Transform buttonsParent;
        public TMPro.TextMeshProUGUI textDisplay;
        private bool isPaused = false;
        protected Dictionary<string, Vector2> positions = new Dictionary<string, Vector2>(){
    {"hidden", new Vector2(0, -178)},
    {"onScreen", new Vector2(0, 0)}
   };

        [System.Serializable]
        public class Text
        {
            public string dialogueID;
            public string image;
            public string explanationImg;
        }

        [System.Serializable]
        public class TextList
        {
            public Text[] Text;
        }

        public TextList myTextList = new TextList();
        void Start()
        {
            buttonsParent = gameObject.transform.GetChild(6).transform.GetChild(0).transform.GetChild(0);
            for (int i = 0; i < Singleton.Instance.gameData.storedDialogs; i++)
            {
                buttonsParent.GetChild(i).gameObject.SetActive(true);
                TMPro.TextMeshProUGUI buttonsText = buttonsParent.GetChild(i).gameObject.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                buttonsText.text = SharedState.LanguageDefs["RD" + i.ToString()];
            }
            TMPro.TextMeshProUGUI nextText = nextBTN.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            TMPro.TextMeshProUGUI retryText = retryBTN.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            TMPro.TextMeshProUGUI yesText = yesBTN.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            TMPro.TextMeshProUGUI noText = noBTN.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            nextText.text = SharedState.LanguageDefs["next"];
            retryText.text = SharedState.LanguageDefs["reload"];
            yesText.text = SharedState.LanguageDefs["yes"];
            noText.text = SharedState.LanguageDefs["no"];
            energyAlert.text = SharedState.LanguageDefs["alert"];
        }

        public void CallDialog(int dialogNumber = 0)
        {
            if (!isRevisionDialog)
            {
                UpdateVisibleChildren();
                myTextList = JsonUtility.FromJson<TextList>(textJSONList[dialogCounter].text);
                dialogCounter += 1;
            }
            else
            {
                myTextList = JsonUtility.FromJson<TextList>(revisionTextsJSONList[dialogNumber].text);
            }
            AUM.Play("dialogUP");
            darkenPanel.DOFade(0.5f, transTime);
            charExpression.sprite = Resources.Load<Sprite>(myTextList.Text[0].image);
            charExpression.DOFade(1, 0.8f);
            dialogueID = myTextList.Text[0].dialogueID;
            textDisplay.text = SharedState.LanguageDefs[dialogueID];
            LOLSDK.Instance.SpeakText(dialogueID);
            panelTransform.DOAnchorPosY(positions["onScreen"].y, transTime);
            if (myTextList.Text[0].explanationImg != "")
            {
                explanationImage.sprite = Resources.Load<Sprite>(myTextList.Text[0].explanationImg);
                explanationImage.SetNativeSize();
                explanationImage.DOFade(1, 0.8f);
            }
            isPaused = true;
            isDialogOpen = true;
        }

        public void CloseDialog()
        {
            phraseIndex = 0;
            darkenPanel.DOFade(0f, transTime);
            panelTransform.DOAnchorPosY(positions["hidden"].y, transTime);
            charExpression.DOFade(0, 0.2f);
            explanationImage.DOFade(0, 0.2f);
            textDisplay.text = "";
            isPaused = false;
            isDialogOpen = false;
        }

        public void NextPhrase()
        {
            phraseIndex++;
            dialogueID = (int.Parse(dialogueID) + 1).ToString();
            if (phraseIndex >= myTextList.Text.Length)
            {
                CloseDialog();
                return;
            }
            AUM.Play("nextClick");
            charExpression.sprite = Resources.Load<Sprite>(myTextList.Text[phraseIndex].image);
            explanationImage.sprite = Resources.Load<Sprite>(myTextList.Text[phraseIndex].explanationImg);
            explanationImage.color = new Color(1, 1, 1, 0);
            explanationImage.SetNativeSize();
            if (myTextList.Text[phraseIndex].explanationImg != "")
            {
                explanationImage.color = new Color(1, 1, 1, 1);
            }
            textDisplay.text = SharedState.LanguageDefs[dialogueID];//myTextList.Text[phraseIndex].dialogue;
            LOLSDK.Instance.SpeakText(dialogueID);
        }

        public void UpdateVisibleChildren()
        {
            for (int i = 0; i < textJSONList.Length; i++)
            {
                print("textJSONList element: " + textJSONList[i].name);
                print("revisionTextsJSONList element: " + revisionTextsJSONList[Singleton.Instance.gameData.storedDialogs].name);
                print("dialog Counter is: " + dialogCounter);
                print("i = " + i);
                print("Stored dialogs: " + Singleton.Instance.gameData.storedDialogs);
                if (textJSONList[i].name == revisionTextsJSONList[Singleton.Instance.gameData.storedDialogs].name)
                {
                    print("passou");
                    buttonsParent.GetChild(Singleton.Instance.gameData.storedDialogs).gameObject.SetActive(true);
                    Singleton.Instance.gameData.storedDialogs += 1;
                    break;
                }
            }
        }

        // public void UpdateVisibleChildren(int comparedItem)
        // {

        //     if (textJSONList[dialogCounter].name == revisionTextsJSONList[comparedItem].name)
        //     {
        //         Singleton.Instance.gameData.storedDialogs += 1;
        //         buttonsParent.GetChild(comparedItem).gameObject.SetActive(true);
        //     }
        // }

        public void SetIsRevisionDialog(bool isRV) { isRevisionDialog = isRV; }
        public void SetIsPaused(bool pause) { isPaused = pause; }
        public bool GetIsPaused() { return isPaused; }

        public void CallRetry()
        {
            gameObject.transform.GetChild(4).gameObject.SetActive(false);
            retryBTN.gameObject.SetActive(true);
            nextBTN.gameObject.SetActive(false);
            AUM.Play("dialogUP");
            textDisplay.text = SharedState.LanguageDefs[failMessage];
            panelTransform.DOAnchorPosY(positions["onScreen"].y, transTime);
            darkenPanel.DOFade(0.5f, transTime);
            charExpression.sprite = Resources.Load<Sprite>("CHAR/normal");
            charExpression.DOFade(1, 0.8f);
        }
    }
}