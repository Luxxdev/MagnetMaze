using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    [SerializeField] private int nextScene;

    void OnTriggerEnter2D(Collider2D other)
    {
       if (other.gameObject.CompareTag("Player"))
       {
            LoadNextScene(nextScene);
       }    
    }

    public void LoadNextScene(int scene)
    {
        SceneManager.LoadScene($"LEVEL_{scene}");
    }
}
