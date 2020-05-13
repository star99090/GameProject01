using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMove : MonoBehaviour
{
    public float rotationSpeed = 1;
    public GameObject Player;
    Transform Target;
    float X, Y;
    
    void Aware()
    {
        Target = Player.transform;
    }

    void LateUpdate() // Update() 이후에 실행되는 업데이트, 주로 UI나 카메라 이동에 대한 것은 여기서 이루어짐
    {
        cameraController();
    }

    void cameraController()
    {
        X += Input.GetAxis("Horizontal") * rotationSpeed;
        Y -= Input.GetAxis("Vertical") * rotationSpeed;
        Y = Mathf.Clamp(Y, -45, 70);

        transform.LookAt(Target);

        Target.rotation = Quaternion.Euler(Y, X, 0.0f);
    }
}
