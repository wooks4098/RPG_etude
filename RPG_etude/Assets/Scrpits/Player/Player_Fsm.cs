using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Player_Fsm : Player_Base
{

    [SerializeField]
    private int Player_State;               //플레이어 상태

    private bool isMove;                    //이동중인지 체크

    private bool Can_BaseAttack = true;     //기본공격을 할수 있는지 확인하는 변수
    private float BaseAttack_time = 0;      //기본공격 쿨타임 측정용 변수

    private bool isUseSkill = false;        //스킬 사용중인지 (기본공격 or 버프스킬 제외 모든 스킬은 스킬임)

    private bool CanUseDodge = true;        //회피 스킬 사용가능한지 (true 사용가능 false 사용불가능)
    private float Skill_Dodge_time = 0;     //회피 스킬 쿨타임 측정용 변수

    private bool CanUseSkill_Q = true;      //Q스킬 사용가능한지
    private float Skill_Q_time = 0;        //Q스킬 쿨타임 측정용 변수

    private bool CanUseSkill_W = true;      //w스킬 사용가능한지
    private float Skill_W_time = 0;        //w스킬 쿨타임 측정용 변수

    private bool CanUseSkill_E = true;      //E스킬 사용가능한지
    private float Skill_E_time = 0;        //E스킬 쿨타임 측정용 변수

    private bool CanUseSkill_R = true;      //R스킬 사용가능한지
    private float Skill_R_time = 0;        //R스킬 쿨타임 측정용 변수

    private RaycastHit Target;              //몬스터 추척 타겟
    RaycastHit hit;


    //필요한 컴포넌트
    private Camera camera;
    private Animator animator;
    private NavMeshAgent agent;
    public Player_UI player_ui;
    public ItemPickUp itemPickUp;

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
                Look_SetPoint();
                break;
            case (int)PLAYER_STATE.GetItem:
                GetItem();
                Look_SetPoint();

                break;

            case (int)PLAYER_STATE.Base_Attack:
                Base_Attack();
                Look_SetPoint();
                break;
            case (int)PLAYER_STATE.Dodge:
                Look_SetPoint();
                break;
            case (int)PLAYER_STATE.SkillQ:
                break;
        }

        SkillCoolTime_Count();
    }

    

    //클릭 체크
    void ClickCheck()
    {
        if (Input.GetMouseButton(1) && !isUseSkill)
        {
            if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (hit.collider.tag == "Monster_Slime")
                {
                    //몬스터를 클릭했을 경우
                    Player_State = (int)PLAYER_STATE.Base_Attack;
                    Target = hit;
                    //StartCoroutine("LookAt_Target");
                }
                else if(hit.collider.tag == "Item")
                {
                    Player_State = (int)PLAYER_STATE.GetItem;
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
            agent.acceleration = 60f;
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

    #region 아이템획득


    void GetItem()
    {
        if(Vector3.Distance(transform.position, hit.transform.position) <= GetItem_Range)
        {
            //아이템 획득
            itemPickUp.Get_Item(hit);
            ResetMove();
            Player_State = (int)PLAYER_STATE.Idle;
        }
        else
        {
            //이동
            agent.SetDestination(hit.point);
        }
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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (CanUseSkill_Q && !isUseSkill) //사용가능한지
            {
                Q_Skill();
            }
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (CanUseSkill_W && !isUseSkill) //사용가능한지
            {
                W_Skill();
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (CanUseSkill_E && !isUseSkill) //사용가능한지
            {
                E_Skill();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (CanUseSkill_R && !isUseSkill) //사용가능한지
            {
                R_Skill();
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

        if (!CanUseSkill_Q)//Q스킬
        {
            Skill_Q_time += Time.deltaTime;
            if (Skill_Q_time >= Skill_Q_CoolTime)
            {
                CanUseSkill_Q = true;
                Skill_Q_time = 0;
            }
        }

        if (!CanUseSkill_W)//W스킬
        {
            Skill_W_time += Time.deltaTime;
            if (Skill_W_time >= Skill_W_CoolTime)
            {
                CanUseSkill_W = true;
                Skill_W_time = 0;
            }
        }

        if (!CanUseSkill_E)//E스킬
        {
            Skill_E_time += Time.deltaTime;
            if (Skill_E_time >= Skill_E_CoolTime)
            {
                CanUseSkill_E = true;
                Skill_E_time = 0;
            }
        }

        if (!CanUseSkill_R)//R스킬
        {
            Skill_R_time += Time.deltaTime;
            if (Skill_R_time >= Skill_E_CoolTime)
            {
                CanUseSkill_R = true;
                Skill_R_time = 0;
            }
        }

        if (!Can_BaseAttack) //기본공격
        {
            BaseAttack_time += Time.deltaTime;
            if(BaseAttack_time >= BaseAttack_CoolTime)
            {
                Can_BaseAttack = true;
                BaseAttack_time = 0f;
            }
        }
    }

    IEnumerator Skill_Look()
    {
        for(int i = 0; i<50; i++)
        {

            Vector3 dir = hit.point - animator.transform.position;
            Quaternion targetangle = Quaternion.LookRotation(dir);
            animator.transform.rotation = Quaternion.Slerp(animator.transform.rotation, targetangle, 0.1f);
            animator.transform.eulerAngles = new Vector3(0, animator.transform.rotation.eulerAngles.y, 0);
            yield return null;
        }

        yield break;
    }

    #region Skill_Q

    void Q_Skill()
    {
        ResetMove();    //움직임 리셋
        if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
        {
            //회전
            StartCoroutine("Skill_Look");
            animator.SetTrigger("SkillQ");
            player_ui.Skill_Use_SkillQ(Skill_Q_CoolTime);

            CanUseSkill_Q = false;
            isUseSkill = true;
            Skill_Q_time = 0;
            Player_State = (int)PLAYER_STATE.SkillQ;
            Invoke("QSkillOut", 1f);
        }
    }

    void QSkillOut()
    {

        Player_State = (int)PLAYER_STATE.Idle;
        animator.SetBool("isMove", false);
        isMove = false;
        isUseSkill = false;

    }
    #endregion

    #region Skill_W

    void W_Skill()
    {
        ResetMove();    //움직임 리셋
        if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
        {
            StartCoroutine("Skill_Look");
            //Skill_Look();
            animator.SetTrigger("SkillW");
            player_ui.Skill_Use_SkillW(Skill_W_CoolTime);

            CanUseSkill_W = false;
            isUseSkill = true;
            Skill_W_time = 0;
            Player_State = (int)PLAYER_STATE.SkillW;
            Invoke("WSkillOut", 1f);
        }
    }

    void WSkillOut()
    {

        Player_State = (int)PLAYER_STATE.Idle;
        animator.SetBool("isMove", false);
        isMove = false;
        isUseSkill = false;

    }
    #endregion

    #region Skill_E

    void E_Skill()
    {
        ResetMove();    //움직임 리셋
        if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
        {
            //회전
            StartCoroutine("Skill_Look");
            //Skill_Look();
            animator.SetTrigger("SkillE");
            player_ui.Skill_Use_SkillE(Skill_E_CoolTime);

            CanUseSkill_E = false;
            isUseSkill = true;
            Skill_E_time = 0;
            Player_State = (int)PLAYER_STATE.SkillE;
            Invoke("ESkillOut", 1f);
        }
    }

    void ESkillOut()
    {

        Player_State = (int)PLAYER_STATE.Idle;
        animator.SetBool("isMove", false);
        isMove = false;
        isUseSkill = false;

    }
    #endregion

    #region Skill_R

    void R_Skill()
    {
        ResetMove();    //움직임 리셋
        if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
        {
            //회전
            StartCoroutine("Skill_Look");
            //Skill_Look();
            animator.SetTrigger("SkillR");
            player_ui.Skill_Use_SkillR(Skill_R_CoolTime);

            CanUseSkill_R = false;
            isUseSkill = true;
            Skill_R_time = 0;
            Player_State = (int)PLAYER_STATE.SkillR;
            Invoke("RSkillOut", 1f);
        }
    }

    void RSkillOut()
    {

        Player_State = (int)PLAYER_STATE.Idle;
        animator.SetBool("isMove", false);
        isMove = false;
        isUseSkill = false;

    }
    #endregion

    #region 기본공격
    //기본공격
    void Base_Attack()
    {


        if (Can_BaseAttack_Check())
        {
            //기본공격
            if (Target.transform.gameObject.activeSelf == false)
            {
                Player_State = (int)PLAYER_STATE.Idle;
                return;
            }
            ResetMove();
            StartCoroutine("LookAt_Target");
            animator.SetTrigger("BasicAttack");
            StartCoroutine("Base_Attack_Hit_Monster");

        }
        else if (Can_BaseAttack)
        {
            //추격
            agent.SetDestination(Target.transform.position);
            isMove = true;
            animator.SetBool("isMove", isMove);
        }
    }

    IEnumerator Base_Attack_Hit_Monster()
    {
        yield return new WaitForSeconds(0.3f);

        if (hit.transform.tag == "Monster_Slime")
        {
            Slime slime = hit.transform.gameObject.GetComponent<Slime>();
            slime.Slime_Hit_Base_Attack(1);
        }

        yield break;
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
        while (time <= 0.7)
        {
            time += Time.deltaTime;
            //에이전트의 이동방향
            Vector3 direction = Target.transform.position - transform.position;
            //회전각도(쿼터니언)산출
            Quaternion targetangle = Quaternion.LookRotation(direction);
            //선형보간 함수를 이용해 부드러운 회전
            animator.transform.rotation = Quaternion.Slerp(animator.transform.rotation, targetangle, Time.deltaTime * 5.0f);
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
            //animator.transform.forward = hit.point - animator.transform.position;
            agent.speed *= 2;
            agent.SetDestination(hit.point);
            animator.SetTrigger("Dodge");
            player_ui.Skill_Use_Dodge(Skill_Dodge_CoolTime);

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
        isMove = false;
        animator.SetBool("isMove", false);

        isUseSkill = false;

    }
    #endregion



    #endregion


    private void OnDrawGizmosSelected()
    {
        //기본공격 범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, BaseAttack_Range);

        //아이템 획득 범위
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, GetItem_Range);


    }
}
