#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using AIBehavior;
using System;
using System.IO;
using System.Reflection;


namespace AIBehaviorEditor
{
	public class AIBehaviorsMenuItems : Editor
	{
		[MenuItem("AI Behavior/Mecanim Setup", true, 0)]
		public static bool MecanimSetupValidator()
		{
			return GetGameObject() != null && GetGameObject().GetComponent<Animator>() != null;
		}


		[MenuItem("AI Behavior/Mecanim Setup", false, 0)]
		public static void MecanimSetup()
		{
			GameObject go = GetGameObject();

			go.AddComponent<AIBehaviors>();
			go.AddComponent<MecanimAnimation>();
			go.AddComponent<MecanimNavMeshPathScript>();
		}


		[MenuItem("AI Behavior/Legacy Setup", true, 0)]
		public static bool LegacySetupValidator()
		{
			return GetGameObject() != null;
		}
		

		[MenuItem("AI Behavior/Legacy Setup", false, 0)]
		public static void LegacySetup()
		{
			GameObject go = GetGameObject();

			go.AddComponent<AIBehaviors>();
			go.AddComponent<CharacterAnimator>();
			go.AddComponent<NavMeshAgent>();
		}


		[MenuItem("AI Behavior/AI Behavior Component Only", true, 0)]
		public static bool AddAIBehaviorsComponentValidator()
		{
			return GetGameObject() != null;
		}


		[MenuItem("AI Behavior/AI Behavior Component Only", false, 0)]
		public static void AddAIBehaviorsComponent()
		{
			GetGameObject().AddComponent(typeof(AIBehaviors));
		}


		private static GameObject GetGameObject()
		{
			return Selection.activeObject as GameObject;
		}
		
		
		[MenuItem("AI Behavior/Other/Astar Pathfinding Project Setup", true, 11)]
		public static bool EnableAstarValidator()
		{
			GameObject gameObject = GetGameObject();

			if ( gameObject != null && gameObject.GetComponent<AstarCharacterController>() == null )
			{
				bool hasAstarPathfinding = false;
				bool hasAIPathScript = false;

				foreach ( Assembly assembly in AppDomain.CurrentDomain.GetAssemblies() )
				{
					foreach ( Type type in assembly.GetTypes() )
					{
						if ( type.Namespace == "Pathfinding" )
						{
							hasAstarPathfinding = true;
						}

						if ( type.Name == "AIPath" )
						{
							hasAIPathScript = true;
						}
					}
				}

				return hasAstarPathfinding && hasAIPathScript;
			}

			return false;
		}


		[MenuItem("AI Behavior/Other/Astar Pathfinding Project Setup", false, 11)]
		public static void EnableAstar()
		{
			GameObject gameObject = GetGameObject();
			string[] components = new string[] { "AstarCharacterController", "AIBehaviors", "CharacterAnimator", "AIPath" };

			ReplaceStringInAstarCode("//#define USE_ASTAR", "#define USE_ASTAR");
			
			AssetDatabase.Refresh();

			for ( int i = 0; i < components.Length; i++ )
			{
				if ( gameObject.GetComponent(components[i]) == null )
				{
					gameObject.AddComponent(components[i]);
				}
			}
		}
		
		
		static void ReplaceStringInAstarCode (string oldString, string newString)
		{
			const string astarScriptPath = "/AIBehavior/ExampleScripts/AstarCharacterController.cs";
			string astarCode = File.ReadAllText(Application.dataPath + astarScriptPath);
			
			astarCode = astarCode.Replace(oldString, newString);

			File.WriteAllText(Application.dataPath + astarScriptPath, astarCode);
		}


		[MenuItem("AI Behavior/Documentation", false, 22)]
		public static void Documentation()
		{
			Application.OpenURL("http://walkerboystudio.com/html/aibehavior/index.html");
		}
		

		[MenuItem("AI Behavior/About AI Behavior", false, 22)]
		public static void About()
		{
			AIBehaviorsAboutWindow.ShowAboutWindow();
		}


		[MenuItem("AI Behavior/Contact Us or Report a Bug (via your email client)", false, 22)]
		public static void ReportBug()
		{
			Application.OpenURL("mailto:nathanwardenlee@icloud.com?cc=webmaster@walkerboystudio.com");
		}
	}
}
#endif