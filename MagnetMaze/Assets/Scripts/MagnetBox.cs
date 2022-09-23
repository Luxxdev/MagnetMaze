using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetBox : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    public bool canInteract = false;
 
    private void OnTriggerStay2D(Collider2D collision)
    {
        int direction = 0;
        if (collision.transform.position.x - transform.position.x > 0)
        {
            direction = 1;
        }
        else if (collision.transform.position.x - transform.position.x < 0)
        {
            direction = -1;
        }

        if (collision.gameObject.layer == 7)
        {
            if (gameObject.layer == 7)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(10 * direction, 0));
            }
            else if (gameObject.layer == 8)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-10 * direction, 0));
            }
        }

        if (collision.gameObject.layer == 8)
        {
            if (gameObject.layer == 7)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-10 * direction, 0));
            }
            else if (gameObject.layer == 8)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(10 * direction, 0));
            }
        }
    }
   /* private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("entrou");
        if (other.gameObject.layer == 7)
        {
            Debug.Log("negativo");
            if (gameObject.layer == 7)
            {
                Move(-1);
            }
            else if (gameObject.layer == 8)
            {
                Move(1);
            }
        }
        else if (other.gameObject.layer == 8)
        {
            Debug.Log("positivo");
            if (gameObject.layer == 7)
            {
                Move(1);
            }
            else if (gameObject.layer == 8)
            {
                Move(-1);
            }
        }

    }
    /*void ChangePole(bool pole)
    {
        if (pole)
        {
            gameObject.layer = 7;
        }
        else if (!pole)
        {

        }
    }*/

    void Move(int multiplier)
    {
        Debug.Log("moved");
        rb.AddForce(new Vector2(1 * multiplier,0));
    }
}
