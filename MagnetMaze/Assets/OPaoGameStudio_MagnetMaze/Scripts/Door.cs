using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OPaoGameStudio_MagnetMaze
{
    public class Door : SwitchesInteractableObject
    {
        [SerializeField] private Animator anim;
        [SerializeField] private Collider2D coll;
        [SerializeField] private bool startsOpen = false;
        private AudioManager AUM;

        private void Start()
        {
            AUM = FindObjectOfType<AudioManager>();
            if (startsOpen)
            {
                Activate();
            }
        }
        public override void Activate()
        {
            AUM.Play("openDoor");
            anim.SetBool("isOpen", coll.enabled);
            //coll.enabled = !coll.enabled;
        }
    }
}
