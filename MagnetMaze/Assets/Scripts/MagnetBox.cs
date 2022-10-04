using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetBox : MonoBehaviour
{
    //[SerializeField] private Rigidbody2D rigidBody;
    public Sprite[] spriteArray;
    public bool canInteract = false;
    public bool conducting = false;
    private List<GameObject> touchingConductingBoxes = new List<GameObject>();

    private void Update()
    {
        if (conducting)
        {
            GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 1);
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Box") && collision.gameObject.GetComponent<MagnetBox>().conducting)
        {
            conducting = true;
            touchingConductingBoxes.Add(collision.gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {

            if (!collision.gameObject.GetComponent<MagnetBox>().conducting)
            {
                if (touchingConductingBoxes.Count != 0 && touchingConductingBoxes.Contains(collision.gameObject))
                {
                    touchingConductingBoxes.Remove(collision.gameObject);
                    if(touchingConductingBoxes.Count == 0)
                    {
                        conducting = false;
                    }
                }
            }
            else if (collision.gameObject.GetComponent<MagnetBox>().conducting)
            {
                if (touchingConductingBoxes.Count == 0 || !touchingConductingBoxes.Contains(collision.gameObject))
                {
                    touchingConductingBoxes.Add(collision.gameObject);
                    conducting = true;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Box") && collision.gameObject.GetComponent<MagnetBox>().conducting)
        {
            if (touchingConductingBoxes.Count != 0 && touchingConductingBoxes.Contains(collision.gameObject))
            {
                touchingConductingBoxes.Remove(collision.gameObject);
            }

            if (touchingConductingBoxes.Count == 0)
            {
                conducting = false;
            }
        }
    }
}
