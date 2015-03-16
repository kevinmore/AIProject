using FSM_NS;
using UnityEngine;
using System.Collections;

public class bobController: stateController ,stateControllerInterface 
{
void Start ()
  {
    string pushString = FSM_NS.HelperFile.getTextFileFromResource("bobFSM");
    myStateMachine = new FSM_NS.FSM(pushString,this);

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
private void Entered_State_EnterMineAndDigForNugget ()
  {
 Debug.Log("Entered State EnterMineAndDigForNugget");  
   //state entered handling code goes here

  }
//.......................................................................



//.......................................................................
//This state happens when...
private void Left_State_EnterMineAndDigForNugget ()
  {
 Debug.Log("Left State EnterMineAndDigForNugget");  
   //state left handling code goes here

  }
//.......................................................................



//.......................................................................
//This state happens when...
private void Entered_State_VisitBankAndDepositGold ()
  {
 Debug.Log("Entered State VisitBankAndDepositGold");  
   //state entered handling code goes here

  }
//.......................................................................



//.......................................................................
//This state happens when...
private void Left_State_VisitBankAndDepositGold ()
  {
 Debug.Log("Left State VisitBankAndDepositGold");  
   //state left handling code goes here

  }
//.......................................................................



//.......................................................................
//This state happens when...
private void Entered_State_QuenchThirst ()
  {
 Debug.Log("Entered State QuenchThirst");  
   //state entered handling code goes here

  }
//.......................................................................



//.......................................................................
//This state happens when...
private void Left_State_QuenchThirst ()
  {
 Debug.Log("Left State QuenchThirst");  
   //state left handling code goes here

  }
//.......................................................................



//.......................................................................
//This state happens when...
private void Entered_State_GoHomeAndSleepTilRested ()
  {
 Debug.Log("Entered State GoHomeAndSleepTilRested");  
   //state entered handling code goes here

  }
//.......................................................................



//.......................................................................
//This state happens when...
private void Left_State_GoHomeAndSleepTilRested ()
  {
 Debug.Log("Left State GoHomeAndSleepTilRested");  
   //state left handling code goes here

  }
//.......................................................................





  }

