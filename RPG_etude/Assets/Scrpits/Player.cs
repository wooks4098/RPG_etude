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
        agent.updateRotation = false;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetMouseButton(1))
        {
            RaycastHit hit;
            if(Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition),out hit))
            {
                setDestination(hit.point);
            }
        }
        LookMoveDirateion();
    }

    void setDestination(Vector3 dest)
    {
        agent.SetDestination(dest);

        destination = dest;
        isMove = true;
        animator.SetBool("isMove", true);
    }

    void LookMoveDirateion()
    {
        if(isMove) 
        {
            if (agent.velocity.magnitude == 0f)
            {
                isMove = false;
                animator.SetBool("isMove", false);
                return;
            }
            var dir = new Vector3(agent.steeringTarget.x, transform.position.y, agent.steeringTarget.z) - transform.position;
            transform.forward = dir;
            //transform.position += dir.normalized * Time.deltaTime * Speed;
        }

    }


}
