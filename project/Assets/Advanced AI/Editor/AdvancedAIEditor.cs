using UnityEditor;
using UnityEngine;

public class AdvancedAIEditor
{
    [MenuItem("GameObject/Advanced AI Pro/Add Advanced AI Enemy")]
    private static void AddAdvancedAIEnemy()
    {
        if (Selection.activeTransform)
        {
            Transform currentT = Selection.activeTransform.transform;
            var go = new GameObject(currentT.name + " AI");
            currentT.name = "_" + currentT.name;
            var pp = new GameObject("_ProjectileOrigin");
            go.transform.position = currentT.position;
            go.transform.rotation = currentT.rotation;
            currentT.parent = go.transform;
            pp.transform.parent = go.transform;
            pp.transform.localPosition = Vector3.zero;
            pp.transform.rotation = currentT.rotation;
            go.AddComponent("AdvancedAiEnemy");
            go.AddComponent("CapsuleCollider");
        }
        else
        {
            Debug.LogWarning(
                "You need to select your bot game object first from the scene hierarchy before adding an AI!");
        }
    }

    [MenuItem("GameObject/Advanced AI Pro/Add Advanced AI NPC Aggressive")]
    private static void AddAdvancedAINPCAggressive()
    {
        if (Selection.activeTransform)
        {
            Transform currentT = Selection.activeTransform.transform;
            var go = new GameObject(currentT.name + " AI");
            currentT.name = "_" + currentT.name;
            var pp = new GameObject("_ProjectileOrigin");
            go.transform.position = currentT.position;
            go.transform.rotation = currentT.rotation;
            currentT.parent = go.transform;
            pp.transform.parent = go.transform;
            pp.transform.localPosition = Vector3.zero;
            pp.transform.rotation = currentT.rotation;
            go.AddComponent("AdvancedAiNpcAggressive");
            go.AddComponent("CapsuleCollider");
        }
        else
        {
            Debug.LogWarning(
                "You need to select your bot game object first from the scene hierarchy before adding an AI!");
        }
    }

    [MenuItem("GameObject/Advanced AI Pro/Add Advanced AI NPC Passive")]
    private static void AddAdvancedAINPCPassive()
    {
        if (Selection.activeTransform)
        {
            Transform currentT = Selection.activeTransform.transform;
            var go = new GameObject(currentT.name + " AI");
            currentT.name = "_" + currentT.name;
            go.transform.position = currentT.position;
            go.transform.rotation = currentT.rotation;
            currentT.parent = go.transform;

            go.AddComponent("AdvancedAiNpcPassive");
            go.AddComponent("CapsuleCollider");
        }
        else
        {
            Debug.LogWarning(
                "You need to select your bot game object first from the scene hierarchy before adding an AI!");
        }
    }

    [MenuItem("GameObject/Advanced AI Pro/Add Advanced AI Companion")]
    private static void AddAdvancedAICompanion()
    {
        if (Selection.activeTransform)
        {
            Transform currentT = Selection.activeTransform.transform;
            var go = new GameObject(currentT.name + " AI");
            currentT.name = "_" + currentT.name;
            go.transform.position = currentT.position;
            go.transform.rotation = currentT.rotation;
            currentT.parent = go.transform;
            go.AddComponent("AdvancedAiCompanion");
            go.AddComponent("CapsuleCollider");
        }
        else
        {
            Debug.LogWarning(
                "You need to select your bot game object first from the scene hierarchy before adding an AI!");
        }
    }

    [MenuItem("GameObject/Advanced AI Pro/Add Advanced AI Defender Ally")]
    private static void AddAdvancedAIDefender()
    {
        if (Selection.activeTransform)
        {
            Transform currentT = Selection.activeTransform.transform;
            var go = new GameObject(currentT.name + " AI");
            currentT.name = "_" + currentT.name;
            var pp = new GameObject("_ProjectileOrigin");
            go.transform.position = currentT.position;
            go.transform.rotation = currentT.rotation;
            currentT.parent = go.transform;
            pp.transform.parent = go.transform;
            pp.transform.localPosition = Vector3.zero;
            pp.transform.rotation = currentT.rotation;
            go.AddComponent("AdvancedAiDefender");
            go.AddComponent("CapsuleCollider");
        }
        else
        {
            Debug.LogWarning(
                "You need to select your bot game object first from the scene hierarchy before adding an AI!");
        }
    }


    [MenuItem("GameObject/Advanced AI Pro/Other/Add Player Health component")]
    private static void AddAdvancedAIHealth()
    {
        if (Selection.activeTransform)
        {
            Transform currentT = Selection.activeTransform.transform;

            currentT.gameObject.AddComponent("PlayerHealth");
        }
        else
        {
            Debug.LogWarning(
                "You need to select your Player game object first from the scene hierarchy!");
        }
    }

    [MenuItem("GameObject/Advanced AI Pro/Other/Add Projectile component")]
    private static void AddAdvancedAIProjectile()
    {
        if (Selection.activeTransform)
        {
            Transform currentT = Selection.activeTransform.transform;

            currentT.gameObject.AddComponent("Projectile");
        }
        else
        {
            Debug.LogWarning(
                "You need to select your projectile game object first from the scene hierarchy!");
        }
    }

    [MenuItem("GameObject/Advanced AI Pro/About")]
    private static void About()
    {
        EditorUtility.DisplayDialog("Advanced AI Pro 6.01",
                                    "For questions and support please contact me at z.abbas83@yahoo.com",
                                    "OK");
    }
}