using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{

    public float Speed;
    private bool isMove;

    private Vector3 destination;


    private Camera camera;
    private Animator animator;
    private NavMeshAgent agent;

    private void Awake()
    {
        camera = Camera.main;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        //NavMeshAgent회전 제한
        agent.updateRotation = false;
    }

    void Start()
    {

    }

    void Update()
    {
       

        Move();
        EndMoveCheck();
    }

    void Move()
    {
        
        //if (Input.GetMouseButtonDown(1))
        //    agent.velocity = Vector3.zero;
        if (Input.GetMouseButton(1))
        {
            RaycastHit hit;
            if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (Vector3.Distance(transform.position, hit.point) > 0.2f)
                {
                    //if (agent.hasPath)
                    {
                        //Vector3 pp = hit.point - transform.position;
                        //Vector3 p2 = destination - transform.position;
                        Vector3 pp = transform.position-hit.point;
                        Vector3 p2 = transform.position-destination;
                        float dot, mag;
                        dot = Vector3.Dot(pp, p2);
                        mag = Vector3.Magnitude(pp) * Vector3.Magnitude(p2);

                        if(dot >=0 && dot <mag)

//Debug.Log(mag);
                        Debug.Log(Mathf.Acos(Vector3.Dot(pp,p2)/Vector3.Magnitude(pp)/Vector3.Magnitude(p2))*Mathf.Rad2Deg);
                    if(Mathf.Acos(Vector3.Dot(pp, p2) / Vector3.Magnitude(pp) / Vector3.Magnitude(p2)) * Mathf.Rad2Deg >= 5)
                        agent.velocity = Vector3.zero;
                    }

                    //이동
                    // agent.velocity = Vector3.zero;
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
           





            //if (agent.velocity.magnitude <= 0.2f)
            //{
            //    isMove = false;
            //    animator.SetBool("isMove", false);
            //    return;
            //}

            ////Vector3 dir = agent.desiredVelocity;
            ////Quaternion targetangle = Quaternion.LookRotation(dir);
            ////animator.transform.rotation = Quaternion.Slerp(transform.rotation, targetangle, Time.deltaTime * 8.0f);




            //var dir = new Vector3(agent.steeringTarget.x, transform.position.y, agent.steeringTarget.z) - transform.position;
            //animator.transform.forward = dir;

        }

    }


}
