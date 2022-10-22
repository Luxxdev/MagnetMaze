using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigMagnet : SwitchesInteractableObject
{
    [SerializeField] private bool pole;
    [SerializeField] private bool isActive;
    [SerializeField] private Mode interactionMode;
    [SerializeField] private Collider2D coll;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private bool isHorizontal = false;
    [SerializeField] private float magneticForce = 200000;
    private Collider2D positiveCollision;
    private Collider2D negativeCollision;
    private PlayerScript player;
    private Collider2D playerPositive;
    private Collider2D playerNegative;
    private MagnetBox currentBoxMagnetized;
    private enum Mode{ onOff, invert }

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
        }
        UpdateSprite();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerToolArea"))
        {
            player = collision.GetComponent<ToolArea>().playerScript;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BoxMagnetArea"))
        {
            // boxBodyCollider = collision.transform.parent.transform.parent.GetComponent<Collider2D>();
            if (collision.gameObject.layer == 7 && positiveCollision != collision)
            {
                currentBoxMagnetized =  collision.transform.parent.transform.parent.GetComponent<MagnetBox>();
                positiveCollision = collision;
                //currentBoxMagnetized.polesArea[1].enabled = false;
            }
            else if (collision.gameObject.layer == 8 && negativeCollision != collision)
            {
                currentBoxMagnetized = collision.transform.parent.transform.parent.GetComponent<MagnetBox>();
                negativeCollision = collision;
                //currentBoxMagnetized.polesArea[0].enabled = false;
            }
            if (positiveCollision == null)
            {
                //print("entrou1");
                MagnetMovement(negativeCollision);
            }
            else if (negativeCollision == null)
            {
                //print("entrou2");
                MagnetMovement(positiveCollision);
            }
            else if (Vector3.Distance(positiveCollision.transform.position, transform.position) > Vector3.Distance(negativeCollision.transform.position, transform.position))
            {
                //print("entrou3");
                MagnetMovement(negativeCollision);
            }
            else
            {
                //print("entrou4");
                MagnetMovement(positiveCollision);
            }
        }



        if (collision.gameObject.CompareTag("PlayerToolArea"))
        {
            // boxBodyCollider = collision.transform.parent.transform.parent.GetComponent<Collider2D>();
            if (collision.gameObject.layer == 10 && playerPositive != collision)
            {
                playerPositive = collision;
                //currentBoxMagnetized.polesArea[1].enabled = false;
            }
            else if (collision.gameObject.layer == 11 && playerNegative != collision)
            {
                playerNegative = collision;
                //currentBoxMagnetized.polesArea[0].enabled = false;
            }
            if (playerPositive == null)
            {
                //print("entrou1");
                MagnetMovement(playerNegative);
            }
            else if (playerNegative == null)
            {
                MagnetMovement(playerPositive);
            }
            else if (Vector3.Distance(playerPositive.transform.position, transform.position) > Vector3.Distance(playerNegative.transform.position, transform.position))
            {
                //print("entrou3");
                MagnetMovement(playerNegative);
            }
            else
            {
                //print("entrou4");
                MagnetMovement(playerPositive);
            }
        }
    }
    public void MagnetMovement(Collider2D obj)
    {
        if (obj.CompareTag("BoxMagnetArea") && !currentBoxMagnetized.holded)
        {
            float distance = Vector3.Distance(obj.transform.position, transform.position);
            if (CheckIfSameOrOppositeBoxPole(obj) == "Opposite" && isHorizontal == currentBoxMagnetized.isHorizontal && !currentBoxMagnetized.holded)
            {
                //if ((!isHorizontal && (transform.position.y > 0.2f || transform.position.y < -0.2f)) || (isHorizontal && (transform.position.y < 0.2f || transform.position.y > -0.2f)))
                //{
                //    if (!currentBoxMagnetized.canInteract && !currentBoxMagnetized.holded)
                //    {
                obj.attachedRigidbody.AddForce((-magneticForce * MagneticForceDirection(obj)) / Mathf.Pow(distance, 2));
                //    }
                //}
            }
            else if (CheckIfSameOrOppositeBoxPole(obj) == "Same" && isHorizontal == currentBoxMagnetized.isHorizontal && !currentBoxMagnetized.holded)
            {
                //if ((!isHorizontal && (transform.position.y > 0.2f || transform.position.y < -0.2f)) || (isHorizontal && (transform.position.y < 0.2f || transform.position.y > -0.2f)))
                //{
                //    //currentBoxMagnetized.holded = false;
                obj.attachedRigidbody.AddForce((magneticForce * MagneticForceDirection(obj)) / Mathf.Pow(distance, 2));
                //}
            }
        }
        if (obj.CompareTag("PlayerToolArea"))
        {
            float distance = Vector3.Distance(obj.transform.position, transform.position);
            if (CheckIfSameOrOppositeBoxPole(obj) == "Opposite" && isHorizontal == player.isHorizontal)
            {
                obj.attachedRigidbody.AddForce((-magneticForce * MagneticForceDirection(obj)) / Mathf.Pow(distance, 2));
                  
            }
            else if (CheckIfSameOrOppositeBoxPole(obj) == "Same" && isHorizontal == player.isHorizontal)
            {
                print("magforce" + magneticForce);
                print("direction" + MagneticForceDirection(obj));
                print("div" + Mathf.Pow(distance, 2));

                print((magneticForce * MagneticForceDirection(obj)) / Mathf.Pow(distance, 2));

                obj.attachedRigidbody.AddForce((magneticForce * MagneticForceDirection(obj)) / Mathf.Pow(distance, 2));

            }
        }
    }
    private void UpdateSprite()
    {
        if (pole)
        {
            if (isActive)
            {
                spriteRenderer.sprite = sprites[0];
            }
            else
            {
                spriteRenderer.sprite = sprites[1];
            }
        }
        else
        {
            if (isActive)
            {
                spriteRenderer.sprite = sprites[2];
            }
            else
            {
                spriteRenderer.sprite = sprites[3];
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
        if (!isHorizontal)
        {
            if (obj.transform.position.y - transform.position.y > 0)
            {
                direction.y = 1;
            }
            else if (obj.transform.position.y - transform.position.y < 0)
            {
                direction.y = -1;
            }
        }
        else
        {
            if (obj.transform.position.x - transform.position.x > 0)
            {
                direction.x = 1;
            }
            else if (obj.transform.position.x - transform.position.x < 0)
            {
                direction.x = -1;
            }

        }
        return direction;
    }
}
