using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OPaoGameStudio_MagnetMaze
{
    public class ShowDialogsButton : MonoBehaviour
    {
        [SerializeField] private Button thisButton;
        [SerializeField] private BottomTextManagement HUDScript;
        // Start is called before the first frame update
        void Start()
        {
            thisButton.onClick.AddListener(() =>
            {
                if (!HUDScript.isDialogOpen)
                {
                    HUDScript.SetIsPaused(!HUDScript.GetIsPaused());
                    transform.parent.GetChild(6).gameObject.SetActive(!transform.parent.GetChild(6).gameObject.activeSelf);
                }
            });
        }
    }
}
