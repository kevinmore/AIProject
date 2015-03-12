using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class animgroup
{
    public AnimationClip[] chasingAnim;
    public AnimationClip[] deathAnim;
    public AnimationClip[] gotHitAnim;
    public AnimationClip[] idleAnim;
    public AnimationClip jumpAnim;
    public AnimationClip[] meleeAttackAnim;
    public AnimationClip rangedAttackAnim;
    public AnimationClip reloadAnim;
    public AnimationClip retreatAnim;
    public AnimationClip[] wanderingAnim;
    public AnimationClip woundedIdle;
    public AnimationClip retreatArrivalAnim;
}

[Serializable]
public class animpara
{
    public float chaseBlendingTime = 0.3f;
    public float chasingAnimSpeed = 1f;
    public float deathAnimSpeed = 1f;
    public float deathBlendingTime = 0.3f;
    public float gotHitAnimSpeed = 1f;
    public float hitBlendingTime = 0.3f;
    public float idleAnimSpeed = 1f;
    public float idleBlendingTime = 0.3f;
    public float jumpBlendTime = 0.3f;
    public float meleeAnimSpeed = 1f;
    public float meleeBlendingTime = 0.3f;
    public float rangedAnimSpeed = 1f;
    public float rangedBlendingTime = 0.3f;
    public float reloadAnimSpeed = 1f;
    public float reloadBlendingTime = 0.3f;
    public float retreatAnimSpeed = 1f;
    public float retreatBlendingTime = 0.3f;
    public float wanderAnimSpeed = 1f;
    public float wanderBlendingTime = 0.3f;
    public float woundedAnimSpeed = 1f;
    public float woundedBlendingTime = 0.3f;
    public float retreatArrivalSpeed = 1f;
    public float retreatArrivalBlend = 0.3f;
}

[Serializable]
public class soundgroup
{
    public AudioClip alertSfx;
    public AudioClip chasingSfx;
    public AudioClip detectionSfx;
    public AudioClip dieSfx;
    public AudioClip gotHitSfx;
    public AudioClip idleSfx;
    public AudioClip jumpSfx;
    public AudioClip[] meleeAttackSfx;
    public AudioClip projectileImpactSfx;
    public AudioClip rangedAttackSfx;
    public AudioClip reloadSfx;
    public AudioClip retreatSfx;
    public AudioClip wanderingSfx;
    public AudioClip woundedIdle;
}

[Serializable]
public class soundpara
{
    public float _maxDistance = 10f;
    [Range(0, 1)] public float alertVolume = 1f;
    [Range(0, 1)] public float chaseVolume = 1f;
    [Range(0, 1)] public float detectionVolume = 1f;
    [Range(0, 1)] public float dieVolume = 1f;
    [Range(0, 1)] public float gotHitVolume = 1f;
    [Range(0, 1)] public float idleVolume = 1f;
    [Range(0, 1)] public float jumpVolume = 1f;
    [Range(0, 1)] public float meleeAttackVolume = 1f;
    [Range(0, 1)] public float rangedAttackVolume = 1f;
    [Range(0, 1)] public float reloadVolume = 1f;
    [Range(0, 1)] public float retreatVolume = 1f;
    [Range(0, 1)] public float wanderingVolume = 1f;
    [Range(0, 1)] public float woundedVolume = 1f;
}

[Serializable]
public class layergroup
{
    public LayerMask mainTarget;
    public LayerMask secondTargets;
    public LayerMask viewObstruction;
}

[Serializable]
public class navmov
{
    public float acceleration = 8f;
    public float alertDistance = 12f;
    public float arrivalDistance = 1.5f;
    public float chasingSpeed = 5.0f;
    public float goIdleRadius = 20.0f;
    [Range(0, 1)] public float hearVolumeMin = 0.2f;
    public float hearingDistance = 12.0f;
    public float jumpSpeed = 2f;
    public float lookAtSpeed = 6.0f;
    public float patrolSpeed = 2.5f;
    public float retreatDistance = 10f;
    public float rotationSpeed = 140.0f;
    public Vector3 viewSphereCenter = new Vector3(0, 0, 5);
    public float viewSphereRadius = 5.0f;
    public float minIdleInterval = 2.0f;
    public float maxIdleInterval = 7.0f;
    public float wanderingRadius = 10.0f;
}

[Serializable]
public class meleeA
{
    public float[] damageAmount;
    public float attackInterval = 1.0f;
    public string damageMethodName = "SubtractHealth";
    public float damageDelay = 1f;
    public bool forceMeleeConclude;
}

[Serializable]
public class rangedA
{
    public string damageMethodName = "SubtractHealth";
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
    public float shootRange = 7.0f;
    public bool loopRangedAnimation;
    public bool canReload = true;
}

[Serializable]
public class general
{
    public string MessageAiOnDeath;
    public string MessageTargetOnDeath;
    public GameObject bloodDecalDead;
    public bool canAlert = true;
    public bool canHear = false;
    public bool canRetreat = false;
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
    public float retreatHealth = 10f;
    public Transform target;
    public Transform retreatDestination;
    public float replenishHealth = 50f;
    public bool multipleRetreats;
    public bool retreatAndStop = true;
}

public class AdvancedAiEnemy : MonoBehaviour
{
    public enum attackM
    {
        Melee,
        Ranged,
        Melee_Ranged
    }

    public enum intelMode
    {
        Stupid,
        Smart
    }

    public enum patmode
    {
        In_Place,
        Dynamic_Wandering,
        Waypoints
    }

    private bool alerted;
    public animpara animationParameters;
    public animgroup animations;
    public attackM attackMode;
    private AudioSource[] audioC;
    private AudioSource audioS;
    private Transform bot;
    private bool canMesure;
    private bool cancheck;
    private int chosenChase;
    private int chosenDie;
    private int chosenHit;
    private int chosenMelee;
    private int chosenWander;
    private float comHealth;
    private GameObject companionGo;
    private bool companionKilled;
    private Vector3 dPositionn;
    private float dist;
    private float dist2;
    private AdvancedAiEnemy[] enemies;
    private GameObject projfx;
    public general generalParameters;
    private bool goCompanion;
    private bool gosearch;
    private GameObject hitfx;
    public intelMode intelligenceMode;
    private bool isAi;
    private bool isDead;
    private bool isDetected = true;
    private bool isExited = true;
    private bool isHit;
    private bool isIdle;
    private bool isretreat;
    private Vector3 lastTargetPos;
    private Vector3 latestPos;
    private GameObject latestWp;
    public layergroup layers;
    public Color linesColor;
    private bool mainTarget = true;
    public meleeA meleeAttack;
    private bool meleeAttacksSwitch = true;
    private NavMeshAgent nav;
    public navmov navigationRanges;
    private bool onetimecheck = true;
    [HideInInspector] public Collider other;
    private NavMeshPath path;
    public patmode patrolMode;
    private bool played;
    private bool playedJump;
    private string previous;
    private GameObject pro;
    private Transform projectileOrigin;
    private Vector3 rPosition;
    public rangedA rangedAttack;
    private bool rangedAttacksSwitch = true;
    private int reloadCount;
    private bool rotateUpdate;
    private bool rotateUpdateFromDefender;
    [HideInInspector] public bool showHide = true;
    public soundgroup sounds;
    public soundpara soundsVolume;
    private bool startSwitch = true;
    private GameObject triggerChil;
    private bool wanderingSwitch = true;
    private GameObject[] wayPoints = new GameObject[1];
    private int waypointCount;
    private bool waypointSwitch = true;
    public string waypointsGroup = "WP_Group1";
    private Transform[] wptransforms;
    private bool onetimeretreat;
    private bool oneTimeArrival;
    private AdvancedAiNpcAggressive[] aggressives;

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


        if (!audio) gameObject.AddComponent("AudioSource");


        triggerChil = new GameObject();
        triggerChil.transform.parent = transform;
        triggerChil.transform.localPosition = Vector3.zero;
        triggerChil.AddComponent("SphereCollider");
        triggerChil.layer = LayerMask.NameToLayer("Ignore Raycast");
        triggerChil.AddComponent<IdleSphere>();
        triggerChil.name = "TriggerSphere";
        triggerChil.GetComponent<SphereCollider>().radius = navigationRanges.goIdleRadius;
        triggerChil.GetComponent<SphereCollider>().isTrigger = true;


        if (!GetComponent<NavMeshAgent>()) gameObject.AddComponent("NavMeshAgent");

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
            foreach (AnimationClip anim in animations.gotHitAnim)
            {
                anim.wrapMode = WrapMode.Once;
                bot.animation[anim.name].speed = animationParameters.gotHitAnimSpeed;
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

        if (animations.wanderingAnim.Length != 0)
        {
            foreach (AnimationClip wAnim in animations.wanderingAnim)
            {
                wAnim.wrapMode = WrapMode.Loop;
                bot.animation[wAnim.name].speed = animationParameters.wanderAnimSpeed;
            }
        }
        if (animations.reloadAnim)
        {
            animations.reloadAnim.wrapMode = WrapMode.Once;
            bot.animation[animations.reloadAnim.name].speed = animationParameters.reloadAnimSpeed;
        }

        if (animations.retreatAnim)
        {
            animations.retreatAnim.wrapMode = WrapMode.Loop;
            bot.animation[animations.retreatAnim.name].speed = animationParameters.retreatAnimSpeed;
        }

        if (animations.retreatArrivalAnim)
        {
            animations.retreatArrivalAnim.wrapMode = WrapMode.Once;
            bot.animation[animations.retreatArrivalAnim.name].speed = animationParameters.retreatArrivalSpeed;
        }

        if (animations.woundedIdle)
        {
            animations.woundedIdle.wrapMode = WrapMode.Loop;
            bot.animation[animations.woundedIdle.name].speed = animationParameters.woundedAnimSpeed;
        }


        nav = gameObject.GetComponent<NavMeshAgent>();
        nav.speed = navigationRanges.patrolSpeed;
        nav.angularSpeed = navigationRanges.rotationSpeed;
        nav.stoppingDistance = navigationRanges.arrivalDistance;
        nav.acceleration = navigationRanges.acceleration;

        if (generalParameters.canHear)
        {
            if (generalParameters.target.audio)
                audioS = generalParameters.target.audio;

            if (generalParameters.target.GetComponentInChildren<AudioSource>())
                audioC = generalParameters.target.GetComponentsInChildren<AudioSource>();
        }

        if (layers.mainTarget == 0)
            Debug.LogWarning("You did not assign a unique layer for the target: " +
                             generalParameters.target.gameObject.name + ", in AI: " +
                             gameObject.name);

        if (layers.viewObstruction.value == 0)
            Debug.LogWarning("You did not assign the view's obstruction layer for AI: " + gameObject.name);


        if (patrolMode == patmode.Waypoints)
        {
            Transform wp = GameObject.Find(waypointsGroup).transform;
            int wpcount = wp.childCount;
            wptransforms = new Transform[wpcount];

            for (int i = 0; i < wpcount; i++)
            {
                wptransforms[i] = wp.GetChild(i);
            }
        }

        if (rangedAttack.projectileFx)
        {
            if (rangedAttack.projectileFx.GetComponent<ParticleAnimator>())
                rangedAttack.projectileFx.GetComponent<ParticleAnimator>().autodestruct = true;
        }


        GoIdle();
    }


    private void Update()
    {
        if (!generalParameters.target) return;

        if (rotateUpdate)
        {
            Vector3 targetDir = generalParameters.target.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(targetDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation,
                Time.deltaTime*navigationRanges.lookAtSpeed);

            Vector3 forward = transform.forward;
            float angle = Vector3.Angle(targetDir, forward);
            if (angle < 20.0f)
            {
                rotateUpdate = false;
            }
        }

        if (generalParameters.canRetreat && generalParameters.healthPoints <= generalParameters.retreatHealth &&
            !isDead && !isHit && !isretreat && !onetimeretreat)
        {
            onetimeretreat = true;
            triggerChil.GetComponent<SphereCollider>().enabled = false;
            isretreat = true;
            GoRetreat();
        }

        if (nav && nav.isOnOffMeshLink)
        {
            nav.speed = navigationRanges.jumpSpeed;
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
        if (nav && !nav.isOnOffMeshLink) playedJump = false;

        if ((companionGo && isAi && !companionGo.GetComponent<AdvancedAiDefender>() &&
             !companionGo.GetComponent<AdvancedAiCompanion>() && !companionGo.GetComponent<AdvancedAiNpcPassive>()) ||
            (!isAi && !companionGo && !mainTarget))
        {
            goCompanion = false;
            companionGo = null;
            StopAllCoroutines();
            meleeAttacksSwitch = true;
            rangedAttacksSwitch = true;
            wanderingSwitch = true;
            nav.stoppingDistance = navigationRanges.arrivalDistance;
            mainTarget = true;
        }


        dist = Vector3.Distance(transform.position, generalParameters.target.position);


        if (companionGo)
            dist2 = Vector3.Distance(transform.position, companionGo.transform.position);


        if (generalParameters.healthPoints > 0 && (!waypointSwitch || !wanderingSwitch) &&
            nav.remainingDistance <= navigationRanges.arrivalDistance && !isIdle
            )
            StartCoroutine("IdleInWandering");


        if (generalParameters.healthPoints > 0 && !goCompanion &&
            layers.secondTargets != LayerMask.NameToLayer("Nothing"))
        {
            Collider[] colinfo =
                Physics.OverlapSphere(transform.TransformPoint(navigationRanges.viewSphereCenter),
                    navigationRanges.viewSphereRadius, layers.secondTargets);

            foreach (Collider colhit in colinfo)
            {
                RaycastHit hitinfo;
                if (
                    !Physics.Linecast(transform.position, colhit.gameObject.transform.position, out hitinfo,
                        layers.viewObstruction))
                {
                    goCompanion = true;
                    companionGo = colhit.gameObject;
                    rotateUpdate = false;
                    nav.stoppingDistance = 1.5f;
                    if (companionGo.GetComponent<AdvancedAiCompanion>() ||
                        companionGo.GetComponent<AdvancedAiDefender>() ||
                        companionGo.GetComponent<AdvancedAiNpcPassive>()) isAi = true;
                    else isAi = false;

                    break;
                }
            }
        }


        if (companionGo)
        {
            if (companionGo.GetComponent<AdvancedAiDefender>())
                comHealth = companionGo.GetComponent<AdvancedAiDefender>().generalParameters.healthPoints;

            else if (companionGo.GetComponent<AdvancedAiCompanion>())
                comHealth = companionGo.GetComponent<AdvancedAiCompanion>().generalParameters.healthPoints;

            else if (companionGo.GetComponent<AdvancedAiNpcPassive>())
                comHealth = companionGo.GetComponent<AdvancedAiNpcPassive>().generalParameters.healthPoints;

            if (comHealth <= 0 && nav && isAi)
            {
                goCompanion = false;
                companionGo = null;
                StopAllCoroutines();
                meleeAttacksSwitch = true;
                rangedAttacksSwitch = true;
                wanderingSwitch = true;
                nav.stoppingDistance = navigationRanges.arrivalDistance;
                mainTarget = true;
            }
        }


        if (generalParameters.healthPoints > 0 && !isretreat)
        {
            if (goCompanion)
            {
                mainTarget = false;
                wanderingSwitch = true;
                isIdle = false;
                StopCoroutine("IdleInWandering");
                if (!isHit)
                {
                    if (meleeAttack.forceMeleeConclude)
                    {
                        if (!bot.animation.IsPlaying(animations.meleeAttackAnim[chosenMelee].name))
                        {
                            nav.SetDestination(companionGo.transform.position);
                        }
                    }
                    else
                    {
                        nav.SetDestination(companionGo.transform.position);
                    }
                }

                if (!nav.isOnOffMeshLink) nav.speed = navigationRanges.chasingSpeed;

                switch (attackMode)
                {
                    case attackM.Melee:

                        if (nav.remainingDistance <= navigationRanges.arrivalDistance)
                        {
                            nav.Stop(true);
                            transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);

                            GoMelee();
                        }

                        else
                        {
                            StopCoroutine("MeleeDelay");
                            meleeAttacksSwitch = true;
                            if (meleeAttack.forceMeleeConclude)
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

                        break;

                    case attackM.Ranged:

                        if (!isHit)
                        {
                            if (animations.gotHitAnim.Length != 0)
                            {
                                if (!bot.animation.IsPlaying(animations.gotHitAnim[chosenHit].name))
                                    nav.SetDestination(companionGo.transform.position);
                            }
                        }
                        if (nav.remainingDistance <= rangedAttack.shootRange)
                        {
                            nav.Stop(true);
                            transform.rotation = new Quaternion(0, transform.rotation.y, 0,
                                transform.rotation.w);
                            GoRanged();
                        }
                        else
                        {
                            StopCoroutine("RangedDelay");
                            rangedAttacksSwitch = true;

                            if (projfx && !projfx.GetComponent<ParticleSystem>())
                                Destroy(projfx);

                            if (!isHit) nav.Resume();
                            GoChase();
                        }
                        break;

                    case attackM.Melee_Ranged:
                        if (!isHit)
                        {
                            if (animations.gotHitAnim.Length != 0)
                            {
                                if (!bot.animation.IsPlaying(animations.gotHitAnim[chosenHit].name))
                                    nav.SetDestination(companionGo.transform.position);
                            }
                        }
                        if (nav.remainingDistance <= rangedAttack.shootRange &&
                            nav.remainingDistance > navigationRanges.arrivalDistance)
                        {
                            StopCoroutine("MeleeDelay");

                            meleeAttacksSwitch = true;
                            if (meleeAttack.forceMeleeConclude)
                            {
                                if (!bot.animation.IsPlaying(animations.meleeAttackAnim[chosenMelee].name))
                                {
                                    nav.Stop(true);
                                    transform.rotation = new Quaternion(0, transform.rotation.y, 0,
                                        transform.rotation.w);
                                    GoRanged();
                                }
                            }
                            else
                            {
                                nav.Stop(true);
                                transform.rotation = new Quaternion(0, transform.rotation.y, 0,
                                    transform.rotation.w);
                                GoRanged();
                            }
                        }
                        else if (nav.remainingDistance > rangedAttack.shootRange)
                        {
                            StopAllCoroutines();
                            meleeAttacksSwitch = true;
                            rangedAttacksSwitch = true;

                            if (projfx && !projfx.GetComponent<ParticleSystem>())
                                Destroy(projfx);

                            if (!isHit) nav.Resume();
                            GoChase();
                        }
                        else if (nav.remainingDistance <= navigationRanges.arrivalDistance)
                        {
                            StopCoroutine("RangedDelay");
                            rangedAttacksSwitch = true;
                            if (projfx && !projfx.GetComponent<ParticleSystem>())
                                Destroy(projfx);
                            nav.Stop(true);
                            transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);

                            GoMelee();
                        }
                        break;
                }
            }
        }

        

        if (isDead && generalParameters.disappearOnDeath && animations.deathAnim.Length != 0 &&
            !bot.animation.IsPlaying(animations.deathAnim[chosenDie].name) &&
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
            if (isHit && !bot.animation.IsPlaying(animations.gotHitAnim[chosenHit].name) && !isDead)
            {
                isHit = false;
                rangedAttacksSwitch = true;
                meleeAttacksSwitch = true;
                wanderingSwitch = true;
                waypointSwitch = true;
                if (intelligenceMode == intelMode.Smart)
                {
                    if (!isretreat)
                    {
                        if (nav.destination != lastTargetPos) nav.SetDestination(lastTargetPos);

                        gosearch = true;
                    }
                }
                else
                {
                    rotateUpdate = true;
                }
            }
        }
        else
        {
            if (isHit && !isDead)
            {
                isHit = false;
                rangedAttacksSwitch = true;
                meleeAttacksSwitch = true;
                wanderingSwitch = true;
                waypointSwitch = true;


                if (intelligenceMode == intelMode.Smart)
                {
                    if (!isretreat)
                    {
                        if (nav.destination != lastTargetPos) nav.SetDestination(lastTargetPos);

                        gosearch = true;
                    }
                }
                else
                {
                    rotateUpdate = true;
                }
            }
        }

        if (nav && (nav.pathStatus == NavMeshPathStatus.PathInvalid || nav.pathStatus == NavMeshPathStatus.PathPartial))
        {
            wanderingSwitch = true;
        }


        if (generalParameters.healthPoints > 0)
        {
            if (rotateUpdateFromDefender)
            {
                Vector3 targetDir = dPositionn - transform.position;
                Quaternion rotation = Quaternion.LookRotation(targetDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation,
                    Time.deltaTime*navigationRanges.lookAtSpeed);

                Vector3 forward = transform.forward;
                float angle = Vector3.Angle(targetDir, forward);
                if (angle < 20.0f)
                {
                    rotateUpdateFromDefender = false;

                    nav.Stop(true);
                }
            }
        }


        if (generalParameters.healthPoints < 0) GoDeath();


        if (rangedAttack.projectileFx && projfx)
        {
            if (projfx.GetComponent<ParticleSystem>())
            {
                projfx.GetComponent<ParticleSystem>().loop = false;
                var fxp = projfx.GetComponent<ParticleSystem>();
                ParticleSystem[] fxc = fxp.GetComponentsInChildren<ParticleSystem>();
                foreach (ParticleSystem fxcc in fxc)
                {
                    fxcc.loop = false;
                }

                if (!projfx.particleSystem.IsAlive(true))
                    DestroyImmediate(projfx);
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
                    DestroyImmediate(hitfx);
            }
            else Destroy(hitfx.gameObject, 1f);
        }

        if (canMesure && !nav.pathPending)
        {
            if (nav.remainingDistance <= navigationRanges.arrivalDistance)
            {
                canMesure = false;
                if (isretreat)
                {
                    if (animations.woundedIdle)
                    {
                        animations.idleAnim = new AnimationClip[1];
                        animations.idleAnim[0] = animations.woundedIdle;
                    }

                    if (animations.retreatArrivalAnim)
                    {
                        oneTimeArrival = false;
                        bot.animation.CrossFade(animations.retreatArrivalAnim.name,
                            animationParameters.retreatArrivalBlend, PlayMode.StopAll);
                    }

                    if (sounds.woundedIdle)
                    {
                        sounds.idleSfx = sounds.woundedIdle;
                    }

                    generalParameters.healthPoints += generalParameters.replenishHealth;

                    if (generalParameters.retreatAndStop)
                        patrolMode = patmode.In_Place;

                    if (generalParameters.multipleRetreats)
                        onetimeretreat = false;

                    if (generalParameters.canAlert)
                    {
                        enemies = FindObjectsOfType(typeof (AdvancedAiEnemy)) as AdvancedAiEnemy[];
                        aggressives = FindObjectsOfType(typeof (AdvancedAiNpcAggressive)) as AdvancedAiNpcAggressive[];

                        if (!played && sounds.alertSfx && enemies.Length != 0)
                        {
                            played = true;
                            audio.PlayOneShot(sounds.alertSfx, soundsVolume.alertVolume);
                        }


                        foreach (AdvancedAiEnemy enemy in enemies)
                        {
                            if (enemy && enemy.transform != transform &&
                                Vector3.Distance(transform.position, enemy.transform.position) <=
                                navigationRanges.alertDistance)
                                enemy.AlertOnSight();
                        }

                        foreach (AdvancedAiNpcAggressive aggressive in aggressives)
                        {
                            if (aggressive && aggressive.transform != transform &&
                                Vector3.Distance(transform.position, aggressive.transform.position) <=
                                navigationRanges.alertDistance)
                                aggressive.AlertOnSight();
                        }
                    }

                    wanderingSwitch = true;
                    rangedAttacksSwitch = true;
                    meleeAttacksSwitch = true;
                    waypointSwitch = true;
                    cancheck = false;
                    gosearch = false;
                    isretreat = false;
                    triggerChil.GetComponent<SphereCollider>().enabled = true;
                    if (!animations.retreatArrivalAnim) GoIdle();
                }
            }
        }


        if (isExited && intelligenceMode == intelMode.Smart && ((gosearch && !isHit) || (cancheck && !isHit)))
        {
            if (!gosearch && onetimecheck)
            {
                onetimecheck = false;
                nav.SetDestination(generalParameters.target.position);
            }

            GoSearch();
        }

        if (animations.retreatArrivalAnim && !bot.animation.IsPlaying(animations.retreatArrivalAnim.name) &&
            !oneTimeArrival)
        {
            oneTimeArrival = true;
            GoIdle();
        }
    }


    public void TriggerExit()
    {
        if (generalParameters.healthPoints > 0 && other.transform == generalParameters.target)
        {
            isExited = true;
            GoIdle();
            nav.Stop(true);
            wanderingSwitch = true;
        }
    }

    public void TriggerStay()
    {
        if (generalParameters.healthPoints > 0 && mainTarget && other.transform == generalParameters.target)
        {
            if (startSwitch)
            {
                startSwitch = false;
                audio.Stop();
            }
            isExited = false;
            Collider[] colinfo =
                Physics.OverlapSphere(transform.TransformPoint(navigationRanges.viewSphereCenter),
                    navigationRanges.viewSphereRadius, layers.mainTarget);

            foreach (Collider colhit in colinfo)
            {
                if (colhit.transform == generalParameters.target)
                {
                    RaycastHit hitinfo;
                    if (
                        !Physics.Linecast(transform.position, generalParameters.target.position, out hitinfo,
                            layers.viewObstruction))
                    {
                        waypointSwitch = true;
                        wanderingSwitch = true;
                        waypointCount = 0;
                        isIdle = false;
                        gosearch = false;
                        StopCoroutine("IdleInWandering");
                        cancheck = true;
                        onetimecheck = true;
                        rotateUpdate = false;
                        if (generalParameters.canAlert)
                        {
                            enemies = FindObjectsOfType(typeof (AdvancedAiEnemy)) as AdvancedAiEnemy[];
                            aggressives =
                                FindObjectsOfType(typeof (AdvancedAiNpcAggressive)) as AdvancedAiNpcAggressive[];

                            if (!played && sounds.alertSfx && enemies.Length != 0)
                            {
                                played = true;
                                audio.PlayOneShot(sounds.alertSfx, soundsVolume.alertVolume);
                            }

                            foreach (AdvancedAiEnemy enemy in enemies)
                            {
                                if (enemy && enemy.transform != transform &&
                                    Vector3.Distance(transform.position, enemy.transform.position) <=
                                    navigationRanges.alertDistance)
                                    enemy.AlertOnSight();
                            }

                            foreach (AdvancedAiNpcAggressive aggressive in aggressives)
                            {
                                if (aggressive && aggressive.transform != transform &&
                                    Vector3.Distance(transform.position, aggressive.transform.position) <=
                                    navigationRanges.alertDistance)
                                    aggressive.AlertOnSight();
                            }
                        }

                        if (!isHit)
                        {
                            if (meleeAttack.forceMeleeConclude)
                            {
                                if (!bot.animation.IsPlaying(animations.meleeAttackAnim[chosenMelee].name))
                                {
                                    nav.SetDestination(generalParameters.target.transform.position);
                                }
                            }
                            else
                            {
                                nav.SetDestination(generalParameters.target.transform.position);
                            }
                        }

                        if (!nav.isOnOffMeshLink) nav.speed = navigationRanges.chasingSpeed;

                        switch (attackMode)
                        {
                            case attackM.Melee:

                                if (nav.remainingDistance <= navigationRanges.arrivalDistance)
                                {
                                    nav.Stop(true);
                                    transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);

                                    GoMelee();
                                }

                                else
                                {
                                    StopCoroutine("MeleeDelay");
                                    meleeAttacksSwitch = true;
                                    if (meleeAttack.forceMeleeConclude)
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

                                break;

                            case attackM.Ranged:

                                if (!isHit)
                                {
                                    if (animations.gotHitAnim.Length != 0)
                                    {
                                        if (!bot.animation.IsPlaying(animations.gotHitAnim[chosenHit].name))
                                            nav.SetDestination(generalParameters.target.position);
                                    }
                                }
                                if (nav.remainingDistance <= rangedAttack.shootRange)
                                {
                                    nav.Stop(true);
                                    transform.rotation = new Quaternion(0, transform.rotation.y, 0,
                                        transform.rotation.w);
                                    GoRanged();
                                }
                                else
                                {
                                    StopCoroutine("RangedDelay");
                                    rangedAttacksSwitch = true;

                                    if (projfx && !projfx.GetComponent<ParticleSystem>())
                                        Destroy(projfx);

                                    if (!isHit) nav.Resume();
                                    GoChase();
                                }
                                break;

                            case attackM.Melee_Ranged:
                                if (!isHit)
                                {
                                    if (animations.gotHitAnim.Length != 0)
                                    {
                                        if (!bot.animation.IsPlaying(animations.gotHitAnim[chosenHit].name))
                                            nav.SetDestination(generalParameters.target.position);
                                    }
                                }
                                if (nav.remainingDistance <= rangedAttack.shootRange &&
                                    nav.remainingDistance > navigationRanges.arrivalDistance)
                                {
                                    StopCoroutine("MeleeDelay");

                                    meleeAttacksSwitch = true;
                                    if (meleeAttack.forceMeleeConclude)
                                    {
                                        if (!bot.animation.IsPlaying(animations.meleeAttackAnim[chosenMelee].name))
                                        {
                                            nav.Stop(true);
                                            transform.rotation = new Quaternion(0, transform.rotation.y, 0,
                                                transform.rotation.w);
                                            GoRanged();
                                        }
                                    }
                                    else
                                    {
                                        nav.Stop(true);
                                        transform.rotation = new Quaternion(0, transform.rotation.y, 0,
                                            transform.rotation.w);
                                        GoRanged();
                                    }
                                }
                                else if (nav.remainingDistance > rangedAttack.shootRange)
                                {
                                    StopAllCoroutines();
                                    meleeAttacksSwitch = true;
                                    rangedAttacksSwitch = true;

                                    if (projfx && !projfx.GetComponent<ParticleSystem>())
                                        Destroy(projfx);

                                    if (!isHit) nav.Resume();
                                    GoChase();
                                }
                                else if (nav.remainingDistance <= navigationRanges.arrivalDistance)
                                {
                                    StopCoroutine("RangedDelay");
                                    rangedAttacksSwitch = true;
                                    if (projfx && !projfx.GetComponent<ParticleSystem>())
                                        Destroy(projfx);
                                    nav.Stop(true);
                                    transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);

                                    GoMelee();
                                }
                                break;
                        }
                    }

                    else
                    {
                        if (projfx && !projfx.GetComponent<ParticleSystem>())
                            Destroy(projfx);

                        StopCoroutine("MeleeDelay");
                        StopCoroutine("RangedDelay");
                        isDetected = true;
                        rangedAttacksSwitch = true;
                        meleeAttacksSwitch = true;
                        played = false;

                        if (intelligenceMode == intelMode.Smart && (gosearch || cancheck))
                        {
                            if (!isHit)
                            {
                                if (!gosearch && onetimecheck)
                                {
                                    onetimecheck = false;
                                    nav.SetDestination(generalParameters.target.position);
                                }

                                GoSearch();
                            }
                        }

                        else
                        {
                            switch (patrolMode)
                            {
                                case patmode.Dynamic_Wandering:
                                    if (wanderingSwitch)
                                    {
                                        if (!bot.animation.IsPlaying(animations.gotHitAnim[chosenHit].name))
                                        {
                                            nav.speed = navigationRanges.patrolSpeed;
                                            Wandering();
                                        }
                                    }
                                    break;
                                case patmode.In_Place:
                                    GoIdle();
                                    break;
                                case patmode.Waypoints:
                                    if (waypointSwitch)
                                    {
                                        if (!bot.animation.IsPlaying(animations.gotHitAnim[chosenHit].name))
                                        {
                                            nav.speed = navigationRanges.patrolSpeed;
                                            PatrolWayPoint();
                                        }
                                    }
                                    break;
                            }
                            if (generalParameters.canHear)
                                Hearing();
                        }
                    }
                }
            }

            if (colinfo.Length == 0)
            {
                if (projfx && !projfx.GetComponent<ParticleSystem>()) Destroy(projfx);


                StopCoroutine("MeleeDelay");
                StopCoroutine("RangedDelay");
                isDetected = true;
                rangedAttacksSwitch = true;
                meleeAttacksSwitch = true;
                played = false;

                if (intelligenceMode == intelMode.Smart && (gosearch || cancheck))
                {
                    if (!isHit)
                    {
                        if (!gosearch && onetimecheck)
                        {
                            onetimecheck = false;
                            nav.SetDestination(generalParameters.target.position);
                        }
                        GoSearch();
                    }
                }

                else
                {
                    switch (patrolMode)
                    {
                        case patmode.Dynamic_Wandering:
                            if (wanderingSwitch)
                            {
                                if (animations.gotHitAnim.Length != 0)
                                {
                                    if (!bot.animation.IsPlaying(animations.gotHitAnim[chosenHit].name))
                                    {
                                        nav.speed = navigationRanges.patrolSpeed;
                                        Wandering();
                                    }
                                }
                                else
                                {
                                    nav.speed = navigationRanges.patrolSpeed;
                                    Wandering();
                                }
                            }
                            break;
                        case patmode.In_Place:
                            GoIdle();
                            break;
                        case patmode.Waypoints:
                            if (waypointSwitch)
                            {
                                if (!bot.animation.IsPlaying(animations.gotHitAnim[chosenHit].name))
                                {
                                    nav.speed = navigationRanges.patrolSpeed;
                                    PatrolWayPoint();
                                }
                            }
                            break;
                    }
                    if (generalParameters.canHear)
                        Hearing();
                }
            }
        }
    }

    private void Wandering()
    {
        wanderingSwitch = false;
        rPosition = new Vector3(
            transform.position.x + (Random.Range(-1.0f, 1.0f)*navigationRanges.wanderingRadius),
            transform.position.y,
            transform.position.z + (Random.Range(-1.0f, 1.0f)*navigationRanges.wanderingRadius));


        path = new NavMeshPath();

        bool canit;
        canit = NavMesh.CalculatePath(transform.position, rPosition, 1, path);
        if (canit)
        {
            isIdle = false;
            nav.SetDestination(rPosition);
            nav.Resume();
            GoWanderAnim();
        }
        else wanderingSwitch = true;
    }


    private void GoDeath
        ()
    {
        if (generalParameters.MessageTargetOnDeath != "")
            generalParameters.target.BroadcastMessage(generalParameters.MessageTargetOnDeath,
                SendMessageOptions.DontRequireReceiver);
        if (generalParameters.MessageAiOnDeath != "")
            gameObject.BroadcastMessage(generalParameters.MessageAiOnDeath, SendMessageOptions.DontRequireReceiver);

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


        if (projfx && !projfx.GetComponent<ParticleSystem>()) Destroy(projfx);


        if (generalParameters.ragdollifyOnDeath)
        {
            if (generalParameters.ragdollPlayDieAnim && animations.deathAnim.Length != 0) GoRagdoll();

            else GoRagdollDirect();
        }
        else
        {
            if (animations.deathAnim.Length != 0)
            {
                chosenDie = Random.Range(0, animations.deathAnim.Length);
                bot.animation.CrossFade(animations.deathAnim[chosenDie].name, animationParameters.deathBlendingTime,
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

                Destroy(GetComponent<AdvancedAiEnemy>());
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

    private void GoMelee
        ()
    {
        if (mainTarget)
        {
            if (dist <= (navigationRanges.arrivalDistance*2))
            {
                if (meleeAttacksSwitch)
                {
                    meleeAttacksSwitch = false;

                    transform.LookAt(generalParameters.target.transform);


                    StartCoroutine("MeleeDelay");
                }
            }
        }
        else
        {
            if (dist2 <= (3))
            {
                if (meleeAttacksSwitch)
                {
                    meleeAttacksSwitch = false;
                    transform.LookAt(companionGo.transform);


                    StartCoroutine("MeleeDelay");
                }
            }
        }
    }

    private void GoChase
        ()
    {
        if (animations.chasingAnim.Length != 0 && !nav.isOnOffMeshLink)
        {
            if (animations.gotHitAnim.Length != 0)
            {
                if (!bot.animation.IsPlaying(animations.gotHitAnim[chosenHit].name))
                {
                    chosenChase = Random.Range(0, animations.chasingAnim.Length);
                    bot.animation.CrossFade(animations.chasingAnim[chosenChase].name,
                        animationParameters.chaseBlendingTime,
                        PlayMode.StopAll);
                }
            }
            else
            {
                chosenChase = Random.Range(0, animations.chasingAnim.Length);
                bot.animation.CrossFade(animations.chasingAnim[chosenChase].name, animationParameters.chaseBlendingTime,
                    PlayMode.StopAll);
            }
        }

        if (!isretreat)
        {
            if (audio.clip == sounds.wanderingSfx) audio.loop = false;
            if (sounds.chasingSfx)
            {
                if (!audio.isPlaying && !isHit && !playedJump)
                {
                    audio.loop = true;
                    audio.clip = sounds.chasingSfx;
                    audio.volume = soundsVolume.chaseVolume;
                    audio.Play();
                }
            }
            else if (!sounds.detectionSfx) audio.loop = false;


            if (isDetected && sounds.detectionSfx && !isHit)
            {
                audio.PlayOneShot(sounds.detectionSfx, soundsVolume.detectionVolume);
                isDetected = false;
            }
        }
    }

    private void GoWanderAnim
        ()
    {
        if (animations.wanderingAnim.Length != 0)
        {
            chosenWander = Random.Range(0, animations.wanderingAnim.Length);
            bot.animation.CrossFade(animations.wanderingAnim[chosenWander].name, animationParameters.wanderBlendingTime,
                PlayMode.StopAll);
        }
        if (sounds.wanderingSfx)
        {
            if (audio.clip == sounds.chasingSfx) audio.Stop();

            if (!audio.isPlaying && !playedJump)
            {
                audio.loop = true;
                audio.clip = sounds.wanderingSfx;
                audio.volume = soundsVolume.wanderingVolume;
                audio.Play();
            }
        }
        else audio.Stop();
    }

    private void GoRanged
        ()
    {
        if (mainTarget)
        {
            Vector3 dir = generalParameters.target.position - transform.position;
            Quaternion tarpos = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, tarpos,
                Time.deltaTime*navigationRanges.lookAtSpeed);
        }
        else
        {
            Vector3 dir = companionGo.transform.position - transform.position;
            Quaternion tarpos = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, tarpos,
                Time.deltaTime*navigationRanges.lookAtSpeed);
        }

        if (rangedAttack.loopRangedAnimation && sounds.rangedAttackSfx && !audio.isPlaying)
        {
            audio.loop = true;
            audio.clip = sounds.rangedAttackSfx;
            audio.Play();
        }

        if (rangedAttacksSwitch && !isHit)
        {
            if (projfx && !projfx.GetComponent<ParticleSystem>()) Destroy(projfx);

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
        if (animations.wanderingAnim.Length != 0)
            bot.animation.Stop(animations.wanderingAnim[chosenWander].name);

        if (animations.idleAnim.Length != 0)
        {
            foreach (AnimationClip anim in animations.idleAnim)
            {
                bot.animation.Stop(anim.name);
            }
        }
        if (bot.animation.IsPlaying(animations.chasingAnim[chosenChase].name))
            bot.animation.Stop(animations.chasingAnim[chosenChase].name);


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
        else audio.Stop();


        if (rangedAttack.projectilePrefab)
        {
            pro =
                Instantiate(rangedAttack.projectilePrefab, projectileOrigin.position, projectileOrigin.rotation) as
                    GameObject;

            if (!pro.GetComponent<Projectile>())
                pro.AddComponent("Projectile");

            if (!pro.rigidbody)
                pro.AddComponent("Rigidbody");

            Physics.IgnoreCollision(collider, pro.collider);

            pro.rigidbody.useGravity = false;
            var procomponent = pro.GetComponent<Projectile>();

            procomponent.playerTarget = generalParameters.target.gameObject;
            procomponent.damage = rangedAttack.projectileDamage;
            procomponent.destructionTime = rangedAttack.projectileDestroyDelay;
            procomponent.damageMethodName = rangedAttack.damageMethodName;


            if (rangedAttack.projectileFx && !projfx)
            {
                projfx =
                    Instantiate(rangedAttack.projectileFx, projectileOrigin.position,
                        projectileOrigin.rotation)
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
            if (mainTarget)
            {
                if (generalParameters.target.position.y > pro.transform.position.y*1.3 ||
                    generalParameters.target.position.y*2 < pro.transform.position.y)
                    pro.transform.LookAt(generalParameters.target);
            }
            else
            {
                if (companionGo.transform.position.y > pro.transform.position.y*1.3 ||
                    companionGo.transform.position.y*2 < pro.transform.position.y)
                    pro.transform.LookAt(companionGo.transform);
            }

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
                if (animations.idleAnim.Length != 0)
                {
                    int chosenidle = Random.Range(0, animations.idleAnim.Length);
                    bot.animation.CrossFadeQueued(animations.idleAnim[chosenidle].name,
                        animationParameters.idleBlendingTime,
                        QueueMode.CompleteOthers);
                }
            }

            if (sounds.chasingSfx) audio.loop = false;

            if (sounds.meleeAttackSfx.Length != 0)
                audio.PlayOneShot(sounds.meleeAttackSfx[chosenMelee], soundsVolume.meleeAttackVolume);


            else if (!sounds.detectionSfx) audio.loop = false;

            if (isDetected && sounds.detectionSfx && !isHit)
            {
                isDetected = false;
                audio.PlayOneShot(sounds.detectionSfx, soundsVolume.detectionVolume);
            }

            yield return new WaitForSeconds(meleeAttack.damageDelay);
            if (nav.remainingDistance <= navigationRanges.arrivalDistance)
            {
                if (mainTarget || (!mainTarget && !isAi))
                {
                    if (mainTarget)
                        generalParameters.target.SendMessage(meleeAttack.damageMethodName,
                            meleeAttack.damageAmount[chosenMelee],
                            SendMessageOptions.DontRequireReceiver);
                    else
                    {
                        companionGo.SendMessage(meleeAttack.damageMethodName,
                            meleeAttack.damageAmount[chosenMelee],
                            SendMessageOptions.DontRequireReceiver);
                    }
                }


                else if (!mainTarget && isAi)
                {
                    companionGo.SendMessage("GetEnemyPosition", transform.position,
                        SendMessageOptions.DontRequireReceiver);
                    companionGo.SendMessage("GotHit", meleeAttack.damageAmount[chosenMelee],
                        SendMessageOptions.DontRequireReceiver);
                }
            }
            yield return new WaitForSeconds(meleeAttack.attackInterval);
            meleeAttacksSwitch = true;
        }
        else
        {
            if (sounds.meleeAttackSfx.Length != 0)
            {
                if (sounds.chasingSfx)
                    audio.loop = false;
                audio.PlayOneShot(sounds.meleeAttackSfx[chosenMelee], soundsVolume.meleeAttackVolume);
            }
            else
            {
                audio.Stop();
            }
            if (isDetected && sounds.detectionSfx && !isHit)
            {
                isDetected = false;
                audio.PlayOneShot(sounds.detectionSfx, soundsVolume.detectionVolume);
            }
            if (mainTarget && nav.remainingDistance <= navigationRanges.arrivalDistance)
                generalParameters.target.SendMessage(meleeAttack.damageMethodName,
                    meleeAttack.damageAmount[
                        Random.Range(0, meleeAttack.damageAmount.Length)],
                    SendMessageOptions.DontRequireReceiver);


            if (!mainTarget && nav.remainingDistance <= navigationRanges.arrivalDistance)
            {
                companionGo.SendMessage("GetEnemyPosition", transform.position, SendMessageOptions.DontRequireReceiver);
                companionGo.SendMessage("GotHit",
                    meleeAttack.damageAmount[
                        Random.Range(0, meleeAttack.damageAmount.Length)],
                    SendMessageOptions.DontRequireReceiver);
            }

            yield return new WaitForSeconds(meleeAttack.attackInterval);
            meleeAttacksSwitch = true;
        }
    }

    private void OnDrawGizmos
        ()
    {
        if (!wanderingSwitch)
        {
            Gizmos.DrawCube(rPosition, Vector3.one);
        }
        linesColor = new Color(linesColor.r, linesColor.g, linesColor.b, 255f);
        Gizmos.color = linesColor;
        if (showHide && patrolMode == patmode.Waypoints && GameObject.Find(waypointsGroup))
        {
            int WpC = GameObject.Find(waypointsGroup).transform.childCount;
            for (int t = 0; t < WpC; t++)
            {
                Gizmos.DrawIcon(
                    GameObject.Find(waypointsGroup).transform.GetChild(t).gameObject.transform.position,
                    "WP.png",
                    true);
                if (t < (WpC - 1))
                {
                    Gizmos.DrawLine(
                        GameObject.Find(waypointsGroup).transform.GetChild(t).gameObject.transform.position,
                        GameObject.Find(waypointsGroup)
                            .transform.GetChild(t + 1)
                            .gameObject.transform.position);
                }
                else if (t == (WpC - 1))
                {
                    Gizmos.DrawLine(
                        GameObject.Find(waypointsGroup).transform.GetChild(t).gameObject.transform.position,
                        GameObject.Find(waypointsGroup)
                            .transform.GetChild(0)
                            .gameObject.transform.position);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.TransformPoint(navigationRanges.viewSphereCenter),
            navigationRanges.viewSphereRadius);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, navigationRanges.goIdleRadius);
        if (generalParameters.canHear)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, navigationRanges.hearingDistance);
        }
        if (patrolMode == patmode.Dynamic_Wandering)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, navigationRanges.wanderingRadius);
        }
    }

    private void Hearing()
    {
        if (audioS.isPlaying && dist <= navigationRanges.hearingDistance &&
            audioS.volume >= navigationRanges.hearVolumeMin)
        {
            lastTargetPos = generalParameters.target.position;
            nav.SetDestination(lastTargetPos);
            gosearch = true;
            return;
        }

        if (audioC.Length != 0 && dist <= navigationRanges.hearingDistance)
        {
            foreach (AudioSource asou in audioC)
            {
                if (asou.isPlaying && asou.volume >= navigationRanges.hearVolumeMin)
                {
                    lastTargetPos = generalParameters.target.position;
                    nav.SetDestination(lastTargetPos);
                    gosearch = true;
                    break;
                }
            }
        }
    }

    private void GotHit(float healthAmount)
    {
        if (generalParameters.healthPoints > 0)
        {
            nav.Stop(true);


            if (projfx && !projfx.GetComponent<ParticleSystem>()) Destroy(projfx);

            StopAllCoroutines();


            lastTargetPos = generalParameters.target.position;


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
                    int chosenidle = Random.Range(0, animations.idleAnim.Length);
                    bot.animation.CrossFadeQueued(animations.idleAnim[chosenidle].name,
                        animationParameters.idleBlendingTime,
                        QueueMode.CompleteOthers);
                }
            }


            HitParticleEffect();
            AlertOtherEnemies();
            isHit = true;
        }
    }

    private void AlertOtherEnemies()
    {
        if (generalParameters.canAlert)
        {
            enemies = FindObjectsOfType(typeof (AdvancedAiEnemy)) as AdvancedAiEnemy[];
            aggressives = FindObjectsOfType(typeof (AdvancedAiNpcAggressive)) as AdvancedAiNpcAggressive[];

            if (sounds.alertSfx && enemies.Length != 0)
                audio.PlayOneShot(sounds.alertSfx, soundsVolume.alertVolume);


            foreach (AdvancedAiEnemy enemy in enemies)
            {
                if (enemy && enemy.transform != transform &&
                    Vector3.Distance(transform.position, enemy.transform.position) <=
                    navigationRanges.alertDistance)
                    enemy.AlertOnHit(transform);
            }

            foreach (AdvancedAiNpcAggressive aggressive in aggressives)
            {
                if (aggressive && aggressive.transform != transform &&
                    Vector3.Distance(transform.position, aggressive.transform.position) <=
                    navigationRanges.alertDistance)
                    aggressive.AlertOnSight();
            }
        }
    }

    private void GotHitFromDefender(float healthAmount)
    {
        if (generalParameters.healthPoints > 0)
        {
            nav.Stop(true);

            if (projfx)
            {
                if (!projfx.GetComponent<ParticleSystem>())
                {
                    Destroy(projfx);
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
            if (!wanderingSwitch)
            {
                StopCoroutine("IdleInWandering");

                rotateUpdateFromDefender = true;
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
            if (sounds.gotHitSfx)
            {
                audio.clip = sounds.gotHitSfx;
                audio.volume = soundsVolume.gotHitVolume;
                audio.loop = false;

                audio.Play();
            }
            if (isExited || isIdle)
            {
                rotateUpdateFromDefender = true;
            }

            HitParticleEffect();
            AlertOtherEnemies();
            isHit = true;
        }
    }

    private void GetPosition(Vector3 dPosition)
    {
        dPositionn = dPosition;
    }

    private void HitParticleEffect()
    {
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

    private IEnumerator IdleInWandering()
    {
        GoIdle();
        float waitingtime = Random.Range(navigationRanges.minIdleInterval, navigationRanges.maxIdleInterval);

        if (waitingtime > 0)
        {
            if (sounds.idleSfx)
            {
                audio.loop = true;
                audio.clip = sounds.idleSfx;
                audio.volume = soundsVolume.idleVolume;
                if (!audio.isPlaying)
                {
                    audio.Play();
                }
            }

            else
            {
                audio.loop = false;
            }
        }

        yield return new WaitForSeconds(waitingtime);

        if (waitingtime > 0)
            audio.Stop();

        wanderingSwitch = true;
        if (!waypointSwitch)
        {
            if (waypointCount == (wptransforms.Length - 1))
            {
                waypointCount = 0;
            }
            else
            {
                waypointCount++;
            }
            waypointSwitch = true;
            isIdle = false;
        }
    }

    private void GoIdle()
    {
        if (isIdle) return;
        isIdle = true;
        if (animations.gotHitAnim.Length != 0)
        {
            if (!bot.animation.IsPlaying(animations.gotHitAnim[chosenHit].name) && animations.rangedAttackAnim &&
                !bot.animation.IsPlaying(animations.rangedAttackAnim.name))
            {
                if (animations.idleAnim.Length != 0)
                {
                    int chosenidle = Random.Range(0, animations.idleAnim.Length);
                    bot.animation.CrossFade(animations.idleAnim[chosenidle].name, animationParameters.idleBlendingTime,
                        PlayMode.StopAll);
                }
            }

            if (!bot.animation.IsPlaying(animations.gotHitAnim[chosenHit].name) && !animations.rangedAttackAnim)
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
            if (animations.rangedAttackAnim && !bot.animation.IsPlaying(animations.rangedAttackAnim.name))
            {
                if (animations.idleAnim.Length != 0)
                {
                    int chosenidle = Random.Range(0, animations.idleAnim.Length);
                    bot.animation.CrossFade(animations.idleAnim[chosenidle].name, animationParameters.idleBlendingTime,
                        PlayMode.StopAll);
                }
            }

            if (!animations.rangedAttackAnim)
            {
                if (animations.idleAnim.Length != 0)
                {
                    int chosenidle = Random.Range(0, animations.idleAnim.Length);
                    bot.animation.CrossFade(animations.idleAnim[chosenidle].name, animationParameters.idleBlendingTime,
                        PlayMode.StopAll);
                }
            }
            if (!wanderingSwitch)
            {
                if (sounds.idleSfx)
                {
                    audio.loop = true;
                    audio.clip = sounds.idleSfx;
                    audio.volume = soundsVolume.idleVolume;
                    if (!audio.isPlaying)
                    {
                        audio.Play();
                    }
                }
                else audio.loop = false;
            }
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

    private void AddWay()
    {
        if (patrolMode == patmode.Waypoints)
        {
            previous = waypointsGroup;

            if (!GameObject.Find(waypointsGroup))
            {
                new GameObject(waypointsGroup);
            }
            wayPoints[wayPoints.Length - 1] = new GameObject("WP" + (wayPoints.Length - 1));


            if ((wayPoints.Length - 1) == 0)
            {
                wayPoints[wayPoints.Length - 1].transform.position = transform.TransformPoint(0, 0, 1);
            }
            else
            {
                if (latestWp)
                    wayPoints[wayPoints.Length - 1].transform.position = latestWp.transform.TransformPoint(0, 0, 1);
                else
                {
                    wayPoints[wayPoints.Length - 1].transform.position = latestPos;
                }
            }
            wayPoints[wayPoints.Length - 1].transform.parent = GameObject.Find(waypointsGroup).transform;
            latestWp = wayPoints[wayPoints.Length - 1];
            latestPos = latestWp.transform.position;
            wayPoints = new GameObject[wayPoints.Length + 1];
        }
        else
        {
            Debug.LogWarning("You have to be in Static_Waypoints mode first!");
        }
    }

    private void DeleteWP()
    {
        if (patrolMode == patmode.Waypoints)
        {
            string nam = "WP" + (wayPoints.Length - 2);
            Transform wp = GameObject.Find(waypointsGroup).transform;
            Transform latestwp = wp.FindChild(nam);
            DestroyImmediate(latestwp.gameObject);
            latestWp = wayPoints[wayPoints.Length - 3];

            wayPoints = new GameObject[wayPoints.Length - 1];
        }
    }

    private void IniWP()
    {
        if (patrolMode == patmode.Waypoints)
        {
            DestroyImmediate(GameObject.Find(waypointsGroup));
            wayPoints = new GameObject[1];
        }
    }

    private void ShowHide()
    {
        if (patrolMode == patmode.Waypoints)
        {
            showHide = !showHide;
        }
    }

    private void PatrolWayPoint()
    {
        waypointSwitch = false;
        isIdle = false;
        nav.SetDestination(wptransforms[waypointCount].position);
        nav.Resume();
        GoWanderAnim();
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
        chosenDie = Random.Range(0, animations.deathAnim.Length);

        AnimationClip deathi = bot.animation.GetClip(animations.deathAnim[chosenDie].name);
        ani.AddClip(deathi, "DeathAnimation");
        ani.CrossFade("DeathAnimation", animationParameters.deathBlendingTime, PlayMode.StopAll);

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


    private void ChangeName()
    {
        if (GameObject.Find(previous))
        {
            GameObject.Find(previous).name = waypointsGroup;
        }
    }

    private void Damage(float hp)
    {
        GotHit(hp);
    }

    private void GoSearch()
    {
        isIdle = false;

        nav.Resume();

        nav.speed = navigationRanges.chasingSpeed;

        GoChase();

        if (
            nav.hasPath && nav.remainingDistance <= navigationRanges.arrivalDistance && !nav.pathPending)
        {
            if (!alerted)
            {
                cancheck = false;
                gosearch = false;
                if (patrolMode == patmode.In_Place && intelligenceMode == intelMode.Smart)
                    patrolMode = patmode.Dynamic_Wandering;
            }
            else
            {
                alerted = false;
                lastTargetPos = generalParameters.target.position;
                nav.SetDestination(lastTargetPos);
            }
        }
    }

    public void AlertOnHit(Transform hitenemy)
    {
        lastTargetPos = hitenemy.position;
        if (nav) nav.SetDestination(lastTargetPos);

        alerted = true;
        gosearch = true;
    }

    public void AlertOnSight()
    {
        if (animations.gotHitAnim.Length != 0)
        {
            if (!bot.animation.IsPlaying(animations.gotHitAnim[chosenHit].name))
            {
                lastTargetPos = generalParameters.target.position;
                if (nav) nav.SetDestination(lastTargetPos);
                gosearch = true;
            }
        }
        else
        {
            lastTargetPos = generalParameters.target.position;
            if (nav) nav.SetDestination(lastTargetPos);
            gosearch = true;
        }
    }

    private void GoRetreat
        ()
    {
        isIdle = false;
        if (!generalParameters.retreatDestination)
        {
            int prop = Random.Range(0, 8);
            switch (prop)
            {
                case 0:
                    rPosition = transform.TransformPoint(0, 0, -navigationRanges.retreatDistance);
                    break;
                case 1:
                    rPosition = transform.TransformPoint(-navigationRanges.retreatDistance, 0, 0);
                    break;
                case 2:
                    rPosition = transform.TransformPoint(navigationRanges.retreatDistance, 0, 0);
                    break;
                case 3:
                    rPosition = transform.TransformPoint(navigationRanges.retreatDistance, 0,
                        navigationRanges.retreatDistance);
                    break;
                case 4:
                    rPosition = transform.TransformPoint(navigationRanges.retreatDistance, 0,
                        -navigationRanges.retreatDistance);
                    break;
                case 5:
                    rPosition = transform.TransformPoint(-navigationRanges.retreatDistance, 0,
                        navigationRanges.retreatDistance);
                    break;
                case 6:
                    rPosition = transform.TransformPoint(-navigationRanges.retreatDistance, 0,
                        -navigationRanges.retreatDistance);
                    break;
            }

            path = new NavMeshPath();

            bool canit = NavMesh.CalculatePath(transform.position, rPosition, 1, path);
            if (canit)
            {
                nav.SetDestination(rPosition);
                nav.Resume();
                canMesure = true;
                RetreatAnim();
            }
            else GoRetreat();
        }
        else
        {
            nav.SetDestination(generalParameters.retreatDestination.position);
            nav.Resume();
            canMesure = true;
            RetreatAnim();
        }
    }

    private void RetreatAnim()
    {
        if (animations.retreatAnim)
        {
            if (!nav.isOnOffMeshLink)
            {
                if (animations.gotHitAnim.Length != 0)
                {
                    if (!bot.animation.IsPlaying(animations.gotHitAnim[chosenHit].name))
                    {
                        bot.animation.CrossFade(animations.retreatAnim.name, animationParameters.retreatBlendingTime,
                            PlayMode.StopAll);
                    }
                }
                else
                {
                    bot.animation.CrossFade(animations.retreatAnim.name, animationParameters.retreatBlendingTime,
                        PlayMode.StopAll);
                }
            }
        }
        else GoChase();


        if (audio.clip == sounds.wanderingSfx) audio.loop = false;


        if (sounds.retreatSfx)
        {
            if (!audio.isPlaying && !isHit && !playedJump)
            {
                audio.loop = true;
                audio.clip = sounds.retreatSfx;
                audio.volume = soundsVolume.retreatVolume;
                audio.Play();
            }
        }
        else if (sounds.chasingSfx)
        {
            if (!audio.isPlaying && !isHit && !playedJump)
            {
                audio.loop = true;
                audio.clip = sounds.chasingSfx;
                audio.volume = soundsVolume.chaseVolume;
                audio.Play();
            }
        }
        else if (!sounds.detectionSfx) audio.loop = false;
    }
}