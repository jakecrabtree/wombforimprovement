using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private bool locked = true;

    public void Unlock(){
        if(GetComponent<BoxCollider2D>() && locked){
            locked = false;
            Debug.Log("Door unlocked");
            Destroy(GetComponent<BoxCollider2D>());
        }
    }
}
