using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private InputController io;
    [SerializeField] private PlayerController player;
    [SerializeField] private Goal goal;
    [SerializeField] private CameraFollow cam;

    [SerializeField] private CountDownTimer timer;
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
        HandleTimer();
    }

    private void FixedUpdate()
    {
        player.OnFixedUpdate(io);
    }

    private void HandleRestarts(){
        if(io.RestartKeyPressed){
            RestartLevel();
        }
    }

    private void RestartLevel(){
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

    private void HandleTimer(){
        timer.OnUpdate();

        if(timer.Done){
            RestartLevel();
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
