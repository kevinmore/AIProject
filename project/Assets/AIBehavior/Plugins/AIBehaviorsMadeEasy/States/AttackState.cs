using UnityEngine;
using AIBehaviorEditor;

#if UNITY_EDITOR
using UnityEditor;
using System;
using System.Reflection;
using System.Collections.Generic;
#endif


namespace AIBehavior
{
	public class AttackState : CooldownableState
	{
		public float attackDamage = 10.0f;

		public string attackAnimName = "";
		public float attackPoint = 0.5f;
		public float animAttackTime = 0.0f;

		public Component scriptWithAttackMethod;
		public string methodName = "";

		private Animation attackAnimation;
		private float animationLength = 0.0f;
		private float curAnimPosition = 0.0f;
		protected float previousSamplePosition = 0.0f;

		public int attacksBeforeReload = 10;
		public int attackCount = 0;
		public BaseState reloadState = null;

		private SkinnedMeshRenderer skinnedMeshRenderer = null;


		protected override void Awake()
		{
			skinnedMeshRenderer = transform.root.GetComponentInChildren<SkinnedMeshRenderer>();
			base.Awake();
		}


		protected override void Init(AIBehaviors fsm)
		{
			fsm.MoveAgent(fsm.transform, 0.0f, rotationSpeed);
			attackAnimation = fsm.gameObject.GetComponentInChildren<Animation>();

			if ( attackAnimation != null && attackAnimation[attackAnimName] != null )
			{
				animationLength = attackAnimation[attackAnimName].length;
			}
			else
			{
				animationLength = 1.0f;
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
			Transform target = fsm.GetClosestPlayer(objectFinder.GetTransforms());
			string attackAnimName = animationStates[0].name;
			bool useAnimationTime = skinnedMeshRenderer != null ? skinnedMeshRenderer.isVisible : true;

			if ( !useAnimationTime && attackAnimation != null )
			{
				useAnimationTime = attackAnimation.cullingType == AnimationCullingType.AlwaysAnimate;
			}

			if ( target != null )
			{
				fsm.MoveAgent(target, movementSpeed, rotationSpeed);
			}

			AIAnimationState animState = fsm.animationStates.GetStateWithName(attackAnimName);
			fsm.PlayAnimation(animState);

			if ( scriptWithAttackMethod != null )
			{
				if ( !string.IsNullOrEmpty(methodName) )
				{
					if ( useAnimationTime && attackAnimation != null && attackAnimation[attackAnimName] != null )
					{
						curAnimPosition = attackAnimation[attackAnimName].normalizedTime % 1.0f;
					}
					else
					{
						curAnimPosition %= 1.0f;
						curAnimPosition += Time.deltaTime / animationLength;
					}

					if ( previousSamplePosition > attackPoint || curAnimPosition < attackPoint )
					{
						previousSamplePosition = curAnimPosition;
						return;
					}
					
					previousSamplePosition = curAnimPosition;

					TriggerCooldown();
					Attack(fsm, target);
				}
			}
		}


		protected virtual void Attack(AIBehaviors fsm, Transform target)
		{
			scriptWithAttackMethod.SendMessage(methodName, new AttackData(target, attackDamage, this));
			fsm.PlayAudio();

			attackCount++;

			if ( attackCount > attacksBeforeReload )
			{
				attackCount = 0;
				fsm.ChangeActiveState(reloadState);
			}
		}


		public override bool RotatesTowardTarget ()
		{
			return true;
		}


#if UNITY_EDITOR
		// === Editor Methods === //

		public override void OnStateInspectorEnabled(SerializedObject m_ParentObject)
		{
		}


		protected override void DrawStateInspectorEditor(SerializedObject m_State, AIBehaviors fsm)
		{
			SerializedProperty m_property;

			string[] animNames = AIBehaviorsAnimationEditorGUI.GetAnimationStateNames(m_State);
			int curAttackAnimIndex = 0;

			for ( int i = 0; i < animNames.Length; i++ )
			{
				if ( animNames[i] == animationStates[0].name )
				{
					curAttackAnimIndex = i;
				}
			}

			m_property = m_State.FindProperty("attackDamage");
			EditorGUILayout.PropertyField(m_property);

			// Animation Settings

			GUILayout.Label("Animation Settings:", EditorStyles.boldLabel);

			m_property = m_State.FindProperty("attackPoint");
			EditorGUILayout.Slider(m_property, 0.0f, 1.0f);

			if ( !Application.isPlaying )
			{
				if ( curAttackAnimIndex != -1 && curAttackAnimIndex < animNames.Length )
				{
					float calcAttackTime = SampleAttackAnimation(fsm, animNames[curAttackAnimIndex], m_property.floatValue);

					m_property = m_State.FindProperty("animAttackTime");
					m_property.floatValue = calcAttackTime;
				}
			}

			// === Reload Properties === //

			GUILayout.Label("Reload Settings:", EditorStyles.boldLabel);

			m_property = m_State.FindProperty("attacksBeforeReload");
			EditorGUILayout.PropertyField(m_property, new GUIContent("Attacks Before Reload"));

			m_property = m_State.FindProperty("attackCount");
			EditorGUILayout.PropertyField(m_property, new GUIContent("Attack count (of " + attacksBeforeReload + ")"));

			GUILayout.BeginHorizontal();
			{
				GUILayout.Label("Reload State:");
				m_property = m_State.FindProperty("reloadState");
				m_property.objectReferenceValue = AIBehaviorsStatePopups.DrawEnabledStatePopup(fsm, m_property.objectReferenceValue as BaseState);

				if ( reloadState == null )
				{
					m_property.objectReferenceValue = this;
				}
			}
			GUILayout.EndHorizontal();

			// === Attack Method === //

			GUILayout.Label("Attack Method:", EditorStyles.boldLabel);

			Component[] components = GetAttackMethodComponents(fsm.gameObject);
			int selectedComponent = -1, newSelectedComponent = 0;

			if ( components.Length > 0 )
			{
				string[] componentNames = GetAttackMethodComponentNames(components);

				for ( int i = 0; i < components.Length; i++ )
				{
					if ( components[i] == scriptWithAttackMethod )
					{
						selectedComponent = i;
						break;
					}
				}

				newSelectedComponent = EditorGUILayout.Popup(selectedComponent, componentNames);

				if ( selectedComponent != newSelectedComponent )
				{
					m_property = m_State.FindProperty("scriptWithAttackMethod");
					m_property.objectReferenceValue = components[newSelectedComponent];
				}
			}
			else
			{
				AIBehaviorsCodeSampleGUI.Draw(typeof(AttackData), "attackData", "OnAttack");
			}

			if ( components.Length > 0 )
			{
				string[] methodNames = GetAttackMethodNamesForComponent(components[selectedComponent < 0 ? 0 : selectedComponent]);
				int curSelectedMethod = -1, newSelectedMethod = 0;

				for ( int i = 0; i < methodNames.Length; i++ )
				{
					if ( methodNames[i] == methodName )
					{
						curSelectedMethod = i;
						break;
					}
				}

				newSelectedMethod = EditorGUILayout.Popup(curSelectedMethod, methodNames);
		
				if ( curSelectedMethod != newSelectedMethod )
				{
					m_property = m_State.FindProperty("methodName");
					m_property.stringValue = methodNames[newSelectedMethod];
				}
			}

			m_State.ApplyModifiedProperties();
		}


		Component[] GetAttackMethodComponents(GameObject fsmGO)
		{
			Component[] components = AIBehaviorsComponentInfoHelper.GetNonFSMComponents(fsmGO);
			List<Component> componentList = new List<Component>();

			foreach ( Component component in components )
			{
				if ( GetAttackMethodNamesForComponent(component).Length > 0 )
					componentList.Add(component);
			}

			return componentList.ToArray();
		}


		string[] GetAttackMethodComponentNames(Component[] components)
		{
			string[] componentNames = new string[components.Length];

			for ( int i = 0; i < components.Length; i++ )
			{
				componentNames[i] = components[i].GetType().ToString();
			}

			return componentNames;
		}


		string[] GetAttackMethodNamesForComponent(Component component)
		{
			if ( component != null )
			{
				List<string> methodNames = new List<string>();
				Type type = component.GetType();
				MethodInfo[] methods = type.GetMethods();

				foreach ( MethodInfo mi in methods )
				{
					ParameterInfo[] parameters = mi.GetParameters();

					if ( parameters.Length == 1 )
					{
						if ( parameters[0].ParameterType == typeof(AttackData) )
						{
							methodNames.Add(mi.Name);
						}
					}
				}

				return methodNames.ToArray();
			}

			return new string[0];
		}


		float SampleAttackAnimation(AIBehaviors stateMachine, string clipName, float position)
		{
			Animation anim = stateMachine.gameObject.GetComponentInChildren<Animation>();

			if ( anim != null )
			{
				AnimationClip clip = anim.GetClip(clipName);

				if ( clip != null )
				{
					anim.Play(clip.name);
					anim[clip.name].normalizedTime = position;
					anim.Sample();
					anim[clip.name].normalizedTime = 0.0f;

					return anim[clip.name].length * position;
				}
			}

			return 0.0f;
		}


		protected override bool UsesMultipleAnimations()
		{
			return false;
		}
#endif
	}
}