using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Player : MonoBehaviour
{
    public float Speed;
    private bool isMove;
    private bool isDodge = true;
    [SerializeField]
    private float DodgeTime; //회피 쿨타임
    private Vector3 destination;


    private Camera camera;
    private Animator animator;
    private NavMeshAgent agent;

    private void Awake()
    {
        camera = Camera.main;
        animator = GetComponent<Animator>();
        agent = gameObject.GetComponentInChildren<NavMeshAgent>();

        //NavMeshAgent회전 제한
        agent.updateRotation = false;
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

    }



    #region click move
    void Move()
    {
        if (Input.GetMouseButton(1)&& isDodge)
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
            if(agent.velocity.sqrMagnitude >= 0.1f * 0.1f && agent.remainingDistance <= 0.1f)
            {
                isMove = false;
                animator.SetBool("isMove", false);
            }
            else if (agent.desiredVelocity.sqrMagnitude >= 0.1f * 0.1f)
            {
                //에이전트의 이동방향
                Vector3 direction = agent.desiredVelocity;
                //회전각도(쿼터니언)산출
                Quaternion targetangle = Quaternion.LookRotation(direction);
                //선형보간 함수를 이용해 부드러운 회전
                transform.rotation = Quaternion.Slerp(transform.rotation, targetangle, Time.deltaTime * 8.0f);
            }
        }
    }
    #endregion


    void Dodge()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (isDodge)
            {
                RaycastHit hit;
                agent.ResetPath();
                agent.velocity = Vector3.zero;
                if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    transform.forward = hit.point;
                    Speed *= 5;
                    agent.SetDestination(hit.point);
                    animator.SetBool("isDodge", true);
                    isDodge = false;
                    Invoke("DodgeOut", 1f);
                }
               
            }
        }
       
    }

    void DodgeOut()
    {
        Speed /= 5;

        agent.ResetPath();
        agent.velocity = Vector3.zero;
        animator.SetBool("isMove", false);
        isDodge = true;
    }


}
