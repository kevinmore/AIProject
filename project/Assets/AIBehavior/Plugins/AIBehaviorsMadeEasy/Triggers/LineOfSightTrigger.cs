using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace AIBehavior
{
	public class LineOfSightTrigger : BaseTrigger
	{
		protected override void Init()
		{
		}


		protected override bool Evaluate(AIBehaviors fsm)
		{
			return fsm.GetClosestPlayerWithinSight(objectFinder.GetTransforms(), true) != null;
		}


	#if UNITY_EDITOR
		public override void DrawInspectorProperties(AIBehaviors fsm, SerializedObject sObject)
		{
		}
	#endif
	}
}