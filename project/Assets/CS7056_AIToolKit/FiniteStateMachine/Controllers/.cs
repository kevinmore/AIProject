using UnityEngine;
using System.Collections;
using CS7056_AIToolKit;

public class  : StateController, StateControllerInterface 
{
void Start ()
  {
    string pushString = HelperFile.getTextFileFromResource("FSM");
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
private void Entered_State_state ()
  {
 Debug.Log("Entered State state");  
   //state entered handling code goes here

  }
//.......................................................................



//.......................................................................
//This state happens when...
private void Left_State_state ()
  {
 Debug.Log("Left State state");  
   //state left handling code goes here

  }
//.......................................................................



//.......................................................................
//This state happens when...
private void Entered_State_state ()
  {
 Debug.Log("Entered State state");  
   //state entered handling code goes here

  }
//.......................................................................



//.......................................................................
//This state happens when...
private void Left_State_state ()
  {
 Debug.Log("Left State state");  
   //state left handling code goes here

  }
//.......................................................................





  }

