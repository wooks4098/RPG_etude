using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Player_Fsm : MonoBehaviour
{
    enum PLAYER_STATE { Idle, ClickMove, Base_Attack, Dodge }
    [SerializeField]
    private int Player_State;               //플레이어 상태
    public float Speed;                     //이동 속도
    private bool isMove;                    //이동중인지 체크

    private bool Can_BaseAttack = true;     //기본공격을 할수 있는지 확인하는 변수
    private float BaseAttack_time = 0;      //기본공격 쿨타임 측정용 변수
    public float BaseAttack_Range;          //기본공격 거리
    public float BaseAttack_CoolTime;       //기본공격 쿨타임

    private bool isUseSkill = false;        //스킬 사용중인지 (기본공격 or 버프스킬 제외 모든 스킬은 스킬임)
    private bool CanUseDodge = true;        //회피 스킬 사용가능한지 (true 사용가능 false 사용불가능)
    public float Skill_Dodge_CoolTime;      //회피 스킬 쿨타임
    private float Skill_Dodge_time = 0;     //회피 스킬 쿨타임 측정용 변수

    private RaycastHit Target;              //몬스터 추척 타겟
    RaycastHit hit;


    //필요한 컴포넌트
    private Camera camera;
    private Animator animator;
    private NavMeshAgent agent;
    public Player_UI player_ui;

    private void Awake()
    {
        camera = Camera.main;
        animator = GetComponentInChildren<Animator>();
        agent = gameObject.GetComponent<NavMeshAgent>();

        agent.updateRotation = false;//NavMeshAgent회전 제한
        agent.speed = Speed;

    }

    private void Update()
    {
        ClickCheck();
        UseSkill_Check();
        switch (Player_State)
        {
            case (int)PLAYER_STATE.Idle:
                break;
            case (int)PLAYER_STATE.ClickMove:
                ActionReset();
                EndMoveCheck();
                break;
            case (int)PLAYER_STATE.Base_Attack:
                Base_Attack();
                break;

        }
        Look_SetPoint();
        SkillCoolTime_Count();
    }

    

    //클릭 체크
    void ClickCheck()
    {
        if (Input.GetMouseButton(1) && !isUseSkill)
        {
            if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (hit.collider.tag == "Monster")
                {
                    //몬스터를 클릭했을 경우
                    Player_State = (int)PLAYER_STATE.Base_Attack;
                    Target = hit;
                }
                else
                {
                    //클릭 이동
                    Click_Move();
                    Player_State = (int)PLAYER_STATE.ClickMove;
                }
            }
        }
    }

    void ActionReset()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            ResetMove();
            Player_State = (int)PLAYER_STATE.Idle;
        }
       
    }
    #region 클릭이동

    void Click_Move()
    {
        if (Vector3.Distance(transform.position, hit.point) > 0.2f)
        {
            isMove = true;
            animator.SetBool("isMove", isMove);
            agent.SetDestination(hit.point);

        }
    }


    void Look_SetPoint()
    {
        if (agent.hasPath)
        {
            //회전 미끄러짐 방지
            agent.acceleration = (agent.remainingDistance < 2f) ? 8f : 60f;
            //에이전트의 이동방향
            Vector3 direction = agent.desiredVelocity;
            //회전각도(쿼터니언)산출
            Quaternion targetangle = Quaternion.LookRotation(direction);
            //선형보간 함수를 이용해 부드러운 회전
            animator.transform.rotation = Quaternion.Slerp(animator.transform.rotation, targetangle, Time.deltaTime * 8.0f);
        }
    }

    //이동이 끝났는지 체크
    void EndMoveCheck()
    {
        if (agent.velocity.sqrMagnitude >= 0.1f * 0.1f && agent.remainingDistance <= 0.1f)//이동종료
        {
            isMove = false;
            animator.SetBool("isMove", isMove);
            ResetMove();
            Player_State = (int)PLAYER_STATE.Idle;
        }
    }
    //이동 초기화
    void ResetMove()
    {
        agent.ResetPath();
        agent.velocity = Vector3.zero;
        isMove = false;
        animator.SetBool("isMove", isMove);

    }
    #endregion



    #region 스킬

    //스킬을 사용했는지 체크
    void UseSkill_Check()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
           if(CanUseDodge && !isUseSkill) //사용가능한지
            {
                Dodge();
            }
        }
    }

    //스킬 쿨타임 측정
    void SkillCoolTime_Count()
    {
        if (!CanUseDodge) //회피
        {
            Skill_Dodge_time += Time.deltaTime;
            if (Skill_Dodge_time >= Skill_Dodge_CoolTime)
            {
                CanUseDodge = true;
                Skill_Dodge_time = 0;
            }
        }

        if(!Can_BaseAttack) //기본공격
        {
            BaseAttack_time += Time.deltaTime;
            if(BaseAttack_time >= BaseAttack_CoolTime)
            {
                Can_BaseAttack = true;
                BaseAttack_time = 0f;
            }
        }
    }

    #region 기본공격
    //기본공격
    void Base_Attack()
    {

        if (Can_BaseAttack_Check())
        {
            //기본공격
            ResetMove();
            StartCoroutine("LookAt_Target");
            animator.SetTrigger("BasicAttack");
        }
        else if (Can_BaseAttack)
        {
            //추격
            agent.SetDestination(Target.transform.position);
            isMove = true;
            animator.SetBool("isMove", isMove);
        }
    }

    //기본공격을 할 수 있는지 체크
    bool Can_BaseAttack_Check()
    {
        if (!Can_BaseAttack)//기본공격 사용가능한지
            return false;
        float dir = Vector3.Distance(Target.transform.position, transform.position);
        if (dir <= BaseAttack_Range)
        {
            BaseAttack_time = 0f;
            Can_BaseAttack = false;
            return true;
        }
        else
            return false;
    }

    //타겟을 바라보게하는 코루틴 함수
    IEnumerator LookAt_Target()
    {
        float time = 0;
        while (time <= 0.3)
        {
            time += Time.deltaTime;
            //에이전트의 이동방향
            Vector3 direction = Target.transform.position - transform.position;
            //회전각도(쿼터니언)산출
            Quaternion targetangle = Quaternion.LookRotation(direction);
            //선형보간 함수를 이용해 부드러운 회전
            animator.transform.rotation = Quaternion.Slerp(animator.transform.rotation, targetangle, Time.deltaTime * 8.0f);
            yield return null;

        }
        yield break;
    }
    #endregion

    #region 회피
    void Dodge()
    {
        ResetMove();    //움직임 리셋
        if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
        {
            animator.transform.forward = hit.point - animator.transform.position;
            agent.speed *= 2;
            agent.SetDestination(hit.point);
            animator.SetTrigger("Dodge");

            CanUseDodge = false;
            isUseSkill = true;
            Skill_Dodge_time = 0;
            Player_State = (int)PLAYER_STATE.Dodge;
            Invoke("DodgeOut", 0.8f);
        }
    }

    void DodgeOut()
    {
        agent.speed /= 2;
        ResetMove();    //움직임 리셋
        Player_State = (int)PLAYER_STATE.Idle;
        animator.SetBool("isMove", false);
        isMove = false;
        isUseSkill = false;

    }
    #endregion



    #endregion


    private void OnDrawGizmosSelected()
    {
        //기본공격 범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, BaseAttack_Range);


    }
}
