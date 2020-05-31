using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    Transform playerT;
    NavMeshAgent nav;
    Animator anim;
    AudioSource audioSource;

    public AudioClip audioAttack;
    public AudioClip audioDamaged;
    public AudioClip audioIdle;
    public AudioClip audioDie;
    public Player player;


    //public float range; // 공격범위, 사정거리
    public float stoppingDist; // 멈추는 거리
    public float traceDist; // 추적 범위
    public int hp;

    Vector3 originPos; // 생성된 위치
    string monState;
    bool isDead;
    float MonToPlayerDist;
    float OriginDist;

    void Awake()
    {
        isDead = false;
        originPos = transform.position;
        playerT = player.transform;
        nav = this.gameObject.GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        if (!isDead)
        {
            //몬스터와 플레이어 사이의 거리
            MonToPlayerDist = Vector3.Distance(playerT.position, transform.position);
            //몬스터가 처음에 있던 자리와의 거리
            OriginDist = Vector3.Distance(originPos, transform.position);
            
            if (MonToPlayerDist > traceDist && OriginDist <= 0.1f)// 추적범위 밖, 원래거리와 가까울때 IDLE
            {
                monState = "Idle";
                anim.SetBool("isAttack", false);
                nav.isStopped = true;
                anim.SetBool("isRunning", false);
                PlaySound(monState);
            }
            else if (MonToPlayerDist <= traceDist && MonToPlayerDist > stoppingDist) // Trace
            {
                monState = "Running";
                anim.SetBool("isAttack", false);
                nav.isStopped = false;
                nav.SetDestination(playerT.position);
                anim.SetBool("isRunning", true);
            }
            else if (OriginDist > traceDist || MonToPlayerDist > 14f) // 원래있던 Return
            {
                monState = "Running";
                anim.SetBool("isAttack", false);
                nav.isStopped = false;
                nav.SetDestination(originPos);
                anim.SetBool("isRunning", true);
            }
            else if (MonToPlayerDist <= stoppingDist) // Attack
            {
                monState = "Attack";
                anim.SetBool("isRunning", false);
                nav.isStopped = true;
                transform.LookAt(playerT);
                anim.SetBool("isAttack", true);
                PlaySound(monState);
            }
        }
        else
        {
            Invoke("monDel()", 4);
            timeControl();
        }

        PlaySound(monState);
    }
    /*
    void MonsterDamaged()
    {
        anim.SetTrigger("Damaged");
    }
    */
    void timeControl()
    {
        Time.timeScale = 0.1f;
        for(int i = 1; Time.timeScale < 1; i++)
        {
            Time.timeScale += i/10;
        }
    }

    void monDel()
    {
        this.gameObject.SetActive(false);
    }

    public void PlaySound(string monState)
    {
        switch (monState)
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
