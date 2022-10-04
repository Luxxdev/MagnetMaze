using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Switches : MonoBehaviour
{
    [SerializeField] protected GameObject interactableObject;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected Sprite[] sprite;
    protected bool pressed = false;

    public virtual void OnSwitchActivate()
    {
        interactableObject.GetComponent<SwitchesInteractableObject>().Activate();
    }

}
