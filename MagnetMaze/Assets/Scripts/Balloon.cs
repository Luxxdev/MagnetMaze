using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    public float speed = 1.0f;


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
            //if (Physics2D.OverlapCircle(transform.position,0.6f, 3))
            if (deathOffset.x < 0.2f && deathOffset.x > -0.2f && deathOffset.y < 0.2f && deathOffset.y > -0.2f)
            Destroy(this.gameObject);
        }
    }
}
