using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void OnEnter();     //The way to enter the state
    void OnUpdate();    //Ways to maintain status
    void OnExit();      //Method of exiting status
}