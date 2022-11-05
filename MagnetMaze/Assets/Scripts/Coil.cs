using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coil : Switches
{
    [SerializeField] private Collider2D coll;
    private List<int> layers = new List<int> { 7, 8, 10, 11 };
    public Battery batteryScript;
    public bool isVertical = false;
    public bool moving = false;
    public List<Collider2D> collInArea;
    public Collider2D movingCollision;

    private void Start()
    {
        if (hasBattery)
        {
            foreach (var item in interactableObject)
            {
                if (item.CompareTag("Battery"))
                {
                    batteryScript = item.GetComponent<Battery>();
                }
            }
        }
        enabled = false;
    }

    public override void OnSwitchActivate()
    {
        base.OnSwitchActivate();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (layers.Contains(collision.gameObject.layer))
        {
            collInArea.Add(collision);
            enabled = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collInArea.Contains(collision))
        {
            float check = isVertical ? check = collision.attachedRigidbody.velocity.y : check = collision.attachedRigidbody.velocity.x;
            if (check > 0.01f || check < -0.01f)
            {
                print("moving");
                movingCollision = collision;
            }
            else
            {
                if (moving)
                {
                    batteryScript.pressedButtons -= 1;
                }
                if (movingCollision = collision)
                {
                    movingCollision = null;
                }
                moving = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (movingCollision == collision)
        {
            movingCollision = null;
            if (moving)
            {
                moving = false;
                if (hasBattery)
                {
                    batteryScript.pressedButtons -= 1;
                }
            }
        }
        if (collInArea.Contains(collision))
        {
            collInArea.Remove(collision);
        }
        if (collInArea.Count == 0)
        {
            enabled = true;
        }
    }

    private void Update()
    {
        if (!moving)
        {
            print("stopped moving");
        }
        if (movingCollision != null)
        {
            if (!moving)
            {
                batteryScript.pressedButtons += 1;
            }
            moving = true;
            if (hasBattery)
            {
                if (moving)
                {
                    if (batteryScript.energy < batteryScript.requiredEnergy)
                    {
                        OnSwitchActivate();
                    }
                }
            }
            if (batteryScript.energy >= batteryScript.requiredEnergy)
            {
                if (!hasBattery)
                {
                    coll.enabled = false;
                    OnSwitchActivate();
                }
                else
                {
                    if (batteryScript.isFull)
                    {
                        coll.enabled = false;
                        enabled = false;
                    }
                }
            }
        }

    }
}
