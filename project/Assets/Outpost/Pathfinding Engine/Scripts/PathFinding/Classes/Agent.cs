using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class Agent{
	[HideInInspector] 	static public List<Agent> agents = new List<Agent>();
	[HideInInspector] 	public List<Vector3> path = new List<Vector3>();
	[HideInInspector] 	public int 			 ID;
	[HideInInspector] 	public GameObject 	 pivot;
	[HideInInspector] 	public Node 		 startNode;
	[HideInInspector] 	public Node 		 endNode;
	[HideInInspector]	public bool			 search;
						public float 		 speed;
						public float 		 yOffset;
						public bool  		 selected;
	
	public Agent(float _speed, float _yOffset, bool _selected){
		speed 	 = _speed;
		yOffset  = _yOffset;
		selected = _selected;
	}
	
	public void Launch(Transform transformObj){
		pivot = new GameObject ();
		pivot.transform.position = transformObj.position; 
		if (transformObj.parent != null) {pivot.transform.parent = transformObj.parent;} 
		transformObj.parent = pivot.transform; 
		Agent.agents.Add (this); 
		ID = Agent.agents.Count;
		pivot.name = "AGENT"+ID;
	}

	public void GoTo(Vector3 pos){
		PathFindingSolver.SearchPath (this , pos);
	}

}	
