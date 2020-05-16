using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    Transform Target;
    Animator anim;
    

    public float monSpeed;

    void Awake()
    {
        Target = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }
    void Update()
    {
        
    }
}
