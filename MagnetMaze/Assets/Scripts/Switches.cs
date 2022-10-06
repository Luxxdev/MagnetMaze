using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Switches : MonoBehaviour
{
    [SerializeField] protected GameObject interactableObject;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected Sprite[] sprite;
    protected int energyRequired;
    protected bool canActivate = true;
    protected bool hasBattery = false;
    protected int steps = 1;
    protected float energy = 0;

    protected void Awake()
    {
        if (interactableObject.CompareTag("Battery"))
        {
            energyRequired = interactableObject.GetComponent<Battery>().requiredEnergy;
            hasBattery = true;
            print(steps);
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
