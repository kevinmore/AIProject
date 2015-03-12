using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class animigroup
{
    public AnimationClip[] chasingAnim;
    public AnimationClip[] deathAnim;
    public AnimationClip[] followAnim;
    public AnimationClip[] gotHitAnim;
    public AnimationClip[] idleAnim;
    public AnimationClip jumpAnim;
    public AnimationClip[] meleeAttackAnim;
    public AnimationClip rangedAttackAnim;
    public AnimationClip reloadAnim;
}

[Serializable]
public class animpar
{
    public float chaseBlendingTime = 0.3f;
    public float chasingAnimSpeed = 1f;
    public float deathAnimSpeed = 1f;
    public float deathBlendingTime = 0.3f;
    public float followAnimSpeed = 1f;
    public float followBlendingTime = 0.3f;
    public float gotHitAnimSpeed = 1f;
    public float hitBlendingTime = 0.3f;
    public float idleAnimSpeed = 1f;
    public float idleBlendingTime = 0.3f;
    public float jumpBlendingTime = 0.3f;
    public float meleeAnimSpeed = 1f;
    public float meleeBlendingTime = 0.3f;
    public float rangedAnimSpeed = 1f;
    public float rangedBlendingTime = 0.3f;
    public float reloadAnimSpeed = 1f;
    public float reloadBlendingTime = 0.3f;
}

[Serializable]
public class soundigroup
{
    public AudioClip chasingSfx;
    public AudioClip dieSfx;
    public AudioClip followSfx;
    public AudioClip gotHitSfx;
    public AudioClip idleSfx;
    public AudioClip jumpSfx;
    public AudioClip[] meleeAttackSfx;
    public AudioClip projectileImpactSfx;
    public AudioClip rangedAttackSfx;
    public AudioClip reloadSfx;
}

[Serializable]
public class soundparoma
{
    public float _maxDistance = 10f;
    [Range(0, 1)]
    public float chaseVolume = 1f;
    [Range(0, 1)]
    public float detectionVolume = 1f;
    [Range(0, 1)]
    public float dieVolume = 1f;
    [Range(0, 1)]
    public float followVolume = 1f;
    [Range(0, 1)]
    public float gotHitVolume = 1f;
    [Range(0, 1)]
    public float idleVolume = 1f;
    [Range(0, 1)]
    public float jumpVolume = 1f;
    [Range(0, 1)]
    public float meleeAttackVolume = 1f;
    [Range(0, 1)]
    public float rangedAttackVolume = 1f;
    [Range(0, 1)]
    public float reloadVolume = 1f;
}

[Serializable]
public class naviig
{
    public float acceleration = 8f;
    public float arrivalDistance = 1.5f;
    public float chasingSpeed = 5.0f;
    public float commandRange = 3f;
    public float followSpeed = 2.5f;
    public float jumpSpeed = 2f;
    public float lookAtSpeed = 6.0f;
    public float rotationSpeed = 140f;
    public Vector3 viewSphereCenter = new Vector3(0, 0, 5);
    public float viewSphereRadius = 5.0f;
}

[Serializable]
public class lay
{
    public LayerMask enemyLayer;
    public LayerMask mainTarget;
    public LayerMask viewObstruction;
}

[Serializable]
public class geni
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

[Serializable]
public class meattack
{
    public float[] damageAmount;
    public float attackInterval = 1.0f;
    public float damageDelay = 1f;
}

[Serializable]
public class raattack
{
    public float decalDestroyDelay = 10.0f;
    public float projImpactFxDestTime = 2.0f;
    public float projectileDamage = 10.0f;
    public Texture2D projectileDecal;
    public float projectileDelay = 1f;
    public float projectileDestroyDelay = 2.5f;
    public GameObject projectileFx;
    public GameObject projectileImpactFx;
    public GameObject projectilePrefab;
    public float projectileVelocity = 20.0f;
    public int reloadInterval = 10;
    public float shootInterval = 1.0f;
    public bool loopRangedAnimation;
    public bool canReload = true;
}

public class AdvancedAiDefender : MonoBehaviour
{
    public enum attackM
    {
        Melee,
        Ranged
    }

    public animpar animationParameters;
    public animigroup animations;
    public attackM attackMode;
    private Transform bot;
    private bool canCommand = true;
    private int chosenChase;
    private int chosenDeath;
    private int chosenFollow;
    private int chosenHit;
    private int chosenIdle;
    private int chosenMelee;
    private float dist;
    private GameObject enemy;
    private Vector3 enemyPos;
    private bool followSwitch;
    private GameObject fx;
    public geni generalParameters;
    private GameObject hitfx;
    private bool isAttacking;
    private bool isDead;
    private bool isExited = true;
    private bool isHit;
    private bool isIdle;
    public lay layers;
    public meattack meleeAttack;
    private bool meleeAttacksSwitch = true;
    private NavMeshAgent nav;
    public naviig navigationMovement;
    [HideInInspector] public Collider other;
    private bool playedJump;
    private GameObject pro;
    private Transform projectileOrigin;
    public raattack rangedAttack;
    private bool rangedAttacksSwitch = true;
    private int reloadCount;
    private bool rotateUpdate;
    public soundigroup sounds;
    public soundparoma soundsVolume;
    private GameObject triggerChil;


    private void Start()
    {
            if (transform.FindChild("TriggerSphere")) Destroy(transform.FindChild("TriggerSphere").gameObject);

            bot = transform.GetChild(0);
            projectileOrigin = transform.GetChild(1);

            if (bot.name == "_ProjectileOrigin")
            {
                bot = transform.GetChild(1);
                projectileOrigin = transform.GetChild(0);
            }

            if (!generalParameters.target)
            {
                if (GameObject.FindGameObjectWithTag("Player"))
                    generalParameters.target = GameObject.FindGameObjectWithTag("Player").transform;
                else
                    Debug.LogWarning("You forgot to assign a target transform to your AI: " + gameObject.name);
            }


            if (!GetComponent<NavMeshAgent>()) gameObject.AddComponent("NavMeshAgent");

            if (!audio) gameObject.AddComponent("AudioSource");


            triggerChil = new GameObject();
            triggerChil.transform.parent = transform;
            triggerChil.transform.localPosition = Vector3.zero;
            triggerChil.AddComponent("SphereCollider");
            triggerChil.layer = LayerMask.NameToLayer("Ignore Raycast");
            triggerChil.AddComponent<IdleSphere>();
            triggerChil.name = "TriggerSphere";
            triggerChil.GetComponent<SphereCollider>().radius = navigationMovement.commandRange;
            triggerChil.GetComponent<SphereCollider>().isTrigger = true;


            audio.rolloffMode = AudioRolloffMode.Linear;
            audio.maxDistance = soundsVolume._maxDistance;

            if (animations.idleAnim.Length != 0)
            {
                foreach (AnimationClip iAnim in animations.idleAnim)
                {
                    iAnim.wrapMode = WrapMode.Loop;
                    bot.animation[iAnim.name].speed = animationParameters.idleAnimSpeed;
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

            if (animations.meleeAttackAnim.Length != 0)
            {
                foreach (AnimationClip mAttack in animations.meleeAttackAnim)
                {
                    mAttack.wrapMode = WrapMode.Once;
                    bot.animation[mAttack.name].speed = animationParameters.meleeAnimSpeed;
                }
            }
            if (animations.rangedAttackAnim)
            {
                if (!rangedAttack.loopRangedAnimation)
                    animations.rangedAttackAnim.wrapMode = WrapMode.Once;
                else
                {
                    animations.rangedAttackAnim.wrapMode = WrapMode.Loop;
                }
                bot.animation[animations.rangedAttackAnim.name].speed = animationParameters.rangedAnimSpeed;
            }

            if (animations.deathAnim.Length != 0)
            {
                foreach (AnimationClip dAnim in animations.deathAnim)
                {
                    dAnim.wrapMode = WrapMode.Once;
                    bot.animation[dAnim.name].speed = animationParameters.deathAnimSpeed;
                }
            }
            if (animations.chasingAnim.Length != 0)
            {
                foreach (AnimationClip cAnim in animations.chasingAnim)
                {
                    cAnim.wrapMode = WrapMode.Loop;
                    bot.animation[cAnim.name].speed = animationParameters.chasingAnimSpeed;
                }
            }


            if (animations.reloadAnim)
            {
                animations.reloadAnim.wrapMode = WrapMode.Once;
                bot.animation[animations.reloadAnim.name].speed = animationParameters.reloadAnimSpeed;
            }

            if (animations.followAnim.Length != 0)
            {
                foreach (AnimationClip fAnim in animations.followAnim)
                {
                    fAnim.wrapMode = WrapMode.Loop;
                    bot.animation[fAnim.name].speed = animationParameters.followAnimSpeed;
                }
            }


            nav = gameObject.GetComponent<NavMeshAgent>();
            nav.speed = navigationMovement.followSpeed;
            nav.angularSpeed = navigationMovement.rotationSpeed;
            nav.stoppingDistance = navigationMovement.arrivalDistance;
            nav.acceleration = navigationMovement.acceleration;

            if (layers.enemyLayer.value == 0)
            {
                Debug.LogWarning("You forgot to setup the Enemy layer for Defender AI: " + gameObject.name);
            }
            if (layers.viewObstruction.value == 0)
            {
                Debug.LogWarning("You did not assign the view's obstruction layer for Defender AI: " + gameObject.name);
            }

            if (rangedAttack.projectileFx)
            {
                if (rangedAttack.projectileFx.GetComponent<ParticleAnimator>())
                    rangedAttack.projectileFx.GetComponent<ParticleAnimator>().autodestruct = true;
            }

            StartInIdle();
       
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
                        bot.animation.CrossFade(animations.jumpAnim.name, animationParameters.jumpBlendingTime,
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
            if (nav && !nav.isOnOffMeshLink) playedJump = false;

            // See Enemy
            if (generalParameters.healthPoints > 0 && layers.enemyLayer.value != 0)
            {
                Collider[] colinfo =
                    Physics.OverlapSphere(transform.TransformPoint(navigationMovement.viewSphereCenter),
                        navigationMovement.viewSphereRadius, layers.enemyLayer);
                int i = 0;
                foreach (Collider colhit in colinfo)
                {
                    i += 1;
                    if (Vector3.Distance(transform.position, colhit.gameObject.transform.position) <=
                        (navigationMovement.viewSphereRadius*2))
                    {
                        RaycastHit hitinfo;
                        if (colhit.gameObject.GetComponent<AdvancedAiEnemy>() &&
                            colhit.GetComponent<AdvancedAiEnemy>().generalParameters.healthPoints > 0 &&
                            !Physics.Linecast(transform.position, colhit.transform.position, out hitinfo,
                                layers.viewObstruction)
                            )
                        {
                            canCommand = false;
                            enemy = colhit.gameObject;

                            isAttacking = true;
                            enemyPos = Vector3.zero;
                            rotateUpdate = false;

                            break;
                        }
                        isAttacking = false;
                        StopAllCoroutines();
                        meleeAttacksSwitch = true;
                        rangedAttacksSwitch = true;
                        nav.stoppingDistance = navigationMovement.arrivalDistance;
                        if (!nav.isOnOffMeshLink) nav.speed = navigationMovement.followSpeed;

                        canCommand = true;

                        if (fx)
                        {
                            if (!fx.GetComponent<ParticleSystem>())
                            {
                                Destroy(fx);
                            }
                        }
                        break;
                    }
                    if (i == colinfo.Length)
                    {
                        isAttacking = false;
                        StopAllCoroutines();
                        meleeAttacksSwitch = true;
                        rangedAttacksSwitch = true;
                        nav.stoppingDistance = navigationMovement.arrivalDistance;
                        if (!nav.isOnOffMeshLink) nav.speed = navigationMovement.followSpeed;
                        canCommand = true;

                        if (fx)
                        {
                            if (!fx.GetComponent<ParticleSystem>())
                            {
                                Destroy(fx);
                            }
                        }
                    }
                    else
                    {
                    }
                }
                if (colinfo.Length == 0)
                {
                    isAttacking = false;
                    StopAllCoroutines();
                    meleeAttacksSwitch = true;
                    rangedAttacksSwitch = true;
                    nav.stoppingDistance = navigationMovement.arrivalDistance;
                    if (!nav.isOnOffMeshLink) nav.speed = navigationMovement.followSpeed;

                    canCommand = true;

                    if (fx)
                    {
                        if (!fx.GetComponent<ParticleSystem>())
                        {
                            Destroy(fx);
                        }
                    }
                }
            }

            //Attack Update

            if (generalParameters.healthPoints > 0 && isAttacking)
            {
                if (attackMode == attackM.Melee)
                {
                    nav.stoppingDistance = navigationMovement.arrivalDistance;
                    if (!nav.isOnOffMeshLink) nav.speed = navigationMovement.chasingSpeed;
                    if (!isHit) nav.destination = enemy.transform.position;
                    dist = Vector3.Distance(transform.position, enemy.transform.position);


                    if (nav.remainingDistance <= navigationMovement.arrivalDistance)
                    {
                        nav.Stop(true);
                        transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
                        GoMelee();
                    }
                    else
                    {
                        StopCoroutine("MeleeDelay");
                        meleeAttacksSwitch = true;
                        if (animations.meleeAttackAnim.Length != 0)
                        {
                            if (!bot.animation.IsPlaying(animations.meleeAttackAnim[chosenMelee].name))
                            {
                                if (!isHit) nav.Resume();
                                GoChase();
                            }
                        }
                        else
                        {
                            if (!isHit) nav.Resume();
                            GoChase();
                        }
                    }
                }
                else if (attackMode == attackM.Ranged)
                {
                    nav.Stop(true);
                    GoRanged();
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


            if (animations.gotHitAnim.Length != 0)
            {
                if (isHit && !bot.animation.IsPlaying(animations.gotHitAnim[chosenHit].name) && !rotateUpdate)
                {
                    isHit = false;
                    rangedAttacksSwitch = true;
                    meleeAttacksSwitch = true;
                }
            }
            else
            {
                if (isHit && !rotateUpdate)
                {
                    isHit = false;
                    rangedAttacksSwitch = true;
                    meleeAttacksSwitch = true;
                }
            }


            if (generalParameters.healthPoints > 0)
            {
                if (rotateUpdate)
                {
                    if (enemyPos != Vector3.zero)
                    {
                        Vector3 targetDir = enemyPos - transform.position;
                        Quaternion rotation = Quaternion.LookRotation(targetDir);
                        transform.rotation = Quaternion.Slerp(transform.rotation, rotation,
                            Time.deltaTime*navigationMovement.lookAtSpeed);

                        Vector3 forward = transform.forward;
                        float angle = Vector3.Angle(targetDir, forward);
                        if (angle < 20.0f)
                        {
                            rotateUpdate = false;
                            enemyPos = Vector3.zero;
                        }
                    }
                    else
                    {
                        Vector3 targetPos = transform.TransformPoint(0, 0, -1);
                        transform.LookAt(targetPos);
                        rotateUpdate = false;
                    }
                }
            }


            if (generalParameters.healthPoints < 0)
            {
                GoDeath();
            }

            if (rangedAttack.projectileFx && fx)
            {
                if (fx.GetComponent<ParticleSystem>())
                {
                    fx.GetComponent<ParticleSystem>().loop = false;
                    var fxp = fx.GetComponent<ParticleSystem>();
                    ParticleSystem[] fxc = fxp.GetComponentsInChildren<ParticleSystem>();
                    foreach (ParticleSystem fxcc in fxc)
                    {
                        fxcc.loop = false;
                    }

                    if (!fx.particleSystem.IsAlive(true))
                    {
                        DestroyImmediate(fx);
                    }
                }
            }


            if (generalParameters.healthPoints > 0 && !isAttacking)
            {
                if (followSwitch)
                {
                    nav.SetDestination(generalParameters.target.position);

                    if (nav.remainingDistance <= navigationMovement.arrivalDistance)
                    {
                        nav.Stop(true);
                        transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
                        GoIdle();
                    }
                    else
                    {
                        if (animations.gotHitAnim.Length != 0)
                        {
                            if (!bot.animation.IsPlaying(animations.gotHitAnim[chosenHit].name))
                            {
                                nav.Resume();
                                GoFollow();
                            }
                            else
                            {
                                nav.Stop(true);
                                transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
                            }
                        }
                        else
                        {
                            nav.Resume();
                            GoFollow();
                        }
                    }
                }
                else
                {
                    nav.SetDestination(transform.position);
                    GoIdle();
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

    public void TriggerStay()
    {
        if (other.gameObject.transform == generalParameters.target && canCommand)
        {
            if (Input.GetKeyDown(generalParameters.followCommandKey)) followSwitch = true;
            if (Input.GetKeyDown(generalParameters.stopCommandKey)) followSwitch = false;
        }
    }

    private void GoIdle
        ()
    {
        isIdle = true;

        nav.Stop(true);

        if (animations.gotHitAnim.Length != 0)
        {
            if (!bot.animation.IsPlaying(animations.gotHitAnim[chosenHit].name) &&
                !bot.animation.IsPlaying(animations.rangedAttackAnim.name))
            {
                if (animations.idleAnim.Length != 0)
                {
                    chosenIdle = Random.Range(0, animations.idleAnim.Length);
                    bot.animation.CrossFade(animations.idleAnim[chosenIdle].name, animationParameters.idleBlendingTime,
                        PlayMode.StopAll);
                }
            }
        }
        else
        {
            if (!bot.animation.IsPlaying(animations.rangedAttackAnim.name))
            {
                if (animations.idleAnim.Length != 0)
                {
                    chosenIdle = Random.Range(0, animations.idleAnim.Length);
                    bot.animation.CrossFade(animations.idleAnim[chosenIdle].name, animationParameters.idleBlendingTime,
                        PlayMode.StopAll);
                }
            }
        }

        if (sounds.idleSfx)
        {
            audio.loop = false;
            audio.clip = sounds.idleSfx;
            audio.volume = soundsVolume.idleVolume;
            if (!audio.isPlaying) audio.Play();
        }

        else audio.loop = false;
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
            AudioSource.PlayClipAtPoint(sounds.dieSfx, transform.position, soundsVolume.dieVolume);

        else audio.Stop();


        isDead = true;
        nav.Stop();
        rotateUpdate = false;

        if (fx)
        {
            if (!fx.GetComponent<ParticleSystem>())
            {
                Destroy(fx);
            }
        }

        if (generalParameters.ragdollifyOnDeath)
        {
            if (generalParameters.ragdollPlayDieAnim && animations.deathAnim.Length != 0)
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
            if (animations.deathAnim.Length != 0)
            {
                chosenDeath = Random.Range(0, animations.deathAnim.Length);
                bot.animation.CrossFade(animations.deathAnim[chosenDeath].name, animationParameters.deathBlendingTime,
                    PlayMode.StopAll);
            }
            if (collider)
                Destroy(collider);
            Destroy(nav);

            if (!generalParameters.disappearOnDeath)
            {
                Destroy(audio);
                Destroy(GetComponent<AdvancedAiDefender>());
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

    private void GoMelee
        ()
    {
        if (dist <= (navigationMovement.arrivalDistance*3))
        {
            if (meleeAttacksSwitch)
            {
                meleeAttacksSwitch = false;
                transform.LookAt(enemy.transform);

                if (animations.chasingAnim.Length != 0)
                {
                    bot.animation.Stop(animations.chasingAnim[chosenChase].name);
                }


                StartCoroutine("MeleeDelay");
            }
        }
    }

    private void GoChase
        ()
    {
        if (animations.chasingAnim.Length != 0 && !nav.isOnOffMeshLink)
        {
            if (!bot.animation.IsPlaying(animations.gotHitAnim[chosenHit].name))
            {
                chosenChase = Random.Range(0, animations.chasingAnim.Length);
                bot.animation.CrossFade(animations.chasingAnim[chosenChase].name, animationParameters.chaseBlendingTime,
                    PlayMode.StopAll);
            }
        }

        if (audio.clip == sounds.followSfx) audio.clip = null;

        if (sounds.chasingSfx && !isHit && !playedJump)
        {
            if (!audio.isPlaying)
            {
                audio.loop = true;
                audio.clip = sounds.chasingSfx;
                audio.volume = soundsVolume.chaseVolume;
                audio.Play();
            }
        }
    }


    private void GoRanged
        ()
    {
        Vector3 dir = enemy.transform.position - transform.position;
        Quaternion tarpos = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, tarpos,
            Time.deltaTime*navigationMovement.lookAtSpeed);

        if (rangedAttack.loopRangedAnimation && sounds.rangedAttackSfx && !audio.isPlaying)
        {
            audio.loop = true;
            audio.clip = sounds.rangedAttackSfx;
            audio.Play();
        }

        if (rangedAttacksSwitch && !rotateUpdate && !isHit)
        {
            if (fx)
            {
                if (!fx.GetComponent<ParticleSystem>())
                {
                    Destroy(fx);
                }
            }

            if (rangedAttack.canReload)
            {
                if (reloadCount < rangedAttack.reloadInterval)
                {
                    rangedAttacksSwitch = false;
                    reloadCount += 1;
                    StartCoroutine("RangedDelay");
                }
                else
                {
                    rangedAttacksSwitch = false;
                    StartCoroutine("Reload");
                }
            }
            else
            {
                rangedAttacksSwitch = false;
                StartCoroutine("RangedDelay");
            }
        }
    }

    private IEnumerator RangedDelay
        ()
    {
        if (animations.followAnim.Length != 0)
        {
            bot.animation.Stop(animations.followAnim[chosenFollow].name);
        }
        if (animations.idleAnim.Length != 0)
        {
            bot.animation.Stop(animations.idleAnim[chosenIdle].name);
        }
        if (animations.rangedAttackAnim)
        {
            bot.animation.CrossFadeQueued(animations.rangedAttackAnim.name, animationParameters.rangedBlendingTime,
                QueueMode.CompleteOthers);
        }


        yield return new WaitForSeconds(rangedAttack.projectileDelay);

        if (sounds.rangedAttackSfx && !rangedAttack.loopRangedAnimation)
        {
            audio.PlayOneShot(sounds.rangedAttackSfx, soundsVolume.rangedAttackVolume);
        }
        else
        {
            audio.Stop();
        }


        if (rangedAttack.projectilePrefab)
        {
            pro =
                Instantiate(rangedAttack.projectilePrefab, projectileOrigin.position, projectileOrigin.rotation) as
                    GameObject;

            if (!pro.GetComponent<Projectile>())
                pro.AddComponent("Projectile");

            if (!pro.rigidbody)
                pro.AddComponent("Rigidbody");

            Physics.IgnoreCollision(collider, pro.gameObject.collider);


            pro.rigidbody.useGravity = false;
            var procomponent = pro.GetComponent<Projectile>();

            procomponent.damage = rangedAttack.projectileDamage;
            procomponent.destructionTime = rangedAttack.projectileDestroyDelay;


            if (rangedAttack.projectileFx && !fx)
            {
                fx =
                    Instantiate(rangedAttack.projectileFx, projectileOrigin.position, projectileOrigin.rotation)
                        as
                        GameObject;
            }

            if (rangedAttack.projectileDecal)
            {
                procomponent.projectileDecal = rangedAttack.projectileDecal;
                procomponent.decalsDestructionTime = rangedAttack.decalDestroyDelay;
            }
            if (rangedAttack.projectileImpactFx)
            {
                procomponent.projectileImpactFx = rangedAttack.projectileImpactFx;
                procomponent.ImpactFxDestructionTime = rangedAttack.projImpactFxDestTime;
            }

            if (sounds.projectileImpactSfx)
            {
                procomponent.projectileImpactSfx = sounds.projectileImpactSfx;
            }

            if (enemy.transform.position.y > pro.transform.position.y*1.3 ||
                enemy.transform.position.y*2 < pro.transform.position.y)
                pro.transform.LookAt(enemy.transform);

            pro.rigidbody.velocity = pro.transform.forward*rangedAttack.projectileVelocity;
        }


        yield return new WaitForSeconds(rangedAttack.shootInterval);
        rangedAttacksSwitch = true;
    }

    private IEnumerator MeleeDelay
        ()
    {
        if (animations.meleeAttackAnim.Length != 0)
        {
            chosenMelee = Random.Range(0, animations.meleeAttackAnim.Length);
            bot.animation.CrossFade(animations.meleeAttackAnim[chosenMelee].name, animationParameters.meleeBlendingTime,
                PlayMode.StopAll);
            if (meleeAttack.attackInterval > 0)
            {
                chosenIdle = Random.Range(0, animations.idleAnim.Length);
                bot.animation.CrossFadeQueued(animations.idleAnim[chosenIdle].name, animationParameters.idleBlendingTime,
                    QueueMode.CompleteOthers);
            }
            if (sounds.meleeAttackSfx.Length != 0)
            {
                audio.loop = false;
                audio.clip = sounds.meleeAttackSfx[chosenMelee];
                audio.volume = soundsVolume.meleeAttackVolume;
                if (!audio.isPlaying)
                {
                    audio.Play();
                }
            }
            else
            {
                audio.Stop();
            }

            yield return new WaitForSeconds(meleeAttack.damageDelay);

            enemy.SendMessage("GotHitFromDefender", meleeAttack.damageAmount[chosenMelee],
                SendMessageOptions.DontRequireReceiver);

            enemy.SendMessage("GetPosition", transform.position);
            yield return new WaitForSeconds(meleeAttack.attackInterval);
            meleeAttacksSwitch = true;
        }
        else
        {
            if (sounds.meleeAttackSfx.Length != 0)
            {
                audio.loop = false;
                audio.clip = sounds.meleeAttackSfx[Random.Range(0, sounds.meleeAttackSfx.Length)];
                audio.volume = soundsVolume.meleeAttackVolume;
                if (!audio.isPlaying)
                {
                    audio.Play();
                }
            }
            else
            {
                audio.Stop();
            }


            enemy.SendMessage("GotHit",
                meleeAttack.damageAmount[Random.Range(0, meleeAttack.damageAmount.Length)],
                SendMessageOptions.DontRequireReceiver);


            yield return new WaitForSeconds(meleeAttack.attackInterval);
            meleeAttacksSwitch = true;
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.TransformPoint(navigationMovement.viewSphereCenter),
            navigationMovement.viewSphereRadius);
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(transform.position, navigationMovement.commandRange);
    }


    private void GotHit(float healthAmount)
    {
        if (generalParameters.healthPoints > 0)
        {
            nav.Stop(true);

            if (fx)
            {
                if (!fx.GetComponent<ParticleSystem>())
                {
                    Destroy(fx);
                }
            }

            if (!rangedAttacksSwitch)
            {
                StopCoroutine("RangedDelay");
            }

            if (!meleeAttacksSwitch)
            {
                StopCoroutine("MeleeDelay");
            }

            generalParameters.healthPoints -= healthAmount;
            if (animations.gotHitAnim.Length != 0)
            {
                chosenHit = Random.Range(0, animations.gotHitAnim.Length);
                bot.animation.CrossFade(animations.gotHitAnim[chosenHit].name, animationParameters.hitBlendingTime,
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

            if (isExited || isIdle)
            {
                if (animations.idleAnim.Length != 0)
                {
                    chosenIdle = Random.Range(0, animations.idleAnim.Length);
                    bot.animation.CrossFadeQueued(animations.idleAnim[chosenIdle].name,
                        animationParameters.idleBlendingTime,
                        QueueMode.CompleteOthers);
                }

                rotateUpdate = true;
            }
            isHit = true;

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


    private void StartInIdle()
    {
        isIdle = true;

        if (animations.idleAnim.Length != 0)
        {
            chosenIdle = Random.Range(0, animations.idleAnim.Length);
            bot.animation.CrossFade(animations.idleAnim[chosenIdle].name, animationParameters.idleBlendingTime,
                PlayMode.StopAll);
        }


        if (sounds.idleSfx)
        {
            audio.clip = sounds.idleSfx;
            audio.volume = soundsVolume.idleVolume;
            audio.loop = true;

            audio.Play();
        }

        if (sounds.idleSfx == null)
        {
            audio.loop = false;
        }
    }

    private IEnumerator Reload()
    {
        bot.animation.CrossFade(animations.reloadAnim.name, animationParameters.reloadBlendingTime, PlayMode.StopAll);

        if (sounds.reloadSfx)
        {
            audio.PlayOneShot(sounds.reloadSfx, soundsVolume.reloadVolume);

        }
        else
        {
            audio.Stop();
        }

        yield return new WaitForSeconds(bot.animation[animations.reloadAnim.name].length);
        reloadCount = 0;
        rangedAttacksSwitch = true;
    }

    private void GoFollow()
    {
        if (animations.followAnim.Length != 0 && !bot.animation.IsPlaying(animations.followAnim[chosenFollow].name) &&
            !nav.isOnOffMeshLink)
        {
            chosenFollow = Random.Range(0, animations.followAnim.Length);
            bot.animation.CrossFade(animations.followAnim[chosenFollow].name, animationParameters.followBlendingTime,
                PlayMode.StopAll);
        }
        if (sounds.followSfx && !playedJump)
        {
            audio.clip = sounds.followSfx;
            audio.volume = soundsVolume.followVolume;
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

    private void GetEnemyPosition(Vector3 enemyPosition)
    {
        enemyPos = enemyPosition;
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
        chosenDeath = Random.Range(0, animations.deathAnim.Length);
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
}