using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plug : MonoBehaviour
{
    public bool emmiting;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Box") && !collision.isTrigger)
        {
            collision.gameObject.GetComponent<MagnetBox>().conducting = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Box") && !collision.isTrigger)
        {
            collision.gameObject.GetComponent<MagnetBox>().conducting = false;
        }
    }
}
