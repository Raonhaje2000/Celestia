using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    Rigidbody _rigidbody;

    GameObject explosionEffect; // 폭발 효과

    float fallAngle;            // 떨어뜨리는 각도
    float forceAmount;          // 가하는 힘의 양

    Vector3 forceDirection;     // 힘의 방향

    float damage;               // 데미지

    private void Awake()
    {
        // 관련 리소스 불러오기
        LoadResources();
        SetDamage(0.0f);
    }

    void Start()
    {
        // 초기화
        Initialize();
    }

    void Update()
    {
        // 운석에 힘 가하기
        AddForceMeteor();
    }

    // 관련 리소스 불러오기
    void LoadResources()
    {
        _rigidbody = GetComponent<Rigidbody>();

        explosionEffect = Resources.Load<GameObject>("Prefabs/Skill/Player/Meteor/MeteorExplosion");                            
    }

    // 초기화
    void Initialize()
    {
        fallAngle = 30.0f;
        forceAmount = 5.0f;

        // 운석이 떨어지는 각도가 30도 일 때, x와 z거리는 중심으로부터 tan(운석이 떨어지는 각도)와 같음                                  
        float xz = Mathf.Tan(fallAngle * Mathf.Deg2Rad);

        forceDirection = new Vector3(-xz, -1.0f, -xz); // 힘의 방향

        // 떨어지는 각도만큼 운석을 회전하여 이펙트 방향이 동일하도록 변경
        transform.Rotate(new Vector3(fallAngle, 0, -fallAngle));
    }

    // 데미지 값 세팅
    // (주로 해당 스킬 스크립트에서 호출해 값을 초기화 하는 함수)
    public void SetDamage(float damageValue)
    {
        damage = damageValue;
    }

    // 운석에 힘 가하기
    void AddForceMeteor()
    {
        // 운석에 힘을 가함 (질량 영항 없음)
        _rigidbody.AddForce(forceDirection * forceAmount, ForceMode.Acceleration);                                                 
    } 

    // 다른 오브젝트와 충돌했을 때의 처리
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            // 지면과 충돌했을 경우
            // 충돌 이펙트 생성 및 운석 오브젝트 제거
            Instantiate(explosionEffect, transform.position, transform.rotation);                                    
            Destroy(gameObject);
        }

        if(other.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            // 몬스터와 충돌했을 경우
            // 해당 몬스터의 체력을 플레이어 스킬 데미지만큼 감소 시킴

            //Debug.Log("몬스터 피격 데미지: " + damage);
            other.gameObject.GetComponent<Monster>().DamagedPlayerSkill(damage);
        }

        // 아무것도 충돌하지 않았을 경우 5초 뒤 제거
        Destroy(gameObject, 5.0f);
    }
}
