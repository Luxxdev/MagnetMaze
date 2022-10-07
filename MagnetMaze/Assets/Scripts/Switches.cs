using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Switches : MonoBehaviour
{
    [SerializeField] protected GameObject interactableObject;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected Sprite[] sprite;
    protected float energyRequired;
    protected bool canActivate = true;
    protected bool hasBattery = false;
    [SerializeField] protected float steps = 1.0f;
    protected float stepsCount;
    protected float energy = 0;

    protected void Awake()
    {
        if (interactableObject.CompareTag("Battery"))
        {
            stepsCount = steps;
            energyRequired = interactableObject.GetComponent<Battery>().requiredEnergy * stepsCount;
            hasBattery = true;
            print(steps);
        }
    }

    public virtual void OnSwitchActivate()
    {
        if (canActivate)
        {
            if (hasBattery)
            {
                steps += stepsCount;
            }
            interactableObject.GetComponent<SwitchesInteractableObject>().Activate();
        }
    }

}
