using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator anim;
    Rigidbody rigid;

    public Monster monster;
    public float Speed;
    public int Power;
    bool isJump;
    
    void Awake()
    {
        isJump = false;
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && !isJump)
        {
            anim.SetTrigger("jumpTrigger");
            isJump = true;
            rigid.AddForce(Vector3.up * 5, ForceMode.Impulse);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            anim.SetTrigger("attackTrigger");
        }
    }

    void OnCollisionEnter(Collision collision) // 충돌 시 생기는 이벤트
    {
        if (collision.gameObject.name == "Field")
            isJump = false;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "EnterZone")
        {
            monster.PlaySound("Idle");
            other.gameObject.SetActive(false);
        }

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
            //지금까지 이동을 rigid.AddForce로 했었지만, 캐릭터가 바라보는 시점으로 이동하기 위해서는
            //캐릭터의 모델 좌표 기준으로 이동해야하기때문에 Translate()로 하여금 Space.Self를 부여한다.
            if (v <= 0.0f)
            {
                transform.Translate(new Vector3(0, 0, v * 0.4f) * Speed * Time.deltaTime, Space.Self);
            }
            else
            {
                transform.Translate(new Vector3(h * 0.5f, 0, v) * Speed * Time.deltaTime, Space.Self);
            }
        anim.SetBool("isRunning", true);
        }
    }
}
