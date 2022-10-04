using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Switches
{
    public override void OnSwitchActivate()
    {
        base.OnSwitchActivate();
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }


}
