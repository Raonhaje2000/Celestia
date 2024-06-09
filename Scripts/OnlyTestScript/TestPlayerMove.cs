using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerMove : MonoBehaviour
{
    float playerMoveSpeed;    // 플레이어 이동 속도

    void Start()
    {
        playerMoveSpeed = 10.0f;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();   // 플레이어 이동 처리
    }

    // 플레이어 이동 처리
    void PlayerMove()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        transform.Translate(h * Vector3.right * playerMoveSpeed * Time.deltaTime);
        transform.Translate(v * Vector3.forward * playerMoveSpeed * Time.deltaTime);
    }
}
