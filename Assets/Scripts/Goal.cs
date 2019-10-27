using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    private bool activated = false;
    void OnTriggerEnter2D(Collider2D collider){
        activated = true;
        SceneManager.LoadScene("EndScreen");
    }

    public bool Activated{
        get{return activated;}
    }
}
