using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : ButtonInteractableObject
{
    public override void Activate()
    {
        transform.Translate(new Vector3(1, 0, 0));
        print("movi");
    }
}
