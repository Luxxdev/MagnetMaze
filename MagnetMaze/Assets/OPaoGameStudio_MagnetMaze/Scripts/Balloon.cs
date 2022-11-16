using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OPaoGameStudio_MagnetMaze
{
    public class Balloon : MonoBehaviour
    {
        public float speed = 1.0f;
        public int energyAmount = 20;
        void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("StaticArea"))
            {
                var step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, collision.transform.parent.transform.position, step);
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
