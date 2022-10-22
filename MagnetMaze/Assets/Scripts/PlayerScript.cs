using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public MagnetBox currentBoxMagnetized = null;
    public Transform boxHolder;
    public Collider2D positiveToolArea;
    public Collider2D negativeToolArea;
    public AudioManager AUM;
    public int energy = 10;
    private float horizontal;
    private float speed = 2.5f;
    private float jumpingPower = 3.0f;
    private bool isFacingRight = true;
    public bool isHorizontal = true;
    private bool isToolActive = false;
    private bool currentPole = false;
    private bool canInteract = false;
    private Collider2D positiveCollision;
    private Collider2D negativeCollision;
    private GameObject lastBoxInteracted = null;
    [SerializeField] private List<GameObject> objects = new List<GameObject>();
    [SerializeField] private GameObject hud;
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
        if (!hud.GetComponent<BottomTextManagement>().GetIsPaused())
        {
            // -1 esquerda, 1 direita
            horizontal = Input.GetAxisRaw("Horizontal");

            if (Input.GetButtonDown("ToggleTool"))
            {
                if (energy > 0)
                {
                    ToggleTool();
                }
                else
                {
                    ActivateRestartPanel();
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
                else
                {
                    ActivateRestartPanel();
                }
            }

            if (Input.GetButtonUp("Jump") && rigidBody.velocity.y > 0.1f)
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y * 0.5f);
            }
            //1 pra cima, -1 pra baixo
            //if (Input.GetAxisRaw("Vertical") > 0)
            //{
            //    tool.transform.eulerAngles = new Vector3()
            //}

            if (Input.GetAxisRaw("Vertical") > 0)
            {
                tool.transform.eulerAngles = new Vector3(0, 0, 180);
                isHorizontal = false;
            }
            else if (Input.GetAxisRaw("Vertical") < 0)
            {
                tool.transform.eulerAngles = new Vector3(0, 0, 0);
                isHorizontal = false;
            }
            else
            {
                tool.transform.eulerAngles = new Vector3(0, 0, 90 * transform.localScale.x);
                isHorizontal = true;
            }

            //if (Input.GetButtonDown("Vertical") && isToolActive && Input.GetAxisRaw("Vertical") != 0)
            //{
            //    tool.transform.eulerAngles = new Vector3(0,0,0);
            //    tool.transform.localScale = new Vector3(tool.transform.localScale.x, Input.GetAxisRaw("Vertical") * -1f, tool.transform.localScale.z);
            //}
            //else if (Input.GetAxisRaw("Vertical") == 0)
            //{
            //    tool.transform.eulerAngles = new Vector3(0,0,90);
            //    //tool.transform.localScale = new Vector3(tool.transform.localScale.x, 1f, tool.transform.localScale.z);
            //}
        }

        if (currentBoxMagnetized != null)
        {
            if (currentBoxMagnetized.playerBoxHolder == null)
            {
                currentBoxMagnetized.playerBoxHolder = boxHolder;
            }
            if (!isToolActive)
            {
                currentBoxMagnetized.holded = false;
            }
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
            tool.SetActive(true);
        }
        else if (isToolActive)
        {
            anim.SetTrigger("toolOn");
            isToolActive = !isToolActive;
            tool.SetActive(false);
        }
        else if (energy == 0)
        {
            ActivateRestartPanel();
        }
        if (currentPole && isToolActive && energy > 0)
        {
            //tool.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.3f);
            energy -= 1;
        }
        else if (!currentPole && isToolActive && energy > 0)
        {
            //tool.GetComponent<SpriteRenderer>().color = new Color(0, 0, 1, 0.3f);
            energy -= 1;
        }
        else if (!isToolActive)
        {
            //gameObject.layer = LayerMask.NameToLayer("Player");
        }
        ChangeText();
    }
    private void ToggleTool()
    {
        anim.SetTrigger("toolClick");
        AUM.Play("click");
        currentPole = !currentPole;
        energy -= 1;
        print(tool.transform.localScale);
        tool.transform.localScale = new Vector3(tool.transform.localScale.x, tool.transform.localScale.y * -1, tool.transform.localScale.z);
        print(tool.transform.localScale);
        ChangeText();
        if(currentBoxMagnetized != null)
        {
            currentBoxMagnetized.holded = false;
        }
    }
    private void Jump()
    {
        AUM.Play("jump");
        if (objects.Count != 0)
        {
            if (objects[0].CompareTag("Box"))
            {
                objects[0].GetComponent<Rigidbody2D>().velocity = new Vector2(-rigidBody.velocity.x, -jumpingPower);
            }
        }
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpingPower);
    }

    private void Interact()
    {
        energy -= 1;
        if (objects[0].CompareTag("Box"))
        {
            if(lastBoxInteracted != null && lastBoxInteracted != objects[0])
            {
                lastBoxInteracted.GetComponent<MagnetBox>().ChangePole("Neutral", Vector2.zero);
            }
            if (!currentPole)
            {
                objects[0].GetComponent<MagnetBox>().ChangePole("Negative", MagneticForceDirection(objects[0].GetComponent<Collider2D>()));
            }
            else if (currentPole)
            {
                objects[0].GetComponent<MagnetBox>().ChangePole("Positive", MagneticForceDirection(objects[0].GetComponent<Collider2D>()));
            }
            currentBoxMagnetized = objects[0].GetComponent<MagnetBox>();
            lastBoxInteracted = objects[0];
            if(currentBoxMagnetized.lastPole == "Neutral")
            {
                currentBoxMagnetized = null;
                lastBoxInteracted = null;
            }

        }

        else if (objects[0].CompareTag("Interactable"))
        {
            energy += 1;
            objects[0].GetComponent<Switches>().OnSwitchActivate();
        }
        ChangeText();
    }

   private void FixedUpdate()
   {
      rigidBody.velocity = new Vector2(horizontal * speed, rigidBody.velocity.y);
   }

   private bool IsGrounded()
   {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, 0.03f, groundLayer);
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
        if (collision.gameObject.layer == 12 && collision.transform.parent.CompareTag("Box"))
        {
            objects.Insert(0, collision.transform.parent.gameObject);
            objects[0].GetComponent<MagnetBox>().canInteract = true;
            canInteract = true;
        }
        else if (collision.gameObject.CompareTag("Interactable"))
        {
            objects.Insert(0, collision.gameObject);
            canInteract = true;
        }
        else if (collision.gameObject.CompareTag("Energy"))
        {
            energy += 1;
            Destroy(collision.gameObject);
            ChangeText();
        }
    }
    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (isToolActive && collision.gameObject.CompareTag("BoxMagnetArea"))
    //    {
    //        if (collision.gameObject.layer == 7 && positiveCollision != collision)
    //        {
    //            positiveCollision = collision;
    //            //currentBoxMagnetized.polesArea[1].enabled = false;
    //        }
    //        else if (collision.gameObject.layer == 8 && negativeCollision != collision)
    //        {
    //            negativeCollision = collision;

    //            //currentBoxMagnetized.polesArea[0].enabled = false;
    //        }
    //        if (positiveCollision == null)
    //        {
    //            MagnetMovement(negativeCollision);
    //        }
    //        else if (negativeCollision == null)
    //        {
    //            MagnetMovement(positiveCollision);
    //        }
    //        else if (positiveCollision.Distance(coll).distance > negativeCollision.Distance(coll).distance)
    //        {
    //            MagnetMovement(negativeCollision);
    //        }
    //        else
    //        {
    //            MagnetMovement(positiveCollision);
    //        }
    //    }
    //}
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 12 && collision.transform.parent.CompareTag("Box"))
        {
            objects[0].GetComponent<MagnetBox>().canInteract = false;
            objects.Remove(collision.transform.parent.gameObject);
            if (objects.Count == 0)
            {
                canInteract = false;
            }
        }
        //if (collision.gameObject.layer == 7)
        //{
        //    currentBoxMagnetized.polesArea[1].enabled = true;
        //}
        //else if (collision.gameObject.layer == 8)
        //{
        //    currentBoxMagnetized.polesArea[0].enabled = true;
        //}
    }
    private Vector2 MagneticForceDirection(Collider2D obj)
    {
        Vector2 direction = new Vector2(0, 0);
        if (obj.transform.position.x - transform.position.x > 0.25)
        {
            direction.x = 1;
        }
        else if (obj.transform.position.x - transform.position.x < -0.25)
        {
            direction.x = -1;
        }
        if (obj.transform.position.y - transform.position.y > 0.2f)
        {
            direction.y = 1;
        }
        else if (obj.transform.position.y - transform.position.y < -0.2f)
        {
            direction.y = -1;
        }
        return direction;
    }

    private string CheckIfSameOrOppositeBoxPole(Collider2D obj, Collider2D area)
    {
        if ((area.gameObject.layer == 11 && obj.gameObject.layer == 7) || (area.gameObject.layer == 10 && obj.gameObject.layer == 8))
        {
            return "Opposite";
        }
        else if ((area.gameObject.layer == 11 && obj.gameObject.layer == 8) || (area.gameObject.layer == 10 && obj.gameObject.layer == 7))
        {
            return "Same";
        }
        else if (obj.gameObject.layer == 9)
        {
            return "Neutral";
        }
        else
        {
            return "NotABox";
        }
    }

    public void MagnetMovement(Collider2D obj, Collider2D area)
    {
        float distance = Vector3.Distance(obj.transform.position, rigidBody.transform.position);
        if (CheckIfSameOrOppositeBoxPole(obj, area) == "Opposite" && isHorizontal == currentBoxMagnetized.isHorizontal)
        {
            if ((!isHorizontal && (transform.position.y > 0.2f || transform.position.y < -0.2f)) || (isHorizontal && (transform.position.y < 0.2f || transform.position.y > -0.2f)))
            {
                if (currentBoxMagnetized.canInteract && !currentBoxMagnetized.holded)
                {
                    currentBoxMagnetized.holded = true;
                }
                else if (!currentBoxMagnetized.canInteract && !currentBoxMagnetized.holded)
                {
                    obj.attachedRigidbody.AddForce((-magneticForce * MagneticForceDirection(obj)) / Mathf.Pow(distance, 2));
                    rigidBody.AddForce((magneticForce * MagneticForceDirection(obj)) / Mathf.Pow(distance, 2));
                }
            }
        }
        else if (CheckIfSameOrOppositeBoxPole(obj, area) == "Same" && isHorizontal == currentBoxMagnetized.isHorizontal)
        {
            if ((!isHorizontal && (transform.position.y > 0.2f || transform.position.y < -0.2f)) || (isHorizontal && (transform.position.y < 0.2f || transform.position.y > -0.2f)))
            {
                //currentBoxMagnetized.holded = false;
                obj.attachedRigidbody.AddForce((magneticForce * MagneticForceDirection(obj)) / Mathf.Pow(distance, 2));
                rigidBody.AddForce((-magneticForce * MagneticForceDirection(obj)) / Mathf.Pow(distance, 2));
            }
        }
    }


    private void ChangeText()
    {
        UIText.GetComponent<TMPro.TextMeshProUGUI>().text = "X " + energy.ToString();
    }

    private void ActivateRestartPanel()
    {
        UIText.transform.parent.transform.parent.GetChild(2).gameObject.SetActive(true);
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
