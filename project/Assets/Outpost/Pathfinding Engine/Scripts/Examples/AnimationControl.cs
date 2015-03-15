using UnityEngine;
using System.Collections;


public class AnimationControl : MonoBehaviour {

	private Animation anim;
	private PathFindingAgent agentComponent;

	// Use this for initialization
	void Start () {
		anim = transform.GetComponent<Animation> ();
		agentComponent = transform.GetComponent<PathFindingAgent> ();

		anim.Play ("Take 001");
	}


	// Update is called once per frame
	void Update () {
		if( agentComponent.agent.path.Count > 0 ) {
			anim.CrossFade("forward"); 
		}else{
			anim.CrossFade("awake"); 
		}

	}

}
