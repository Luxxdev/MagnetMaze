using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carpet : MonoBehaviour
{
    public PlayerScript player;
    public bool isVertical = false;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.staticArea.SetActive(true);
            player.onCarpet = true;
            float moving = isVertical ? moving = collision.attachedRigidbody.velocity.y : moving = collision.attachedRigidbody.velocity.x;
            if (moving > 0.1f || moving < -0.1f)
            {
                if (player.staticArea.transform.localScale.x < 1)
                {
                    player.staticArea.transform.localScale += new Vector3(0.03f, 0.03f, 0.03f);
                }
            }
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.onCarpet = false;
        }
    }
}
