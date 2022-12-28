using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace OPaoGameStudio_MagnetMaze
{
    public class ImageHelper : MonoBehaviour
    {
        public VideoPlayer selfVideoPlayer;
        public SpriteRenderer spriteRenderer;
        public SpriteRenderer borderSprite;

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                selfVideoPlayer.Play();
                spriteRenderer.enabled = true;
                borderSprite.enabled = true;
            }
        }
        void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                spriteRenderer.enabled = false;
                borderSprite.enabled = false;
                selfVideoPlayer.Stop();
            }
        }
    }
}
