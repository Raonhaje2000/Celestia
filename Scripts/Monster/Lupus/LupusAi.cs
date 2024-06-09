using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LupusAi : MonoBehaviour
{
    NavMeshAgent navMeshAgent;   // NavMeshAgent 컴포넌트

    Transform lupus;             // 루푸스 (몬스터)

    [SerializeField] Vector3 beforeWalkPosition;  // 걷기 전에 있던 위치
    [SerializeField] Vector3 beforeChasePosition; // 추적 전에 있던 위치
    [SerializeField] Vector3 destination;
    
    float walkSpeed;             // 걷을 때의 속력
    float chaseSpeed;            // 추적할 때의 속력
    float returnSpeed;           // 돌아갈 때의 속력

    [SerializeField] bool isWalkBack;           
    [SerializeField] bool isChaseBack;

    [SerializeField] float remain;
    [SerializeField] float velocity;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        lupus = transform;
    }

    void Start()
    {
        beforeWalkPosition = lupus.position;
        beforeChasePosition = lupus.position;

        walkSpeed = 2.0f;
        chaseSpeed = walkSpeed * 2.0f;
        returnSpeed = chaseSpeed * 2.0f;

        isWalkBack = false;
        isChaseBack = false;
    }

    private void Update()
    {
        //velocity = navMeshAgent.velocity.magnitude;
        //remain = navMeshAgent.remainingDistance;
    }

    public void SetSpeeds(float walkSpeed, float chaseSpeed, float returnSpeed)
    {
        this.walkSpeed = walkSpeed;
        this.chaseSpeed = chaseSpeed;
        this.returnSpeed = returnSpeed;
    }

    // 이동 전 몬스터가 있던 위치 저장
    public void SetBeforeWalkPosition()
    {
        beforeWalkPosition = lupus.position;
    }

    // 추적 전 몬스터가 있던 위치 저장
    public void SetBeforeChasePosition()
    {
        beforeChasePosition = lupus.position;                                                                 
        //Debug.Log(beforeChasePosition);
    }

    // 걷기 목표 위치로 이동
    public void MoveWalkDestination(Vector3 walkDestination)
    {
        isWalkBack = false;

        navMeshAgent.speed = walkSpeed;
        navMeshAgent.SetDestination(walkDestination);
    }

    // 걷기 전 위치로 이동
    public void MoveBeforeWalkPosition()
    {
        isWalkBack = true;

        navMeshAgent.speed = walkSpeed;
        navMeshAgent.SetDestination(beforeWalkPosition);
    }

    // 추적 목표 위치로 이동
    public void MoveChaseDestination(Vector3 chaseTargetPosition)
    {
        isChaseBack = false;

        navMeshAgent.speed = chaseSpeed;
        navMeshAgent.SetDestination(chaseTargetPosition);
    }

    // 추적 전 위치로 이동
    public void MoveBeforeChasePosition()
    {
        isChaseBack = true;

        navMeshAgent.speed = returnSpeed;
        navMeshAgent.SetDestination(beforeChasePosition);                                                                           
    }

    // 이동 또는 추적 정지
    public void StopMove()
    {
        // 설정한 경로 삭제 (SetDestination 호출 전까지 경로 찾기를 시작하지 않음)
        navMeshAgent.ResetPath();
        //navMeshAgent.velocity = new Vector3(0, 0, 0);
    }

    // 목적지에 도착했는 지 확인하는 함수
    bool ArriveDestination()
    {
        return ((int) navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && (int) navMeshAgent.velocity.magnitude <= 0.0f);
    }

    // 걷기 목표 위치에 도착했는지 확인
    public bool ArriveWalkDestination()
    {
        return (ArriveDestination() && !isWalkBack) ? true : false;
    }

    // 걷기 전 목표 위치에 도착했는지 확인
    public bool ArriveBeforeWalkPosition()
    {
        return (ArriveDestination() && isWalkBack) ? true : false;
    }

    // 추적 목표 위치에 도착 했는지 확인
    public bool ArriveChaseDestination()
    {
        return (ArriveDestination() && !isChaseBack) ? true : false;
    }

    // 추적 전 위치에 도착 했는지 확인
    public bool ArriveBeforeChasePosition()
    {
        return (ArriveDestination() && isChaseBack) ? true : false;                                                                      
    }
}
