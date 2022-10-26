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
            Vector2 euodeioumagameengine;
            euodeioumagameengine = transform.position - collision.transform.position;
            if (euodeioumagameengine.x < 0.2f && euodeioumagameengine.x > -0.2f && euodeioumagameengine.y < 0.2f && euodeioumagameengine.y > -0.2f)
            Destroy(this.gameObject);
        }
    }
}
