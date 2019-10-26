using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    private bool activated = false;
    [SerializeField] private Door door;
    void OnTriggerEnter2D(Collider2D collider){
        activated = true;
    }

    public bool Activated{
        get{return activated;}
    }

    public Door Door{
        get{return door;}
    }
}
