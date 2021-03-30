using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : Player_Base
{
    // public float Speed;                 //이동 속도
    private bool isMove;                //이동중인가?
    private bool useSkill = false;      //스킬을 사용중인가?
    private bool isDodge = true;        //스킬_회피를 사용할 수 있는가?
    //public float Dodge_CoolTime;        //스킬_회피 쿨타임
    [SerializeField]
    private float DodgeTime_Count;      //회피 쿨타임 카운트
    private bool isBasic_attack = false;        //기본공격 하는중인지?
    private float Basic_attack_time;

    private Vector3 destination;        //방향

    private bool isMonsterClick = false;//몬스터를 클릭했는가 (true면 몬스터 추적)
    private RaycastHit Target;          //몬스터 추척 타겟

    private Camera camera;
    private Animator animator;
    private NavMeshAgent agent;

    private IEnumerator coroutin;

    public Player_UI player_ui;
    private void Awake()
    {
        camera = Camera.main;
        animator = GetComponentInChildren<Animator>();
        agent = gameObject.GetComponent<NavMeshAgent>();

        agent.updateRotation = false;//NavMeshAgent회전 제한
        agent.speed = Speed;

    }

    void Start()
    {

    }

    void Update()
    {
        ClickCheck();
        Tracking_Monster();
        //Move();
        EndMoveCheck();
        Dodge();
        SkillCooltime();
        ResetMove();
    }

    void ResetMove()
    {
        if(Input.GetKeyDown(KeyCode.S)&& agent.hasPath && !useSkill)
        {
            agent.ResetPath();
            agent.velocity = Vector3.zero;
            animator.SetBool("isMove", false);
            isMove = false;
        }
    }
    void ClickCheck()
    {
        if (Input.GetMouseButton(1) && !useSkill)
        {
            RaycastHit hit;
            if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (hit.collider.tag == "Monster")
                {
                    //몬스터 추척
                    isMonsterClick = true;
                    Target = hit;
                    animator.SetBool("isMove", true);

                }
                else
                {
                    //이동
                    if (Vector3.Distance(transform.position, hit.point) > 0.2f)
                    {
                        if (agent.hasPath)
                            agent.acceleration = (agent.remainingDistance < 2f) ? 8f : 60f;
                        agent.SetDestination(hit.point);
                        destination = hit.point;
                        agent.stoppingDistance = 0f;
                       isMove = true;
                        isMonsterClick = false;
                        animator.SetBool("isMove", true);

                    }
                }
            }
        }
    }

    #region click move
    //이동
    void Move()
    {
        if (Input.GetMouseButton(1)&& !useSkill)
        {
            RaycastHit hit;
            if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (Vector3.Distance(transform.position, hit.point) > 0.2f)
                {
                    if (agent.hasPath)
                        agent.acceleration = (agent.remainingDistance < 2f) ? 8f : 60f;
                    agent.SetDestination(hit.point);
                    destination = hit.point;
                    isMove = true;
                    animator.SetBool("isMove", true);
                }
            }
        }
    }
           

    void EndMoveCheck()
    {
        if(agent.hasPath)
        {
            //에이전트의 이동방향
            Vector3 direction = agent.desiredVelocity;
            //회전각도(쿼터니언)산출
            Quaternion targetangle = Quaternion.LookRotation(direction);
            //선형보간 함수를 이용해 부드러운 회전
            animator.transform.rotation = Quaternion.Slerp(animator.transform.rotation, targetangle, Time.deltaTime * 8.0f);
        }
       
        if (isMove)
        {
            if(agent.velocity.sqrMagnitude >= 0.1f * 0.1f && agent.remainingDistance <= 0.1f)//이동종료
            {
                isMove = false;
                animator.SetBool("isMove", false);
                //회전코드 수정
                agent.ResetPath();
                agent.velocity = Vector3.zero;
            }
            else if (agent.desiredVelocity.sqrMagnitude >= 0.1f * 0.1f)//이동중 -> 회전
            {
                ////에이전트의 이동방향
                //Vector3 direction = agent.desiredVelocity;
                ////회전각도(쿼터니언)산출
                //Quaternion targetangle = Quaternion.LookRotation(direction);
                ////선형보간 함수를 이용해 부드러운 회전
                //animator.transform.rotation = Quaternion.Slerp(animator.transform.rotation, targetangle, Time.deltaTime * 8.0f);
            }
        }
    }
    #endregion

    //몬스터를 클릭하였을 경우 | 몬스터를 향해 이동하면서 공격가능한 거리면 공격해야함
    void Tracking_Monster()
    {
        if (!isMonsterClick)
            return;

        if ( CanAttack_Check())//NormalAttack02_SingleTwohandSword
        {
            agent.ResetPath();
            agent.velocity = Vector3.zero;
            animator.SetTrigger("BasicAttack");

        }
        else if(!isBasic_attack)
        {
            agent.SetDestination(Target.transform.position);
            animator.SetBool("isMove", true);
        }
    }

  


    bool CanAttack_Check()
    {
        if (isBasic_attack)
            return false;
        float dir = Vector3.Distance(Target.transform.position, transform.position);
        if (dir <= 2f)
        {
            isBasic_attack = true;
            Basic_attack_time = 0;
            return true;
        }
        else
            return false;

    }





    #region 회피

    void SkillCooltime()
    {
        if(isDodge == false)
            DodgeTime_Count += Time.deltaTime;

        //if (DodgeTime_Count >= Dodge_CoolTime)
        //    isDodge = true;

        if (isBasic_attack)
            Basic_attack_time += Time.deltaTime;

        if (Basic_attack_time >= 2f)
            isBasic_attack = false;
    }

    void Dodge()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isDodge && !useSkill)
            {
                //이전 이동 초기화
                agent.ResetPath();
                agent.velocity = Vector3.zero;
                RaycastHit hit;
                if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    animator.transform.forward = hit.point - animator.transform.position;
                    agent.speed *= 2;
                    agent.SetDestination(hit.point);

                    //animator.SetBool("isDodge", true);
                    animator.SetTrigger("Dodge");
                    
                    isDodge = false;
                    useSkill = true;
                    isMonsterClick = false;
                    DodgeTime_Count = 0;
                    //player_ui.Skill_Use_Dodge(Dodge_CoolTime);
                    Invoke("DodgeOut", 0.8f);
                }
               
            }
        }
       
    }

    void DodgeOut()
    {
        //agent 속도 초기화
        agent.speed /= 2;
        agent.ResetPath();
        agent.velocity = Vector3.zero;

        animator.SetBool("isMove", false);
        isMove = false;
        useSkill = false;
    }
    #endregion

    

}
