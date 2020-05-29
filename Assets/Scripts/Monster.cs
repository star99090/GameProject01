using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
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

    bool isDead = false;
    float MonToPlayerDist;
    float OriginDist;

    void Awake()
    {
        originPos = transform.position;
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        navAgent = this.gameObject.GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!isDead)
        {
            //몬스터와 플레이어 사이의 거리
            MonToPlayerDist = Vector3.Distance(player.position, transform.position);

            //몬스터가 처음에 있던 자리와의 거리
            OriginDist = Vector3.Distance(transform.position, originPos);

            if (MonToPlayerDist > traceDist && OriginDist <= 0.1f)
            {
                MonsterIdle();
                PlaySound("Idle");
            }
            else if (OriginDist > traceDist)
            {
                MonsterReturn();
            }
            else if (MonToPlayerDist <= traceDist && MonToPlayerDist > stoppingDist)
            {
                TracePlayer();
            }
            else if (MonToPlayerDist <= stoppingDist)
            {
                AttackPlayer();
            }
        }
    }
    void MonsterIdle()
    {
        navAgent.isStopped = true;
        anim.SetBool("isRunning", false);
    }
    void TracePlayer()
    {
        anim.SetBool("isAttack", false);
        navAgent.isStopped = false;
        navAgent.SetDestination(player.position);
        anim.SetBool("isRunning", true);
    }
    void AttackPlayer()
    {
        anim.SetBool("isRunning", false);
        navAgent.isStopped = true;
        transform.LookAt(player);
        anim.SetBool("isAttack", true);
        PlaySound("Attack");
    }
    void MonsterReturn()
    {
        anim.SetBool("isAttack", false);
        navAgent.isStopped = false;
        navAgent.SetDestination(originPos);
        anim.SetBool("isRunning", true);
    }
    void MonsterDamaged()
    {
        anim.SetTrigger("Damaged");
        PlaySound("Damaged");
    }
    void MonsterDie()
    {
        isDead = true;
        anim.SetBool("isDead", true);
        PlaySound("Die");
    }
    public void PlaySound(string action)
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
