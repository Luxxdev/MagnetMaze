using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetBox : MonoBehaviour
{
   //[SerializeField] private Rigidbody2D rigidBody;
   public Transform playerBoxHolder;
   public Sprite[] spriteArray;
   public SpriteRenderer spriteRenderer;
   public bool canInteract = false;
   public bool holded = false;
   public bool conducting = false;
   public bool isHorizontal = false;
   public Vector2 magnetOrientation;
   public string lastPole = "Neutral";
   public List<Collider2D> polesArea;
   public GameObject polesAreaObject;
   private List<GameObject> touchingConductingBoxes = new List<GameObject>();
   //[SerializeField] private Collider2D coll;
   private void Update()
   {
        if (conducting)
        {
            spriteRenderer.sprite = spriteArray[2];
        }
        else
        {
            if (lastPole == "Neutral")
            {
                spriteRenderer.sprite = spriteArray[0];
            }
            else
            {
                spriteRenderer.sprite = spriteArray[1];
            }
        }


      if (!polesArea[0].enabled && !polesArea[1].enabled)
      {
         polesArea[0].enabled = true;
         polesArea[1].enabled = true;
      }

      if (holded)
      {
         transform.parent = playerBoxHolder;
         transform.position = playerBoxHolder.position;
         gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
         gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
         polesAreaObject.SetActive(false);
      }
      else
      {
         if (lastPole != "Neutral")
         {
            polesAreaObject.SetActive(true);
         }
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
               if (touchingConductingBoxes.Count == 0)
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

   public void ChangePole(string pole, Vector2 direction)
   {
      if ((direction != magnetOrientation || pole != lastPole) && pole != "Neutral")
      {
         transform.localScale = new Vector3(1, 1, 1);
         transform.eulerAngles = new Vector3(0, 0, 0);

         if (direction.y == 0)
         {
            isHorizontal = true;
            transform.eulerAngles = new Vector3(0, 0, 90);
         }
         else
         {
            isHorizontal = false;
         }
         if (pole == "Positive")
         {
            polesAreaObject.SetActive(true);
            if (direction.x == -1 || direction.y > 0)
            {
               transform.localScale = new Vector3(1, -1, 1);
            }
         }
         else if (pole == "Negative")
         {
            polesAreaObject.SetActive(true);
            if (direction.x == 1 || direction.y < 0)
            {
               transform.localScale = new Vector3(1, -1, 1);
            }
         }
         spriteRenderer.sprite = spriteArray[1];
         lastPole = pole;
         magnetOrientation = direction;
      }
      else
      {
         spriteRenderer.sprite = spriteArray[0];
         polesAreaObject.SetActive(false);
         holded = false;
         lastPole = "Neutral";
         magnetOrientation = new Vector2(0, 0);
      }
   }
}
