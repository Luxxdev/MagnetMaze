using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Switches : MonoBehaviour
{
    [SerializeField] protected GameObject interactableObject;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected Sprite[] sprite;
    protected bool canActivate = true;
    protected bool hasBattery = false;

    private void Start()
    {
        if (interactableObject.CompareTag("Battery"))
        {
            hasBattery = true;
        }
    }

    public virtual void OnSwitchActivate()
    {
        if (canActivate)
        {
            interactableObject.GetComponent<SwitchesInteractableObject>().Activate();
        }
    }

}
