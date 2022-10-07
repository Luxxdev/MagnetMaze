using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlugReceptor : Switches
{
    private List<GameObject> touchingObjects = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Box") && !collision.isTrigger && collision.gameObject.GetComponent<MagnetBox>().conducting)
        {
            if(touchingObjects.Count == 0 && !hasBattery)
            {
                OnSwitchActivate();
            }
            touchingObjects.Add(collision.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Box") && !collision.isTrigger)
        {
            print("entrou");
            if (collision.gameObject.GetComponent<MagnetBox>().conducting && !touchingObjects.Contains(collision.gameObject))
            {
                if(touchingObjects.Count == 0 && !hasBattery)
                {
                    OnSwitchActivate();
                }
                touchingObjects.Add(collision.gameObject);
            }
            if (!collision.gameObject.GetComponent<MagnetBox>().conducting && touchingObjects.Contains(collision.gameObject))
            {
                touchingObjects.Remove(collision.gameObject);
                if(touchingObjects.Count == 0 && !hasBattery)
                {
                    OnSwitchActivate();
                }
            }
            if (hasBattery && touchingObjects.Contains(collision.gameObject))
            {
                if (collision.attachedRigidbody.sleepMode != RigidbodySleepMode2D.NeverSleep)
                {
                    collision.attachedRigidbody.sleepMode = RigidbodySleepMode2D.NeverSleep;
                }
                energy += Time.deltaTime;
                if (energy >= steps)
                {
                    OnSwitchActivate();
                }
                if (energy >= energyRequired)
                {
                    collision.attachedRigidbody.sleepMode = RigidbodySleepMode2D.StartAsleep;
                    canActivate = false;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Box") && !collision.isTrigger && collision.gameObject.GetComponent<MagnetBox>().conducting)
        {
            if (touchingObjects.Count > 0)
            {
                touchingObjects.Remove(collision.gameObject);
            }
            if (touchingObjects.Count == 0 && !hasBattery)
            {
                OnSwitchActivate();
            }
        }
    }
    public override void OnSwitchActivate()
    {
        base.OnSwitchActivate();
    }
}

