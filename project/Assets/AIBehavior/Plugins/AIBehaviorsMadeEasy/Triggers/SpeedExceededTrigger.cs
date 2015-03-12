using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace AIBehavior
{
	public class SpeedExceededTrigger : BaseTrigger
	{
		public float speedThreshold = 1.0f;
		public string speedObjectsTag = "Untagged";
		public string seekObjectTag = "Untagged";
		public float maxDistanceFromAI = 10.0f;
		public float keepSeekObjectAliveDuration = 10.0f;

		Dictionary<GameObject, Vector3> gameObjectPreviousPositions = new Dictionary<GameObject, Vector3>();
		float previousCheckTime = 0.0f;


		float timeTotal = 0.0f;


		protected override void Init()
		{
			gameObjectPreviousPositions = new Dictionary<GameObject, Vector3>();
			previousCheckTime = Time.time;
		}


		protected override bool Evaluate(AIBehaviors fsm)
		{
			float time = Time.time;
			Transform target;

			if ( SpeedExceededForTarget(out target, time) )
			{
				StartCoroutine(HandleSeekObject(target));
				return true;
			}

			timeTotal += time - previousCheckTime;
			previousCheckTime = time;

			return false;
		}


		public bool SpeedExceededForTarget(out Transform target, float time)
		{
			GameObject[] gos = GameObject.FindGameObjectsWithTag(speedObjectsTag);
			float sqrSpeedThreshold = speedThreshold * speedThreshold;
			float sqrMaxDistanceFromAI = maxDistanceFromAI * maxDistanceFromAI;
			float timeDiff = time - previousCheckTime;
			float sqrTimeDiff = timeDiff * timeDiff;

			foreach ( GameObject go in gos )
			{
				Vector3 curPosition = go.transform.position;

				if ( gameObjectPreviousPositions.ContainsKey(go) )
				{

					// Is the object within the max distanceFromAI?
					if ( (curPosition - transform.position).sqrMagnitude < sqrMaxDistanceFromAI )
					{
						Vector3 diffVector = curPosition - gameObjectPreviousPositions[go];

						// Is the objects speed greater than the minimum speed
						if ( diffVector.sqrMagnitude / sqrTimeDiff > sqrSpeedThreshold )
						{
							target = go.transform;
							return true;
						}
					}
				}

				gameObjectPreviousPositions[go] = curPosition;
			}

			target = null;
			return false;
		}


		IEnumerator HandleSeekObject(Transform speedItemTransform)
		{
			float destroyTime = Time.time + keepSeekObjectAliveDuration;
			GameObject speedSeekObject = new GameObject("Speed Seek Object");
			Transform speedSeekTransform = speedSeekObject.transform;

			speedSeekObject.tag = seekObjectTag;

			Destroy(speedSeekObject, keepSeekObjectAliveDuration);

			while ( destroyTime > Time.time )
			{
				if ( speedItemTransform != null )
					speedSeekTransform.position = speedItemTransform.position;

				yield return null;
			}
		}


	#if UNITY_EDITOR
		public override void DrawInspectorProperties(AIBehaviors fsm, SerializedObject sObject)
		{
			SerializedProperty prop;

			prop = sObject.FindProperty("speedThreshold");
			EditorGUILayout.PropertyField(prop);

			GUILayout.BeginHorizontal();
			{
				GUILayout.Label("Speed Objects Tag:");
				prop = sObject.FindProperty("speedObjectsTag");
				string newTag = EditorGUILayout.TagField(prop.stringValue);

				if ( newTag != prop.stringValue )
				{
					prop.stringValue = newTag;
				}
			}
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			{
				GUILayout.Label("Seek Objects Tag:");
				prop = sObject.FindProperty("seekObjectTag");
				string newTag = EditorGUILayout.TagField(prop.stringValue);

				if ( newTag != prop.stringValue )
				{
					prop.stringValue = newTag;
				}
			}
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			{
				prop = sObject.FindProperty("maxDistanceFromAI");
				EditorGUILayout.PropertyField(prop);
			}
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			{
				prop = sObject.FindProperty("keepSeekObjectAliveDuration");
				EditorGUILayout.PropertyField(prop);
			}
			GUILayout.EndHorizontal();
		}
	#endif
	}
}