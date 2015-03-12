using UnityEngine;

public class ExampleGUI : MonoBehaviour
{
    private GameObject bot;
    private GameObject bot2;
    private float bot2Health;
    private float botHealth;

    private GameObject companion;
    private float companionHealth;
    private bool defender;


    private GameObject player;
    private float playerHealth;

    private void Start()
    {
        bot = GameObject.Find("DummyAI");
        if (GameObject.Find("DummyCompanionAI"))
        {
            companion = GameObject.Find("DummyCompanionAI");
        }
        else if (GameObject.Find("DummyDefenderAI"))
        {
            companion = GameObject.Find("DummyDefenderAI");
            defender = true;
        }
        player = GameObject.FindGameObjectWithTag("Player");
        if (GameObject.Find("Dummy2AI"))
        {
            bot2 = GameObject.Find("Dummy2AI");
        }
    }

    private void Update()
    {
        if (!player) return;

        playerHealth = player.GetComponent<PlayerHealth>().health;

        if (bot)
        {
            if (bot.GetComponent<AdvancedAiEnemy>())
            {
                botHealth = (bot.GetComponent<AdvancedAiEnemy>()).generalParameters.healthPoints;
            }
            else if (bot.GetComponent<AdvancedAiNpcAggressive>())
            {
                botHealth = bot.GetComponent<AdvancedAiNpcAggressive>().generalParameters.healthPoints;
            }
            else if (bot.GetComponent<AdvancedAiNpcPassive>())
            {
                botHealth = bot.GetComponent<AdvancedAiNpcPassive>().generalParameters.healthPoints;
            }
        }

        if (companion != null)
        {
            if (companion.GetComponent<AdvancedAiCompanion>())
            {
                companionHealth = companion.GetComponent<AdvancedAiCompanion>().generalParameters.healthPoints;
            }
            else if (companion.GetComponent<AdvancedAiDefender>())
            {
                companionHealth = companion.GetComponent<AdvancedAiDefender>().generalParameters.healthPoints;
            }
        }
        if (bot2 && bot2.GetComponent<AdvancedAiEnemy>())
        {
            bot2Health = bot2.GetComponent<AdvancedAiEnemy>().generalParameters.healthPoints;
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(Screen.width - 150, 10, 150, 50), "Enemy Health: " + botHealth);

        if (bot2)
        {
            GUI.Label(new Rect(Screen.width - 150, 25, 150, 50), "Enemy 2 Health: " + bot2Health);
        }

        GUI.Label(new Rect(10, 10, 100, 50), "Your Health: " + playerHealth);
        
        if (playerHealth > 0)
        {
            GUI.Label(new Rect(Screen.width/4, Screen.height/1.17f, Screen.width/2, Screen.height/10),
                      "Press left mouse button to shoot!");
        }
        else
        {
            GUI.Box(new Rect(Screen.width/4, Screen.height/1.17f, Screen.width/2, Screen.height/10),
                      "You are dead!");
        }
        if (bot)
        {
            if (bot.GetComponent<AdvancedAiNpcAggressive>())
            {
                GUI.Box(new Rect(Screen.width/12, Screen.height/1.11f, Screen.width/1.05f, Screen.height/10),
                          "This NPC acts friendly to you, but when you attack it, it will become your enemy!");
            }
            if (bot.GetComponent<AdvancedAiNpcPassive>())
            {
                GUI.Box(new Rect(Screen.width/12, Screen.height/1.11f, Screen.width/1.05f, Screen.height/10),
                          "This NPC is neutral, when it sees you and/or you attack it, it will flee away from you!");
            }
        }
        if (companion != null && !defender)
        {
            GUI.Label(new Rect(Screen.width/3, 10, 150, 50), "Companion Health: " +
                                                             companionHealth);
            if (companionHealth > 0)
            {
                GUI.Box(new Rect(Screen.width/12, Screen.height/1.11f, Screen.width/1.05f, Screen.height/10),
                          "Press F to command your companion to follow you, G to Stop, protect your companion from enemies!");
            }
            else
            {
                GUI.Box(new Rect(Screen.width/12, Screen.height/1.11f, Screen.width/1.05f, Screen.height/10),
                          "Your companion is dead!");
            }
        }
        else if (companion != null && defender)
        {
            GUI.Label(new Rect(Screen.width/3, 10, 150, 50), "Defender Health: " +
                                                             companionHealth);
            if (companionHealth > 0)
            {
                GUI.Box(new Rect(Screen.width/12, Screen.height/1.11f, Screen.width/1.05f, Screen.height/10),
                          "Press F to command your defender to follow you, G to stay in place!");
            }
            else
            {
                GUI.Box(new Rect(Screen.width/12, Screen.height/1.11f, Screen.width/1.05f, Screen.height/10),
                          "Your Defender is dead!");
            }
        }
    }
}