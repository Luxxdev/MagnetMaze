using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private float horizontal;
    private float speed = 3;
    private float jumpingPower = 5.0f;
    private bool isFacingRight = true;
    private bool isToolActive = false;
    private bool currentPole = false;
    private bool canInteract = false;
    private List<GameObject> objects = new List<GameObject>();
    private GameObject lastObjectInteracted = null;
    [SerializeField] protected Animator anim;
    [SerializeField] private GameObject tool;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float magneticForce = 10;

   void Start()
   {
      //  private GameObject tool = this.gameObject.transform.GetChild(1).GetChild(0).gameObject;
   }


   // Update is called once per frame
   void Update()
   {
      horizontal = Input.GetAxisRaw("Horizontal");

      if (Input.GetButtonDown("ToggleTool"))
      {
         anim.SetTrigger("toolClick");
         currentPole = !currentPole;
         if (currentPole && isToolActive)
         {
            this.gameObject.layer = LayerMask.NameToLayer("ToolPositive");
            tool.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.3f);
         }
         else if (!currentPole && isToolActive)
         {
            this.gameObject.layer = LayerMask.NameToLayer("ToolNegative");
            tool.GetComponent<SpriteRenderer>().color = new Color(0, 0, 1, 0.3f);
         }

      }

      if (Input.GetButtonDown("ActivateTool"))
      {
         anim.SetTrigger("toolOn");
         isToolActive = !isToolActive;
         tool.GetComponent<SpriteRenderer>().enabled = isToolActive;
         tool.GetComponent<CapsuleCollider2D>().enabled = isToolActive;
         if (currentPole && isToolActive)
         {
            this.gameObject.layer = LayerMask.NameToLayer("ToolPositive");
            tool.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.3f);
         }
         else if (!currentPole && isToolActive)
         {
            this.gameObject.layer = LayerMask.NameToLayer("ToolNegative");
            tool.GetComponent<SpriteRenderer>().color = new Color(0, 0, 1, 0.3f);
         }
         else if (!isToolActive)
         {
            this.gameObject.layer = LayerMask.NameToLayer("Player");
         }
      }

      if (Input.GetButtonDown("Jump") && IsGrounded())
      {
         if (objects.Count != 0)
            {
                print("empurrei");
                objects[0].GetComponent<Rigidbody2D>().velocity = new Vector2(-rb.velocity.x, -jumpingPower);
            }
         rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
         print("pulei");
      }

      if (Input.GetButtonDown("Interact") && canInteract)
      {
        if (objects[0].layer == 7 && currentPole || objects[0].layer == 8 && !currentPole)
         {
            objects[0].layer = 9;
            objects[0].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            objects[0].transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().enabled = false;
            lastObjectInteracted = null;
         }
         else if (objects[0].layer >= 7 && objects[0].layer <= 9)
         {
            objects[0].transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().enabled = true;
            if (!currentPole)
            {
               objects[0].layer = 8;
               objects[0].GetComponent<SpriteRenderer>().color = new Color(0, 0, 1, 1);
               objects[0].transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 1, 0.4f);
            }
            else if (currentPole)
            {
               objects[0].layer = 7;
               objects[0].GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
               objects[0].transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.4f);
            }
            if (lastObjectInteracted != null && lastObjectInteracted != objects[0])
            {
               lastObjectInteracted.layer = 9;
               lastObjectInteracted.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
               lastObjectInteracted.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
            lastObjectInteracted = objects[0];
         }
      }

      if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
      {
         anim.SetFloat("Yvelocity", rb.velocity.y);
         rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
      }

      Flip();
   }

   private void FixedUpdate()
   {
      anim.SetFloat("Yvelocity", rb.velocity.y);
      anim.SetBool("isOnFloor", rb.velocity.y != 0 ? false : true);
      anim.SetBool("isMoving", rb.velocity.x != 0 ? true : false);

      rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
   }

   private bool IsGrounded()
   {
       return Physics2D.Raycast(transform.position, -Vector2.up, gameObject.GetComponent<BoxCollider2D>().bounds.extents.y + 0.1f, groundLayer);
       //return Physics2D.OverlapBox(groundCheck.position, new Vector2(0.1f, 0.02f), 0, groundLayer);

      //return Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
    }
   private void Flip()
   {
      if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
      {
         isFacingRight = !isFacingRight;
         Vector3 localScale = transform.localScale;
         localScale.x *= -1f;
         transform.localScale = localScale;
      }
   }

   /*private void OnCollisionEnter2D(Collision2D other)
   {
      //if (other.gameObject.layer >= 7 && other.gameObject.layer <= 9)
      if (other.gameObject.layer == 12 && (other.transform.parent.gameObject.layer >= 7 && other.transform.parent.gameObject.layer <= 9))
      {
         objects.Insert(0, other.transform.parent.gameObject);
         objects[0].GetComponent<MagnetBox>().canInteract = true;
         canInteract = true;
      }
   }
//collision stay pra checar qual objeto interagir
    private void OnCollisionExit2D(Collision2D other)
   {
      // if (other.gameObject.layer >= 7 && other.gameObject.layer <= 9)
      if (other.gameObject.layer == 12 && (other.transform.parent.gameObject.layer >= 7 && other.transform.parent.gameObject.layer <= 9))
      {
          objects[0].GetComponent<MagnetBox>().canInteract = false;
          objects.Remove(other.transform.parent.gameObject);
          if (objects.Count == 0)
          {
                canInteract = false;
          }
      }
   }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 12 && (collision.transform.parent.gameObject.layer >= 7 && collision.transform.parent.gameObject.layer <= 9))
        {
            objects.Insert(0, collision.transform.parent.gameObject);
            objects[0].GetComponent<MagnetBox>().canInteract = true;
            canInteract = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isToolActive)
        {
            Vector2 direction = new Vector2(0,0);
            if (collision.transform.position.x - transform.position.x > 0.2)
            {
                direction.x = 1;
            }
            else if (collision.transform.position.x - transform.position.x < -0.2)
            {
                direction.x = -1;
            }
            if (collision.transform.position.y - transform.position.y > 0.6)
            {
                direction.y = 1;
            }
            else if (collision.transform.position.y - transform.position.y < -0.6)
            {
                direction.y = -1;
            }

            if (collision.gameObject.layer == 7)
            {
                if (currentPole)
                {
                    collision.gameObject.GetComponent<Rigidbody2D>().AddForce(magneticForce * direction);
                    gameObject.GetComponent<Rigidbody2D>().AddForce(-magneticForce * direction);

                }
                else if (!currentPole && !collision.gameObject.GetComponent<MagnetBox>().canInteract)
                {
                    collision.gameObject.GetComponent<Rigidbody2D>().AddForce(-magneticForce * direction);
                    gameObject.GetComponent<Rigidbody2D>().AddForce(magneticForce * direction);


                }
            }

            if (collision.gameObject.layer == 8)
            {
                if (currentPole && collision.gameObject.GetComponent<MagnetBox>().canInteract == false)
                {
                    collision.gameObject.GetComponent<Rigidbody2D>().AddForce(-magneticForce * direction);
                    gameObject.GetComponent<Rigidbody2D>().AddForce(magneticForce * direction);

                }
                else if (!currentPole)
                {
                    collision.gameObject.GetComponent<Rigidbody2D>().AddForce(magneticForce * direction);
                    gameObject.GetComponent<Rigidbody2D>().AddForce(-magneticForce * direction);

                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 12 && (collision.transform.parent.gameObject.layer >= 7 && collision.transform.parent.gameObject.layer <= 9))
        {
            objects[0].GetComponent<MagnetBox>().canInteract = false;
            objects.Remove(collision.transform.parent.gameObject);
            if (objects.Count == 0)
            {
                canInteract = false;
            }
        }
    }
}
