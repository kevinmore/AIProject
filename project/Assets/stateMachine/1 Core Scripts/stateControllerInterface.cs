using UnityEngine;
using System.Collections;


public interface StateControllerInterface 
{
    void enteredState(string stateName);
	void leftState(string stateName);
	void tickFired();
}
