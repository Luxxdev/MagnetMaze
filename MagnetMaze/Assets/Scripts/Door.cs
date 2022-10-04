using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : SwitchesInteractableObject
{
    [SerializeField]private Animator anim;
    [SerializeField] private Collider2D coll;
    public override void Activate()
    {
        anim.SetBool("isOpen", coll.enabled);
        coll.enabled = !coll.enabled;
    }
}
