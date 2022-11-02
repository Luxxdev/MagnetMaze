using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SwitchesInteractableObject : MonoBehaviour
{
    public abstract void Activate();
    // public enum Mode { onOff, invert, powerUp }
    // public Mode interactionMode;
    // public virtual void ChangeMode(int newMode)
    // {
    //     if ((int)interactionMode != newMode)
    //     {
    //         interactionMode = (Mode)newMode;
    //     }
    // }
}
