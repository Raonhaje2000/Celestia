using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    Rigidbody _rigidbody;

    GameObject explosionEffect; // ���� ȿ��

    float fallAngle;            // ����߸��� ����
    float forceAmount;          // ���ϴ� ���� ��

    Vector3 forceDirection;     // ���� ����

    float damage;               // ������

    private void Awake()
    {
        // ���� ���ҽ� �ҷ�����
        LoadResources();
        SetDamage(0.0f);
    }

    void Start()
    {
        // �ʱ�ȭ
        Initialize();
    }

    void Update()
    {
        // ��� �� ���ϱ�
        AddForceMeteor();
    }

    // ���� ���ҽ� �ҷ�����
    void LoadResources()
    {
        _rigidbody = GetComponent<Rigidbody>();

        explosionEffect = Resources.Load<GameObject>("Prefabs/Skill/Player/Meteor/MeteorExplosion");                            
    }

    // �ʱ�ȭ
    void Initialize()
    {
        fallAngle = 30.0f;
        forceAmount = 5.0f;

        // ��� �������� ������ 30�� �� ��, x�� z�Ÿ��� �߽����κ��� tan(��� �������� ����)�� ����                                  
        float xz = Mathf.Tan(fallAngle * Mathf.Deg2Rad);

        forceDirection = new Vector3(-xz, -1.0f, -xz); // ���� ����

        // �������� ������ŭ ��� ȸ���Ͽ� ����Ʈ ������ �����ϵ��� ����
        transform.Rotate(new Vector3(fallAngle, 0, -fallAngle));
    }

    // ������ �� ����
    // (�ַ� �ش� ��ų ��ũ��Ʈ���� ȣ���� ���� �ʱ�ȭ �ϴ� �Լ�)
    public void SetDamage(float damageValue)
    {
        damage = damageValue;
    }

    // ��� �� ���ϱ�
    void AddForceMeteor()
    {
        // ��� ���� ���� (���� ���� ����)
        _rigidbody.AddForce(forceDirection * forceAmount, ForceMode.Acceleration);                                                 
    } 

    // �ٸ� ������Ʈ�� �浹���� ���� ó��
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            // ����� �浹���� ���
            // �浹 ����Ʈ ���� �� � ������Ʈ ����
            Instantiate(explosionEffect, transform.position, transform.rotation);                                    
            Destroy(gameObject);
        }

        if(other.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            // ���Ϳ� �浹���� ���
            // �ش� ������ ü���� �÷��̾� ��ų ��������ŭ ���� ��Ŵ

            //Debug.Log("���� �ǰ� ������: " + damage);
            other.gameObject.GetComponent<Monster>().DamagedPlayerSkill(damage);
        }

        // �ƹ��͵� �浹���� �ʾ��� ��� 5�� �� ����
        Destroy(gameObject, 5.0f);
    }
}
