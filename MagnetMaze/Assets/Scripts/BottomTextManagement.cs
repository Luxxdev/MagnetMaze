using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomTextManagement : MonoBehaviour
{
   [SerializeField]
   private List<string> frases;
   [SerializeField]
   protected RectTransform panelTransform;
   protected Dictionary<string, Vector2> positions = new Dictionary<string, Vector2>(){
    {"hidden", new Vector2(0, -178)},
    {"onScreen", new Vector2(0, 0)}
   };
   protected
   // Start is called before the first frame update
   void Start()
   {
      panelTransform.anchoredPosition = positions["hidden"];
   }

   public void CallDialog()
   {
      panelTransform.anchoredPosition = positions["onScreen"];
   }
   public void CloseDialog()
   {
      panelTransform.anchoredPosition = positions["hidden"];
      // Update is called once per frame
   }
   void Update()
   {

   }
   IEnumerator Tween(Vector3 targetPosition, float moveDuration = 1.0f)
   {
      Vector3 previousPosition = gameObject.transform.position;
      float time = 0.0f;
      do
      {
         time += Time.deltaTime;
         gameObject.transform.position = Vector3.Lerp(previousPosition, targetPosition, time / moveDuration);
         yield return 0;
      } while (time < moveDuration);
   }
}
