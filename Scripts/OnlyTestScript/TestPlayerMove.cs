using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerMove : MonoBehaviour
{
    float playerMoveSpeed;    // �÷��̾� �̵� �ӵ�

    void Start()
    {
        playerMoveSpeed = 10.0f;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();   // �÷��̾� �̵� ó��
    }

    // �÷��̾� �̵� ó��
    void PlayerMove()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        transform.Translate(h * Vector3.right * playerMoveSpeed * Time.deltaTime);
        transform.Translate(v * Vector3.forward * playerMoveSpeed * Time.deltaTime);
    }
}
