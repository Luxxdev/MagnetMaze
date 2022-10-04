using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    [SerializeField] private GameObject hudObject;
    private bool textShown = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        print(textShown);
        if (other.gameObject.CompareTag("Player") && !textShown)
        {
            hudObject.GetComponent<BottomTextManagement>().CallDialog();
            textShown = true;
        }
    }

}
