using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private InputController io;
    [SerializeField] private PlayerController player;
    
    void Start()
    {
        
    }

    void Update()
    {
        player.OnUpdate(io);
    }

    private void FixedUpdate()
    {
        player.OnFixedUpdate(io);
    }
}
