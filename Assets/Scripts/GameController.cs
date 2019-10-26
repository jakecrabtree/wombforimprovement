using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private InputController io;
    [SerializeField] private PlayerController player;
    [SerializeField] private float timeLeft;

    
    void Start()
    {
        
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;
        /*if (timeLeft < 0)
        {
            Application.LoadLevel(Application.loadedLevel);
        }*/

        player.OnUpdate(io);
    }

    private void FixedUpdate()
    {
        player.OnFixedUpdate(io);
    }

    private void HandleNukeCollisions()
    {

    }
}
