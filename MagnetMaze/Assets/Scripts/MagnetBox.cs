using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetBox : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    public bool canInteract = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        Vector2 direction = new Vector2(0,0);
        if (collision.transform.position.x - transform.position.x > 0)
        {
            direction.x = 1;
        }
        else if (collision.transform.position.x - transform.position.x < 0)
        {
            direction.x = -1;
        }
        if (collision.transform.position.y - transform.position.y > 0)
        {
            direction.y = 1;
        }
        else if (collision.transform.position.y - transform.position.y < 0)
        {
            direction.y = -1;
        }

        if (collision.gameObject.layer == 7)
        {
            if (gameObject.layer == 7)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(10 * direction.x, 10 * direction.y));
            }
            else if (gameObject.layer == 8)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-10 * direction.x, -10 * direction.y));
            }
        }

        if (collision.gameObject.layer == 8)
        {
            if (gameObject.layer == 7)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-10 * direction.x, -10 * direction.y));
            }
            else if (gameObject.layer == 8)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(10 * direction.x, 10 * direction.y));
            }
        }
    }

    //interação pra quando dois imãs opostos se juntam
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
