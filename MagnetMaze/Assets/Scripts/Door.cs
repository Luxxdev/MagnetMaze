using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : ButtonInteractableObject
{
    [SerializeField]private Animator anim;
    [SerializeField] private Collider2D collider;
    public override void Activate()
    {
        anim.SetBool("isOpen", collider.enabled);
        collider.enabled = !collider.enabled;
    }
}
