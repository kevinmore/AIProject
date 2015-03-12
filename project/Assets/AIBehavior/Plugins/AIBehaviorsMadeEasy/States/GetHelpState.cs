using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace AIBehavior
{
	public class GetHelpState : CooldownableState
	{
		public float helpRadius = 30.0f;
		public string helpTag = "Untagged";


		protected override void Init(AIBehaviors fsm)
		{
			GameObject[] gos = GameObject.FindGameObjectsWithTag(helpTag);
			Vector3 tfmPosition = fsm.transform.position;

			foreach ( GameObject go in gos )
			{
				if ( Vector3.Distance(go.transform.position, tfmPosition) < helpRadius )
				{
					Vector3 aiTargetPosition = tfmPosition;
					aiTargetPosition.y = go.transform.position.y;

					AIBehaviors helperFSM = go.GetComponent<AIBehaviors>();

					if ( helperFSM != null )
					{
						HelpAnotherFSM(aiTargetPosition, helperFSM);
					}
				}
			}

			fsm.PlayAudio();
			fsm.gameObject.SendMessage("OnGetHelp", SendMessageOptions.DontRequireReceiver);

			TriggerCooldown();
		}


		// Change the other FSMs state to the HelpState
		private void HelpAnotherFSM(Vector3 helpTargetPosition, AIBehaviors otherFSM)
		{
			HelpState helpState = null;

			helpState = otherFSM.GetState<HelpState>();

			if ( helpState != null )
			{
				helpState.helpPoint = helpTargetPosition;
				otherFSM.ChangeActiveState(helpState);
			}
		}


		protected override void StateEnded(AIBehaviors fsm)
		{
		}


		protected override bool Reason(AIBehaviors fsm)
		{
			return true;
		}


		protected override void Action(AIBehaviors fsm)
		{
		}
		
		
	#if UNITY_EDITOR
		// === Editor Methods === //

		public override void OnStateInspectorEnabled(SerializedObject m_ParentObject)
		{
		}


		protected override void DrawStateInspectorEditor(SerializedObject m_Object, AIBehaviors stateMachine)
		{
			SerializedObject m_State = new SerializedObject(this);
			SerializedProperty m_Prop;

			EditorGUILayout.Separator();

			GUILayout.Label("Get Help From Tagged Objects");
			string newTag = EditorGUILayout.TagField(helpTag);

			if ( newTag != helpTag )
			{
				m_Prop = m_State.FindProperty("helpTag");
				m_Prop.stringValue = newTag;
			}

			EditorGUILayout.Separator();

			EditorGUILayout.Separator();

			m_Prop = m_State.FindProperty("helpRadius");
			EditorGUILayout.PropertyField(m_Prop);

			m_State.ApplyModifiedProperties();
		}
	#endif
	}
}