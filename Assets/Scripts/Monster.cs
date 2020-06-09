using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    public GameObject player;
    public AudioClip audioAttack;
    public AudioClip audioDamaged;
    public AudioClip audioIdle;
    public AudioClip audioDie;
    public AudioClip audioRunning;

    NavMeshAgent nav;
    Animator anim;
    AudioSource audioSource;
    Player character;
    Transform playerT;

    //public float range; // 공격범위, 사정거리
    public float stoppingDist; // 멈추는 거리
    public float traceDist; // 추적 범위
    public int hp;

    Vector3 originPos; // 생성된 위치
    string monState = "beforeIdle";
    float MonToPlayerDist;
    float OriginDist;
    bool isReturn;

    void Awake()
    {
        isReturn = false;
        originPos = transform.position;
        playerT = player.GetComponent<Transform>();
        nav = this.gameObject.GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        //몬스터가 처음에 있던 자리와의 거리
        OriginDist = Vector3.Distance(originPos, transform.position);
    }
    
    void Update()
    {
        //몬스터와 플레이어 사이의 거리
        MonToPlayerDist = Vector3.Distance(transform.position, playerT.position);

        if (MonToPlayerDist > traceDist && OriginDist == Vector3.Distance(originPos, transform.position))// 추적범위 밖, 원래거리와 가까울때 Idle
        {
            if (nav.isStopped == false)
            {
                nav.speed = 1.5f;
                CancelInvoke();
                monState = "Idle";
                anim.SetBool("isAttack", false);
                nav.isStopped = true;
                anim.SetBool("isRunning", false);
                isReturn = false;
            }
        }
        else if (MonToPlayerDist <= traceDist && MonToPlayerDist > stoppingDist) // Trace
        {
            if (nav.isStopped == true)
            {
                CancelInvoke();
                monState = "Running";
                anim.SetBool("isAttack", false);
                nav.isStopped = false;
                nav.SetDestination(playerT.position);
                anim.SetBool("isRunning", true);
                PlaySound(monState);
            }
            else
            {
                nav.SetDestination(playerT.position);
            }
        }
        else if (OriginDist > traceDist || MonToPlayerDist > 14f) // Return
        {
            if (!isReturn)
            {
                CancelInvoke();
                nav.isStopped = true;
                isReturn = true;
            }
            if (nav.isStopped == true)
            {
                nav.speed = 3f;
                monState = "Running";
                anim.SetBool("isAttack", false);
                nav.isStopped = false;
                nav.SetDestination(originPos);
                anim.SetBool("isRunning", true);
                PlaySound(monState);
            }
        }
        else if (MonToPlayerDist <= stoppingDist) // Attack
        {
            if (nav.isStopped == false)
            {
                CancelInvoke();
                monState = "Attack";
                anim.SetBool("isRunning", false);
                nav.isStopped = true;
                anim.SetBool("isAttack", true);
                PlaySound(monState);
                FocusPlayer();
            }
            transform.LookAt(null);
        }
    }

    public void PlaySound(string monState)
    {
        switch (monState)
        {
            case "Running":
                audioSource.clip = audioRunning;
                Invoke("PlaySoundRepeat", 0.62f);
                break;
            case "Attack":
                audioSource.clip = audioAttack;
                Invoke("PlaySoundRepeat", 2.9f);
                break;
            case "Idle":
                audioSource.clip = audioIdle; break;
            case "Damaged":
                audioSource.clip = audioDamaged; break;
        }
        audioSource.Play();
    }

    void FocusPlayer()
    {
        Invoke("FocusPlayer", 2.2f);
        transform.LookAt(playerT);
    }

    void PlaySoundRepeat()
    {
         PlaySound(monState);
    }

    private void OnCollisionEnter(Collision collision)
    {
    }
}
