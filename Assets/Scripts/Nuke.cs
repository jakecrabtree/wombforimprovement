using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Nuke : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider){
        Debug.Log(collider.gameObject.name);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
