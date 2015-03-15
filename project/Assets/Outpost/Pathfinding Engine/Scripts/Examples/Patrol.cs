using UnityEngine;
using System.Collections;

public class Patrol : MonoBehaviour {
	private PathFindingAgent agentComponent;
	private Agent agent;
	public Transform[] points;
	private int pointPos=0;
	
	void Start () {
		agentComponent = transform.GetComponent<PathFindingAgent> ();
		agent = agentComponent.agent;
	}

	void Update () {
		if ( points != null &&  points.Length > 0) {
			if(agent.path.Count==0){
				pointPos+=1;
				if( pointPos>=points.Length ) pointPos=0;
				agent.GoTo(points[pointPos].position);
			}
		}
	}

}
