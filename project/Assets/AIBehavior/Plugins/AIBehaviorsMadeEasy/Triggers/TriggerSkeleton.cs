using UnityEngine;
using AIBehavior;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class TriggerSkeleton : BaseTrigger
{
	protected override void Init()
	{
	}


	protected override bool Evaluate(AIBehaviors fsm)
	{
		// Logic here, return true if the trigger was triggered
		return true;
	}


#if UNITY_EDITOR
	/*
	// Implement your own custom GUI here if you want to
	public override void DrawInspectorProperties(AIBehaviors fsm, SerializedObject sObject)
	{
	}
	*/
#endif
}