using UnityEngine;
using AIBehaviorEditor;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace AIBehavior
{
	public class SeekState : BaseState
	{
		public string seekItemsWithTag = "Untagged";
		private Transform targetItem = null;

		public BaseState seekTargetReachedState;
		public float distanceToTargetThreshold = 0.25f;
		private float sqrDistanceToTargetThreshold = 1.0f;

		public bool destroyTargetWhenReached = false;


		protected override void Init(AIBehaviors fsm)
		{
			sqrDistanceToTargetThreshold = distanceToTargetThreshold * distanceToTargetThreshold;
			fsm.PlayAudio();
		}


		protected override void StateEnded(AIBehaviors fsm)
		{
		}


		protected override bool Reason(AIBehaviors fsm)
		{
			if ( targetItem == null )
			{
				GameObject[] gos = GameObject.FindGameObjectsWithTag(seekItemsWithTag);
				Vector3 pos = fsm.transform.position;
				Vector3 diff;
				float nearestItem = Mathf.Infinity;

				foreach ( GameObject go in gos )
				{
					Transform tfm = go.transform;
					float sqrMagnitude;

					diff = tfm.position - pos;
					sqrMagnitude = diff.sqrMagnitude;

					if ( sqrMagnitude < nearestItem )
					{
						targetItem = tfm;
						nearestItem = sqrMagnitude;
					}
				}
			}
			else
			{
				float sqrDist = (fsm.transform.position - targetItem.position).sqrMagnitude;

				if ( sqrDist < sqrDistanceToTargetThreshold )
				{
					if ( destroyTargetWhenReached )
					{
						Destroy(targetItem.gameObject);
					}

					fsm.ChangeActiveState(seekTargetReachedState);
					return false;
				}
			}

			return true;
		}


		protected override void Action(AIBehaviors fsm)
		{
			if ( targetItem != null )
			{
				fsm.MoveAgent(targetItem, movementSpeed, rotationSpeed);
			}
		}


	#if UNITY_EDITOR
		// === Editor Methods === //

		public override void OnStateInspectorEnabled(SerializedObject m_ParentObject)
		{
		}


		protected override void DrawStateInspectorEditor(SerializedObject m_Object, AIBehaviors stateMachine)
		{
			SerializedObject m_State = new SerializedObject(this);
			SerializedProperty m_property;

			GUILayout.Label("Seek items with tag:");
			m_property = m_State.FindProperty("seekItemsWithTag");
			m_property.stringValue = EditorGUILayout.TagField(m_property.stringValue);

			EditorGUILayout.Separator();
			GUILayout.BeginHorizontal();
			{
				GUILayout.Label("Seek Target Reached Transition:");
				m_property = m_State.FindProperty("seekTargetReachedState");
				m_property.objectReferenceValue = AIBehaviorsStatePopups.DrawEnabledStatePopup(stateMachine, m_property.objectReferenceValue as BaseState);
			}
			GUILayout.EndHorizontal();

			m_property = m_State.FindProperty("distanceToTargetThreshold");
			float prevValue = m_property.floatValue;
			EditorGUILayout.PropertyField(m_property);

			if ( m_property.floatValue <= 0.0f )
				m_property.floatValue = prevValue;

			m_property = m_State.FindProperty("destroyTargetWhenReached");
			EditorGUILayout.PropertyField(m_property);

			m_State.ApplyModifiedProperties();
		}
	#endif
	}
}