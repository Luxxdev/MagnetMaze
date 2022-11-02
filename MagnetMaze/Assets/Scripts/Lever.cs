using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Switches
{
    public bool changeMode = false;
    public int newMode = 0;
    public override void OnSwitchActivate()
    {
        if (changeMode)
        {
            interactableObject[0].GetComponent<BigMagnet>().ChangeMode(newMode);
        }
        base.OnSwitchActivate();
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }


}
