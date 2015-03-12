using UnityEngine;
using AIBehaviorEditor;

#if UNITY_EDITOR
using UnityEditor;
#endif

using Random = UnityEngine.Random;


namespace AIBehavior
{
	public abstract class BaseState : SavableComponent
	{
		public bool isEnabled = true;

		new public string name = "";

		// === Triggers === //

		public BaseTrigger[] triggers = new BaseTrigger[0];

		// === Tagged Object Finder === //

		public bool ownsObjectFinder = false;
		public TaggedObjectFinder objectFinder;

		// === General Variables === //

		public float movementSpeed = 1.0f;
		public float rotationSpeed = 90.0f;
		private float lastActionTime = 0.0f;
		protected float deltaTime = 0.0f;

		// === Animation Variables === //

		public AIAnimationStates animationStatesComponent;
		public AIAnimationState[] animationStates = new AIAnimationState[1];

		// === Audio Variables === //

		public AudioClip audioClip = null;
		public float audioVolume = 1.0f;
		public float audioPitch = 1.0f;
		public float audioPitchRandomness = 0.0f;
		public bool loopAudio = false;

		// === State Methods === //

		protected abstract void Init(AIBehaviors fsm);
		protected abstract bool Reason(AIBehaviors fsm);
		protected abstract void Action(AIBehaviors fsm);
		protected abstract void StateEnded(AIBehaviors fsm);


		public BaseState()
		{
			objectFinder = CreateObjectFinder();
		}
		
		
		protected virtual TaggedObjectFinder CreateObjectFinder()
		{
			return new TaggedObjectFinder();
		}


		protected virtual void Awake()
		{
			objectFinder.CacheTransforms(CachePoint.Awake);
		}


		// === Init === //

		public virtual void InitState(AIBehaviors fsm)
		{
			lastActionTime = Time.time;
			deltaTime = 0.0f;

			InitObjectFinder(fsm);
			InitTriggers();
			Init(fsm);
			PlayRandomAnimation(fsm);
		}


		void InitObjectFinder (AIBehaviors fsm)
		{
			if ( !objectFinder.useCustomTags )
			{
				objectFinder = fsm.objectFinder;
			}
			else
			{
				ownsObjectFinder = true;
				objectFinder.CacheTransforms(CachePoint.StateChanged);
			}
		}


		// === EndState === //

		public void EndState(AIBehaviors fsm)
		{
			StateEnded(fsm);
		}


		private void InitTriggers()
		{
			foreach ( BaseTrigger trigger in triggers )
			{
				trigger.HandleInit(objectFinder);
			}
		}


		// === Reason === //
		// Returns true if the state remained the same

		public bool HandleReason(AIBehaviors fsm)
		{
			objectFinder.CacheTransforms(CachePoint.EveryFrame);

			if ( CheckTriggers(fsm) )
			{
				return false;
			}

			return Reason(fsm);
		}


		protected bool CheckTriggers(AIBehaviors fsm)
		{
			foreach ( BaseTrigger trigger in triggers )
			{
				if ( trigger.HandleEvaluate(fsm) )
				{
					return true;
				}
			}

			return false;
		}


		// === Action === //

		public virtual void HandleAction(AIBehaviors fsm)
		{
			CalculateDeltaTime();
			Action(fsm);
		}


		private void CalculateDeltaTime ()
		{
			deltaTime = Time.time - lastActionTime;
			lastActionTime = Time.time;
		}


		// === Animation === //

		public void PlayRandomAnimation(AIBehaviors fsm)
		{
			if ( animationStates.Length > 0 )
			{
				int randAnimState = (int)(Random.value * animationStates.Length);

				fsm.PlayAnimation(animationStates[randAnimState]);
			}
		}


		// === Other === //

		public virtual bool RotatesTowardTarget()
		{
			return false;
		}


		public override string ToString ()
		{
			return name;
		}


#if UNITY_EDITOR
		// === Editor Methods === //

		public abstract void OnStateInspectorEnabled(SerializedObject m_ParentObject);
		protected abstract void DrawStateInspectorEditor(SerializedObject m_Object, AIBehaviors stateMachine);


		public void OnInspectorEnabled(SerializedObject m_ParentObject)
		{
			SerializedObject m_Object = new SerializedObject(this);

			AIBehaviorsAnimationEditorGUI.OnInspectorEnabled(m_ParentObject, m_Object);

			OnStateInspectorEnabled(m_ParentObject);
		}


		public void DrawInspectorEditor(AIBehaviors fsm)
		{
			// Return in case this is null in the middle of the OnGUI calls
			if ( this == null )
				return;

			SerializedObject m_Object = new SerializedObject(this);
			bool oldEnabled = GUI.enabled;
			bool drawEnabled = DrawIsEnabled(m_Object);

			GUI.enabled = oldEnabled & drawEnabled;

			EditorGUILayout.Separator();
			objectFinder.DrawPlayerTagsSelection(fsm, m_Object, "objectFinder", false);

			AIBehaviorsTriggersGUI.Draw(m_Object, fsm, "Triggers:", "AIBehaviors_TriggersFoldout");
			EditorGUILayout.Separator();

			this.DrawAnimationFields(m_Object);

			DrawFoldouts(m_Object, fsm);

			DrawStateInspectorEditor(m_Object, fsm);

			m_Object.ApplyModifiedProperties();

			GUI.enabled = oldEnabled;
		}


		protected virtual void DrawFoldouts(SerializedObject m_Object, AIBehaviors fsm)
		{
			if ( HasMovementOptions() )
			{
				if ( DrawFoldout("movementFoldout", "Movement Properties:") )
				{
					GUILayout.BeginVertical(GUI.skin.box);
					DrawMovementOptions(m_Object);
					GUILayout.EndVertical();
				}
				
				EditorGUILayout.Separator();
			}
			
			if ( DrawFoldout("audioFoldout", "Audio Properties:") )
			{
				GUILayout.BeginVertical(GUI.skin.box);
				DrawAudioProperties(m_Object);
				GUILayout.EndVertical();
			}
			
			EditorGUILayout.Separator();
		}



		public bool DrawIsEnabled(SerializedObject m_Object)
		{
			if ( m_Object.targetObject != null )
			{
				SerializedProperty m_isEnabled = m_Object.FindProperty("isEnabled");
				EditorGUILayout.PropertyField(m_isEnabled);
				m_Object.ApplyModifiedProperties();

				return m_isEnabled.boolValue;
			}

			return false;
		}


		protected virtual void DrawAnimationFields(SerializedObject mObject)
		{
			AIBehaviorsAnimationEditorGUI.DrawAnimationFields(mObject, UsesMultipleAnimations());
			EditorGUILayout.Separator();
		}


		protected virtual bool UsesMultipleAnimations()
		{
			return true;
		}


		public void DrawPlayerFields(SerializedObject m_ParentObject)
		{
			SerializedProperty m_Prop = m_ParentObject.FindProperty("checkForNewPlayersInterval");
			EditorGUILayout.PropertyField(m_Prop);
		}


		protected bool DrawFoldout(string foldoutKey, string label)
		{
			bool showProperties = EditorPrefs.GetBool(foldoutKey, false);

			if ( EditorGUILayout.Foldout(showProperties, label, EditorStyles.foldoutPreDrop) != showProperties )
			{
				showProperties = !showProperties;
				EditorPrefs.SetBool(foldoutKey, showProperties);
			}

			return showProperties;
		}


		public void DrawTransitionStatePopup(AIBehaviors fsm, SerializedObject m_Object, string propertyName)
		{
			DrawTransitionStatePopup("Transition to state:", fsm, m_Object, propertyName);
		}


		public void DrawTransitionStatePopup(string label, AIBehaviors fsm, SerializedObject m_Object, string propertyName)
		{
			SerializedProperty m_InitialState = m_Object.FindProperty(propertyName);
			BaseState state = m_InitialState.objectReferenceValue as BaseState;
			BaseState updatedState;

			EditorGUILayout.Separator();

			GUILayout.Label(label, EditorStyles.boldLabel);
			updatedState = AIBehaviorsStatePopups.DrawEnabledStatePopup(fsm, state);
			if ( updatedState != state )
			{
				m_InitialState.objectReferenceValue = updatedState;
				m_Object.ApplyModifiedProperties();
			}
		}


		public void DrawAudioProperties(SerializedObject m_State)
		{
			SerializedProperty m_Property;

			GUILayout.Label("Audio Properties:", EditorStyles.boldLabel);

			m_Property = m_State.FindProperty("audioClip");
			EditorGUILayout.PropertyField(m_Property);

			m_Property = m_State.FindProperty("audioVolume");
			EditorGUILayout.PropertyField(m_Property);

			m_Property = m_State.FindProperty("audioPitch");
			EditorGUILayout.PropertyField(m_Property);

			m_Property = m_State.FindProperty("audioPitchRandomness");
			EditorGUILayout.PropertyField(m_Property, new GUIContent("Random Pitch (+/-)"));

			m_Property = m_State.FindProperty("loopAudio");
			EditorGUILayout.PropertyField(m_Property);
		}


		protected virtual bool HasMovementOptions()
		{
			return true;
		}


		public void DrawMovementOptions(SerializedObject m_State)
		{
			SerializedProperty m_property;

			GUILayout.Label("Movement Properties:", EditorStyles.boldLabel);

			// Movement Speed

			m_property = m_State.FindProperty("movementSpeed");
			EditorGUILayout.PropertyField(m_property);

			if ( m_property.floatValue < 0.0f )
				m_property.floatValue = 0.0f;

			// Rotation Speed

			m_property = m_State.FindProperty("rotationSpeed");
			EditorGUILayout.PropertyField(m_property);

			if ( m_property.floatValue < 0.0f )
				m_property.floatValue = 0.0f;
		}
#endif
	}
}