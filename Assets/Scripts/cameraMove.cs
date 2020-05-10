using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMove : MonoBehaviour
{
    GameObject target;
    float moveSpeed;
    Vector3 targetPosition;

    void Awake()
    {
        target = GetComponent<GameObject>();
    }
    void Update()
    {
        targetPosition.Set(target.transform.position.x, target.transform.position.y, this.transform.position.z);
        this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}
