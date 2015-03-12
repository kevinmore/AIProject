//#define USE_ASTAR

using UnityEngine;
using System.Collections;

namespace AIBehavior
{
#if USE_ASTAR
	[RequireComponent(typeof(AIPath))]
#endif
	[RequireComponent(typeof(CharacterController))]
	public class AstarCharacterController : MonoBehaviour
	{
		public Transform target = null;

#if USE_ASTAR
		private AIPath aiPath = null;


		void Awake()
		{
			AIBehaviors ai = GetComponent<AIBehaviors>();

			if ( target == null )
			{
				target = new GameObject().transform;
			}

			aiPath = GetComponent<AIPath>();

			if ( aiPath == null )
			{
				Debug.LogError("You must add the 'AIPath' to the Game Object '" + name + "' in order to use the Astar Pathfinding Project integration.");
				return;
			}

			aiPath.target = target;
			ai.externalMove = OnMove;
		}
	

		void OnMove(Vector3 targetPoint, float targetSpeed, float rotationSpeed)
		{
			aiPath.speed = targetSpeed;
			aiPath.turningSpeed = rotationSpeed;
			target.position = targetPoint;
		}
#endif
	}
}