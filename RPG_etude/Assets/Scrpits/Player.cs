using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public float Speed;
    private bool isMove;
    private bool isDodge = true;
    private bool useSkill = false;
    public float Dodge_CoolTime;
    [SerializeField]
    private float DodgeTime_Count; //회피 쿨타임


    private Vector3 destination;


    private Camera camera;
    private Animator animator;
    private NavMeshAgent agent;

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
        Move();
        EndMoveCheck();
        Dodge();
        SkillCooltime();
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
        if (isMove)
        {
            if(agent.velocity.sqrMagnitude >= 0.1f * 0.1f && agent.remainingDistance <= 0.1f)//이동종료
            {
                isMove = false;
                animator.SetBool("isMove", false);
                agent.ResetPath();
                agent.velocity = Vector3.zero;
            }
            else if (agent.desiredVelocity.sqrMagnitude >= 0.1f * 0.1f)//이동중 -> 회전
            {
                //에이전트의 이동방향
                Vector3 direction = agent.desiredVelocity;
                //회전각도(쿼터니언)산출
                Quaternion targetangle = Quaternion.LookRotation(direction);
                //선형보간 함수를 이용해 부드러운 회전
                animator.transform.rotation = Quaternion.Slerp(animator.transform.rotation, targetangle, Time.deltaTime * 8.0f);
            }
        }
    }
    #endregion

    #region 스킬

    void SkillCooltime()
    {
        if(isDodge == false)
            DodgeTime_Count += Time.deltaTime;

        if (DodgeTime_Count >= Dodge_CoolTime)
            isDodge = true;
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

                    animator.SetBool("isDodge", true);

                    isDodge = false;
                    useSkill = true;
                    DodgeTime_Count = 0;
                    UI.instance.Skill_Use_Dodge(Dodge_CoolTime);
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
        useSkill = false;
    }
    #endregion

}
