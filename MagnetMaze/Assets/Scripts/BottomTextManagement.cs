using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using DG.Tweening;

public class BottomTextManagement : MonoBehaviour
{
   public TextAsset[] textJSONList;
   public string failMessage;
   public Image charExpression;
   public Image explanationImage;
   public Image darkenPanel;
   public Button retryBTN;
   public Button nextBTN;
   public AudioManager AUM;
   private int dialogCounter = 0;
   protected int phraseIndex = 0;
   protected float transTime = 0.5f;
   [SerializeField] protected RectTransform panelTransform;
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
      public string dialogue;
      public string explanationImg;
   }

   [System.Serializable]
   public class TextList
   {
      public Text[] Text;
   }

   public TextList myTextList = new TextList();

   public void CallDialog()
   {
      AUM.Play("dialogUP");
      darkenPanel.DOFade(0.5f, transTime);
      myTextList = JsonUtility.FromJson<TextList>(textJSONList[dialogCounter].text);
      charExpression.sprite = Resources.Load<Sprite>(myTextList.Text[0].image);
      charExpression.DOFade(1, 0.8f);
      textDisplay.text = myTextList.Text[0].dialogue;
      panelTransform.DOAnchorPosY(positions["onScreen"].y, transTime);
      if (myTextList.Text[0].explanationImg != "")
      {
         explanationImage.sprite = Resources.Load<Sprite>(myTextList.Text[0].explanationImg);
         explanationImage.SetNativeSize();
         explanationImage.DOFade(1, 0.8f);
      }
      ResetPhrases();
      isPaused = true;
   }
   public void CloseDialog()
   {
      darkenPanel.DOFade(0f, transTime);
      panelTransform.DOAnchorPosY(positions["hidden"].y, transTime);
      charExpression.DOFade(0, 0.2f);
      explanationImage.DOFade(0, 0.2f);
      textDisplay.text = "";
      isPaused = false;
   }

   public void NextPhrase()
   {
      phraseIndex++;
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
      textDisplay.text = myTextList.Text[phraseIndex].dialogue;
   }
   public void ResetPhrases()
   {
      phraseIndex = 0;
      dialogCounter += 1;
   }

   public bool GetIsPaused() { return isPaused; }

   public void CallRetry()
   {
      retryBTN.gameObject.SetActive(true);
      nextBTN.gameObject.SetActive(false);
      AUM.Play("dialogUP");
      panelTransform.DOAnchorPosY(positions["onScreen"].y, transTime);
      darkenPanel.DOFade(0.5f, transTime);
      textDisplay.text = failMessage;
      charExpression.sprite = Resources.Load<Sprite>("CHAR/normal");
      charExpression.DOFade(1, 0.8f);
   }
}