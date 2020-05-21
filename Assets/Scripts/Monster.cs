using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    public enum mState
    {
        Idle,
        Trace,
        Attack,
        Return,
        Damaged,
        Die
    }

    private Transform player;
    private NavMeshAgent navAgent;
    private NavMeshPath path;
    private Animator anim;
    private AudioSource audioSource;

    public AudioClip audioAttack;
    public AudioClip audioDamaged;
    public AudioClip audioIdle;
    public AudioClip audioDie;


    public float range; // 공격범위, 사정거리
    public float stoppingDist; // 멈추는 거리, nav와 동일하게 설정하기
    public float traceDist; // 추적 범위

    Vector3 originPos;
    mState currentState = mState.Idle;

    bool isDead = false;
    float MonToPlayerDist;
    float OriginDist;

    void EnterState(mState state)
    {
        switch (state)
        {
            case mState.Idle:
                PlaySound("Idle");
                MonsterIdle();
                break;
            case mState.Trace:
                TracePlayer();
                break;
            case mState.Attack:
                PlaySound("Attack");
                AttackPlayer();
                break;
            case mState.Return:
                Return();
                break;
            case mState.Damaged:
                PlaySound("Damaged");
                MonsterDamaged();
                break;
            case mState.Die:
                PlaySound("Die");
                MonsterDie();
                break;
        }
    }
    void Awake()
    {
        originPos = transform.position;
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        navAgent = this.gameObject.GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        EnterState(currentState);
    }

    void Update()
    {
        if (!isDead)
        {
            //몬스터와 플레이어 사이의 거리
            MonToPlayerDist = Vector3.Distance(player.position, transform.position);
            OriginDist = Vector3.Distance(transform.position, originPos);

            if (currentState == mState.Return && OriginDist <= 0.1f)
            {
                EnterState(mState.Idle);
            }
            if (OriginDist > traceDist)
            {
                EnterState(mState.Return);
            }
            if (MonToPlayerDist <= traceDist && MonToPlayerDist > stoppingDist)
            {
                EnterState(mState.Trace);
            }
            else if (MonToPlayerDist <= stoppingDist)
            {
                EnterState(mState.Attack);
            }
        }
    }
    void MonsterIdle()
    {
        currentState = mState.Idle;
        navAgent.isStopped = true;
        anim.SetBool("isRunning", false);
    }
    void TracePlayer()
    {
        currentState = mState.Trace;
        anim.SetBool("isAttack", false);
        navAgent.isStopped = false;
        navAgent.SetDestination(player.position);
        anim.SetBool("isRunning", true);
    }
    void AttackPlayer()
    {
        currentState = mState.Attack;
        anim.SetBool("isRunning", false);
        navAgent.isStopped = true;
        transform.LookAt(player);
        anim.SetBool("isAttack", true);
    }
    void Return()
    {
        currentState = mState.Return;
        anim.SetBool("isAttack", false);
        navAgent.isStopped = false;
        navAgent.SetDestination(originPos);
        anim.SetBool("isRunning", true);
    }
    void MonsterDamaged()
    {
        currentState = mState.Damaged;
    }
    void MonsterDie()
    {
        currentState = mState.Die;
    }
    void PlaySound(string action)
    {
        switch (action)
        {
            case "Attack":
                audioSource.clip = audioAttack; break;
            case "Damaged":
                audioSource.clip = audioDamaged; break;
            case "Idle":
                audioSource.clip = audioIdle; break;
            case "Die":
                audioSource.clip = audioDie; break;
        }
        audioSource.Play();
    }
}
