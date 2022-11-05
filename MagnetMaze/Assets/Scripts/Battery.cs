using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : SwitchesInteractableObject
{
    [SerializeField] private GameObject interactableObject;
    [SerializeField] private SpriteRenderer energyBar;
    public float requiredEnergy = 10;
    public float energy;
    public bool isFull = false;
    public float conectedSwitches = 1;
    public bool charging = false;
    public float pressedButtons = 0;


    public override void Activate()
    {
        charging = true;
        if (!isFull)
        {
            enabled = true;
            energy += Time.deltaTime;
            energyBar.size += new Vector2(0, 0.04294457f * Time.deltaTime);
            energy = (energy >= requiredEnergy - 0.1f ? energy = requiredEnergy : energy = energy);
            if (energy >= requiredEnergy)
            {
                interactableObject.GetComponent<SwitchesInteractableObject>().Activate();
                isFull = true;
                enabled = false;
            }
        }
    }
    private void Update()
    {
        if (pressedButtons < conectedSwitches)
        {
            charging = false;
        }
        else
        {
            charging = true;
        }
        if (!charging && !isFull && energy >= (requiredEnergy * pressedButtons / conectedSwitches) + 0.1f)
        {
            energy -= Time.deltaTime;
            energyBar.size -= new Vector2(0, 0.04294457f * Time.deltaTime);

            if (energy <= 0)
            {
                energy = 0;
                enabled = false;
            }
        }

    }
}
