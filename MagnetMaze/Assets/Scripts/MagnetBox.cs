using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetBox : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// <param name="other">The Collision2D data associated with this collision.</param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("entrou");
        if (other.gameObject.layer == 7)
        {
            Debug.Log("negativo");
            if (gameObject.layer == 7)
            {
                Move(-1);
            }
            else if (gameObject.layer == 8)
            {
                Move(1);
            }
        }
        else if (other.gameObject.layer == 8)
        {
            Debug.Log("positivo");
            if (gameObject.layer == 7)
            {
                Move(1);
            }
            else if (gameObject.layer == 8)
            {
                Move(-1);
            }
        }

    }
    /*void ChangePole(bool pole)
    {
        if (pole)
        {
            gameObject.layer = 7;
        }
        else if (!pole)
        {

        }
    }*/

    void Move(int multiplier)
    {
        Debug.Log("moved");
        rb.AddForce(new Vector2(1 * multiplier,0));
    }
}
