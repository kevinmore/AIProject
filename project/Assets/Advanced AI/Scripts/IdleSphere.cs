using UnityEngine;

public class IdleSphere : MonoBehaviour
{

    private AdvancedAiEnemy AiEnemy;
    private AdvancedAiNpcAggressive AiAggressive;
    private AdvancedAiNpcPassive AiPassive;
    private AdvancedAiDefender AiDefender;

	void Start ()
	{
        if (transform.parent.gameObject.GetComponent<AdvancedAiEnemy>())
        AiEnemy = transform.parent.gameObject.GetComponent<AdvancedAiEnemy>();
        else if (transform.parent.gameObject.GetComponent<AdvancedAiNpcAggressive>())
            AiAggressive = transform.parent.gameObject.GetComponent<AdvancedAiNpcAggressive>();
        else if (transform.parent.gameObject.GetComponent<AdvancedAiNpcPassive>())
            AiPassive = transform.parent.gameObject.GetComponent<AdvancedAiNpcPassive>();
        else if (transform.parent.gameObject.GetComponent<AdvancedAiDefender>())
            AiDefender = transform.parent.gameObject.GetComponent<AdvancedAiDefender>();
	}
	


    void OnTriggerStay(Collider other)
    {
        if (AiEnemy)
        {
            AiEnemy.other = other;
            AiEnemy.TriggerStay();
        }
        else if (AiAggressive)
        {
            AiAggressive.other = other;
            AiAggressive.TriggerStay();
        }
        else if (AiPassive)
        {
            AiPassive.other = other;
            AiPassive.TriggerStay();
        }
        else if (AiDefender)
        {
            AiDefender.other = other;
            AiDefender.TriggerStay();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (AiEnemy)
        {
            AiEnemy.other = other;
            AiEnemy.TriggerExit();
        }
        else if (AiAggressive)
        {
            AiAggressive.other = other;
            AiAggressive.TriggerExit();
        }
        else if (AiPassive)
        {
            AiPassive.other = other;
            AiPassive.TriggerExit();
        }    
       
    }
}
