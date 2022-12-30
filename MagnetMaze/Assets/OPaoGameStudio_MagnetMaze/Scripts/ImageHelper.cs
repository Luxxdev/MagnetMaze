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
        public string videoName;

        private void Start()
        {
            selfVideoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, videoName + ".mp4");
        }
        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                spriteRenderer.enabled = true;
                borderSprite.enabled = true;
                selfVideoPlayer.Play();
                StartCoroutine(Wait());
            }
        }
        void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                spriteRenderer.enabled = false;
                borderSprite.enabled = false;
                selfVideoPlayer.Stop();
                spriteRenderer.color = new Color(0, 0, 0, 1);
            }
        }
        IEnumerator Wait()
        {
            selfVideoPlayer.Prepare();

            while (!selfVideoPlayer.isPrepared)
                yield return null;

            if (selfVideoPlayer.isPlaying)
                spriteRenderer.color = new Color(1, 1, 1, 1);
            else
                spriteRenderer.color = new Color(0, 0, 0, 1);
        }
    }
}
