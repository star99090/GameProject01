using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    Transform player;
    NavMeshAgent nav;
    Animator anim;
    AudioSource audioSource;

    public AudioClip audioAttack;
    public AudioClip audioDamaged;
    public AudioClip audioIdle;
    public AudioClip audioDie;


    //public float range; // 공격범위, 사정거리
    public float stoppingDist; // 멈추는 거리
    public float traceDist; // 추적 범위
    public int hp;

    Vector3 originPos; // 생성된 위치
    string monState = "beforeIdle";
    float MonToPlayerDist;
    float OriginDist;

    void Awake()
    {
        originPos = transform.position;
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        nav = this.gameObject.GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        //몬스터와 플레이어 사이의 거리
        MonToPlayerDist = Vector3.Distance(player.position, transform.position);
        //몬스터가 처음에 있던 자리와의 거리
        OriginDist = Vector3.Distance(originPos, transform.position);

        if (MonToPlayerDist > traceDist && OriginDist <= 0.1f)// 추적범위 밖, 원래거리와 가까울때 IDLE
        {
            monState = "Idle";
            anim.SetBool("isAttack", false);
            nav.isStopped = true;
            anim.SetBool("isRunning", false);
        }
        else if (MonToPlayerDist <= traceDist && MonToPlayerDist > stoppingDist) // Trace
        {
            monState = "Running";
            anim.SetBool("isAttack", false);
            nav.isStopped = false;
            nav.SetDestination(player.position);
            anim.SetBool("isRunning", true);
        }
        else if (OriginDist > traceDist || MonToPlayerDist > 14f) // Return
        {
            monState = "Running";
            anim.SetBool("isAttack", false);
            nav.SetDestination(originPos);
            nav.isStopped = false;
            anim.SetBool("isRunning", true);
        }
        else if (MonToPlayerDist <= stoppingDist) // Attack
        {
            monState = "Attack";
            anim.SetBool("isRunning", false);
            nav.isStopped = true;
            transform.LookAt(player);
            anim.SetBool("isAttack", true);
        }
    }
    public void PlaySound(string monState)
    {
        switch (monState)
        {
            case "Attack":
                audioSource.clip = audioAttack; break;
            case "Idle":
                audioSource.clip = audioIdle; break;
            default:
                break;
        }
        if(audioSource.clip != null)
            audioSource.Play();
    }
}
