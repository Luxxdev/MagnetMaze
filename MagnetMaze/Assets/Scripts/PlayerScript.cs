using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private float horizontal;
    private float speed = 3;
    private float jumpingPower = 5;
    private bool isFacingRight = true;
    private bool isToolActive = false;
    private bool currentPole = false;
    private bool canInteract = false;
    private List<GameObject> objects = new List<GameObject>();
    [SerializeField] private GameObject tool;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    
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
            currentPole = !currentPole;
            if (currentPole && isToolActive)
            {
                this.gameObject.layer = LayerMask.NameToLayer("ToolPositive");
                tool.GetComponent<SpriteRenderer>().color = new Color (1,0,0,0.3f);
            }
            else if (!currentPole && isToolActive)
            {
                this.gameObject.layer = LayerMask.NameToLayer("ToolNegative");
                tool.GetComponent<SpriteRenderer>().color = new Color (0,0,1,0.3f);
            }

        }

        if (Input.GetButtonDown("ActivateTool"))
        {
            isToolActive = !isToolActive;
            tool.GetComponent<SpriteRenderer>().enabled = isToolActive;
            tool.GetComponent<CapsuleCollider2D>().enabled = isToolActive;
            if (currentPole && isToolActive)
            {
                this.gameObject.layer = LayerMask.NameToLayer("ToolPositive");
                tool.GetComponent<SpriteRenderer>().color = new Color (1,0,0,0.3f);
            }
            else if (!currentPole && isToolActive)
            {
                this.gameObject.layer = LayerMask.NameToLayer("ToolNegative");
                tool.GetComponent<SpriteRenderer>().color = new Color (0,0,1,0.3f);
            }
            else if (!isToolActive)
            {
                this.gameObject.layer = LayerMask.NameToLayer("Player");
            }
        }

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        if (Input.GetButtonDown("Interact") && canInteract)
        {
            if (objects[0].layer == 7 || objects[0].layer == 8)
            {
                objects[0].layer = 9;
            }
            else if (objects[0].layer == 9)
            {
                if (!currentPole)
                {
                    objects[0].layer = 8;
                    Debug.Log(objects[0] + " " + "negativou");
                }
                else if (currentPole)
                {
                    objects[0].layer = 7;
                    Debug.Log(objects[0] + " " + "positivou");
                }
            }
            Debug.Log("interacting");
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        Flip();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapBox(groundCheck.position, new Vector2(0.1f,0.02f), 0, groundLayer);
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
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// <param name="other">The Collision2D data associated with this collision.</param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer >= 7 && other.gameObject.layer <= 9)
        {
            canInteract = true;
            objects.Add(other.gameObject);
            Debug.Log(canInteract + " " + objects);
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.layer >= 7 && other.gameObject.layer <= 9)
        {
            canInteract = false;
            objects.Remove(other.gameObject);
            Debug.Log(canInteract);
        }
    }
}
