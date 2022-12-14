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
        private bool isFinished = false;
        private bool canPass = false;
        private string dialogueID;
        public string failMessage;
        public Image charExpression;
        public Image explanationImage;
        public bool isRevisionDialog = false;
        public Image darkenPanel;
        public Button retryBTN, nextBTN, controlsBTN, backBTN;
        public AudioManager AUM;
        private int dialogCounter = 0;
        protected int phraseIndex = 0;
        protected float transTime = 0.5f;
        [SerializeField] protected RectTransform panelTransform;
        private Transform buttonsParent;
        public TMPro.TextMeshProUGUI textDisplay;
        private bool isPaused = false;
        public Goal goalScript;
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
            dialogCounter = Singleton.Instance.gameData.seenDialogs;
            buttonsParent = gameObject.transform.GetChild(7).transform.GetChild(0).transform.GetChild(0);
            for (int i = 0; i < Singleton.Instance.gameData.storedDialogs; i++)
            {
                buttonsParent.GetChild(i).gameObject.SetActive(true);
                TMPro.TextMeshProUGUI buttonsText = buttonsParent.GetChild(i).gameObject.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                buttonsText.text = SharedState.LanguageDefs["RD" + i.ToString()];
            }
            TMPro.TextMeshProUGUI nextText = nextBTN.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            TMPro.TextMeshProUGUI retryText = retryBTN.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            TMPro.TextMeshProUGUI controlsText = controlsBTN.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            TMPro.TextMeshProUGUI backText = backBTN.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            nextText.text = SharedState.LanguageDefs["next"];
            retryText.text = SharedState.LanguageDefs["reload"];
            controlsText.text = SharedState.LanguageDefs["RD-1"];
            backText.text = SharedState.LanguageDefs["back"];
        }

        void Update()
        {
            if (Input.GetButtonDown("Submit"))
            {
                if (retryBTN.IsActive())
                    goalScript.ReloadScene();
                else if (isDialogOpen)
                    NextPhrase();
            }
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
            StartCoroutine(WaitDialog());
        }

        public void CloseDialog()
        {
            isDialogOpen = false;
            darkenPanel.DOFade(0f, transTime);
            panelTransform.DOAnchorPosY(positions["hidden"].y, transTime);
            charExpression.DOFade(0, 0.2f);
            explanationImage.DOFade(0, 0.2f);
            textDisplay.text = "";
            isPaused = false;
            ((ILOLSDK_EXTENSION)LOLSDK.Instance.PostMessage).CancelSpeakText();
            retryBTN.gameObject.SetActive(false);
            backBTN.gameObject.SetActive(false);
            nextBTN.gameObject.SetActive(true);
            gameObject.transform.GetChild(4).gameObject.SetActive(true);
            phraseIndex = 0;
        }

        public void NextPhrase()
        {
            if (isDialogOpen && canPass)
            {
                canPass = false;
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
                textDisplay.text = SharedState.LanguageDefs[dialogueID];
                LOLSDK.Instance.SpeakText(dialogueID);
                StartCoroutine(WaitDialog());
            }
        }

        public void UpdateVisibleChildren()
        {
            Singleton.Instance.gameData.seenDialogs += 1;
            for (int i = 0; i < textJSONList.Length; i++)
            {
                if (textJSONList[i].name == revisionTextsJSONList[Singleton.Instance.gameData.storedDialogs].name)
                {
                    buttonsParent.GetChild(Singleton.Instance.gameData.storedDialogs).gameObject.SetActive(true);
                    TMPro.TextMeshProUGUI buttonsText = buttonsParent.GetChild(Singleton.Instance.gameData.storedDialogs).gameObject.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                    buttonsText.text = SharedState.LanguageDefs["RD" + Singleton.Instance.gameData.storedDialogs.ToString()];
                    if (Singleton.Instance.gameData.storedDialogs < 7)
                    {
                        Singleton.Instance.gameData.storedDialogs += 1;
                    }
                    break;
                }
            }
        }

        public void SetIsRevisionDialog(bool isRV) { isRevisionDialog = isRV; }
        public void SetIsPaused(bool pause) { isPaused = pause; }
        public bool GetIsPaused() { return isPaused; }

        public void CallRetry()
        {
            if (!isDialogOpen)
            {
                Singleton.Instance.gameData.stagePoints = 0;
                isPaused = true;
                gameObject.transform.GetChild(4).gameObject.SetActive(false);
                retryBTN.gameObject.SetActive(true);
                backBTN.gameObject.SetActive(true);
                nextBTN.gameObject.SetActive(false);
                AUM.Play("dialogUP");
                textDisplay.text = SharedState.LanguageDefs[failMessage];
                panelTransform.DOAnchorPosY(positions["onScreen"].y, transTime);
                darkenPanel.DOFade(0.5f, transTime);
                charExpression.sprite = Resources.Load<Sprite>("CHAR/normal");
                charExpression.DOFade(1, 0.8f);
            }
        }
        IEnumerator<WaitForSeconds> WaitDialog()
        {
            yield return new WaitForSeconds(1.2f);
            canPass = true;
        }
    }
}