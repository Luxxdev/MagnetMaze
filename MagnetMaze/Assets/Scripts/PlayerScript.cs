using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private float horizontal;
    private float speed = 2.5f;
    private float jumpingPower = 3.0f;
    public int energy = 10;
    private bool isFacingRight = true;
    private bool isToolActive = false;
    private bool currentPole = false;
    private bool canInteract = false;
    private List<GameObject> objects = new List<GameObject>();
    private GameObject lastObjectInteracted = null;
    [SerializeField] protected Animator anim;
    [SerializeField] private GameObject tool;
    [SerializeField] private GameObject UIText;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float magneticForce = 10;
    [SerializeField] private Collider2D coll;
    private enum MovementState {idle, running, jumping, falling}

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
        if (energy > 0)
        {
            ToggleTool();
        }
      }

      if (Input.GetButtonDown("ActivateTool"))
      {
            ActivateTool();
      }

      if (Input.GetButtonDown("Jump") && IsGrounded())
      {
            Jump();
      }

      if (Input.GetButtonDown("Interact") && canInteract)
      {
        if (energy > 0)
        {
            Interact();
        }
      }

      if (Input.GetButtonUp("Jump") && rigidBody.velocity.y > 0.1f)
      {
         rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y * 0.5f);
      }

      UpdateAnimation();
      Flip();
   }
    private void ActivateTool()
    {
        if (energy > 0 && !isToolActive)
        {
            anim.SetTrigger("toolOn");
            isToolActive = !isToolActive;
            tool.GetComponent<SpriteRenderer>().enabled = isToolActive;
            tool.GetComponent<CapsuleCollider2D>().enabled = isToolActive;
        }
        else if (isToolActive)
        {
            anim.SetTrigger("toolOn");
            isToolActive = !isToolActive;
            tool.GetComponent<SpriteRenderer>().enabled = isToolActive;
            tool.GetComponent<CapsuleCollider2D>().enabled = isToolActive;
        }
        if (currentPole && isToolActive && energy > 0)
        {
            gameObject.layer = LayerMask.NameToLayer("ToolPositive");
            tool.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.3f);
            energy -= 1;
        }
        else if (!currentPole && isToolActive && energy > 0)
        {
            gameObject.layer = LayerMask.NameToLayer("ToolNegative");
            tool.GetComponent<SpriteRenderer>().color = new Color(0, 0, 1, 0.3f);
            energy -= 1;
        }
        else if (!isToolActive)
        {
            gameObject.layer = LayerMask.NameToLayer("Player");
        }
        ChangeText(energy);
    }
    private void ToggleTool()
    {
        anim.SetTrigger("toolClick");
        currentPole = !currentPole;
        energy -= 1;
        if (currentPole && isToolActive)
        {
            gameObject.layer = LayerMask.NameToLayer("ToolPositive");
            tool.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.3f);
        }
        else if (!currentPole && isToolActive)
        {
            gameObject.layer = LayerMask.NameToLayer("ToolNegative");
            tool.GetComponent<SpriteRenderer>().color = new Color(0, 0, 1, 0.3f);
        }
        ChangeText(energy);
    }
    private void Jump()
    {
        if (objects.Count != 0)
        {
            objects[0].GetComponent<Rigidbody2D>().velocity = new Vector2(-rigidBody.velocity.x, -jumpingPower);
        }
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpingPower);
    }
    private void Interact()
    {
        energy -= 1;
        if (objects[0].layer == 7 && currentPole || objects[0].layer == 8 && !currentPole)
        {
            objects[0].layer = 9;
            objects[0].GetComponent<SpriteRenderer>().sprite = objects[0].GetComponent<MagnetBox>().spriteArray[0];
            objects[0].transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().enabled = false;
            lastObjectInteracted = null;
        }
        else if (objects[0].layer >= 7 && objects[0].layer <= 9)
        {
            objects[0].transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().enabled = true;
            if (!currentPole)
            {
                objects[0].layer = 8;
                objects[0].GetComponent<SpriteRenderer>().sprite = objects[0].GetComponent<MagnetBox>().spriteArray[1];
                objects[0].transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 1, 0.4f);
            }
            else if (currentPole)
            {
                objects[0].layer = 7;
                objects[0].GetComponent<SpriteRenderer>().sprite = objects[0].GetComponent<MagnetBox>().spriteArray[2];
                objects[0].transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.4f);
            }
            if (lastObjectInteracted != null && lastObjectInteracted != objects[0])
            {
                lastObjectInteracted.layer = 9;
                lastObjectInteracted.GetComponent<SpriteRenderer>().sprite = objects[0].GetComponent<MagnetBox>().spriteArray[0];
                lastObjectInteracted.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
            lastObjectInteracted = objects[0];
        }
        ChangeText(energy);
    }

   private void FixedUpdate()
   {
      rigidBody.velocity = new Vector2(horizontal * speed, rigidBody.velocity.y);
  
    //   if (objects.Count > 0 && objects[0].GetComponent<MagnetBox>().canInteract && isToolActive)
    //     {
    //         if (objects[0].layer == 7 && !currentPole)
    //         {
    //             objects[0].GetComponent<Rigidbody2D>().velocity = new Vector2(horizontal * speed / 2, rigidBody.velocity.y);
    //             print("apply");
    //         }
    //         else if(objects[0].layer == 8 && currentPole)
    //         {
    //             objects[0].GetComponent<Rigidbody2D>().velocity = new Vector2(horizontal * speed / 2, rigidBody.velocity.y);
    //             print("apply");
    //         }
    //     }
   }

   private bool IsGrounded()
   {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, 0.03f, groundLayer);
      // return Physics2D.Raycast(transform.position, -Vector2.up, gameObject.GetComponent<BoxCollider2D>().bounds.extents.y + 0.1f, groundLayer);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 12 && (collision.transform.parent.gameObject.layer >= 7 && collision.transform.parent.gameObject.layer <= 9))
        {
            objects.Insert(0, collision.transform.parent.gameObject);
            objects[0].GetComponent<MagnetBox>().canInteract = true;
            canInteract = true;
        }
        else if (collision.gameObject.tag == "Energy")
        {
            energy += 1;
            Destroy(collision.gameObject);
            ChangeText(energy);
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
                    rigidBody.AddForce(-magneticForce * direction);
                }
                else if (!currentPole && !collision.gameObject.GetComponent<MagnetBox>().canInteract)
                {
                    collision.gameObject.GetComponent<Rigidbody2D>().AddForce(-magneticForce * direction);
                    rigidBody.AddForce(magneticForce * direction);
                }
            }

            if (collision.gameObject.layer == 8)
            {
                if (currentPole && !collision.gameObject.GetComponent<MagnetBox>().canInteract)
                {
                    collision.gameObject.GetComponent<Rigidbody2D>().AddForce(-magneticForce * direction);
                    rigidBody.AddForce(magneticForce * direction);
                }
                else if (!currentPole)
                {
                    collision.gameObject.GetComponent<Rigidbody2D>().AddForce(magneticForce * direction);
                    rigidBody.AddForce(-magneticForce * direction);
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

    private void ChangeText(int count)
    {
        UIText.GetComponent<TMPro.TextMeshProUGUI>().text = "X " + count.ToString();
    }

    private void UpdateAnimation()
    {
        MovementState state;

        if(horizontal != 0)
        {
            state = MovementState.running;
        }
        else
        {
            state = MovementState.idle;
        }
        if (rigidBody.velocity.y > 0.1f)
        {
            state = MovementState.jumping;
        }
        else if (rigidBody.velocity.y < -0.1f)
        {
            state = MovementState.falling;
        }

        anim.SetInteger("state", (int)state);
    }

}
