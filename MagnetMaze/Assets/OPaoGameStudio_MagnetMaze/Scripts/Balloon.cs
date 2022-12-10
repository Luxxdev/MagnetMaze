using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OPaoGameStudio_MagnetMaze
{
    public class Balloon : MonoBehaviour
    {
        public float speed = 1.0f;
        public int energyAmount = 20;
        public Animator anim;

        private void Start()
        {
            anim.Play("Balloon", -1, Random.Range(0.0f, 1.0f));
        }
        void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("StaticArea"))
            {
                var step = speed * Time.deltaTime;
                transform.parent.transform.position = Vector3.MoveTowards(transform.parent.transform.position, collision.transform.parent.transform.position, step);
            }
            if (collision.gameObject.CompareTag("Player"))
            {
                Vector2 deathOffset;
                deathOffset = transform.position - collision.transform.position;
                if (deathOffset.x < 0.2f && deathOffset.x > -0.2f && deathOffset.y < 0.4f && deathOffset.y > -0.2f)
                {
                    if (!collision.gameObject.GetComponent<PlayerScript>().isRestartable)
                        collision.gameObject.GetComponent<PlayerScript>().isRestartable = true;
                    collision.gameObject.GetComponent<PlayerScript>().energy += energyAmount;
                    collision.gameObject.GetComponent<PlayerScript>().ChangeText();
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
