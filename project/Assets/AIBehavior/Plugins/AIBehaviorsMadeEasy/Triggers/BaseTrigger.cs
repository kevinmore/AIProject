using UnityEngine;
using AIBehaviorEditor;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
using System.Reflection;
#endif


namespace AIBehavior
{
	public abstract class BaseTrigger : SavableComponent
	{
		public BaseState transitionState;
		public BaseTrigger parentTrigger = null;
		public BaseTrigger[] subTriggers = new BaseTrigger[0];
		public bool ownsObjectFinder = false;
		public TaggedObjectFinder objectFinder;

		// === Trigger Methods === //

		public BaseTrigger()
		{
			objectFinder = CreateObjectFinder();
		}
		
		
		protected virtual TaggedObjectFinder CreateObjectFinder()
		{
			return new TaggedObjectFinder();
		}


		protected abstract void Init();
		protected abstract bool Evaluate(AIBehaviors fsm);


		protected virtual void Awake()
		{
			objectFinder.CacheTransforms(CachePoint.Awake);

			for ( int i = 0; i < subTriggers.Length; i++ )
			{
				subTriggers[i].parentTrigger = this;
			}
		}


		public void HandleInit(TaggedObjectFinder parentObjectFinder)
		{
			if ( !objectFinder.useCustomTags )
			{
				objectFinder = parentObjectFinder;
			}
			else
			{
				ownsObjectFinder = true;
			}

			objectFinder.CacheTransforms(CachePoint.StateChanged);

			Init();

			foreach ( BaseTrigger subTrigger in subTriggers )
			{
				subTrigger.HandleInit(objectFinder);
			}
		}


		public bool HandleEvaluate(AIBehaviors fsm)
		{
			bool result = this.enabled && Evaluate(fsm) && CheckSubTriggers(fsm);

			objectFinder.CacheTransforms(CachePoint.EveryFrame);

			if ( result )
			{
				ChangeToTransitionState(fsm);
			}

			return result;
		}


		protected virtual void ChangeToTransitionState(AIBehaviors fsm)
		{
			fsm.ChangeActiveState(transitionState);
		}


		private bool CheckSubTriggers (AIBehaviors fsm)
		{
			if ( subTriggers.Length == 0 )
			{
				return true;
			}

			foreach ( BaseTrigger trigger in subTriggers )
			{
				if ( trigger.HandleEvaluate(fsm) )
				{
					return true;
				}
			}

			return false;
		}


		public void AddSubTrigger(BaseTrigger subTrigger)
		{
			BaseTrigger[] newSubTriggers = new BaseTrigger[subTriggers.Length+1];

			for ( int i = 0; i < subTriggers.Length; i++ )
			{
				newSubTriggers[i] = subTriggers[i];
			}

			subTrigger.parentTrigger = this;
			newSubTriggers[subTriggers.Length] = subTrigger;

			subTriggers = newSubTriggers;
		}


#if UNITY_EDITOR
		public void DrawInspectorGUI(AIBehaviors fsm)
		{
			SerializedObject m_Object = new SerializedObject(this);

			DrawInspectorProperties(fsm, m_Object);
			
			EditorGUILayout.Separator();
			objectFinder.DrawPlayerTagsSelection(fsm, m_Object, "objectFinder", false);

			m_Object.ApplyModifiedProperties();
		}


		public void DrawTransitionState(AIBehaviors fsm)
		{
			SerializedObject sObject = new SerializedObject(this);
			SerializedProperty prop;

			GUILayout.BeginHorizontal();
			GUILayout.Label("Change to State:");
			prop = sObject.FindProperty("transitionState");
			prop.objectReferenceValue = AIBehaviorsStatePopups.DrawEnabledStatePopup(fsm, prop.objectReferenceValue as BaseState);
			GUILayout.EndHorizontal();

			sObject.ApplyModifiedProperties();
		}
		
		
		public virtual void DrawInspectorProperties(AIBehaviors fsm, SerializedObject sObject)
		{
			InspectorHelper.DrawInspector(sObject);
		}
		

		public virtual void DrawGizmos(Vector3 position, Quaternion rotation)
		{
		}
#endif
	}
}