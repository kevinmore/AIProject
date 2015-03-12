using UnityEngine;

public class DestroyRagdoll : MonoBehaviour
{


    public float lifeTime;


    void Start()
    {
        if (!animation)
        {
                Destroy(gameObject, lifeTime);
        }

    }

    void Update()
    {
        if (animation)
        {
            if (!animation.isPlaying)
                Destroy(gameObject, lifeTime);

        }
    }

}