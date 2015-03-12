using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class animingroupiii
{
    public AnimationClip[] deathAnim;
    public AnimationClip[] fleeAnim;
    public AnimationClip[] gotHitAnim;
    public AnimationClip[] idleAnim;
    public AnimationClip[] wanderingAnim;
}

[Serializable]
public class animparami
{
    public float deathAnimSpeed = 1f;
    public float deathBlendingTime = 0.3f;
    public float fleeAnimSpeed = 1f;
    public float fleeBlendingTime = 0.3f;
    public float gotHitAnimSpeed = 1f;
    public float gotHitBlendTime = 0.3f;
    public float idleAnimSpeed = 1f;
    public float idleBlendingTime = 0.3f;
    public float wanderBlendingTime = 0.3f;
    public float wanderingAnimSpeed = 1f;
}

[Serializable]
public class soundingroup
{
    public AudioClip dieSfx;
    public AudioClip fleeSfx;
    public AudioClip gotHitSfx;
    public AudioClip idleSfx;
    public AudioClip wanderingSfx;
}

[Serializable]
public class soundvolo
{
    public float _maxDistance = 10f;
    [Range(0, 1)] public float dieVolume = 1f;
    [Range(0, 1)] public float fleeVolume = 1f;
    [Range(0, 1)] public float gotHitVolume = 1f;
    [Range(0, 1)] public float idleVolume = 1f;
    [Range(0, 1)] public float wanderingVolume = 1f;
}

[Serializable]
public class navigati
{
    public float acceleration = 8f;
    public float arrivalDistance = 0.5f;
    public float fleeDistance = 15f;
    public float fleeSpeed = 5.0f;
    public float goIdleRadius = 20.0f;
    [Range(0, 1)] public float hearVolumeMin = 0.2f;
    public float hearingDistance = 12.0f;
    public float lookAtSpeed = 6.0f;
    public float rotationSpeed = 140.0f;
    public float viewSphereRadius = 5f;
    public float minIdleInterval = 2.0f;
    public float maxIdleInterval = 7.0f;
    public float wanderingRadius = 10.0f;
    public float wanderingSpeed = 2.5f;
}

[Serializable]
public class generaling

{
    public GameObject bloodDecalDead;
    public bool canHear = false;
    public GameObject deathEffect;
    public float disappearDelay = 2.0f;
    public bool disappearOnDeath = false;
    public Vector3 dropItemOffset = new Vector3(0.5f, 0.5f, 0.5f);
    public GameObject dropItemOnDeath;
    public float healthPoints = 100.0f;
    public GameObject hitParticleFx;
    public bool ragdollPlayDieAnim;
    public GameObject ragdollPrefab;
    public bool ragdollifyOnDeath;
    public Transform target;
}


public class AdvancedAiNpcPassive : MonoBehaviour
{
    public enum fleem
    {
        On_Sight,
        On_Hit,
        Both
    }

    public enum patmode
    {
        In_Place,
        Dynamic_Wandering,
    }

    public animparami animationParameters;
    public animingroupiii animations;
    private AudioSource[] audioC;
    private AudioSource audioS;
    private Transform bot;
    private bool canMesure;
    private int chosenDeath;
    private int chosenHit;
    private float dist;
    public fleem fleeMode;
    private bool fleeSwitch;
    public generaling generalParameters;
    private GameObject hitfx;
    private bool isDead;
    private bool isHit;
    private bool isIdle;
    private NavMeshAgent nav;
    public navigati navigationParameters;
    [HideInInspector] public Collider other;
    private NavMeshPath path;
    public patmode patrolMode;
    private Vector3 rPosition;
    public soundingroup sounds;
    public soundvolo soundsVolume;
    private bool startSwitch = true;
    private GameObject triggerChil;
    private bool wanderingSwitch = true;


    private void Start()
    {
        bot = transform.GetChild(0);
        if (bot.gameObject.name == "TriggerSphere") bot = transform.GetChild(1);

        if (!generalParameters.target)
        {
            if (GameObject.FindGameObjectWithTag("Player"))
                generalParameters.target = GameObject.FindGameObjectWithTag("Player").transform;
            else
                Debug.LogWarning("You forgot to assign a target transform to your AI: " + gameObject.name);
        }

        if (!transform.FindChild("TriggerSphere"))
        {
            triggerChil = new GameObject();
            triggerChil.transform.parent = transform;
            triggerChil.transform.localPosition = Vector3.zero;
            triggerChil.AddComponent("SphereCollider");
            triggerChil.layer = LayerMask.NameToLayer("Ignore Raycast");
            triggerChil.AddComponent<IdleSphere>();
            triggerChil.name = "TriggerSphere";
            triggerChil.GetComponent<SphereCollider>().radius = navigationParameters.goIdleRadius;
            triggerChil.GetComponent<SphereCollider>().isTrigger = true;
        }
        else
        {
            triggerChil = transform.FindChild("TriggerSphere").gameObject;
        }

        if (!GetComponent<NavMeshAgent>()) gameObject.AddComponent("NavMeshAgent");

        if (!audio)
            gameObject.AddComponent("AudioSource");

        audio.maxDistance = soundsVolume._maxDistance;
        audio.rolloffMode = AudioRolloffMode.Linear;

        if (animations.idleAnim.Length != 0)
        {
            foreach (AnimationClip anim in animations.idleAnim)
            {
                anim.wrapMode = WrapMode.Loop;
                bot.animation[anim.name].speed = animationParameters.idleAnimSpeed;
            }
        }

        if (animations.gotHitAnim.Length != 0)
        {
            foreach (AnimationClip hAnim in animations.gotHitAnim)
            {
                hAnim.wrapMode = WrapMode.Once;
                bot.animation[hAnim.name].speed = animationParameters.gotHitAnimSpeed;
            }
        }


        if (animations.deathAnim.Length != 0)
        {
            foreach (AnimationClip dAnim in animations.deathAnim)
            {
                dAnim.wrapMode = WrapMode.Once;
                bot.animation[dAnim.name].speed = animationParameters.deathAnimSpeed;
            }
        }


        if (animations.wanderingAnim.Length != 0)
        {
            foreach (AnimationClip wAnim in animations.wanderingAnim)
            {
                wAnim.wrapMode = WrapMode.Loop;
                bot.animation[wAnim.name].speed = animationParameters.wanderingAnimSpeed;
            }
        }
        if (animations.fleeAnim.Length != 0)
        {
            foreach (AnimationClip fAnim in animations.fleeAnim)
            {
                fAnim.wrapMode = WrapMode.Loop;
                bot.animation[fAnim.name].speed = animationParameters.fleeAnimSpeed;
            }
        }

        nav = gameObject.GetComponent<NavMeshAgent>();
        nav.speed = navigationParameters.wanderingSpeed;
        nav.angularSpeed = navigationParameters.rotationSpeed;
        nav.stoppingDistance = navigationParameters.arrivalDistance;
        nav.acceleration = navigationParameters.acceleration;

        if (generalParameters.canHear)
        {
            if (generalParameters.target.audio)
                audioS = generalParameters.target.audio;

            if (generalParameters.target.GetComponentInChildren<AudioSource>())
                audioC = generalParameters.target.GetComponentsInChildren<AudioSource>();
        }
        StartInIdle();
    }

    private void Update()
    {
        if (!generalParameters.target) return;

        dist = Vector3.Distance(transform.position, generalParameters.target.transform.position);

        if (!fleeSwitch && (fleeMode == fleem.Both || fleeMode == fleem.On_Sight))
        {
            Collider[] colinfo =
                Physics.OverlapSphere(transform.TransformPoint(0, 0, navigationParameters.viewSphereRadius),
                    navigationParameters.viewSphereRadius);

            foreach (Collider colhit in colinfo)
            {
                if (colhit.transform == generalParameters.target)
                {
                    if (fleeMode == fleem.Both || fleeMode == fleem.On_Sight)
                    {
                        fleeSwitch = true;
                        nav.speed = navigationParameters.fleeSpeed;
                        GoFlee();
                        break;
                    }
                }
            }
        }

        if (isDead && generalParameters.disappearOnDeath && animations.deathAnim.Length != 0 &&
            !bot.animation.IsPlaying(animations.deathAnim[chosenDeath].name) &&
            !audio.isPlaying)
        {
            Destroy(gameObject, generalParameters.disappearDelay);
        }

        else if (isDead && generalParameters.disappearOnDeath && animations.deathAnim.Length == 0 &&
            !audio.isPlaying)
        {
            Destroy(gameObject, generalParameters.disappearDelay);
        }



        if (generalParameters.healthPoints > 0)
        {
            if (animations.gotHitAnim.Length != 0)
            {
                if (isHit && !bot.animation.IsPlaying(animations.gotHitAnim[chosenHit].name))
                {
                    isHit = false;
                    isIdle = false;
                    if (fleeMode == fleem.Both || fleeMode == fleem.On_Hit)
                    {
                        fleeSwitch = true;
                        nav.speed = navigationParameters.fleeSpeed;
                        GoFlee();
                    }
                }
            }
            else
            {
                if (isHit)
                {
                    isHit = false;
                    isIdle = false;

                    if (fleeMode == fleem.Both || fleeMode == fleem.On_Hit)
                    {
                        fleeSwitch = true;
                        nav.speed = navigationParameters.fleeSpeed;
                        GoFlee();
                    }
                }
            }
        }

        if (nav && (nav.pathStatus == NavMeshPathStatus.PathInvalid || nav.pathStatus == NavMeshPathStatus.PathPartial))
        {
            wanderingSwitch = true;
            fleeSwitch = false;
        }


        if (generalParameters.healthPoints > 0)
        {
            if (!wanderingSwitch && !fleeSwitch)
            {
                if (nav.remainingDistance <= navigationParameters.arrivalDistance && !isIdle)
                    StartCoroutine("IdleInWandering");
            }
        }
        if (canMesure && !nav.pathPending)
        {
            if (nav.remainingDistance <= navigationParameters.arrivalDistance)
            {
                canMesure = false;
                fleeSwitch = false;
                wanderingSwitch = true;
            }
        }


        if (generalParameters.healthPoints < 0)
            GoDeath();


        if (generalParameters.hitParticleFx && hitfx)
        {
            if (hitfx.GetComponent<ParticleSystem>())
            {
                hitfx.GetComponent<ParticleSystem>().loop = false;
                var fxps = hitfx.GetComponent<ParticleSystem>();
                ParticleSystem[] fxcs = fxps.GetComponentsInChildren<ParticleSystem>();
                foreach (ParticleSystem fxccs in fxcs)
                {
                    fxccs.loop = false;
                }

                if (!hitfx.particleSystem.IsAlive(true))
                    DestroyImmediate(hitfx);
            }
            else Destroy(hitfx.gameObject, 1f);
        }
    }

    public void TriggerExit()
    {
        if (generalParameters.healthPoints > 0)
        {
            if (other.transform == generalParameters.target)
            {
                GoIdleFromExit();
                nav.Stop(true);
                wanderingSwitch = true;
            }
        }
    }

    public void TriggerStay()
    {
        if (generalParameters.healthPoints > 0 && other.transform == generalParameters.target && !fleeSwitch)
        {
            if (startSwitch)
            {
                startSwitch = false;
                audio.Stop();
            }

            switch (patrolMode)
            {
                case patmode.Dynamic_Wandering:
                    if (wanderingSwitch)
                    {
                        if (animations.gotHitAnim.Length != 0)
                        {
                            if (!bot.animation.IsPlaying(animations.gotHitAnim[chosenHit].name))
                            {
                                nav.speed = navigationParameters.wanderingSpeed;
                                Wandering();
                            }
                        }
                        else
                        {
                            nav.speed = navigationParameters.wanderingSpeed;
                            Wandering();
                        }
                    }
                    break;
                case patmode.In_Place:
                    GoIdle();
                    break;
            }


            if (generalParameters.canHear)
            {
                Hearing();
            }
        }
    }

    private void Wandering()
    {
        wanderingSwitch = false;
        isIdle = false;
        nav.speed = navigationParameters.wanderingSpeed;
        rPosition = new Vector3(
            transform.position.x + (Random.Range(-1.0f, 1.0f)*navigationParameters.wanderingRadius),
            transform.position.y,
            transform.position.z + (Random.Range(-1.0f, 1.0f)*navigationParameters.wanderingRadius));


        path = new NavMeshPath();
        bool canit;
        canit = NavMesh.CalculatePath(transform.position, rPosition, 1, path);
        if (canit)
        {
            nav.SetDestination(rPosition);
            nav.Resume();
            GoWanderAnim();
        }
        else wanderingSwitch = true;
    }

    private void GoIdleFromExit
        ()
    {
        isIdle = true;
        if (animations.idleAnim.Length != 0)
        {
            int chosenidle = Random.Range(0, animations.idleAnim.Length);
            bot.animation.CrossFade(animations.idleAnim[chosenidle].name, animationParameters.idleBlendingTime,
                PlayMode.StopAll);
        }
        if (sounds.idleSfx)
        {
            audio.loop = true;
            audio.clip = sounds.idleSfx;
            audio.volume = soundsVolume.idleVolume;
            audio.Play();
        }
        else audio.Stop();
    }

    private void GoIdle
        ()
    {
        if (isIdle) return;
        isIdle = true;
        nav.Stop(true);
        if (!bot.animation.IsPlaying(animations.gotHitAnim[chosenHit].name))
        {
            if (animations.idleAnim.Length != 0)
            {
                int chosenidle = Random.Range(0, animations.idleAnim.Length);
                bot.animation.CrossFade(animations.idleAnim[chosenidle].name, animationParameters.idleBlendingTime,
                    PlayMode.StopAll);
            }
        }

        if (sounds.idleSfx)
        {
            audio.loop = false;
            audio.clip = sounds.idleSfx;
            audio.volume = soundsVolume.idleVolume;
        }
        if (sounds.idleSfx)
        {
            if (!audio.isPlaying)
                audio.Play();
        }
        else audio.loop = false;
    }

    private void GoDeath
        ()
    {
        isIdle = false;
        generalParameters.healthPoints = 0.0f;

        gameObject.layer = LayerMask.NameToLayer("Default");

        if (generalParameters.deathEffect)
        {
            Instantiate(generalParameters.deathEffect, transform.position, Quaternion.identity);
        }

        if (sounds.dieSfx)
        {
            AudioSource.PlayClipAtPoint(sounds.dieSfx, transform.position, soundsVolume.dieVolume);
        }
        else audio.Stop();


        isDead = true;
        nav.Stop(true);

        if (generalParameters.ragdollifyOnDeath)
        {
            if (generalParameters.ragdollPlayDieAnim && animations.deathAnim.Length != 0)
                GoRagdoll();
            else GoRagdollDirect();
        }
        else
        {
            if (animations.deathAnim.Length != 0)
            {
                chosenDeath = Random.Range(0, animations.deathAnim.Length);
                bot.animation.CrossFade(animations.deathAnim[chosenDeath].name, animationParameters.deathBlendingTime,
                    PlayMode.StopAll);
            }
            if (collider)
                Destroy(collider);
            Destroy(nav);
            Destroy(rigidbody);
            Destroy(GetComponent<SphereCollider>());
            if (!generalParameters.disappearOnDeath)
            {
                Destroy(audio);

                Destroy(GetComponent<AdvancedAiNpcPassive>());
            }
        }
        if (generalParameters.dropItemOnDeath)
        {
            Instantiate(generalParameters.dropItemOnDeath, transform.position + generalParameters.dropItemOffset,
                Quaternion.identity);
        }

        if (generalParameters.hitParticleFx)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -Vector3.up, out hit, 1f))
            {
                Vector3 pos = hit.point;
                hitfx =
                    Instantiate(generalParameters.hitParticleFx, pos + new Vector3(0, nav.height/2, 0),
                        transform.rotation) as GameObject;
            }
        }

        if (generalParameters.bloodDecalDead)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -Vector3.up, out hit, 1f))
            {
                Quaternion rot = Quaternion.FromToRotation(Vector3.up, hit.normal);

                Instantiate(generalParameters.bloodDecalDead, hit.point + new Vector3(0, 0.05f, 0), rot);
            }
        }
    }


    private void GoFlee
        ()
    {
        isIdle = false;
        int prop = Random.Range(0, 8);
        switch (prop)
        {
            case 0:
                rPosition = transform.TransformPoint(0, 0, -navigationParameters.fleeDistance);
                break;
            case 1:
                rPosition = transform.TransformPoint(-navigationParameters.fleeDistance, 0, 0);
                break;
            case 2:
                rPosition = transform.TransformPoint(navigationParameters.fleeDistance, 0, 0);
                break;
            case 3:
                rPosition = transform.TransformPoint(navigationParameters.fleeDistance, 0,
                    navigationParameters.fleeDistance);
                break;
            case 4:
                rPosition = transform.TransformPoint(navigationParameters.fleeDistance, 0,
                    -navigationParameters.fleeDistance);
                break;
            case 5:
                rPosition = transform.TransformPoint(-navigationParameters.fleeDistance, 0,
                    navigationParameters.fleeDistance);
                break;
            case 6:
                rPosition = transform.TransformPoint(-navigationParameters.fleeDistance, 0,
                    -navigationParameters.fleeDistance);
                break;
        }

        path = new NavMeshPath();
        bool canit = NavMesh.CalculatePath(transform.position, rPosition, 1, path);
        if (canit)
        {
            nav.SetDestination(rPosition);
            nav.Resume();
            canMesure = true;
            FleeAnim();
        }
        else GoFlee();
    }

    private void GoWanderAnim
        ()
    {
        if (animations.wanderingAnim.Length != 0)
        {
            int chosenWander = Random.Range(0, animations.wanderingAnim.Length);
            bot.animation.CrossFade(animations.wanderingAnim[chosenWander].name, animationParameters.wanderBlendingTime,
                PlayMode.StopAll);
        }
        if (sounds.wanderingSfx)
        {
            if (!audio.isPlaying)
            {
                audio.loop = true;
                audio.clip = sounds.wanderingSfx;
                audio.volume = soundsVolume.wanderingVolume;
                audio.Play();
            }
        }
        else audio.Stop();
    }


    private void OnDrawGizmos
        ()
    {
        if (!wanderingSwitch)
            Gizmos.DrawCube(rPosition, Vector3.one);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, navigationParameters.goIdleRadius);
        if (generalParameters.canHear)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, navigationParameters.hearingDistance);
        }
        if (patrolMode == patmode.Dynamic_Wandering)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, navigationParameters.wanderingRadius);
        }

        if (fleeMode == fleem.Both || fleeMode == fleem.On_Sight)
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(transform.TransformPoint(0, 0, navigationParameters.viewSphereRadius),
                navigationParameters.viewSphereRadius);
        }
    }

    private void Hearing()
    {
        if (audioS.isPlaying && dist <= navigationParameters.hearingDistance && isIdle &&
            audioS.volume >= navigationParameters.hearVolumeMin)
        {
            Quaternion rotation =
                Quaternion.LookRotation(generalParameters.target.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation,
                Time.deltaTime*navigationParameters.lookAtSpeed);

            return;
        }

        if (audioC.Length != 0 && dist <= navigationParameters.hearingDistance && isIdle)
        {
            foreach (AudioSource asou in audioC)
            {
                if (asou.isPlaying && asou.volume >= navigationParameters.hearVolumeMin)
                {
                    Quaternion rotation =
                        Quaternion.LookRotation(generalParameters.target.position - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation,
                        Time.deltaTime*navigationParameters.lookAtSpeed);
                    break;
                }
            }
        }
    }

    private void GotHit(float healthAmount)
    {
        if (generalParameters.healthPoints > 0)
        {
            isIdle = false;
            isHit = true;
            nav.Stop(true);
            generalParameters.healthPoints -= healthAmount;

            if (animations.gotHitAnim.Length != 0)
            {
                chosenHit = Random.Range(0, animations.gotHitAnim.Length);
                bot.animation.CrossFade(animations.gotHitAnim[chosenHit].name, animationParameters.gotHitBlendTime,
                    PlayMode.StopAll);
            }
            if (generalParameters.healthPoints == 0.0f)
                GoDeath();
        }
        if (generalParameters.healthPoints > 0)
        {
            audio.loop = false;

            if (sounds.gotHitSfx)
            {
                audio.PlayOneShot(sounds.gotHitSfx, soundsVolume.gotHitVolume);
            }


            if (!wanderingSwitch)
            {
                StopCoroutine("IdleInWandering");
            }
            if (generalParameters.hitParticleFx)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, -Vector3.up, out hit, 1f))
                {
                    Vector3 pos = hit.point;
                    hitfx =
                        Instantiate(generalParameters.hitParticleFx, pos + new Vector3(0, nav.height/2, 0),
                            transform.rotation) as GameObject;
                }
            }
        }
    }

    private IEnumerator IdleInWandering()
    {
        isIdle = true;

        if (animations.gotHitAnim.Length != 0)
        {
            if (!bot.animation.IsPlaying(animations.gotHitAnim[chosenHit].name))
            {
                if (animations.idleAnim.Length != 0)
                {
                    int chosenidle = Random.Range(0, animations.idleAnim.Length);
                    bot.animation.CrossFade(animations.idleAnim[chosenidle].name, animationParameters.idleBlendingTime,
                        PlayMode.StopAll);
                }
            }
        }
        else
        {
            if (animations.idleAnim.Length != 0)
            {
                int chosenidle = Random.Range(0, animations.idleAnim.Length);
                bot.animation.CrossFade(animations.idleAnim[chosenidle].name, animationParameters.idleBlendingTime,
                    PlayMode.StopAll);
            }
        }

        float waitingtime = Random.Range(navigationParameters.minIdleInterval, navigationParameters.maxIdleInterval);

        if (waitingtime > 0)
        {
            if (sounds.idleSfx)
            {
                audio.loop = false;
                audio.clip = sounds.idleSfx;
                audio.volume = soundsVolume.idleVolume;
            }
            if (sounds.idleSfx)
            {
                if (!audio.isPlaying)
                    audio.Play();
            }
            else audio.loop = false;
        }
        yield return new WaitForSeconds(waitingtime);

        if (waitingtime > 0)
            audio.Stop();

        wanderingSwitch = true;
        isIdle = false;
    }


    private void StartInIdle()
    {
        isIdle = true;
        if (animations.idleAnim.Length != 0)
        {
            int chosenidle = Random.Range(0, animations.idleAnim.Length);
            bot.animation.CrossFade(animations.idleAnim[chosenidle].name, animationParameters.idleBlendingTime,
                PlayMode.StopAll);
        }


        if (sounds.idleSfx)
        {
            audio.clip = sounds.idleSfx;
            audio.loop = true;
            audio.volume = soundsVolume.idleVolume;

            audio.Play();
        }
        else audio.loop = false;
    }

    private void FleeAnim()
    {

        {
            if (animations.fleeAnim.Length != 0)
            {
                if (animations.gotHitAnim.Length != 0)
                {
                    if (!bot.animation.IsPlaying(animations.gotHitAnim[chosenHit].name))
                    {
                        int chosenFlee = Random.Range(0, animations.fleeAnim.Length);
                        bot.animation.CrossFade(animations.fleeAnim[chosenFlee].name,
                            animationParameters.fleeBlendingTime,
                            PlayMode.StopAll);
                    }
                }
                else
                {
                    int chosenFlee = Random.Range(0, animations.fleeAnim.Length);
                    bot.animation.CrossFade(animations.fleeAnim[chosenFlee].name,
                        animationParameters.fleeBlendingTime,
                        PlayMode.StopAll);
                }
            }

            if (sounds.fleeSfx)
            {
                if (!audio.isPlaying)
                {
                    audio.loop = true;
                    audio.clip = sounds.fleeSfx;
                    audio.volume = soundsVolume.fleeVolume;
                    audio.Play();
                }
            }
            else audio.Stop();
        }
    }

    private void GoRagdoll()
    {
        Animation ani;
        var ragdollinstance =
            Instantiate(generalParameters.ragdollPrefab, transform.position, transform.rotation) as GameObject;

        ragdollinstance.transform.localScale = transform.localScale;

        if (!ragdollinstance.GetComponent<Animation>())
        {
            ani = ragdollinstance.AddComponent<Animation>();
        }
        else ani = ragdollinstance.GetComponent<Animation>();

        int chosenDeath = Random.Range(0, animations.deathAnim.Length);
        AnimationClip deathi = bot.animation.GetClip(animations.deathAnim[chosenDeath].name);

        ani.AddClip(deathi, "DeathAnimation");
        ani.CrossFade("DeathAnimation", 0.3f, PlayMode.StopAll);

        ani.animatePhysics = true;

        if (generalParameters.disappearOnDeath)
        {
            ragdollinstance.AddComponent("DestroyRagdoll");
            ragdollinstance.GetComponent<DestroyRagdoll>().lifeTime = generalParameters.disappearDelay;
        }

        Destroy(gameObject);
    }

    private void GoRagdollDirect()
    {
        var ragdollinstance =
            Instantiate(generalParameters.ragdollPrefab, transform.position, transform.rotation) as GameObject;
        ragdollinstance.transform.localScale = transform.localScale;

        if (generalParameters.disappearOnDeath)
        {
            ragdollinstance.AddComponent("DestroyRagdoll");
            ragdollinstance.GetComponent<DestroyRagdoll>().lifeTime = generalParameters.disappearDelay;
        }

        Destroy(gameObject);
    }

    private void Damage(float hp)
    {
        GotHit(hp);
    }
}