using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorExplosion : MonoBehaviour
{
    ParticleSystem particle; // ParticleSystem 컴포넌트

    float particleDuration;  // 파티클 지속 시간

    private void Awake()
    {
        particle = GetComponentInChildren<ParticleSystem>();                                                                      
    }

    void Start()
    {
        // 해당 파티클의 지속시간 불러오기
        particleDuration = particle.main.duration;

        // 파티클 지속시간이 끝나면 해당 오브젝트 제거
        Destroy(gameObject, particleDuration);
    }
}
