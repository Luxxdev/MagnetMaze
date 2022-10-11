using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetBox : MonoBehaviour
{
    //[SerializeField] private Rigidbody2D rigidBody;
    public Transform playerBoxHolder;
    public Sprite[] spriteArray;
    public bool canInteract = false;
    public bool holded = false;
    public bool conducting = false;
    private List<GameObject> touchingConductingBoxes = new List<GameObject>();
    private string currentPole = "Neutral";

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

        if (holded)
        {
            transform.parent = playerBoxHolder;
            transform.position = playerBoxHolder.position;
            gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }
        else
        {
            transform.parent = null;
            gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
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

    public void ChangePole(string pole)
    {
        if(pole == "Positive")
        {
            gameObject.layer = 7;
            GetComponent<SpriteRenderer>().sprite = spriteArray[2];
            transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().enabled = true;
            transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.4f);
        }
        else if (pole == "Negative")
        {
            gameObject.layer = 8;
            GetComponent<SpriteRenderer>().sprite = spriteArray[1];
            transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().enabled = true;
            transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 1, 0.4f);
        }
        else if (pole == "Neutral")
        {
            gameObject.layer = 9;
            GetComponent<SpriteRenderer>().sprite = spriteArray[0];
            transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().enabled = false;
            holded = false;
        }
    }
}
