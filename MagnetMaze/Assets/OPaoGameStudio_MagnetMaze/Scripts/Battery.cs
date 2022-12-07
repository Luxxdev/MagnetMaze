using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OPaoGameStudio_MagnetMaze
{
    public class Battery : SwitchesInteractableObject
    {
        [SerializeField] private GameObject[] interactableObject;
        [SerializeField] private SpriteRenderer energyBar;
        public float requiredEnergy = 10;
        public float energy;
        public int energyIncreaseRate = 3;
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
                energy += Time.deltaTime * energyIncreaseRate;
                energyBar.size += new Vector2(0, 0.04294457f * Time.deltaTime * energyIncreaseRate);
                if (energy >= requiredEnergy - 0.1)
                {
                    energy = requiredEnergy;
                }
                if (energy >= requiredEnergy)
                {
                    foreach (var item in interactableObject)
                    {
                        item.GetComponent<SwitchesInteractableObject>().Activate();
                    }
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
                energy -= Time.deltaTime * energyIncreaseRate;
                energyBar.size -= new Vector2(0, 0.04294457f * Time.deltaTime * energyIncreaseRate);

                if (energy <= 0)
                {
                    energy = 0;
                    enabled = false;
                }
            }

        }
    }

}
