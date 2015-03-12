using System;
using UnityEngine;

[Serializable]
public class animsgroup
{
    public AnimationClip deathAnim;
    public AnimationClip followAnim;
    public AnimationClip gotHitAnim;
    public AnimationClip idleAnim;
    public AnimationClip jumpAnim;
}

[Serializable]
public class soundsgroup
{
    public AudioClip dieSfx;
    public AudioClip followSfx;
    public AudioClip gotHitSfx;
    public AudioClip idleSfx;
    public AudioClip jumpSfx;
}

[Serializable]
public class soundvalo
{
    public float _maxDistance = 10f;
    [Range(0, 1)] public float dieVolume = 1f;
    [Range(0, 1)] public float followVolume = 1f;
    [Range(0, 1)] public float gotHitVolume = 1f;
    [Range(0, 1)] public float idleVolume = 1f;
    [Range(0, 1)] public float jumpVolume = 1f;
}

[Serializable]
public class animparam
{
    public float deathAnimSpeed = 1f;
    public float deathBlendingTime = 0.3f;
    public float followAnimSpeed = 1f;
    public float followBlendingTime = 0.3f;
    public float gotHitAnimSpeed = 1f;
    public float hitBlendingTime = 0.3f;
    public float idleAnimSpeed = 1f;
    public float idleBlendingTime = 0.3f;
    public float jumpBlendTime = 0.3f;
}

[Serializable]
public class navigM
{
    public float acceleration = 8f;
    public float arrivalDistance = 2.0f;
    public float commandRange = 10.0f;
    public float followSpeed = 2.5f;
    public float jumpSpeed = 2f;
    public float rotationSpeed = 140.0f;
}

[Serializable]
public class gener
{
    public GameObject bloodDecalDead;
    public GameObject deathEffect;
    public float disappearDelay = 2.0f;
    public bool disappearOnDeath = false;
    public KeyCode followCommandKey = KeyCode.F;
    public float healthPoints = 100.0f;
    public GameObject hitParticleFx;
    public bool ragdollPlayDieAnim;
    public GameObject ragdollPrefab;
    public bool ragdollifyOnDeath;
    public KeyCode stopCommandKey = KeyCode.G;
    public Transform target;
}

public class AdvancedAiCompanion : MonoBehaviour
{
    public animparam animationParameters;
    public animsgroup animations;
    private Transform bot;
    private bool followSwitch = true;
    public gener generalParameters;
    private GameObject hitfx;
    private bool isDead;
    private bool isHit;
    private NavMeshAgent nav;
    public navigM navigationMovement;
    private bool playedJump;
    public soundsgroup sounds;
    public soundvalo soundsVolume;

    private void Start()
    {
        bot = transform.GetChild(0);


        if (!generalParameters.target)
        {
            if (GameObject.FindGameObjectWithTag("Player"))
                generalParameters.target = GameObject.FindGameObjectWithTag("Player").transform;
            else
                Debug.LogWarning("You forgot to assign a target transform to your AI: " + gameObject.name);
        }


        if (!GetComponent<NavMeshAgent>()) gameObject.AddComponent("NavMeshAgent");

        if (!audio)
            gameObject.AddComponent("AudioSource");


        audio.rolloffMode = AudioRolloffMode.Linear;
        audio.maxDistance = soundsVolume._maxDistance;

        if (animations.idleAnim)
        {
            animations.idleAnim.wrapMode = WrapMode.Loop;
            bot.animation[animations.idleAnim.name].speed = animationParameters.idleAnimSpeed;
        }

        if (animations.gotHitAnim)
        {
            animations.gotHitAnim.wrapMode = WrapMode.Once;
            bot.animation[animations.gotHitAnim.name].speed = animationParameters.gotHitAnimSpeed;
        }


        if (animations.deathAnim)
        {
            animations.deathAnim.wrapMode = WrapMode.Once;
            bot.animation[animations.deathAnim.name].speed = animationParameters.deathAnimSpeed;
        }


        if (animations.followAnim)
        {
            animations.followAnim.wrapMode = WrapMode.Loop;
            bot.animation[animations.followAnim.name].speed = animationParameters.followAnimSpeed;
        }


        nav = gameObject.GetComponent<NavMeshAgent>();


        nav.speed = navigationMovement.followSpeed;
        nav.angularSpeed = navigationMovement.rotationSpeed;
        nav.stoppingDistance = navigationMovement.arrivalDistance;
        nav.acceleration = navigationMovement.acceleration;

        GoIdle();
    }

    private void Update()
    {
        if (!generalParameters.target) return;

        if (nav && nav.isOnOffMeshLink)
        {
            nav.speed = navigationMovement.jumpSpeed;
            if (animations.jumpAnim)
            {
                float distnt = Vector3.Distance(nav.currentOffMeshLinkData.startPos,
                    nav.currentOffMeshLinkData.endPos);
                float time = distnt/nav.speed;
                bot.animation[animations.jumpAnim.name].speed = bot.animation[animations.jumpAnim.name].length/time;
                if (!bot.animation.IsPlaying(animations.jumpAnim.name))
                    bot.animation.CrossFade(animations.jumpAnim.name, animationParameters.jumpBlendTime,
                        PlayMode.StopAll)
                        ;
            }
            if (!playedJump && sounds.jumpSfx)
            {
                playedJump = true;
                audio.Stop();

                audio.PlayOneShot(sounds.jumpSfx, soundsVolume.jumpVolume);
            }
        }
        if (nav && !nav.isOnOffMeshLink)
        {
            playedJump = false;
            nav.speed = navigationMovement.followSpeed;
        }

        if (isDead && generalParameters.disappearOnDeath && animations.deathAnim &&
            !bot.animation.IsPlaying(animations.deathAnim.name) &&
            !audio.isPlaying)
        {
            Destroy(gameObject, generalParameters.disappearDelay);
        }

        else if (isDead && generalParameters.disappearOnDeath && !animations.deathAnim &&
            !audio.isPlaying)
        {
            Destroy(gameObject, generalParameters.disappearDelay);
        }



        if (generalParameters.healthPoints > 0)
        {
            if (isHit && !bot.animation.IsPlaying(animations.gotHitAnim.name))
            {
                isHit = false;

                if (followSwitch)
                {
                    nav.Stop(true);
                    GoIdle();
                }
                else
                {
                    nav.Resume();
                }
            }
        }


        if (generalParameters.healthPoints > 0)
        {
            if (!followSwitch)
            {
                nav.SetDestination(generalParameters.target.position);

                if (nav.remainingDistance <= navigationMovement.arrivalDistance)
                {
                    nav.Stop(true);
                    GoIdle();
                }
                else
                {
                    if (!bot.animation.IsPlaying(animations.gotHitAnim.name))
                    {
                        nav.Resume();
                        GoFollow();
                    }
                    else
                    {
                        nav.Stop(true);
                    }
                }
            }
        }


        if (generalParameters.healthPoints < 0)
        {
            GoDeath();
        }

        if (generalParameters.healthPoints > 0)
        {
            float dist = Vector3.Distance(transform.position, generalParameters.target.position);
            if (dist <= navigationMovement.commandRange)
            {
                if (Input.GetKeyDown(generalParameters.followCommandKey))
                {
                    if (followSwitch)
                    {
                        followSwitch = false;
                    }
                }
                if (Input.GetKeyDown(generalParameters.stopCommandKey))
                {
                    followSwitch = true;
                    nav.SetDestination(transform.position);


                    GoIdle();
                }
            }
        }

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
                {
                    DestroyImmediate(hitfx);
                }
            }
            else
            {
                Destroy(hitfx.gameObject, 1f);
            }
        }
    }


    private void GoIdle()
    {
        if (!bot.animation.IsPlaying(animations.gotHitAnim.name))
        {
            if (animations.idleAnim)
            {
                bot.animation.CrossFade(animations.idleAnim.name, animationParameters.idleBlendingTime, PlayMode.StopAll);
            }
            if (sounds.idleSfx)
            {
                audio.clip = sounds.idleSfx;
                audio.volume = soundsVolume.idleVolume;
                audio.loop = true;
                if (!audio.isPlaying)
                {
                    audio.Play();
                }
            }
            else
            {
                audio.Stop();
            }
        }
    }

    private void GoDeath
        ()
    {
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
        nav.Stop();

        if (generalParameters.ragdollifyOnDeath)
        {
            if (generalParameters.ragdollPlayDieAnim && animations.deathAnim)
            {
                GoRagdoll();
            }
            else
            {
                GoRagdollDirect();
            }
        }
        else
        {
            if (animations.deathAnim)
            {
                bot.animation.CrossFade(animations.deathAnim.name, animationParameters.deathBlendingTime,
                    PlayMode.StopAll);
            }
            if (collider)
                Destroy(collider);
            Destroy(nav);
            if (!generalParameters.disappearOnDeath)
            {
                Destroy(audio);
                Destroy(GetComponent<AdvancedAiCompanion>());
            }
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


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, navigationMovement.commandRange);
    }


    private void GotHit(float healthAmount)
    {
        if (generalParameters.healthPoints > 0)
        {
            isHit = true;
            nav.Stop(true);
            generalParameters.healthPoints -= healthAmount;
            if (animations.gotHitAnim)
            {
                bot.animation.CrossFade(animations.gotHitAnim.name, animationParameters.hitBlendingTime,
                    PlayMode.StopAll);
            }
            if (generalParameters.healthPoints == 0.0f)
            {
                GoDeath();
            }
        }
        if (generalParameters.healthPoints > 0)
        {
            audio.loop = false;

            if (sounds.gotHitSfx)
            {
                audio.PlayOneShot(sounds.gotHitSfx, soundsVolume.gotHitVolume);
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

    private void GoFollow()
    {
        if (animations.followAnim && !nav.isOnOffMeshLink)
        {
            bot.animation.CrossFade(animations.followAnim.name, animationParameters.followBlendingTime, PlayMode.StopAll);
        }
        if (sounds.followSfx)
        {
            if (!playedJump)
            {
                audio.clip = sounds.followSfx;
                audio.volume = soundsVolume.followVolume;
                audio.loop = true;
                if (!audio.isPlaying)
                {
                    audio.Play();
                }
            }
        }
        else
        {
            audio.Stop();
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
        else
        {
            ani = ragdollinstance.GetComponent<Animation>();
        }
        AnimationClip deathi = bot.animation.GetClip(animations.deathAnim.name);
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
}