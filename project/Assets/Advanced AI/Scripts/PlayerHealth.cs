using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float health = 100.0f;


    private void Start()
    {
    }


    private void Update()
    {
        if (health <= 0)
        {
            // Manage your player's death logic here
            // A small example here, you note that I changed the layer of the target so the AI will ingnore it when it is dead.

            health = 0;
            gameObject.layer = 9;
            GetComponent<CharacterMotor>().canControl = false;
        }
    }

    private void SubtractHealth(float healthAmount)
    {
        health -= healthAmount;
    }
}