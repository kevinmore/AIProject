using UnityEngine;
using System.Collections;
using UnityEditor;
using CS7056_AIToolKit;

[CustomEditor(typeof(PathFindingSolver))]
public class PathFindingSolverEditor : Editor
{

    PathFindingSolver solver;
    bool showAreaSettings = true, showSolverSettings = true, showGizmosSettings = true;

    // Enables editor stuff
    public void OnEnable()
    {
        solver = target as PathFindingSolver;
    }

    // Draw the customized editor
    public override void OnInspectorGUI()
    {
        int logoSize = 128;
        GUILayout.Box((Texture2D)Resources.Load("Editor/tcd"), GUILayout.Width(logoSize), GUILayout.Height(logoSize));
        GUILayout.Label("CS7056 Path Finding Solver");
        GUILayout.Label("- Huanxiang Wang 14333168");
        EditorGUILayout.LabelField("");

        SceneView.RepaintAll();

        DrawDefaultInspector();

        // Area settings
        EditorGUILayout.LabelField("");
        showAreaSettings = EditorGUILayout.Foldout(showAreaSettings, "World Settings");
        if (showAreaSettings)
        {
            solver.world.tileSize = EditorGUILayout.IntField("Tiles Size: ", solver.world.tileSize);
            solver.world.tilesInX = EditorGUILayout.IntField("Tiles in X: ", solver.world.tilesInX);
            solver.world.tilesInZ = EditorGUILayout.IntField("Tiles in Z: ", solver.world.tilesInZ);
            solver.world.height = EditorGUILayout.FloatField("Height    : ", solver.world.height);
        }

        // Solver settings
        EditorGUILayout.LabelField("");
        showSolverSettings = EditorGUILayout.Foldout(showSolverSettings, "Solver Settings");
        if (showSolverSettings)
        {
            solver.agentsToFindPathEveryFrame = EditorGUILayout.IntField("Agents To Find Path / Frame: ", solver.agentsToFindPathEveryFrame);
            solver.inclinationMax = EditorGUILayout.FloatField("Inclination Max : ", solver.inclinationMax);
            solver.diagonalConnection = EditorGUILayout.Toggle("Diagonal Connection : ", solver.diagonalConnection);
            solver.erode = EditorGUILayout.Toggle("Erode : ", solver.erode);
            solver.agentAvoidance = EditorGUILayout.Toggle("Agent Avoidance : ", solver.agentAvoidance);
            solver.checkDynamicObstacle = EditorGUILayout.Toggle("Dynamic Obstacle : ", solver.checkDynamicObstacle);
            if (solver.checkDynamicObstacle)
                solver.updateRateForDynamicObstacles = EditorGUILayout.FloatField("  Update Rate(seconds) : ", solver.updateRateForDynamicObstacles);
        }

        // Gizmos settings
        EditorGUILayout.LabelField("");
        showGizmosSettings = EditorGUILayout.Foldout(showGizmosSettings, "Gizmos Settings");
        if (showGizmosSettings)
        {
            solver.showGizmo = EditorGUILayout.Toggle("Enabled : ", solver.showGizmo);
            if (solver.showGizmo)
            {
                solver.showNodes = EditorGUILayout.Toggle("  Show Nodes : ", solver.showNodes);
                if (solver.showNodes)
                    solver.colorNode = EditorGUILayout.ColorField("    Color : ", solver.colorNode);


                solver.showLinks = EditorGUILayout.Toggle("  Links : ", solver.showLinks);
                if (solver.showLinks)
                    solver.colorLinks = EditorGUILayout.ColorField("    Color : ", solver.colorLinks);

                solver.showUnwalkableNodes = EditorGUILayout.Toggle("  Unwalkable Nodes : ", solver.showUnwalkableNodes);
                if (solver.showUnwalkableNodes)
                    solver.colorUnwalkableNode = EditorGUILayout.ColorField("    Color : ", solver.colorUnwalkableNode);
            }
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(solver);
        }
    }

}
