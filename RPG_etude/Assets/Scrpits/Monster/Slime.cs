using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Slime : MonoBehaviour
{
    enum SLIME_STATE
    {
      Find, //탐색상태
      Chase, //플레이어를 쫒아가는 상태
      Die,
    }
    public float Speed; //속도
    public float Attack_Range;  //공격 범위
    public float Attack_CoolTime;   //공격 쿨타임
    public float Attack_CoolTime_Count = 2;   //공격 쿨타임
    public float Find_Range;    //탐색 범위
    public float Chase_Range;   //추격 범위
    private float Move_time = 0; //이동 시간
    private float Move_time_Count = 0;
    [SerializeField]
    private int Slime_State;//슬라임 상태

    [SerializeField]
    private Transform Player;

    //필요한 컴포넌트
    private NavMeshAgent agent;
    private Animator animator;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        agent = gameObject.GetComponent<NavMeshAgent>();

        agent.updateRotation = false;//NavMeshAgent회전 제한
        agent.speed = Speed;
    }


    private void Update()
    {

        switch (Slime_State)
        {
            case (int)SLIME_STATE.Find:
                Find();
                break;
            case (int)SLIME_STATE.Chase:
                Chase();
                break;
            case (int)SLIME_STATE.Die:
                //fade out
                gameObject.SetActive(false);
                break;
        }
        Rotate();
    }

    void Find()
    {
        Move_time_Count += Time.deltaTime;
        if(Move_time_Count >= Move_time)
        {
            agent.speed = Speed;
            agent.SetDestination(new Vector3(transform.position.x + (float)Random.Range(-2, 2), transform.position.y, transform.position.z + (float)Random.Range(-2, 2)));

            //움직임 시간 초기화
            Move_time_Count = 0;
            Move_time = Random.Range(0, 5);

        }

        //애니메이션
        if (agent.hasPath)
            animator.SetBool("isWalk", true);
        else
            animator.SetBool("isWalk", false);

        //탐색
        if (Vector3.Distance(transform.position, Player.position) <= Find_Range)
            Slime_State = (int)SLIME_STATE.Chase;
    }


    void Chase()
    {
        //추격 가능 범위에 있는지 체크
        if(Vector3.Distance(transform.position,Player.position)> Chase_Range)
        {
            Slime_State = (int)SLIME_STATE.Find;
            return;
        }
        //공격 범위
        if(Vector3.Distance(transform.position, Player.position) <= Attack_Range)
        {
            //공격
            Attack();
        }
        else
        {
            agent.SetDestination(Player.position);
            Attack_CoolTime_Count = 0.5f;
        }
        //추격

        //agent.speed;

    }
    void Attack()
    {
        agent.ResetPath();
        agent.velocity = Vector3.zero;
        Attack_CoolTime_Count += Time.deltaTime;
        if(Attack_CoolTime_Count >= Attack_CoolTime)
        {
            StartCoroutine("LookAt_Player");
            animator.SetTrigger("Attack");

            //쿨타임 초기화
            Attack_CoolTime_Count = 0;
        }
        
    }
    IEnumerator LookAt_Player()
    {
        float time = 0;
        while (time <= 0.3)
        {
            time += Time.deltaTime;
            //에이전트의 이동방향
            Vector3 direction = Player.position - transform.position;
            //회전각도(쿼터니언)산출
            Quaternion targetangle = Quaternion.LookRotation(direction);
            //선형보간 함수를 이용해 부드러운 회전
            animator.transform.rotation = Quaternion.Slerp(animator.transform.rotation, targetangle, Time.deltaTime * 8.0f);
            yield return null;

        }
        yield break;
    }

    //회전
    void Rotate()
    {
        if (agent.hasPath)
        {
            //에이전트의 이동방향
            Vector3 direction = agent.desiredVelocity;
            //회전각도(쿼터니언)산출
            Quaternion targetangle = Quaternion.LookRotation(direction);
            //선형보간 함수를 이용해 부드러운 회전
            animator.transform.rotation = Quaternion.Slerp(animator.transform.rotation, targetangle, Time.deltaTime * 8.0f);
        }
    }


    private void OnDrawGizmosSelected()
    {
        //탐색 범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Find_Range);

        //추격 범위
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Chase_Range);

        //공격 범위
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, Attack_Range);
    }
}
