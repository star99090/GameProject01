using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMove : MonoBehaviour
{
    public float rotationSpeed = 1.5f;
    public Transform Player;
    float h;
    void LateUpdate() // Update() 이후에 실행되는 업데이트, 주로 UI나 카메라 이동에 대한 것은 여기서 이루어짐
    {
        cameraController();
    }

    void cameraController()
    {
        h += Input.GetAxis("Horizontal") * rotationSpeed;

        transform.LookAt(Player);

        Player.rotation = Quaternion.Euler(0.0f, h, 0.0f);
        transform.Rotate(Vector3.left * 20);
    }
}
