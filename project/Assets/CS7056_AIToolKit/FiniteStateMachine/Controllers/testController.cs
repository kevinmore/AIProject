using CS7056_AIToolKit;
using UnityEngine;
using System.Collections;

public class testController: StateController ,StateControllerInterface 
{
void Start ()
  {
    string pushString = CS7056_AIToolKit.HelperFile.GetTextFileFromResource("testFSM");
    myStateMachine = new CS7056_AIToolKit.FSM(pushString,this);

myStateMachine.JumpToState(startStateID);
   }

void Update () 
  {
//Your code here

SuperUpdate();

  }



public override void TickFired(){

}

//.......................................................................
//This state happens when...
private void Entered_State_start ()
  {
 Debug.Log("Entered State start");  
 eventToFSM = "start";
   //state entered handling code goes here

  }
//.......................................................................



//.......................................................................
//This state happens when...
private void Left_State_start ()
  {
 Debug.Log("Left State start");  
   //state left handling code goes here

  }
//.......................................................................



//.......................................................................
//This state happens when...
private void Entered_State_playing ()
  {
 Debug.Log("Entered State playing");  
   //state entered handling code goes here

  }
//.......................................................................



//.......................................................................
//This state happens when...
private void Left_State_playing ()
  {
 Debug.Log("Left State playing");  
   //state left handling code goes here

  }
//.......................................................................



//.......................................................................
//This state happens when...
private void Entered_State_gameOver ()
  {
 Debug.Log("Entered State gameOver");  
   //state entered handling code goes here

  }
//.......................................................................



//.......................................................................
//This state happens when...
private void Left_State_gameOver ()
  {
 Debug.Log("Left State gameOver");  
   //state left handling code goes here

  }
//.......................................................................





  }

