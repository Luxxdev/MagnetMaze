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
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Box") && !collision.isTrigger))
        {
            objectsInArea.Remove(collision);
            if(objectsInArea.Count == 0)
            {
                spriteRenderer.sprite = sprite[0];
                pressed = false;
            }
        }
        if (isPressure && !pressed && !hasBattery)
        {
            OnSwitchActivate();
        }
    }
    void OnTriggerStay2D(Collider2D other)
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

                if (energy >= steps && hasBattery)
                {
                    steps += 1;
                    OnSwitchActivate();
                } 
                if (energy >= energyRequired)
                {
                    canActivate = false;
                    if (!hasBattery)
                    {
                        OnSwitchActivate();
                    }
                }
            }
        }
    }
    public override void OnSwitchActivate()
    {
        base.OnSwitchActivate();
    }

}
