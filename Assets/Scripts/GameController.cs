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
    [SerializeField] private CameraFollow cam;
    private List<Nuke> nukes;
    private List<Switch> switches;

    
   

    
    void Start()
    {
        nukes = new List<Nuke>();
        Nuke[] nuk = FindObjectsOfType<Nuke>();
        foreach(Nuke nuke in nuk){
            nukes.Add(nuke);
        }

        switches = new List<Switch>();
        Switch[] swi = FindObjectsOfType<Switch>();
        foreach(Switch _switch in swi){
            switches.Add(_switch);
        }
    }

    void Update()
    {
        player.OnUpdate(io);
        cam.OnUpdate(player.transform);
        HandleNukeActivations();
        HandleRestarts();
        HandleGoalReached();
        HandleSwitches();
        // HandleClock();
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

    private void HandleClock(){
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            Debug.Log("Timer Done");
            Restart();
        }
    }

    private void HandleSwitches(){
        foreach(Switch _switch in switches){
            if(_switch.Activated){
                Debug.Log("unlock");
                _switch.Door.Unlock();
            }
        }
    }
}
