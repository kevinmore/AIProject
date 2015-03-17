using UnityEngine;
using System.Collections;
using CS7056_AIToolKit;

public class myTestController : StateController, StateControllerInterface 
{
void Start ()
  {
    string pushString = HelperFile.getTextFileFromResource("myTestControllerFSM");
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
private void Entered_State_stateA ()
  {
 Debug.Log("Entered State stateA");  
   //state entered handling code goes here

  }
//.......................................................................



//.......................................................................
//This state happens when...
private void Left_State_stateA ()
  {
 Debug.Log("Left State stateA");  
   //state left handling code goes here

  }
//.......................................................................



//.......................................................................
//This state happens when...
private void Entered_State_stateB ()
  {
 Debug.Log("Entered State stateB");  
   //state entered handling code goes here

  }
//.......................................................................



//.......................................................................
//This state happens when...
private void Left_State_stateB ()
  {
 Debug.Log("Left State stateB");  
   //state left handling code goes here

  }
//.......................................................................





  }

