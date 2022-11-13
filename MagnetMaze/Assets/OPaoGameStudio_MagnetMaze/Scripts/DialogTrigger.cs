using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OPaoGameStudio_MagnetMaze
{
    public class DialogTrigger : MonoBehaviour
    {
        [SerializeField] private GameObject hudObject;
        [SerializeField] private DialogTrigger[] otherDialogs;
        public bool textShown = false;
        // Start is called before the first frame update
        #region Movement
        void Start()
        {

        }
        // Update is called once per frame
        #endregion
        void Update()
        {

        }
        // <summary>
        // Sent when another object enters a trigger collider attached to this
        // object (2D physics only).
        // </summary>
        // <param name="other">The other Collider2D involved in this collision.</param>
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player") && !textShown)
            {
                hudObject.GetComponent<BottomTextManagement>().CallDialog();
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