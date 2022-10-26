using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

   private enum Mode { onOff, invert };
   void Start()
   {

      line = GetComponent<LineRenderer>();
   }
   public override void Activate()
   {
      if (tipo == Mode.onOff)
      {
         if (!active)
         {
            ChangeColor("ligado");
            return;
         }
         ChangeColor("desligado");
         return;
      }

      else if (tipo == Mode.invert)
      {
         if (!active)
         {
            ChangeColor("negativo");
            return;
         }
         ChangeColor("positivo");
      }

   }
   public void ChangeColor(string cor)
   {
      line.startColor = cores[cor];
      line.endColor = cores[cor];
      active = !active;
   }
}
