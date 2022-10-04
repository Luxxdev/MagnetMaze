using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BottomTextManagement : MonoBehaviour
{
   [SerializeField] public List<string> frases;
   public Image charExpression;
   public Image explanationImage;
   protected int phraseIndex = 0;
   protected float transTime = 0.5f;
   [SerializeField] protected RectTransform panelTransform;
   public TMPro.TextMeshProUGUI textDisplay;
   protected Dictionary<string, Vector2> positions = new Dictionary<string, Vector2>(){
    {"hidden", new Vector2(0, -178)},
    {"onScreen", new Vector2(0, 0)}
   };

   public void Start()
   {
      textDisplay.text = frases[0];
   }
   public void CallDialog()
   {
      panelTransform.DOAnchorPosY(positions["onScreen"].y, transTime);
      explanationImage.DOFade(1, 0.8f);
      charExpression.DOFade(1, 0.8f);
      ResetPhrases();
   }
   public void CloseDialog()
   {
      panelTransform.DOAnchorPosY(positions["hidden"].y, transTime);
      charExpression.DOFade(0, 0.2f);
      explanationImage.DOFade(0, 0.2f);
      textDisplay.text = "";
   }

   public void NextPhrase()
   {
      phraseIndex++;
      if (phraseIndex >= frases.Count)
      {
         CloseDialog();
      }
      textDisplay.text = frases[phraseIndex];
   }
   public void ResetPhrases()
   {
      phraseIndex = 0;
      textDisplay.text = frases[phraseIndex];
   }
   void Update()
   {

   }
}
