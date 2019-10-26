using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Nuke : MonoBehaviour
{
    private bool activated = false;
    void OnTriggerEnter2D(Collider2D collider){
        activated = true;
    }

    public bool Activated{
        get{return activated;}
    }
}
