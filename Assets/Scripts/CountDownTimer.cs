using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownTimer : MonoBehaviour
{
    [SerializeField] private float timeLeft;
    [SerializeField] private Text display;
    private bool done;
    void Start()
    {
    
    }
    public void OnUpdate()
    {
        int intTime = (int) timeLeft;
        display.text = ("" + intTime);

        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            Debug.Log("Timer Done");
            done = true;
        }
    }

    public bool Done{
        get{return done;}
    }
}
