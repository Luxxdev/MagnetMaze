using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureButton : MonoBehaviour
{
    [SerializeField] private GameObject interactableObject;
    private bool pressed = false;
    private List<Collider2D> objectsInArea = new List<Collider2D>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "Player" || collision.gameObject.tag == "Box") && !collision.isTrigger)
        {
            objectsInArea.Add(collision);
            if (!pressed)
            {
                pressed = true;
                OnButtonPressed();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "Player" || collision.gameObject.tag == "Box") && !collision.isTrigger)
        {
            objectsInArea.Remove(collision);
            if(objectsInArea.Count == 0)
            {
                pressed = false;
            }
        }
    }

    private void OnButtonPressed()
    {
        interactableObject.GetComponent<ButtonInteractableObject>().Activate();
    }
}
