using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace AIBehavior
{
	public class NoPlayerInSightTrigger : BaseTrigger
	{
		protected override void Init()
		{
		}


		protected override bool Evaluate(AIBehaviors fsm)
		{
			return fsm.GetClosestPlayerWithinSight(objectFinder.GetTransforms(), false) == null;
		}
	}
}