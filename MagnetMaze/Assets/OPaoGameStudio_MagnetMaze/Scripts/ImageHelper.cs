using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace OPaoGameStudio_MagnetMaze
{
    public class ImageHelper : MonoBehaviour
    {
        public VideoPlayer selfVideoPlayer;

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                gameObject.SetActive(true);
                selfVideoPlayer.Play();
            }
        }
        void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                gameObject.SetActive(false);
                selfVideoPlayer.Stop();
            }
        }
    }
}
