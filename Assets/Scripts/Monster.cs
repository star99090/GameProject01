using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    private Transform playerTransform;
    private NavMeshAgent navAgent;
    private NavMeshPath path;
    Animator anim;

    Vector3 originPos;

    float distance;
    bool isFind;

    private void Awake()
    {
        originPos = transform.position;
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        navAgent = this.gameObject.GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        SearchPlayer();
    }
    
    void SearchPlayer()
    {
        //플레이어 감지
        Collider[] traceCol = Physics.OverlapSphere(transform.position, 11.0f);
        for (int i=0; i<traceCol.Length; i++)
        {
            if (traceCol[i].CompareTag("Player"))
            {
                path = new NavMeshPath();
                //CalculatePath(A,B) : A까지의 경로를 B에 저장
                navAgent.CalculatePath(traceCol[i].transform.position, path);
                distance = Vector3.Distance(playerTransform.position, originPos);
                if (distance < 10.0f) //거리 10보다 가까우면 추적 시작
                {
                    isFind = true;
                    navAgent.SetDestination(traceCol[i].transform.position);
                }
                else // 거리 10보다 멀어지면 원래 자리로 복귀
                {
                    isFind = false;
                    navAgent.SetDestination(originPos);
                }
            }
        }
    }
}
