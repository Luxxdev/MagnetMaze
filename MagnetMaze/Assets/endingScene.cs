using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class endingScene : MonoBehaviour
{
    [SerializeField] private Button finishButton;
    [SerializeField] private TMPro.TextMeshProUGUI endText;
    // Start is called before the first frame update
    void Start()
    {
        TextDisplayUpdate();
    }

    // Update is called once per frame
    void TextDisplayUpdate()
    {
        finishButton.transform.GetChild(0).GetComponent<TMP_Text>().text = SharedState.LanguageDefs["finish"];
        endText.GetComponent<TMP_Text>().text = SharedState.LanguageDefs["completedText"];
    }
}
