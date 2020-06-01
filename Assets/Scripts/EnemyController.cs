using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{

    [Header("Stats")]
    public float attackRange;
    public float deathTime;
    public float damageValue;
    public Vector3 dyingToColor;

    [Header("Objects")] 
    public SkinnedMeshRenderer playerModelRenderer;
    public Material poisonedMaterial;
    public Material enemyMaterial;
    public Animator enemyAnimator;
    public Transform enemyAttackPoint;
    public enum EnemyState
    {
        Idle,
        Run,
        Busy,
        Attack,
        Death,
        OnPoisoned,
        Panic,
        OnVomit
    }

    public EnemyState enemyState = EnemyState.Idle;
    

    [Header("Particles")] 
    
    public GameObject deathParticle;
    public GameObject attackParticle;
    public GameObject vomitParticle;
    public GameObject poisonedLoopParticle;

    [Header("SFX")] 
    public AudioClip deathClip;
    public AudioClip vomitClip;
    public AudioClip attackClip;
    public AudioClip couchClip;

    private bool _isRunning;
    private bool _isTriggered;
    private bool _isPanic;
    private bool _isAttacking;
    private Transform _enemyHolder;
    private GameObject _barricadeTarget;
    private Vector3 _targetPosition;
    private NavMeshAgent _agent;
    private GameObject _vomitTarget;
    private Vector3 _barricadeHitPosition;
    private bool _startDying;
    private int _randomValueForSfx = 20;


    private void Start()
    {
        _enemyHolder = GameObject.FindGameObjectWithTag("EnemyHolder").transform;
        transform.SetParent(_enemyHolder);
        _agent = GetComponent<NavMeshAgent>();
        _barricadeTarget = GameObject.FindGameObjectWithTag("Target");
        
        
    }

    private void Update()
    {
        if (_startDying)
        {
            //enemyMaterial.color = Color.Lerp(enemyMaterial.color,new Color(dyingToColor.x,dyingToColor.y,dyingToColor.z),Time.deltaTime);
            var color = Color.Lerp(playerModelRenderer.material.GetColor("_Color"), new Color(dyingToColor.x, dyingToColor.y, dyingToColor.z), Time.deltaTime/500);
            playerModelRenderer.material.SetColor("_Color", color);
        }
        
        //FSM
        switch (enemyState)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Run:
                Run();
                break;
            case EnemyState.Busy:
                break;
            case EnemyState.Attack:
                if (!_isAttacking)
                {
                    Attack();
                }
                break;
            case EnemyState.Death:
                break;
            case EnemyState.OnPoisoned:
                StopAllCoroutines();
                OnPoisoned();
                break;
            case EnemyState.Panic:
                OnPanic();
                break;
            case EnemyState.OnVomit:
                OnVomit();
                break;
            default:
                break;
        }
    }

    public void Idle()
    {
        enemyState = EnemyState.Run;
    }

    public void Run()
    {
        if (!_isRunning)
        {
            _isRunning = true;
            var randomTarget = Random.Range(0, NavigationManager.manager.enemyMoveTransformList.Count);
            _agent.SetDestination(NavigationManager.manager.enemyMoveTransformList[randomTarget].position); //G
        }
    }

    public void Attack()
    {
        _agent.SetDestination(transform.position);
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        _isAttacking = true;
        enemyState = EnemyState.Busy;
        //Attack anim here
        enemyAnimator.SetInteger("State",1);
        transform.LookAt(_barricadeTarget.transform.position);
        yield return new WaitForSeconds(0.4f);

        Instantiate(attackParticle, enemyAttackPoint.position,Quaternion.identity);
        if (Random.Range(0,_randomValueForSfx) <= 15)
        {
            AudioManager.manager.PlayEnemySfx(attackClip,55,0.06f);
        }
        
        ScoreManager.manager.Damage(damageValue);
        
        yield return new WaitForSeconds(0.1f);
        
        _isAttacking = false;
        enemyAnimator.SetInteger("State",4);
        enemyState = EnemyState.Attack;
        
    }
    
    public void OnVomit()
    {
        _agent.SetDestination(transform.position);
        StartCoroutine(VomitRoutine());
    }

    IEnumerator VomitRoutine()
    {
        enemyState = EnemyState.Busy;
        //Vomit anim here
        enemyAnimator.SetInteger("State",3);
        
        transform.LookAt(_vomitTarget.transform.position);
        yield return new WaitForSeconds(0.2f);

        
        if (Vector3.Distance(transform.position,_vomitTarget.transform.position)<=attackRange)
        {
            Instantiate(vomitParticle, _vomitTarget.transform.position, Quaternion.identity);

            if (Random.Range(0,_randomValueForSfx) == 5)
            {
                AudioManager.manager.PlayEnemySfx(vomitClip,52,0.03f);
            }
            
            _vomitTarget.SendMessage("SpreadPoison");
        }
        

        yield return new WaitForSeconds(0.1f);
        
        _isPanic = false;
        enemyAnimator.SetInteger("State",2);
        enemyState = EnemyState.Panic;
    }

    public void SpreadPoison()
    {
        if (!gameObject.CompareTag("Poisoned"))
        {
            enemyState = EnemyState.OnPoisoned;
        }
        
    }
    
    public void OnPoisoned()
    {
        //OnPoisoned particle here
        poisonedLoopParticle.SetActive(true);
        gameObject.tag = "Poisoned";

        if (Random.Range(0,_randomValueForSfx) == 5)
        {
            AudioManager.manager.PlayEnemySfx(couchClip,58,0.03f);
        }
        
        _agent.SetDestination(transform.position);
        //playerModelRenderer.material = poisonedMaterial;
        ScoreManager.manager.AddPoisoned();
        //Anim
        
        enemyState = EnemyState.Panic;
        
    }

    public void OnPanic()
    {
        if (!_isPanic)
        {
            _isPanic = true;
            enemyAnimator.SetInteger("State",2);
            _targetPosition = NavigationManager.manager.GetRandomPositionOnNavmesh(30f, transform.position, NavigationManager.manager.outsideArea);
            _agent.SetDestination(_targetPosition);
            StartCoroutine(DeathRoutine());
        }

        if (Vector3.Distance(transform.position,_targetPosition) <= _agent.stoppingDistance+1)
        {
            _targetPosition = NavigationManager.manager.GetRandomPositionOnNavmesh(30f, transform.position, NavigationManager.manager.outsideArea);
            _agent.SetDestination(_targetPosition);
        }
        
    }

    IEnumerator DeathRoutine()
    {
        _startDying = true;
        yield return new WaitForSeconds(deathTime);

        if (Random.Range(0,_randomValueForSfx) == 5)
        {
            AudioManager.manager.PlayEnemySfx(deathClip,56,0.03f);
        }
        
        Instantiate(deathParticle, new Vector3(transform.position.x,transform.position.y+0.5f,transform.position.z), Quaternion.identity);
        ScoreManager.manager.RemovePoisoned();
        ScoreManager.manager.AddDeath();
        _startDying = false;
        Destroy(gameObject);
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag($"Explosion"))
        {
            if (!gameObject.CompareTag($"Poisoned") && !_isTriggered)
            {
                _isTriggered = true;
                enemyState = EnemyState.OnPoisoned;
            }
        }

        if (other.CompareTag("Enemy"))
        {
            if (gameObject.CompareTag("Poisoned") && enemyState != EnemyState.OnVomit && enemyState != EnemyState.Busy)
            {
                _vomitTarget = other.gameObject;
                enemyState = EnemyState.OnVomit;
            }
        }

        // if (other.CompareTag("Poisoned"))
        // {
        //     if (gameObject.CompareTag("Enemy"))
        //     {
        //         //Anim
        //         enemyState = EnemyState.OnPoisoned;
        //     }
        // }

        if (other.CompareTag("Target"))
        {
            if (gameObject.CompareTag("Enemy") && enemyState != EnemyState.Attack && enemyState != EnemyState.Busy)
            {
                enemyState = EnemyState.Attack;
            }
        }
    }
    
    private void LateUpdate()
    {
        if (_isTriggered)
        {
            _isTriggered = false;
        }
    }
}
