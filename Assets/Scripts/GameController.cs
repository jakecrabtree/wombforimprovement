using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private InputController io;
    [SerializeField] private PlayerController player;
    [SerializeField] private float timeLeft;
    [SerializeField] private Goal goal;

    private List<Nuke> nukes;
   

    
    void Start()
    {
        nukes = new List<Nuke>();
        Nuke[] nuk = FindObjectsOfType<Nuke>();
        foreach(Nuke nuke in nuk){
            nukes.Add(nuke);
        }
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            //Debug.Log("Timer Done");
            // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        player.OnUpdate(io);
        HandleNukeActivations();
        HandleRestarts();
        HandleGoalReached();
    }

    private void FixedUpdate()
    {
        player.OnFixedUpdate(io);
    }

    private void HandleRestarts(){
        if(io.RestartKeyPressed){
            Restart();
        }
    }

    private void Restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void HandleNukeActivations()
    {
        foreach(Nuke nuke in nukes){
            if(nuke.Activated){
                Debug.Log("Nuke Activated");
            }
        }
    }

    private void HandleGoalReached(){
        if(goal.Activated){
            Debug.Log("You win");
        }
    }
}
