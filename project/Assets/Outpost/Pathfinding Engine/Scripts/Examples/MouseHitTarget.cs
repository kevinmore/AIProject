using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class MouseHitTarget : MonoBehaviour {
	
	void Update () {
		if(Input.GetMouseButtonUp(0)){
			//get the point where the mouse click land
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			//if mouse lick land on a valid point
			if (Physics.Raycast (ray, out hit, Mathf.Infinity)) {
				foreach(Agent agent in Agent.agents){
					if(agent.selected){
						//Assign position where agent must to go.
						agent.GoTo(hit.point);
					}
				}
			}
		}
	}
	

}
