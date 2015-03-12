using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class animogroup
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
public class soundvol
{
    [Range(0, 1)] public float AlertVolume = 1f;
    public float _maxDistance = 10f;
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
public class animparamo
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
    public float wanderBlendingTime = 0.3f;
    public float wanderingAnimSpeed = 1f;
    public float woundedAnimSpeed = 1f;
    public float woundedBlendingTime = 0.3f;
    public float retreatArrivalSpeed = 1f;
    public float retreatArrivalBlend = 0.3f;
}

[Serializable]
public class songroup
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
public class laye
{
    public LayerMask mainTarget;
    public LayerMask viewObstruction;
}

[Serializable]
public class genpara
{
    public GameObject bloodDecalDead;
    public bool canAlert = true;
    public bool canHear = false;
    public bool canRetreat;
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

[Serializable]
public class navigmovi
{
    public float acceleration = 8f;
    public float alertDistance = 12f;
    public float arrivalDistance = 1.0f;
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
public class meleee
{
    public float[] damageAmount;
    public float attackInterval = 1f;
    public string damageMethodName = "SubtractHealth";
    public float damageDelay = 1f;
    public bool forceMeleeConclude;
}

[Serializable]
public class rang
{
    public string damageMethodName = "SubtractHealth";
    public float decalDestroyDelay = 10.0f;
    public float projImpactFxDestTime = 2.0f;
    public float projectileDamage = 10.0f;
    public Texture2D projectileDecal;
    public float projectileDelay = 1f;
    public float projectileDestroyDelay = 2.0f;
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

public class AdvancedAiNpcAggressive : MonoBehaviour
{
    public enum attackM
    {
        Melee,
        Ranged,
        Melee_Ranged
    }

    public enum intelmode
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

    private AdvancedAiNpcAggressive[] aggressives;
    private bool alerted;
    public animparamo animationParameters;
    public animogroup animations;
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
    private float dist;
    private AdvancedAiEnemy[] enemies;
    private GameObject fx;
    public genpara generalParameters;
    private bool gosearch;
    private GameObject hitfx;
    public intelmode intelligenceMode;
    private bool isDead;
    private bool isDetected = true;
    private bool isExited = true;
    private bool isHit;
    private bool isIdle;
    private bool isNpc = true;
    private bool isretreat;
    private Vector3 lastTargetPos;
    private Vector3 latestPos;
    private GameObject latestWp;
    public laye layers;
    public Color linesColor;
    public meleee meleeAttack;
    private bool meleeAttacksSwitch = true;
    private NavMeshAgent nav;
    public navigmovi navigationMovement;
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
    public rang rangedAttack;
    private bool rangedAttacksSwitch = true;
    private int reloadCount;
    private bool rotateUpdate;
    [HideInInspector] public bool showHide = true;
    public songroup sounds;
    public soundvol soundsVolume;
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


        triggerChil = new GameObject();
        triggerChil.transform.parent = transform;
        triggerChil.transform.localPosition = Vector3.zero;
        triggerChil.AddComponent("SphereCollider");
        triggerChil.layer = LayerMask.NameToLayer("Ignore Raycast");
        triggerChil.AddComponent<IdleSphere>();
        triggerChil.name = "TriggerSphere";
        triggerChil.GetComponent<SphereCollider>().radius = navigationMovement.goIdleRadius;
        triggerChil.GetComponent<SphereCollider>().isTrigger = true;

        if (!GetComponent<NavMeshAgent>()) gameObject.AddComponent("NavMeshAgent");

        if (!audio) gameObject.AddComponent("AudioSource");


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
            foreach (AnimationClip hAnim in animations.deathAnim)
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
            bot.animation[animations.reloadAnim.name].speed = animationParameters.woundedAnimSpeed;
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
                bot.animation[wAnim.name].speed = animationParameters.wanderingAnimSpeed;
            }
        }
        if (animations.reloadAnim)
        {
            animations.reloadAnim.wrapMode = WrapMode.Once;
            bot.animation[animations.reloadAnim.name].speed = animationParameters.reloadAnimSpeed;
        }

        nav = gameObject.GetComponent<NavMeshAgent>();
        nav.speed = navigationMovement.patrolSpeed;
        nav.angularSpeed = navigationMovement.rotationSpeed;
        nav.stoppingDistance = navigationMovement.arrivalDistance;
        nav.acceleration = navigationMovement.acceleration;

        if (generalParameters.canHear)
        {
            if (generalParameters.target.audio)
                audioS = generalParameters.target.audio;

            if (generalParameters.target.GetComponentInChildren<AudioSource>())
                audioC = generalParameters.target.GetComponentsInChildren<AudioSource>();
        }

        if (layers.viewObstruction.value == 0)
        {
            Debug.LogWarning("You did not assign view's obstruction layer to AI: )" + gameObject.name);
        }

        if (layers.mainTarget.value == 0)
        {
            Debug.LogWarning("You did not assign the target's unique layer for AI: " + gameObject.name);
        }

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


        StartInIdle();
    }

    private void Update()
    {
        if (!generalParameters.target) return;
        if (rotateUpdate)
        {
            Vector3 targetDir = generalParameters.target.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(targetDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation,
                Time.deltaTime*navigationMovement.lookAtSpeed);

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
        if (nav && !nav.isOnOffMeshLink) playedJump = false;

        dist = Vector3.Distance(transform.position, generalParameters.target.transform.position);

        if (generalParameters.healthPoints > 0 && (!waypointSwitch || !wanderingSwitch) &&
            nav.remainingDistance <= navigationMovement.arrivalDistance && !isIdle
            )
            StartCoroutine("IdleInWandering");


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
                isNpc = false;

                if (intelligenceMode == intelmode.Smart)
                {
                    if (!isretreat)
                    {
                        nav.SetDestination(lastTargetPos);

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
                isNpc = false;

                if (intelligenceMode == intelmode.Smart)
                {
                    if (!isretreat)
                    {
                        nav.SetDestination(lastTargetPos);

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

        if (canMesure && !nav.pathPending)
        {
            if (nav.remainingDistance <= navigationMovement.arrivalDistance)
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
                        aggressives = FindObjectsOfType(typeof (AdvancedAiNpcAggressive)) as AdvancedAiNpcAggressive[];
                        enemies = FindObjectsOfType(typeof (AdvancedAiEnemy)) as AdvancedAiEnemy[];

                        if (!played && sounds.alertSfx && (enemies.Length != 0 || aggressives.Length != 0))
                        {
                            played = true;
                            audio.PlayOneShot(sounds.alertSfx, soundsVolume.AlertVolume);
                        }
                        foreach (AdvancedAiEnemy enemy in enemies)
                        {
                            if (enemy && enemy.transform != transform &&
                                Vector3.Distance(transform.position, enemy.transform.position) <=
                                navigationMovement.alertDistance)
                                enemy.AlertOnSight();
                        }
                        foreach (AdvancedAiNpcAggressive aggressive in aggressives)
                        {
                            if (aggressive && aggressive.transform != transform &&
                                Vector3.Distance(transform.position, aggressive.transform.position) <=
                                navigationMovement.alertDistance)
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

        if (isExited && intelligenceMode == intelmode.Smart && ((gosearch && !isHit) || (cancheck && !isHit)))
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
        if (generalParameters.healthPoints > 0)
        {
            if (other.transform == generalParameters.target)
            {
                isExited = true;
                GoIdle();
                nav.Stop(true);
                wanderingSwitch = true;
            }
        }
    }

    public void TriggerStay()
    {
        if (generalParameters.healthPoints > 0 && other.transform == generalParameters.target)
        {
            if (!isNpc)
            {
                if (startSwitch)
                {
                    startSwitch = false;
                    audio.Stop();
                }
                isExited = false;
                Collider[] colinfo =
                    Physics.OverlapSphere(transform.TransformPoint(navigationMovement.viewSphereCenter),
                        navigationMovement.viewSphereRadius, layers.mainTarget);

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
                            isIdle = false;
                            gosearch = false;
                            onetimecheck = true;
                            cancheck = true;
                            rotateUpdate = false;
                            StopCoroutine("IdleInWandering");

                            if (generalParameters.canAlert)
                            {
                                aggressives =
                                    FindObjectsOfType(typeof (AdvancedAiNpcAggressive)) as AdvancedAiNpcAggressive[];
                                enemies = FindObjectsOfType(typeof (AdvancedAiEnemy)) as AdvancedAiEnemy[];

                                if (!played && sounds.alertSfx && (enemies.Length != 0 || aggressives.Length != 0))
                                {
                                    played = true;
                                    audio.PlayOneShot(sounds.alertSfx, soundsVolume.AlertVolume);
                                }


                                foreach (AdvancedAiEnemy enemy in enemies)
                                {
                                    if (enemy &&
                                        Vector3.Distance(transform.position, enemy.transform.position) <=
                                        navigationMovement.alertDistance)
                                        enemy.AlertOnSight();
                                }

                                foreach (AdvancedAiNpcAggressive aggressive in aggressives)
                                {
                                    if (aggressive && aggressive.transform != transform &&
                                        Vector3.Distance(transform.position, aggressive.transform.position) <=
                                        navigationMovement.alertDistance)
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

                            if (!nav.isOnOffMeshLink) nav.speed = navigationMovement.chasingSpeed;

                            switch (attackMode)
                            {
                                case attackM.Melee:

                                    if (nav.remainingDistance <= navigationMovement.arrivalDistance)
                                    {
                                        nav.Stop(true);
                                        transform.rotation = new Quaternion(0, transform.rotation.y, 0,
                                            transform.rotation.w);
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

                                        if (fx && !fx.GetComponent<ParticleSystem>())
                                            Destroy(fx);

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
                                        nav.remainingDistance > navigationMovement.arrivalDistance)
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

                                        if (fx && !fx.GetComponent<ParticleSystem>())
                                            Destroy(fx);

                                        if (!isHit) nav.Resume();
                                        GoChase();
                                    }
                                    else if (nav.remainingDistance <= navigationMovement.arrivalDistance)
                                    {
                                        StopCoroutine("RangedDelay");
                                        rangedAttacksSwitch = true;
                                        if (fx && !fx.GetComponent<ParticleSystem>())
                                            Destroy(fx);
                                        nav.Stop(true);
                                        transform.rotation = new Quaternion(0, transform.rotation.y, 0,
                                            transform.rotation.w);

                                        GoMelee();
                                    }
                                    break;
                            }
                        }

                        else
                        {
                            if (fx)
                            {
                                if (!fx.GetComponent<ParticleSystem>())
                                {
                                    Destroy(fx);
                                }
                            }
                            StopCoroutine("MeleeDelay");
                            StopCoroutine("RangedDelay");
                            isDetected = true;
                            rangedAttacksSwitch = true;
                            meleeAttacksSwitch = true;
                            played = false;

                            if (intelligenceMode == intelmode.Smart && ((gosearch && !isHit) || (cancheck && !isHit)))
                            {
                                if (!gosearch && onetimecheck)
                                {
                                    onetimecheck = false;
                                    nav.SetDestination(generalParameters.target.position);
                                }

                                GoSearch();
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
                                                nav.speed = navigationMovement.patrolSpeed;
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
                                                nav.speed = navigationMovement.patrolSpeed;
                                                PatrolWayPoint();
                                            }
                                        }
                                        break;
                                }
                                if (generalParameters.canHear) Hearing();
                            }
                        }
                    }
                }

                if (colinfo.Length == 0)
                {
                    if (fx)
                    {
                        if (!fx.GetComponent<ParticleSystem>())
                        {
                            Destroy(fx);
                        }
                    }
                    StopCoroutine("MeleeDelay");
                    StopCoroutine("RangedDelay");
                    isDetected = true;
                    rangedAttacksSwitch = true;
                    meleeAttacksSwitch = true;
                    played = false;

                    if (intelligenceMode == intelmode.Smart && ((gosearch && !isHit) || (cancheck && !isHit)))
                    {
                        if (!gosearch && onetimecheck)
                        {
                            onetimecheck = false;
                            nav.SetDestination(generalParameters.target.position);
                        }

                        GoSearch();
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
                                        nav.speed = navigationMovement.patrolSpeed;
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
                                        nav.speed = navigationMovement.patrolSpeed;
                                        PatrolWayPoint();
                                    }
                                }
                                break;
                        }
                        if (generalParameters.canHear) Hearing();
                    }
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
                                    nav.speed = navigationMovement.patrolSpeed;
                                    Wandering();
                                }
                            }
                            else
                            {
                                nav.speed = navigationMovement.patrolSpeed;
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
                                nav.speed = navigationMovement.patrolSpeed;
                                PatrolWayPoint();
                            }
                        }
                        break;
                }

                if (generalParameters.canHear)
                {
                    Hearing();
                }
            }
        }
    }

    private void Wandering()
    {
        isIdle = false;
        wanderingSwitch = false;
        rPosition = new Vector3(
            transform.position.x + (Random.Range(-1.0f, 1.0f)*navigationMovement.wanderingRadius),
            transform.position.y,
            transform.position.z + (Random.Range(-1.0f, 1.0f)*navigationMovement.wanderingRadius));


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
        generalParameters.healthPoints = 0.0f;

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

        if (fx && !fx.GetComponent<ParticleSystem>())
            Destroy(fx);


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
                Destroy(GetComponent<AdvancedAiNpcAggressive>());
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
        if (dist <= (navigationMovement.arrivalDistance*2))
        {
            if (meleeAttacksSwitch)
            {
                meleeAttacksSwitch = false;
                transform.LookAt(generalParameters.target.transform);


                StartCoroutine("MeleeDelay");
            }
        }
    }

    private void GoChase
        ()
    {
        if (animations.gotHitAnim.Length != 0)
        {
            if (animations.chasingAnim.Length != 0 && !bot.animation.IsPlaying(animations.gotHitAnim[chosenHit].name) &&
                !nav.isOnOffMeshLink)
            {
                chosenChase = Random.Range(0, animations.chasingAnim.Length);
                bot.animation.CrossFade(animations.chasingAnim[chosenChase].name, animationParameters.chaseBlendingTime,
                    PlayMode.StopAll);
            }
        }
        else
        {
            if (animations.chasingAnim.Length != 0 && !nav.isOnOffMeshLink)
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
        Vector3 dir = generalParameters.target.position - transform.position;
        Quaternion tarpos = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, tarpos,
            Time.deltaTime*navigationMovement.lookAtSpeed);

        if (rangedAttack.loopRangedAnimation && sounds.rangedAttackSfx && !audio.isPlaying)
        {
            audio.loop = true;
            audio.clip = sounds.rangedAttackSfx;
            audio.Play();
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

        if (animations.chasingAnim.Length != 0)
            bot.animation.Stop(animations.chasingAnim[chosenChase].name);

        if (animations.rangedAttackAnim)
            bot.animation.CrossFadeQueued(animations.rangedAttackAnim.name, animationParameters.rangedBlendingTime,
                QueueMode.CompleteOthers);


        yield return new WaitForSeconds(rangedAttack.projectileDelay);


        if (sounds.rangedAttackSfx && !rangedAttack.loopRangedAnimation)
            audio.PlayOneShot(sounds.rangedAttackSfx, soundsVolume.rangedAttackVolume);

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


            if (rangedAttack.projectileFx && !fx)
            {
                fx =
                    Instantiate(rangedAttack.projectileFx, projectileOrigin.position, projectileOrigin.rotation)
                        as GameObject;
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

            if (generalParameters.target.position.y > pro.transform.position.y*1.3 ||
                generalParameters.target.position.y*2 < pro.transform.position.y)
                pro.transform.LookAt(generalParameters.target);

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

            if (nav.remainingDistance <= navigationMovement.arrivalDistance)
                generalParameters.target.SendMessage(meleeAttack.damageMethodName,
                    meleeAttack.damageAmount[chosenMelee],
                    SendMessageOptions.DontRequireReceiver);

            yield return new WaitForSeconds(meleeAttack.attackInterval);
            meleeAttacksSwitch = true;
        }
        else
        {
            if (sounds.meleeAttackSfx.Length != 0)
            {
                if (sounds.chasingSfx)
                    audio.loop = false;
                audio.PlayOneShot(sounds.meleeAttackSfx[chosenMelee]);
            }
            else audio.Stop();

            if (isDetected && sounds.detectionSfx && !isHit)
            {
                isDetected = false;
                audio.PlayOneShot(sounds.detectionSfx, soundsVolume.detectionVolume);
            }
            if (nav.remainingDistance <= navigationMovement.arrivalDistance)
                generalParameters.target.SendMessage(meleeAttack.damageMethodName,
                    meleeAttack.damageAmount[
                        Random.Range(0, meleeAttack.damageAmount.Length)],
                    SendMessageOptions.DontRequireReceiver);

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
        Gizmos.DrawWireSphere(transform.TransformPoint(navigationMovement.viewSphereCenter),
            navigationMovement.viewSphereRadius);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, navigationMovement.goIdleRadius);
        if (generalParameters.canHear)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, navigationMovement.hearingDistance);
        }
        if (patrolMode == patmode.Dynamic_Wandering)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, navigationMovement.wanderingRadius);
        }
    }

    private void Hearing()
    {
        if (audioS.isPlaying && dist <= navigationMovement.hearingDistance &&
            audioS.volume >= navigationMovement.hearVolumeMin)
        {
            if (!isNpc)
            {
                lastTargetPos = generalParameters.target.position;
                nav.SetDestination(lastTargetPos);

                gosearch = true;

                return;
            }
            if (isIdle)
            {
                Quaternion rotation =
                    Quaternion.LookRotation(generalParameters.target.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation,
                    Time.deltaTime*navigationMovement.lookAtSpeed);
                return;
            }
        }

        if (audioC.Length != 0 && (dist <= navigationMovement.hearingDistance))
        {
            foreach (AudioSource asou in audioC)
            {
                if (asou.isPlaying && !isNpc && asou.volume >= navigationMovement.hearVolumeMin)
                {
                    lastTargetPos = generalParameters.target.position;
                    nav.SetDestination(lastTargetPos);

                    gosearch = true;
                    break;
                }
                if (asou.isPlaying && isNpc && isIdle && asou.volume >= navigationMovement.hearVolumeMin)
                {
                    Quaternion rotation =
                        Quaternion.LookRotation(generalParameters.target.position - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation,
                        Time.deltaTime*navigationMovement.lookAtSpeed);
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


            if (fx && !fx.GetComponent<ParticleSystem>()) Destroy(fx);


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

            if (generalParameters.canAlert)
            {
                aggressives = FindObjectsOfType(typeof (AdvancedAiNpcAggressive)) as AdvancedAiNpcAggressive[];
                enemies = FindObjectsOfType(typeof (AdvancedAiEnemy)) as AdvancedAiEnemy[];

                if (sounds.alertSfx && (enemies.Length != 0 || aggressives.Length != 0))
                    audio.PlayOneShot(sounds.alertSfx, soundsVolume.AlertVolume);

                foreach (AdvancedAiEnemy enemy in enemies)
                {
                    if (enemy &&
                        Vector3.Distance(transform.position, enemy.transform.position) <=
                        navigationMovement.alertDistance)
                        enemy.AlertOnHit(transform);
                }

                foreach (AdvancedAiNpcAggressive aggressive in aggressives)
                {
                    if (aggressive && aggressive.transform != transform &&
                        Vector3.Distance(transform.position, aggressive.transform.position) <=
                        navigationMovement.alertDistance)
                        aggressive.AlertOnHit(transform);
                }
            }
        }
    }

    private IEnumerator IdleInWandering()
    {
        GoIdle();
        float waitingtime = Random.Range(navigationMovement.minIdleInterval, navigationMovement.maxIdleInterval);


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
        }

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
            Debug.LogWarning("You have to be in Waypoints mode first!");
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

        nav.speed = navigationMovement.chasingSpeed;
        GoChase();


        if (nav.hasPath && nav.remainingDistance <= navigationMovement.arrivalDistance && !nav.pathPending)
        {
            if (!alerted)
            {
                cancheck = false;
                gosearch = false;
                if (patrolMode == patmode.In_Place && intelligenceMode == intelmode.Smart)
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
        isNpc = false;
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
                isNpc = false;
                lastTargetPos = generalParameters.target.position;
                if (nav) nav.SetDestination(lastTargetPos);
                gosearch = true;
            }
        }
        else
        {
            isNpc = false;
            lastTargetPos = generalParameters.target.position;
            if (nav) nav.SetDestination(lastTargetPos);
            gosearch = true;
        }
    }

    private void GoRetreat
        ()
    {
        isIdle = false;
        int prop = Random.Range(0, 8);
        switch (prop)
        {
            case 0:
                rPosition = transform.TransformPoint(0, 0, -navigationMovement.retreatDistance);
                break;
            case 1:
                rPosition = transform.TransformPoint(-navigationMovement.retreatDistance, 0, 0);
                break;
            case 2:
                rPosition = transform.TransformPoint(navigationMovement.retreatDistance, 0, 0);
                break;
            case 3:
                rPosition = transform.TransformPoint(navigationMovement.retreatDistance, 0,
                    navigationMovement.retreatDistance);
                break;
            case 4:
                rPosition = transform.TransformPoint(navigationMovement.retreatDistance, 0,
                    -navigationMovement.retreatDistance);
                break;
            case 5:
                rPosition = transform.TransformPoint(-navigationMovement.retreatDistance, 0,
                    navigationMovement.retreatDistance);
                break;
            case 6:
                rPosition = transform.TransformPoint(-navigationMovement.retreatDistance, 0,
                    -navigationMovement.retreatDistance);
                break;
        }

        path = new NavMeshPath();

        bool canit;
        canit = NavMesh.CalculatePath(transform.position, rPosition, 1, path);
        if (canit)
        {
            nav.SetDestination(rPosition);
            nav.Resume();
            canMesure = true;
            RetreatAnim();
        }
        else GoRetreat();
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
}