using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBlocker : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 12)
        {
            print(collision.transform.parent.gameObject);
            collision.transform.parent.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            transform.parent.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 12)
        {
            print(collision.transform.parent.gameObject);
            collision.transform.parent.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            transform.parent.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    // private void OnCollisionExit2D(Collision2D collision) {

    // }
}
