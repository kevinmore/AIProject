using UnityEngine;
using System.Collections;


public class AnimationControl : MonoBehaviour {

	private Animation anim;
	private AgentComponent agentComponent;

	// Use this for initialization
	void Start () {
		anim = transform.GetComponent<Animation> ();
		agentComponent = transform.GetComponent<AgentComponent> ();

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
