using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof(PathfindingEngine))]
public class PathfindingEngineEditor : Editor {


	public override void OnInspectorGUI(){
		PathfindingEngine myTarget = (PathfindingEngine)target;
	
		int size = 96;
		GUILayout.Box ((Texture2D)Resources.Load ("Editor/icon"),GUILayout.Width(size),GUILayout.Height(size));
		GUILayout.Label ("version 1.2");
		SceneView.RepaintAll();
		EditorGUILayout.LabelField("");

		DrawDefaultInspector ();

		EditorGUILayout.LabelField("");
		EditorGUILayout.LabelField("AREA");
		myTarget.area.tilesInX 				= EditorGUILayout.IntField   	("      Tiles in X: ", myTarget.area.tilesInX);
		myTarget.area.tilesInZ 				= EditorGUILayout.IntField   	("      Tiles in Z: ", myTarget.area.tilesInZ);
		myTarget.area.height   				= EditorGUILayout.FloatField 	("      Height    : ", myTarget.area.height);
		myTarget.area.tileSize 				= EditorGUILayout.IntField   	("      Tiles Size: ", myTarget.area.tileSize);

		EditorGUILayout.LabelField("");
		EditorGUILayout.LabelField("GENERAL OPTIONS");
		myTarget.agentsToFindPathEveryFrame = EditorGUILayout.IntField 		("      Agents To Find Path / Frame: ", myTarget.agentsToFindPathEveryFrame);
		myTarget.inclinationMax 			= EditorGUILayout.FloatField	("      Inclination Max : ", myTarget.inclinationMax);
		myTarget.diagonalConnection 		= EditorGUILayout.Toggle 		("      Diagonal Connection : ", myTarget.diagonalConnection);
		myTarget.erode 						= EditorGUILayout.Toggle 		("      Erode : ", myTarget.erode);
		myTarget.agentAvoidance 			= EditorGUILayout.Toggle 	    ("      Agent Avoidance : ", myTarget.agentAvoidance);
		myTarget.dynamicObstacle			= EditorGUILayout.Toggle 		("      Dynamic Obstacle : ", myTarget.dynamicObstacle);
		if (myTarget.dynamicObstacle) {
			myTarget.updateTimeForDynamicObstacles 	= EditorGUILayout.FloatField ("             Update Time : ", myTarget.updateTimeForDynamicObstacles);
		}

		EditorGUILayout.LabelField("");
		EditorGUILayout.LabelField("GIZMOS");
		myTarget.showGizmo 					= EditorGUILayout.Toggle 		("      Enabled : ", myTarget.showGizmo);
		if(myTarget.showGizmo){
			myTarget.showNodes 				= EditorGUILayout.Toggle 		("             Nodes : ", myTarget.showNodes);
			myTarget.showLinks 				= EditorGUILayout.Toggle 		("             Links : ", myTarget.showLinks);
			myTarget.showUnwalkableNodes 	= EditorGUILayout.Toggle   		("             Unwalkable Nodes : ", myTarget.showUnwalkableNodes);
			myTarget.colorNode				= EditorGUILayout.ColorField	("             Color Node : ",myTarget.colorNode);
			myTarget.colorLinks				= EditorGUILayout.ColorField	("             Color Node : ",myTarget.colorLinks);
			myTarget.colorUnwalkableNode	= EditorGUILayout.ColorField	("             Color Node : ",myTarget.colorUnwalkableNode);
		}

		if(GUI.changed)
		{
			EditorUtility.SetDirty( myTarget );
		}
	}




}