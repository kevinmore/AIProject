using UnityEngine;
using System.Collections;

public class DynamicObjectMovement : MonoBehaviour {

	public Vector3 displacement = new Vector3 (-7, 0, 0);
	public float speed = 1.0f;
	private Vector3 pos;
	private Vector3 pos2;
	private Vector3 target;

	// Use this for initialization
	void Start () {
		pos = transform.position;
		pos2 = pos + displacement;
		target = pos2;
	}
	
	// Update is called once per frame
	void Update () {
		if (target == pos2) {
			float dist=Vector3.Distance(transform.position, pos2);
			Vector3 direction=(target-transform.position).normalized;
			transform.Translate(direction * Mathf.Min(dist, speed * Time.deltaTime), Space.World);
			if(transform.position == pos2) target=pos;
		}
		if (target == pos) {
			float dist=Vector3.Distance(transform.position, pos);
			Vector3 direction=(target-transform.position).normalized;
			transform.Translate(direction * Mathf.Min(dist, speed * Time.deltaTime), Space.World);
			if(transform.position == pos) target=pos2;
		}
	}
}
