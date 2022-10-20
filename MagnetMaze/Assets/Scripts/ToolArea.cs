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
    public float magneticForce = 20;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BoxMagnetArea"))
        {
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
                playerScript.MagnetMovement(negativeCollision, selfCollider);
            }
            else if (negativeCollision == null)
            {
                //print("entrou2");

                playerScript.MagnetMovement(positiveCollision, selfCollider);
            }
            else if (positiveCollision.Distance(playerColl).distance > negativeCollision.Distance(playerColl).distance)
            {
                //print("entrou3");

                playerScript.MagnetMovement(negativeCollision, selfCollider);
            }
            else
            {
                //print(selfCollider);

                playerScript.MagnetMovement(positiveCollision, selfCollider);
            }
        }
    }
}
