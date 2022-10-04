using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureButton : Switches
{
    private List<Collider2D> objectsInArea = new List<Collider2D>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Box") && !collision.isTrigger))
        {
            objectsInArea.Add(collision);
            if (!pressed)
            {
                pressed = true;
                spriteRenderer.sprite = sprite[1];
                OnSwitchActivate();
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
    }
    public override void OnSwitchActivate()
    {
        base.OnSwitchActivate();
    }

}
