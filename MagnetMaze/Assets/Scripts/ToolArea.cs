using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolArea : MonoBehaviour
{
    public Rigidbody2D playerRigidB;
    public PlayerScript playerScript;
    private Collider2D positiveCollision;
    private Collider2D negativeCollision;
    public Collider2D playerColl;
    public Collider2D selfCollider;
    public Collider2D oppositeCollider;
    private Collider2D boxBodyCollider;
    private MagnetBox boxScript;
    public float magneticForce = 20;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BoxMagnetArea"))
        {
            boxBodyCollider = collision.transform.parent.transform.parent.GetComponent<Collider2D>();
            boxScript = boxBodyCollider.transform.GetComponent<MagnetBox>();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BoxMagnetArea"))
        {
            if (boxBodyCollider == null)
            {
                boxBodyCollider = collision.transform.parent.transform.parent.GetComponent<Collider2D>();
            }

            if (collision.gameObject.layer == 7 && positiveCollision != collision)
            {
                positiveCollision = collision;
                //currentBoxMagnetized.polesArea[1].enabled = false;
            }
            else if (collision.gameObject.layer == 8 && negativeCollision != collision)
            {
                negativeCollision = collision;

                //currentBoxMagnetized.polesArea[0].enabled = false;
            }
            if (positiveCollision == null)
            {
                //print("entrou1");
                playerScript.MagnetMovement(negativeCollision, CheckWhichArea(boxBodyCollider));
            }
            else if (negativeCollision == null)
            {
                //print("entrou2");
                playerScript.MagnetMovement(positiveCollision, CheckWhichArea(boxBodyCollider));
            }
            else if (positiveCollision.Distance(playerColl).distance > negativeCollision.Distance(playerColl).distance)
            {
                //print("entrou3");
                playerScript.MagnetMovement(negativeCollision, CheckWhichArea(boxBodyCollider));
            }
            else
            {
                //print(selfCollider);
                playerScript.MagnetMovement(positiveCollision, CheckWhichArea(boxBodyCollider));
            }
            if (boxScript.startsMagnetized && playerScript.currentBoxMagnetized == null)
            {
                playerScript.currentBoxMagnetized = boxScript;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (positiveCollision != null && collision == positiveCollision)
        {
            positiveCollision = null;
        }
        else if (negativeCollision != null && collision == negativeCollision)
        {
            negativeCollision = null;
        }
        if (collision.gameObject.CompareTag("BoxMagnetArea"))
        {
            if (boxBodyCollider.transform.GetComponent<MagnetBox>().startsMagnetized && playerScript.currentBoxMagnetized != null)
            {
                playerScript.currentBoxMagnetized = null;
            }
        }
    }

    private Collider2D CheckWhichArea(Collider2D a)
    {
        if (selfCollider.Distance(a).distance < oppositeCollider.Distance(a).distance)
        {
            return selfCollider;
        }
        else return oppositeCollider;
    }
}
