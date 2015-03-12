using UnityEngine;


namespace AIBehaviorExamples
{
	public class Shooter : MonoBehaviour
	{
		// Update is called once per frame
		void Update ()
		{
			if ( Input.GetMouseButtonDown(0) )
			{
				Ray mouseRay = Camera.main.ScreenPointToRay(new Vector2(Screen.width * 0.5f, Screen.height * 0.5f));
				GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				Transform tfm = go.transform;
				Rigidbody rb = go.AddComponent<Rigidbody>();

				go.renderer.useLightProbes = true;
				go.AddComponent<ProjectileCollider>();

				tfm.position = mouseRay.origin + mouseRay.direction * 1.0f;
				tfm.localScale *= 0.25f;

				rb.AddForce(mouseRay.direction * 1500.0f);

				go.tag = "Respawn";
			}
		}
	}
}