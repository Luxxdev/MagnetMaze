using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : SwitchesInteractableObject
{
    [SerializeField] private GameObject interactableObject;
    [SerializeField] private GameObject energyBar;
    public int requiredEnergy;
    private float energy;
    public override void Activate()
    {
        energy += 1;
        Instantiate(energyBar, new Vector3(energyBar.transform.position.x, energyBar.transform.position.y + (energy * 0.042f)), Quaternion.identity, transform);
        if (energy >= requiredEnergy)
        {
            interactableObject.GetComponent<SwitchesInteractableObject>().Activate();
        }
    }
}
