using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OPaoGameStudio_MagnetMaze
{
    public class Wires : SwitchesInteractableObject
    {

        private LineRenderer line;
        private Dictionary<string, Color> cores = new Dictionary<string, Color>(){
        {"positivo",new Color(0.90f, 0.27f, 0.22f,1f)},
        {"negativo",new Color(0.57f, 0.91f, 0.75f,1f)},
        {"desligado",new Color(0.49f, 0.22f, 0.20f,1)},
        {"ligado",new Color(0.81f, 0.46f, 0.17f,1)}
    };
        [SerializeField] private Mode tipo;
        [SerializeField] private bool active;
        public bool chargingBattery = false;

        private enum Mode { onOff, invert };
        void Start()
        {
            line = GetComponent<LineRenderer>();
            if (tipo == Mode.onOff)
            {
                ChangeColor("desligado");
            }
            else
            {
                ChangeColor("ligado");
            }
        }
        public override void Activate()
        {
            if (!chargingBattery)
            {
                if (tipo == Mode.onOff)
                {
                    if (!active)
                    {
                        ChangeColor("desligado");
                        return;
                    }
                    ChangeColor("ligado");
                    return;
                }
            }
        }
        public void ChangeColor(string cor)
        {
            line.startColor = cores[cor];
            line.endColor = cores[cor];
            active = !active;
        }
    }
}
