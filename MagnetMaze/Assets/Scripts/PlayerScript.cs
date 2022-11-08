using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
   public MagnetBox currentBoxMagnetized = null;
   //public MagnetBox previousBoxMagnetized = null;
   public GameObject magnetIndicator;
   public Transform boxHolder;
   public bool isHolding = false;
   public Collider2D positiveToolArea;
   public Collider2D negativeToolArea;
   public AudioManager AUM;
   public int energy = 10;
   private float horizontal;
   public float vertical;
   public float speed = 2.5f;
   private float jumpingPower = 3.3f;
   public bool isFacingRight = true;
   public bool isHorizontal = true;
   private bool isToolActive = false;
   public bool currentPole = true;
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
   public GameObject staticArea;
   public bool onCarpet = false;
   private enum MovementState { idle, running, jumping, falling }

   void Start()
   {
      //  private GameObject tool = this.gameObject.transform.GetChild(1).GetChild(0).gameObject;
      AUM.Play("BGM");
   }


   // Update is called once per frame
   void Update()
   {
      if (!hud.GetComponent<BottomTextManagement>().GetIsPaused())
      {
         // -1 esquerda, 1 direita
         horizontal = Input.GetAxisRaw("Horizontal");
         vertical = Input.GetAxisRaw("Vertical");
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

         if (vertical > 0 && !isHolding)
         {
            tool.transform.eulerAngles = new Vector3(0, 0, 180);
            magnetIndicator.transform.eulerAngles = new Vector3(0, 0, 90 * transform.localScale.x);
            isHorizontal = false;
         }
         else if (vertical < 0 && !isHolding)
         {
            tool.transform.eulerAngles = new Vector3(0, 0, 0);
            magnetIndicator.transform.eulerAngles = new Vector3(0, 0, -90 * transform.localScale.x);

            isHorizontal = false;
         }
         else
         {
            tool.transform.eulerAngles = new Vector3(0, 0, 90 * transform.localScale.x);
            magnetIndicator.transform.eulerAngles = new Vector3(0, 0, 0);

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
      else
      {
         horizontal = 0;
      }

      if (currentBoxMagnetized != null)
      {
         if (currentBoxMagnetized.playerBoxHolder == null)
         {
            currentBoxMagnetized.playerBoxHolder = boxHolder;
         }
         if (!isToolActive)
         {
            currentBoxMagnetized.held = false;
            isHolding = false;
         }
      }
      if (!onCarpet && staticArea.transform.localScale.x > 0.1f)
      {
         staticArea.transform.localScale -= new Vector3(0.001f, 0.001f, 0.001f);
      }
      else if (staticArea.transform.localScale.x <= 0.1)
      {
         staticArea.SetActive(false);
      }
      UpdateAnimation();
      Flip();
   }
   private void ActivateTool()
   {
      if (energy > 0 && !isToolActive)
      {
         AUM.Play("toolOn");
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
      //print(tool.transform.localScale);
      tool.transform.localScale = new Vector3(tool.transform.localScale.x, tool.transform.localScale.y * -1, tool.transform.localScale.z);
      magnetIndicator.transform.localScale = new Vector3(magnetIndicator.transform.localScale.x * -1, magnetIndicator.transform.localScale.y, magnetIndicator.transform.localScale.z);
      //print(tool.transform.localScale);
      ChangeText();
      if (currentBoxMagnetized != null)
      {
         currentBoxMagnetized.held = false;
         isHolding = false;
      }
   }
   private void Jump()
   {
      AUM.Play("jump");
      //if (objects.Count != 0)
      //{
      //    if (objects[0].CompareTag("Box"))
      //    {
      //        objects[0].GetComponent<Rigidbody2D>().velocity = new Vector2(-rigidBody.velocity.x, -jumpingPower);
      //    }
      //}
      rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpingPower);
   }

   private void Interact()
   {
      energy -= 1;
      if (objects[0].CompareTag("Box"))
      {
         AUM.Play("magnetize");
         if (lastBoxInteracted != null && lastBoxInteracted != objects[0])
         {
            lastBoxInteracted.GetComponent<MagnetBox>().ChangePole("Neutral", Vector2.zero);
         }
         if (!currentPole)
         {
            objects[0].GetComponent<MagnetBox>().player = this;
            objects[0].GetComponent<MagnetBox>().ChangePole("Negative", MagneticForceDirection(objects[0].GetComponent<Collider2D>()));
         }
         else if (currentPole)
         {
            objects[0].GetComponent<MagnetBox>().player = this;
            objects[0].GetComponent<MagnetBox>().ChangePole("Positive", MagneticForceDirection(objects[0].GetComponent<Collider2D>()));
         }

         currentBoxMagnetized = objects[0].GetComponent<MagnetBox>();
         lastBoxInteracted = objects[0];
         if (currentBoxMagnetized.lastPole == "Neutral")
         {
            currentBoxMagnetized = null;
            lastBoxInteracted = null;
            isHolding = false;
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

   private void OnTriggerExit2D(Collider2D collision)
   {
      if (collision.gameObject.layer == 12 && collision.transform.parent.CompareTag("Box"))
      {
         collision.transform.parent.GetComponent<MagnetBox>().canInteract = false;
         objects.Remove(collision.transform.parent.gameObject);

      }
      else if (collision.gameObject.CompareTag("Interactable"))
      {
         objects.Remove(collision.gameObject);
      }
      if (objects.Count == 0)
      {
         canInteract = false;
      }
   }
   private Vector2 MagneticForceDirection(Collider2D obj)
   {
      Vector2 direction = new Vector2(0, 0);
      if (isHorizontal)
      {
         if (obj.attachedRigidbody.transform.position.x - transform.position.x > 0.2f)
         {
            direction.x = 1;
         }
         else if (obj.attachedRigidbody.transform.position.x - transform.position.x < -0.2f)
         {
            direction.x = -1;
         }
      }
      else
      {
         if (obj.attachedRigidbody.transform.position.y - transform.position.y > 0.2f)
         {
            direction.y = 1;
         }
         else if (obj.attachedRigidbody.transform.position.y - transform.position.y < -0.2f)
         {
            direction.y = -1;
         }
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
      print(obj);
      print(area);
      // float distance;
      // if (isHorizontal)
      // {
      //     distance = Vector2.Distance(new Vector2(obj.attachedRigidbody.transform.position.x, 0), new Vector2(transform.position.x, 0));
      // }
      // else
      // {
      //     distance = Vector2.Distance(new Vector2(0, obj.attachedRigidbody.transform.position.y), new Vector2(0, transform.position.y));
      // }
      float check = transform.position.y - obj.attachedRigidbody.transform.position.y;
      if (CheckIfSameOrOppositeBoxPole(obj, area) == "Opposite" && currentBoxMagnetized != null && isHorizontal == currentBoxMagnetized.isHorizontal)
      {
         if ((!isHorizontal && (check > 0.4f || check < -0.4f)) || (isHorizontal && (check < 0.4f && check > -0.4f)))
         {
            if (currentBoxMagnetized.canInteract && !currentBoxMagnetized.held)
            {
               currentBoxMagnetized.held = true;
               isHolding = true;
               //StartCoroutine(WaitForPoleChange());
            }
            else if (!currentBoxMagnetized.canInteract && !currentBoxMagnetized.held)
            {
               obj.attachedRigidbody.AddForce(-MagneticForceCalc(obj));
               rigidBody.AddForce(MagneticForceCalc(obj));
            }
         }
      }
      else if (CheckIfSameOrOppositeBoxPole(obj, area) == "Same" && currentBoxMagnetized != null && isHorizontal == currentBoxMagnetized.isHorizontal)
      {
         if ((!isHorizontal && (check > 0.4f || check < -0.4f)) || (isHorizontal && (check < 0.4f && check > -0.4f)))
         {
            currentBoxMagnetized.held = false;
            isHolding = false;
            obj.attachedRigidbody.AddForce(MagneticForceCalc(obj));
            rigidBody.AddForce(-MagneticForceCalc(obj));
         }
      }
   }

   public static Vector2 ClampMagnitude(Vector2 v, float max, float min)
   {
      double sm = v.sqrMagnitude;
      if (sm > (double)max * (double)max) return v.normalized * max;
      else if (sm < (double)min * (double)min) return v.normalized * min;
      return v;
   }

   public Vector2 MagneticForceCalc(Collider2D obj)
   {
      float distance;
      if (isHorizontal)
      {
         distance = Vector2.Distance(new Vector2(obj.attachedRigidbody.transform.position.x, 0), new Vector2(transform.position.x, 0));
      }
      else
      {
         distance = Vector2.Distance(new Vector2(0, obj.attachedRigidbody.transform.position.y), new Vector2(0, transform.position.y));
      }
      return ClampMagnitude((magneticForce * MagneticForceDirection(obj)) / Mathf.Pow(distance, 1.2f), 50, 20f);
   }
   //IEnumerator WaitForPoleChange()
   //{
   //    yield return new WaitForSeconds(0.1f);
   //    if (currentPole && currentBoxMagnetized.magnetOrientation != MagneticForceDirection(currentBoxMagnetized.GetComponent<Collider2D>()))
   //    {
   //        currentBoxMagnetized.ChangePole("Positive", MagneticForceDirection(currentBoxMagnetized.GetComponent<Collider2D>()));
   //    }
   //    else if (!currentPole && currentBoxMagnetized.magnetOrientation != MagneticForceDirection(currentBoxMagnetized.GetComponent<Collider2D>()))
   //    {
   //        currentBoxMagnetized.ChangePole("Negative", MagneticForceDirection(currentBoxMagnetized.GetComponent<Collider2D>()));
   //    }

   //}

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

      if (horizontal != 0 && IsGrounded())
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
      else if (!IsGrounded())
      {
         state = MovementState.falling;
      }

      anim.SetInteger("state", (int)state);
   }

}
