using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LupusAi : MonoBehaviour
{
    NavMeshAgent navMeshAgent;   // NavMeshAgent ������Ʈ

    Transform lupus;             // ��Ǫ�� (����)

    [SerializeField] Vector3 beforeWalkPosition;  // �ȱ� ���� �ִ� ��ġ
    [SerializeField] Vector3 beforeChasePosition; // ���� ���� �ִ� ��ġ
    [SerializeField] Vector3 destination;
    
    float walkSpeed;             // ���� ���� �ӷ�
    float chaseSpeed;            // ������ ���� �ӷ�
    float returnSpeed;           // ���ư� ���� �ӷ�

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

    // �̵� �� ���Ͱ� �ִ� ��ġ ����
    public void SetBeforeWalkPosition()
    {
        beforeWalkPosition = lupus.position;
    }

    // ���� �� ���Ͱ� �ִ� ��ġ ����
    public void SetBeforeChasePosition()
    {
        beforeChasePosition = lupus.position;                                                                 
        //Debug.Log(beforeChasePosition);
    }

    // �ȱ� ��ǥ ��ġ�� �̵�
    public void MoveWalkDestination(Vector3 walkDestination)
    {
        isWalkBack = false;

        navMeshAgent.speed = walkSpeed;
        navMeshAgent.SetDestination(walkDestination);
    }

    // �ȱ� �� ��ġ�� �̵�
    public void MoveBeforeWalkPosition()
    {
        isWalkBack = true;

        navMeshAgent.speed = walkSpeed;
        navMeshAgent.SetDestination(beforeWalkPosition);
    }

    // ���� ��ǥ ��ġ�� �̵�
    public void MoveChaseDestination(Vector3 chaseTargetPosition)
    {
        isChaseBack = false;

        navMeshAgent.speed = chaseSpeed;
        navMeshAgent.SetDestination(chaseTargetPosition);
    }

    // ���� �� ��ġ�� �̵�
    public void MoveBeforeChasePosition()
    {
        isChaseBack = true;

        navMeshAgent.speed = returnSpeed;
        navMeshAgent.SetDestination(beforeChasePosition);                                                                           
    }

    // �̵� �Ǵ� ���� ����
    public void StopMove()
    {
        // ������ ��� ���� (SetDestination ȣ�� ������ ��� ã�⸦ �������� ����)
        navMeshAgent.ResetPath();
        //navMeshAgent.velocity = new Vector3(0, 0, 0);
    }

    // �������� �����ߴ� �� Ȯ���ϴ� �Լ�
    bool ArriveDestination()
    {
        return ((int) navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && (int) navMeshAgent.velocity.magnitude <= 0.0f);
    }

    // �ȱ� ��ǥ ��ġ�� �����ߴ��� Ȯ��
    public bool ArriveWalkDestination()
    {
        return (ArriveDestination() && !isWalkBack) ? true : false;
    }

    // �ȱ� �� ��ǥ ��ġ�� �����ߴ��� Ȯ��
    public bool ArriveBeforeWalkPosition()
    {
        return (ArriveDestination() && isWalkBack) ? true : false;
    }

    // ���� ��ǥ ��ġ�� ���� �ߴ��� Ȯ��
    public bool ArriveChaseDestination()
    {
        return (ArriveDestination() && !isChaseBack) ? true : false;
    }

    // ���� �� ��ġ�� ���� �ߴ��� Ȯ��
    public bool ArriveBeforeChasePosition()
    {
        return (ArriveDestination() && isChaseBack) ? true : false;                                                                      
    }
}
