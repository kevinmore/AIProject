using UnityEngine;

public class DecalsAutoDestruct : MonoBehaviour
{
    public float destructionTime = 10.0f;

    private void Start()
    {
        Destroy(gameObject, destructionTime);
    }

    private void Update()
    {
    }
}