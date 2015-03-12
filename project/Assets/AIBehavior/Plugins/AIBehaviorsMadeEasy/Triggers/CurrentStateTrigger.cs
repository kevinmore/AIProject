using UnityEngine;
using AIBehavior;
using System.Collections;

#if UNITY_EDITOR
using AIBehaviorEditor;
using UnityEditor;
#endif


namespace AIBehavior
{
	public class CurrentStateTrigger : BaseTrigger
	{
		public BaseState checkState;
		public IsAndIsNot isOrNot;


		protected override void Init()
		{
		}


		protected override bool Evaluate(AIBehaviors fsm)
		{
			bool result = fsm.currentState == checkState;

			if ( isOrNot == IsAndIsNot.IsNot )
			{
				return !result;
			}

			return result;
		}


#if UNITY_EDITOR
		public override void DrawInspectorProperties(AIBehaviors fsm, SerializedObject sObject)
		{
			SerializedProperty stateProperty = sObject.FindProperty("checkState");

			EditorGUILayout.BeginHorizontal();
			{
				isOrNot = (IsAndIsNot)EditorGUILayout.EnumPopup("Current state: ", isOrNot);
				stateProperty.objectReferenceValue = AIBehaviorsStatePopups.DrawEnabledStatePopup(fsm, stateProperty.objectReferenceValue as BaseState);
			}
			EditorGUILayout.EndHorizontal();
		}
#endif
	}
}