using CS7056_AIToolKit;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class lightController: StateController ,StateControllerInterface 
{

  public  GameObject redLight;
  public  GameObject yellowLight;
  public  GameObject greenLight;
  public List<GameObject>listners;
  
  public GameObject cars;

void Start ()
  {
	listners=new List<GameObject>();
	yellowLight.SetActive(false);
	greenLight.SetActive(false);
    string pushString = CS7056_AIToolKit.HelperFile.GetTextFileFromResource("lightFSM");
    myStateMachine = new CS7056_AIToolKit.FSM(pushString,this);
	myStateMachine.JumpToState(startStateID);
  }





void Update () 
  {
		SuperUpdate();
		
  }
//

	public void sensor1TrippedBy(GameObject car)
	{
		Debug.Log("currentState"+myStateMachine.state.name);
		car.BroadcastMessage("recieveEvent",myStateMachine.state.name);
		listners.Add(car);
		Debug.Log("tripped1 "+myStateMachine.state.name);
	}
	public void sensor2TrippedBy(GameObject car)
	{
	   
		Debug.Log("tripped2");
		listners.Remove(car);
		car.BroadcastMessage("recieveEvent","green");
	}

public override void TickFired(){
}

private void informListners(string event_)
{
   foreach(GameObject listner in listners)
   {
			listner.BroadcastMessage("recieveEvent",event_);
   }
}

//.......................................................................
//red light
private void Entered_State_red ()
  {
	 Debug.Log("red"); 
		redLight.SetActive(true);
		yellowLight.SetActive(false);
		greenLight.SetActive(false);
		informListners("red");
		//cars.BroadcastMessage("recieveEvent","red");
   //state handling code goes here

  }
	private void Left_State_red ()
	{
	}
//.......................................................................



//.......................................................................
//yellow light
	private void Entered_State_yellow ()
  {
        Debug.Log("yellow"); 
		redLight.SetActive(false); 
		yellowLight.SetActive(true);
		greenLight.SetActive(false);
		informListners("yellow");
		//cars.BroadcastMessage("recieveEvent","yellow");
   //state handling code goes here

  }
	private void Left_State_yellow ()
	{
	}
//.......................................................................



//.......................................................................
//green light
	private void Entered_State_green ()
  {
 Debug.Log("green");  
		redLight.SetActive(false); 
		yellowLight.SetActive(false);
		greenLight.SetActive(true);
		//eventToFSM
		informListners("green");
		//cars.BroadcastMessage("recieveEvent","green");
   //state handling code goes here

  }
	private void Left_State_green ()
	{
	}
//.......................................................................





  }

