using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace AIBehavior
{
	public class TimerTrigger : BaseTrigger
	{
		public float duration = 1.0f;
		public float plusOrMinusDuration = 0.0f;
		private float timerExpiration = 0.0f;


		protected override void Init()
		{
			ResetTimer(Time.time, duration + (Random.value * plusOrMinusDuration - Random.value * plusOrMinusDuration));
		}


		protected override bool Evaluate(AIBehaviors fsm)
		{
			return DidTimeExpire(Time.time);
		}


		public void ResetTimer(float currentTime, float duration)
		{
			timerExpiration = currentTime + duration;
		}


		public bool DidTimeExpire(float currentTime)
		{
			return currentTime > timerExpiration;
		}


	#if UNITY_EDITOR
		public override void DrawInspectorProperties(AIBehaviors fsm, SerializedObject sObject)
		{
			SerializedProperty prop;

			prop = sObject.FindProperty("duration");
			EditorGUILayout.PropertyField(prop);

			prop = sObject.FindProperty("plusOrMinusDuration");
			EditorGUILayout.PropertyField(prop);
		}
	#endif
	}
}