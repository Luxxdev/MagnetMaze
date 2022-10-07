using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coil : Switches
{
    [SerializeField] private Collider2D coll;
    private List<int> layers = new List<int> {7,8,10,11};

    public override void OnSwitchActivate()
    {
        base.OnSwitchActivate();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (layers.Contains(collision.gameObject.layer) || (collision.gameObject.CompareTag("Box") && layers.Contains(collision.gameObject.layer)))
        {
            if (collision.attachedRigidbody.velocity.x > 0.1f || collision.attachedRigidbody.velocity.x < -0.1f)
            {
                energy += Time.deltaTime;
            }
            if (energy >= steps && hasBattery)
            {
                OnSwitchActivate();
            } 
            if (energy >= energyRequired)
            {
                coll.enabled = false;
                if (!hasBattery)
                {
                    OnSwitchActivate();
                }
            }
        }
    }
}
