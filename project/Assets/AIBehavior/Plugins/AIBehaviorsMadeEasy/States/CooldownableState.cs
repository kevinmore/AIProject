using UnityEngine;
using AIBehaviorEditor;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AIBehavior
{
	public abstract class CooldownableState : BaseState
	{
		// === Cooldown Properties === //
		
		public float cooldownTime = 0.0f;
		private float cooledDownTime = 0.0f;
		public bool initResetsCooldown = true;
		public bool switchStateIfStillCoolingDown = false;
		public BaseState stillCoolingDownState;
		public bool hasCooldownLimit = false;
		public int cooldownLimit = 3;
		public BaseState cooldownLimitExceededState;
		private int cooldowns = 0;
		

		public override void InitState(AIBehaviors fsm)
		{
			if ( InitCooldown(fsm) )
			{
				base.InitState(fsm);
			}
		}
		
		
		protected void TriggerCooldown()
		{
			cooledDownTime = Time.time + cooldownTime;
		}
		
		
		protected bool CoolDownFinished ()
		{
			return cooledDownTime < Time.time;
		}

		
		bool InitCooldown (AIBehaviors fsm)
		{
			cooldowns = 0;
			
			if ( initResetsCooldown )
			{
				cooledDownTime = 0.0f;
			}
			else if ( switchStateIfStillCoolingDown && !CoolDownFinished() )
			{
				if ( stillCoolingDownState == this )
				{
					Debug.LogWarning ("Switching back to the same state when a cooldown isn't finished would lock up the system, choose another state other than '" + name + "' in the state '" + name + "'");
				}
				else
				{
					fsm.ChangeActiveState(stillCoolingDownState);
					return false;
				}
			}

			return true;
		}


		public override void HandleAction (AIBehaviors fsm)
		{
			// Can we run this state's action again ?
			if ( CoolDownFinished() )
			{
				if ( hasCooldownLimit )
				{
					if ( cooldowns > cooldownLimit )
					{
						fsm.ChangeActiveState(cooldownLimitExceededState);
						return;
					}
					else
					{
						cooldowns++;
					}
				}
				
				base.HandleAction (fsm);
			}
			else if ( switchStateIfStillCoolingDown )
			{
				fsm.ChangeActiveState(stillCoolingDownState);
			}
		}


		#if UNITY_EDITOR
		protected override void DrawFoldouts (UnityEditor.SerializedObject m_Object, AIBehaviors fsm)
		{
			base.DrawFoldouts (m_Object, fsm);

			if ( DrawFoldout("cooldownFoldout", "Cooldown Properties:") )
			{
				DrawCooldownProperties(m_Object, fsm);
			}
			
			EditorGUILayout.Separator();
		}
		
		
		void DrawCooldownProperties(SerializedObject m_State, AIBehaviors fsm)
		{
			SerializedProperty m_Property;
			
			GUILayout.Label("Cooldown Properties:", EditorStyles.boldLabel);
			
			m_Property = m_State.FindProperty("cooldownTime");
			EditorGUILayout.PropertyField(m_Property);
			
			m_Property = m_State.FindProperty("initResetsCooldown");
			EditorGUILayout.PropertyField(m_Property, new GUIContent("Reset on State Change"));
			
			if ( !m_Property.boolValue )
			{
				m_Property = m_State.FindProperty("switchStateIfStillCoolingDown");
				EditorGUILayout.PropertyField(m_Property);
				
				if ( m_Property.boolValue )
				{
					GUILayout.BeginHorizontal();
					{
						GUILayout.Label("Still Cooling Down Transition:");
						
						m_Property = m_State.FindProperty("stillCoolingDownState");
						m_Property.objectReferenceValue = AIBehaviorsStatePopups.DrawEnabledStatePopup(fsm, m_Property.objectReferenceValue as BaseState);
					}
					GUILayout.EndHorizontal();
				}
			}
			
			m_Property = m_State.FindProperty("hasCooldownLimit");
			EditorGUILayout.PropertyField(m_Property);
			
			if ( m_Property.boolValue )
			{
				m_Property = m_State.FindProperty("cooldownLimit");
				EditorGUILayout.PropertyField(m_Property);
				
				GUILayout.BeginHorizontal();
				{
					GUILayout.Label("Limit Exceeded Transition:");
					
					m_Property = m_State.FindProperty("cooldownLimitExceededState");
					m_Property.objectReferenceValue = AIBehaviorsStatePopups.DrawEnabledStatePopup(fsm, m_Property.objectReferenceValue as BaseState);
				}
				GUILayout.EndHorizontal();
			}
		}
		#endif
	}
}
