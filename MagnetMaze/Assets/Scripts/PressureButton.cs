using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureButton : Switches
{
    private List<Collider2D> objectsInArea = new List<Collider2D>();
    [SerializeField] private bool isPressure = false;
    private bool pressed = false;

    void Start()
    {
        enabled = false;
        if (hasBattery)
        {
            isPressure = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Box") && !collision.isTrigger))
        {
            objectsInArea.Add(collision);
            if (!pressed)
            {
                pressed = true;
                spriteRenderer.sprite = sprite[1];
                if (!hasBattery)
                {
                    OnSwitchActivate();
                }
                else
                {
                    foreach (var item in interactableObject)
                    {
                        if (item.CompareTag("Battery"))
                        {
                            item.GetComponent<Battery>().pressedButtons += 1;
                            item.GetComponent<Battery>().charging = true;

                        }
                    }

                }
            }
            if (collision.gameObject.CompareTag("Box"))
            {
                collision.attachedRigidbody.sleepMode = RigidbodySleepMode2D.NeverSleep;

            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Box")) && !collision.isTrigger)
        {
            objectsInArea.Remove(collision);
            if (objectsInArea.Count == 0)
            {
                spriteRenderer.sprite = sprite[0];
                pressed = false;
            }
            if (isPressure && !pressed)
            {
                if (hasBattery)
                {
                    foreach (var item in interactableObject)
                    {
                        if (item.CompareTag("Battery"))
                        {
                            if (!item.GetComponent<Battery>().isFull)
                            {
                                canActivate = true;
                                enabled = true;
                            }
                            item.GetComponent<Battery>().pressedButtons -= 1;
                            item.GetComponent<Battery>().charging = false;
                        }
                    }
                }
                else
                {
                    OnSwitchActivate();
                }
            }
            if (collision.gameObject.CompareTag("Box"))
            {
                collision.attachedRigidbody.sleepMode = RigidbodySleepMode2D.StartAwake;
            }
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Box")) && !collision.isTrigger)
        {
            if (canActivate)
            {
                if (isPressure && !pressed && !hasBattery)
                {
                    spriteRenderer.sprite = sprite[1];
                    OnSwitchActivate();
                }
                else if (hasBattery && isPressure)
                {
                    energy += Time.deltaTime;
                    OnSwitchActivate();

                    if (energy >= energyRequired)
                    {
                        canActivate = false;
                        enabled = false;
                        foreach (var item in interactableObject)
                        {
                            if (item.CompareTag("Battery"))
                            {
                                item.GetComponent<Battery>().charging = false;
                            }
                        }
                    }
                }
            }
        }
    }

    private void Update()
    {
        if (canActivate && !pressed)
        {
            energy -= Time.deltaTime;
            if (energy <= 0)
            {
                energy = 0;
                enabled = false;
            }
        }
    }
    public override void OnSwitchActivate()
    {
        base.OnSwitchActivate();
    }

}
