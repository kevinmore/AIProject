using CS7056_AIToolKit;
using UnityEngine;
using System.Collections;

public class testController: stateController ,stateControllerInterface 
{
void Start ()
  {
    string pushString = HelperFile.getTextFileFromResource("testFSM");
    myStateMachine = new FSM(pushString,this);

myStateMachine.jumpToState(startStateID);
   }

void Update () 
  {
//Your code here

superUpdate();

  }



public override void tickFired(){

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

