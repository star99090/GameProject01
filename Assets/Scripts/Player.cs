using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator anim;
    Rigidbody rigid;
    public float charSpeed;
    bool isJump;

    void Awake()
    {
        isJump = false;
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && !isJump){
            anim.SetTrigger("jumpTrigger");
            isJump = true;
            rigid.AddForce(Vector3.up * 5, ForceMode.Impulse);
        }
    }

    void OnCollisionEnter(Collision collision) // 충돌 시 생기는 이벤트
    {
        if (collision.gameObject.name == "Terrain") // 만약 이 스크립트가 적용된 오브젝트가 Terrain라는 오브젝트를 만나면 진행
            isJump = false;
    }

    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        anim.SetFloat("h", h);
        anim.SetFloat("v", v);

        if (h == 0.0f && v == 0.0f)
            anim.SetBool("isRunning", false);
        else
        {
            rigid.AddForce(new Vector3(h * charSpeed, 0, v * charSpeed), ForceMode.Impulse);
            anim.SetBool("isRunning", true);
        }

    }
}
