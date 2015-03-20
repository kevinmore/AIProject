using UnityEngine;
using System.Collections;


public interface StateControllerInterface 
{
    void EnteredState(string stateName);
	void LeftState(string stateName);
	void TickFired();
}
