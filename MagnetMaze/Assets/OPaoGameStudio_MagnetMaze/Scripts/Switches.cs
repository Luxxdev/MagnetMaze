using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OPaoGameStudio_MagnetMaze
{
    public abstract class Switches : MonoBehaviour
    {
        [SerializeField] protected GameObject[] interactableObject;
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
            foreach (var item in interactableObject)
            {
                if (item.CompareTag("Battery"))
                {
                    stepsCount = steps;
                    energyRequired = item.GetComponent<Battery>().requiredEnergy * stepsCount / item.GetComponent<Battery>().conectedSwitches;
                    hasBattery = true;
                }
            }
        }

        public virtual void OnSwitchActivate()
        {
            foreach (var item in interactableObject)
            {
                if (item.CompareTag("Battery"))
                    if (item.GetComponent<Battery>().isFull)
                        canActivate = false;
            }
            if (canActivate)
            {
                if (hasBattery)
                {
                    steps += stepsCount;
                }
                foreach (var item in interactableObject)
                {
                    item.GetComponent<SwitchesInteractableObject>().Activate();
                }
            }
        }
    }
}