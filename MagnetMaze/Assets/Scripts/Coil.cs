using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coil : Switches
{
    [SerializeField] private Collider2D coll;
    private List<int> layers = new List<int> { 7, 8, 10, 11 };
    public Battery batteryScript;
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
            enabled = false;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        print(batteryScript.charging);
        //if (collision.gameObject.CompareTag("Player") && collision.GetComponent<PlayerScript>().i || (collision.gameObject.CompareTag("Box") && layers.Contains(collision.gameObject.layer)))
        if (collInArea.Contains(collision))//(layers.Contains(collision.gameObject.layer))
        {
            if (collision.attachedRigidbody.velocity.x > 0.01f || collision.attachedRigidbody.velocity.x < -0.01f)
            {
                if (movingCollision == null)
                {
                    movingCollision = collision;
                    batteryScript.charging = true;
                }
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
                    batteryScript.charging = false;
                }
                moving = false;
            }
            if (movingCollision == collision)
            {
                if (energy < energyRequired)
                {
                    energy += Time.deltaTime;
                }
                if (!moving)
                {
                    batteryScript.pressedButtons += 1;
                }
                moving = true;
                if (hasBattery)
                {
                    if (moving)
                    {
                        if (energy < energyRequired)
                        {
                            OnSwitchActivate();
                            //batteryScript.charging = true;
                        }
                    }
                    else
                    {
                        if (energy > 0)
                        {
                            energy -= Time.deltaTime;
                            //batteryScript.charging = false;
                        }
                    }
                }
                if (energy >= energyRequired)
                {
                    if (!hasBattery)
                    {
                        coll.enabled = false;
                        OnSwitchActivate();
                    }
                    else
                    {
                        if (!batteryScript.isFull)
                        {
                            if (!moving && energy > 0)
                            {
                                energy -= Time.deltaTime;
                                //batteryScript.charging = false;
                            }
                        }
                        else
                        {
                            coll.enabled = false;
                        }
                    }
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (movingCollision == collision)
        {
            movingCollision = null;
            batteryScript.charging = false;
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
        energy -= Time.deltaTime;
        if (energy <= 0)
        {
            energy = 0;
            enabled = false;
        }

    }
}
