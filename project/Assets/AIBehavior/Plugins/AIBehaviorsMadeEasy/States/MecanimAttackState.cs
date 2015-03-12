using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AIBehavior
{
	public class MecanimAttackState : AttackState
	{
		private Animator animator;
		float prevNormalizedTime = 0.0f;
		public int mecanimLayerIndex = 0;


		protected override void Awake()
		{
			base.Awake();

			animator = transform.parent.GetComponentInChildren<Animator>();

			if ( animator == null )
			{
				Debug.LogWarning("An Animator component must be attached when using the " + this.GetType());
			}
		}


		protected override void Init (AIBehaviors fsm)
		{
			prevNormalizedTime = 0.0f;
			base.Init(fsm);
		}


		protected override void Action (AIBehaviors fsm)
		{
			AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(mecanimLayerIndex);
			int hash = Animator.StringToHash(animationStates[0].name);

			if ( hash == stateInfo.nameHash )
			{
				float curNormalizedTime = stateInfo.normalizedTime % 1.0f;

				if ( curNormalizedTime > attackPoint && prevNormalizedTime < attackPoint )
				{
					if ( scriptWithAttackMethod != null )
					{
						TriggerCooldown();
						Attack(fsm, fsm.GetClosestPlayerWithinSight(objectFinder.GetTransforms()));
					}
				}

				prevNormalizedTime = curNormalizedTime;
			}
		}


		protected override void StateEnded (AIBehaviors fsm)
		{
			base.StateEnded(fsm);
		}


#if UNITY_EDITOR
		// === Editor Methods === //
		protected override void DrawStateInspectorEditor(SerializedObject m_State, AIBehaviors fsm)
		{
			SerializedProperty m_property = m_State.FindProperty("mecanimLayerIndex");

			EditorGUILayout.PropertyField(m_property);

			base.DrawStateInspectorEditor(m_State, fsm);
		}
#endif
	}
}