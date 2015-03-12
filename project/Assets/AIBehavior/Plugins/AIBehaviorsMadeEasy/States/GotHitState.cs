using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace AIBehavior
{
	public class GotHitState : CooldownableState
	{
		public bool hitMovesPosition = true;
		public float movePositionAmount = 1.0f;


		protected override void Init(AIBehaviors fsm)
		{
			fsm.PlayAudio();
			TriggerCooldown();
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
			fsm.MoveAgent(fsm.currentDestination, movementSpeed, rotationSpeed);
		}


		new public bool CoolDownFinished()
		{
			return base.CoolDownFinished();
		}


		public virtual bool CanGetHit(AIBehaviors fsm)
		{
			return !(fsm.currentState is DeadState);
		}
		
		
	#if UNITY_EDITOR
		// === Editor Methods === //

		public override void OnStateInspectorEnabled(SerializedObject m_ParentObject)
		{
		}


		protected override void DrawStateInspectorEditor(SerializedObject m_Object, AIBehaviors stateMachine)
		{
			SerializedObject m_State = new SerializedObject(this);

			m_State.ApplyModifiedProperties();
		}
	#endif
	}
}