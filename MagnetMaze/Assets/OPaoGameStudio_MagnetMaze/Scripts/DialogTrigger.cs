using UnityEngine;
using LoLSDK;

namespace OPaoGameStudio_MagnetMaze
{
    public class DialogTrigger : MonoBehaviour
    {
        [SerializeField] private GameObject hudObject;
        [SerializeField] private DialogTrigger[] otherDialogs;
        public int triggerID = 0;
        public bool isProgress = false;
        public int currentProgress = 0;
        public bool textShown = false;

        void Start()
        {
            if (Singleton.Instance.gameData.seenDialogs >= triggerID)
            {
                gameObject.SetActive(false);
            }
        }
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player") && !textShown)
            {
                hudObject.GetComponent<BottomTextManagement>().isRevisionDialog = false;
                hudObject.GetComponent<BottomTextManagement>().CallDialog();
                if (isProgress)
                {
                    Singleton.Instance.gameData.playerProgress = currentProgress;
                    Singleton.Instance.SetPlayerProgress(currentProgress);
                    LOLSDK.Instance.SubmitProgress(0, currentProgress, 12);
                }
                textShown = true;
                Destroy(this.gameObject);
                if (otherDialogs.Length > 0)
                {
                    foreach (DialogTrigger dialog in otherDialogs)
                    {
                        dialog.textShown = false;
                    }
                }
            }
        }

    }
}