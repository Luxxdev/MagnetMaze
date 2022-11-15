using UnityEngine;
using LoLSDK;

namespace OPaoGameStudio_MagnetMaze
{
    public class DialogTrigger : MonoBehaviour
    {
        [SerializeField] private GameObject hudObject;
        [SerializeField] private DialogTrigger[] otherDialogs;
        public bool isProgress = false;
        public int currentProgress = 0;
        public bool textShown = false;
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player") && !textShown)
            {
                hudObject.GetComponent<BottomTextManagement>().CallDialog();
                if (isProgress)
                {
                    Singleton.Instance.SetPlayerProgress(currentProgress);
                    LOLSDK.Instance.SubmitProgress(0, currentProgress, 15);
                }
                textShown = true;
                Destroy(this.gameObject);
            }
            if (otherDialogs.Length > 0)
            {
                foreach (DialogTrigger dialog in otherDialogs)//for (int i = 0; i < otherDialogs, i++)
                {
                    dialog.textShown = false;
                }
            }
        }

    }
}