using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PistrisAi : MonoBehaviour
{
    NavMeshAgent navMeshAgent;   // NavMeshAgent 컴포넌트

    Transform pistris;
    Transform player;

    float chaseSpeed;
    float dashSpeed;

    [SerializeField] float remain;
    [SerializeField] float velocity;

    [SerializeField] Vector3 destination;

    [SerializeField] Vector3 dashPosition;

    bool isWaitNextFrame;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        pistris = transform;
        player = GameObject.FindWithTag("Player").transform;
    }

    void Start()
    {
        chaseSpeed = 4.0f;
        dashSpeed = chaseSpeed * 2.0f;
    }

    void Update()
    {
        destination = navMeshAgent.destination;

        velocity = navMeshAgent.velocity.magnitude;
        remain = navMeshAgent.remainingDistance;

        if(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            navMeshAgent.velocity = new Vector3(0, 0, 0);
        }
    }

    public void SetSpeeds(float chaseSpeed, float dashSpeed)
    {
        this.chaseSpeed = chaseSpeed;
        this.dashSpeed = dashSpeed;
    }

    public void MoveChaseDestination()
    {
        navMeshAgent.speed = chaseSpeed;
        navMeshAgent.SetDestination(player.position);
    }

    public void MoveDashDestination()
    {
        navMeshAgent.speed = dashSpeed;

        //Vector3 vector = player.position - pistris.position;
        //dashPosition = vector.normalized * (Vector3.Distance(player.position, pistris.position) + 8.0f);                                

        //navMeshAgent.SetDestination(dashPosition);

        navMeshAgent.SetDestination(player.position);
    }

    public void StopMove()
    {
        // 설정한 경로 삭제 (SetDestination 호출 전까지 경로 찾기를 시작하지 않음)
        navMeshAgent.ResetPath();
    }

    public bool ArriveDestination()
    {
        StartCoroutine(WaitNextFrame());

        return (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance);
    }

    IEnumerator WaitNextFrame()
    {
        if(!isWaitNextFrame)
        {
            yield return new WaitForEndOfFrame();

            isWaitNextFrame = true;
        }
    }
}
