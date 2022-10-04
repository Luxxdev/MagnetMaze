using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coil : Switches
{
    [SerializeField] private float energyRequired;
    [SerializeField] private Collider2D coll;
    private List<int> layers = new List<int> {7,8,10,11};
    private float energy = 0.0f;
    public override void OnSwitchActivate()
    {
        base.OnSwitchActivate();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (layers.Contains(collision.gameObject.layer) || (collision.gameObject.CompareTag("Box") && layers.Contains(collision.transform.parent.gameObject.layer)))
        {
            if (collision.attachedRigidbody.velocity.x > 0.1f || collision.attachedRigidbody.velocity.x < -0.1f)
            {
                energy += Time.deltaTime;
            }
            if (energy >= energyRequired)
            {
                coll.enabled = false;
                OnSwitchActivate();
            }
        }
    }
}
