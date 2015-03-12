using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float ImpactFxDestructionTime = 2.0f;
    public float damage;
    public string damageMethodName;
    private bool decalSwitch;
    public float decalsDestructionTime = 10.0f;
    private bool destroySwitch;
    public float destructionTime = 2.5f;

    private GameObject impact;
    private bool impactFxSwitch;
    private bool playSwitch;
    [HideInInspector] public GameObject playerTarget;
    public Texture2D projectileDecal;
    public GameObject projectileImpactFx;
    public AudioClip projectileImpactSfx;

    private void Start()
    {
        if (projectileImpactSfx)
        {
            gameObject.AddComponent("AudioSource");
            gameObject.GetComponent<AudioSource>().playOnAwake = false;
            GetComponent<AudioSource>().loop = false;
        }
        Destroy(gameObject, destructionTime);
    }


    private void Update()
    {
        if (!GameObject.Find("Decals"))
        {
            new GameObject("Decals");
        }
        if (GameObject.Find("Plane(Clone)"))
        {
            GameObject decal = GameObject.Find("Plane(Clone)");
            decal.name = "decal";
            decal.transform.parent = GameObject.Find("Decals").transform;
        }


        if (projectileImpactFx && impact)
        {
            if (impact.GetComponent<ParticleSystem>())
            {
                impact.GetComponent<ParticleSystem>().loop = false;
                var fxp = impact.GetComponent<ParticleSystem>();
                ParticleSystem[] fxc = fxp.GetComponentsInChildren<ParticleSystem>();
                foreach (ParticleSystem fxcc in fxc)
                {
                    fxcc.loop = false;
                }

                if (!impact.particleSystem.IsAlive(true))
                {
                    DestroyImmediate(impact);
                }
            }
            else
            {
                impact.GetComponent<ParticleAnimator>().autodestruct = true;

                Destroy(impact, ImpactFxDestructionTime);
            }

            destroySwitch = true;
        }

        if (destroySwitch)
        {
            if (projectileImpactSfx)
            {
                if (!GetComponent<AudioSource>().isPlaying)
                {
                    Destroy(gameObject);
                }
                else
                {
                    collider.enabled = false;
                    renderer.enabled = false;
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collisioninfo)
    {
        if (collisioninfo.gameObject == playerTarget)
        {
            collisioninfo.gameObject.SendMessage(damageMethodName, damage, SendMessageOptions.DontRequireReceiver);
            Destroy(gameObject);
        }
        else if (collisioninfo.gameObject.GetComponent<AdvancedAiEnemy>() ||
                 collisioninfo.gameObject.GetComponent<AdvancedAiCompanion>() ||
                 collisioninfo.gameObject.GetComponent<AdvancedAiDefender>() ||
                 collisioninfo.gameObject.GetComponent<AdvancedAiNpcAggressive>() ||
                 collisioninfo.gameObject.GetComponent<AdvancedAiNpcPassive>())
        {
            collisioninfo.gameObject.SendMessage("GotHit", damage, SendMessageOptions.DontRequireReceiver);

            Destroy(gameObject);
        }
        else if (!collisioninfo.collider.isTrigger && !collisioninfo.gameObject.GetComponent<Projectile>())
        {
            if (projectileImpactSfx && !playSwitch && audio.enabled)
            {
                playSwitch = true;
                GetComponent<AudioSource>().clip = projectileImpactSfx;
                GetComponent<AudioSource>().volume = 0.01f;
                GetComponent<AudioSource>().Play();
            }

            if (projectileDecal && !decalSwitch)
            {
                decalSwitch = true;
                Quaternion hitRotation = Quaternion.FromToRotation(Vector3.up, collisioninfo.contacts[0].normal);


                var decalPlane =
                    Instantiate(GameObject.CreatePrimitive(PrimitiveType.Plane),
                                collisioninfo.contacts[0].thisCollider.ClosestPointOnBounds(transform.position),
                                hitRotation) as GameObject;


                decalPlane.transform.localScale = new Vector3(0.01f, 1, 0.01f);
                decalPlane.collider.enabled = false;
                decalPlane.renderer.material.shader = Shader.Find("Transparent/Diffuse");

                decalPlane.renderer.material.mainTexture = projectileDecal;
                decalPlane.renderer.material.color = new Color(1, 1, 1, 1);
                decalPlane.AddComponent("DecalsAutoDestruct");
                decalPlane.GetComponent<DecalsAutoDestruct>().destructionTime = decalsDestructionTime;
                Destroy(GameObject.Find("Plane"));
            }


            if (projectileImpactFx && !impactFxSwitch)
            {
                impactFxSwitch = true;
                impact = Instantiate(projectileImpactFx,
                                     collisioninfo.contacts[0].thisCollider.ClosestPointOnBounds(transform.position),
                                     Quaternion.identity) as GameObject;
            }
            else
            {
                destroySwitch = true;
            }
        }
    }
}