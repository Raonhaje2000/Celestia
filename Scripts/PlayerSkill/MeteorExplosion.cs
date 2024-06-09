using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorExplosion : MonoBehaviour
{
    ParticleSystem particle; // ParticleSystem ������Ʈ

    float particleDuration;  // ��ƼŬ ���� �ð�

    private void Awake()
    {
        particle = GetComponentInChildren<ParticleSystem>();                                                                      
    }

    void Start()
    {
        // �ش� ��ƼŬ�� ���ӽð� �ҷ�����
        particleDuration = particle.main.duration;

        // ��ƼŬ ���ӽð��� ������ �ش� ������Ʈ ����
        Destroy(gameObject, particleDuration);
    }
}
