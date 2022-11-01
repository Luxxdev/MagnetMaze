using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigMagnet : SwitchesInteractableObject
{
    public GameObject particles;
    [SerializeField] private bool pole;
    [SerializeField] private bool isActive;
    [SerializeField] private Mode interactionMode;
    [SerializeField] private Collider2D coll;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private bool isHorizontal = false;
    [SerializeField] private float magneticForce = 200000;
    [SerializeField] private float magneticForceIncrease;
    private Collider2D positiveCollision;
    private Collider2D negativeCollision;
    private PlayerScript player;
    private Collider2D playerPositive;
    private Collider2D playerNegative;
    private MagnetBox currentBoxMagnetized;
    private enum Mode { onOff, invert, powerUp }

    private void Start()
    {
        UpdateSprite();
    }
    public override void Activate()
    {
        switch ((int)interactionMode)
        {
            case 0:
                isActive = !isActive;
                coll.enabled = isActive;
                break;
            case 1:
                pole = !pole;
                if (pole)
                {
                    coll.gameObject.layer = 7;
                }
                else
                {
                    coll.gameObject.layer = 8;
                }
                break;
            case 2:
                magneticForce += magneticForceIncrease;
                break;
        }
        UpdateSprite();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerToolArea"))
        {
            player = collision.GetComponent<ToolArea>().playerScript;
        }
        if (collision.gameObject.CompareTag("BoxMagnetArea"))
        {
            print("entrou");
            collision.attachedRigidbody.sleepMode = RigidbodySleepMode2D.NeverSleep;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BoxMagnetArea") && isActive)
        {
            if (collision.gameObject.layer == 7 && positiveCollision != collision)
            {
                currentBoxMagnetized = collision.transform.parent.transform.parent.GetComponent<MagnetBox>();
                positiveCollision = collision;
            }
            else if (collision.gameObject.layer == 8 && negativeCollision != collision)
            {
                currentBoxMagnetized = collision.transform.parent.transform.parent.GetComponent<MagnetBox>();
                negativeCollision = collision;
            }
            if (positiveCollision == null)
            {
                print("negative1");
                MagnetMovement(negativeCollision);
            }
            else if (negativeCollision == null)
            {
                print("positive1");
                MagnetMovement(positiveCollision);
            }
            else if (Vector3.Distance(positiveCollision.transform.position, transform.position) > Vector3.Distance(negativeCollision.transform.position, transform.position))
            {

                print("negative2");
                MagnetMovement(negativeCollision);
            }
            else
            {
                print("positive2");
                MagnetMovement(positiveCollision);
            }
        }



        if (collision.gameObject.CompareTag("PlayerToolArea") && isActive)
        {
            if (collision.gameObject.layer == 10 && playerPositive != collision)
            {
                playerPositive = collision;
            }
            else if (collision.gameObject.layer == 11 && playerNegative != collision)
            {
                playerNegative = collision;
            }
            if (playerPositive == null)
            {
                MagnetMovement(playerNegative);
            }
            else if (playerNegative == null)
            {
                MagnetMovement(playerPositive);
            }
            else if (Vector3.Distance(playerPositive.transform.position, transform.position) > Vector3.Distance(playerNegative.transform.position, transform.position))
            {
                MagnetMovement(playerNegative);
            }
            else
            {
                MagnetMovement(playerPositive);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BoxMagnetArea"))
        {
            print("saiu");
            collision.attachedRigidbody.sleepMode = RigidbodySleepMode2D.StartAwake;
        }
        if (positiveCollision != null && collision == positiveCollision)
        {
            positiveCollision = null;
        }
        else if (negativeCollision != null && collision == negativeCollision)
        {
            negativeCollision = null;
        }
    }
    public void MagnetMovement(Collider2D obj)
    {
        if (obj.CompareTag("BoxMagnetArea") && !currentBoxMagnetized.held)
        {
            if (CheckIfSameOrOppositeBoxPole(obj) == "Opposite" && isHorizontal == currentBoxMagnetized.isHorizontal && !currentBoxMagnetized.held)
            {
                obj.attachedRigidbody.AddForce(-MagneticForceCalc(obj));
            }
            else if (CheckIfSameOrOppositeBoxPole(obj) == "Same" && isHorizontal == currentBoxMagnetized.isHorizontal && !currentBoxMagnetized.held)
            {
                obj.attachedRigidbody.AddForce(MagneticForceCalc(obj));
            }
        }
        if (obj.CompareTag("PlayerToolArea"))
        {
            if (CheckIfSameOrOppositeBoxPole(obj) == "Opposite" && isHorizontal == player.isHorizontal)
            {
                obj.attachedRigidbody.AddForce(-MagneticForceCalc(obj));
            }
            else if (CheckIfSameOrOppositeBoxPole(obj) == "Same" && isHorizontal == player.isHorizontal)
            {
                obj.attachedRigidbody.AddForce(MagneticForceCalc(obj));
            }
        }
    }
    private void UpdateSprite()
    {
        Animator partAnim = particles.GetComponent<Animator>();

        if (pole)
        {
            coll.gameObject.layer = 7;

            if (isActive)
            {
                spriteRenderer.sprite = sprites[0];
                particles.SetActive(true);
                partAnim.SetBool("pole", true);
                return;
            }
            else
            {
                spriteRenderer.sprite = sprites[1];
                particles.SetActive(false);
            }
        }
        else
        {
            coll.gameObject.layer = 8;

            if (isActive)
            {
                spriteRenderer.sprite = sprites[2];
                particles.SetActive(true);
                partAnim.SetBool("pole", false);
                return;
            }
            else
            {
                spriteRenderer.sprite = sprites[3];
                particles.SetActive(false);
            }

        }
    }
    private string CheckIfSameOrOppositeBoxPole(Collider2D obj)
    {
        if ((coll.gameObject.layer == 7 && (obj.gameObject.layer == 8 || obj.gameObject.layer == 11)) || (coll.gameObject.layer == 8 && (obj.gameObject.layer == 7 || obj.gameObject.layer == 10)))
        {
            return "Opposite";
        }
        else if ((coll.gameObject.layer == 7 && (obj.gameObject.layer == 7 || obj.gameObject.layer == 10)) || (coll.gameObject.layer == 8 && (obj.gameObject.layer == 8 || obj.gameObject.layer == 11)))
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

    private Vector2 MagneticForceDirection(Collider2D obj)
    {
        Vector2 direction = new Vector2(0, 0);
        if (obj.gameObject.CompareTag("PlayerToolArea"))
        {
            if (!isHorizontal)
            {
                if (player.transform.position.y - transform.position.y > 0)
                {
                    direction.y = 1;
                }
                else if (player.transform.position.y - transform.position.y < 0)
                {
                    direction.y = -1;
                }
            }
            else
            {
                if (player.transform.position.x - transform.position.x > 0)
                {
                    direction.x = 1;
                }
                else if (player.transform.position.x - transform.position.x < 0)
                {
                    direction.x = -1;
                }
            }
        }
        else if (obj.CompareTag("BoxMagnetArea"))
        {
            if (!isHorizontal)
            {
                if (currentBoxMagnetized.transform.position.y - transform.position.y > 0)
                {
                    direction.y = 1;
                }
                else if (currentBoxMagnetized.transform.position.y - transform.position.y < 0)
                {
                    direction.y = -1;
                }
            }
            else
            {
                if (currentBoxMagnetized.transform.position.x - transform.position.x > 0)
                {
                    direction.x = 1;
                }
                else if (currentBoxMagnetized.transform.position.x - transform.position.x < 0)
                {
                    direction.x = -1;
                }
            }
        }
        return direction;
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
        //print(ClampMagnitude((magneticForce * MagneticForceDirection(obj)) / Mathf.Pow(distance, 2), 50f, 20f));
        return ClampMagnitude((magneticForce * MagneticForceDirection(obj)) / Mathf.Pow(distance, 2), 50f, 20f);
    }
}
