using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class Slime : MonoBehaviour
{
    public float Max_Hp;
    public float Hp;
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
    public bool isDie = false;
    [SerializeField]
    private ParticleSystem Blood;

    [SerializeField]
    private Transform Player;

    //UI

    [SerializeField]
    GameObject Hp_bar;
    private Slider Hp_bar_slider;
    //필요한 컴포넌트
    private NavMeshAgent agent;
    private Animator animator;
    private Camera camera;

    private void Awake()
    {
        Hp = Max_Hp;
        camera = Camera.main;
        Hp_bar.SetActive(true);
        Hp_bar_slider = Hp_bar.GetComponent<Slider>();
        animator = GetComponentInChildren<Animator>();
        agent = gameObject.GetComponent<NavMeshAgent>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        agent.updateRotation = false;//NavMeshAgent회전 제한
        agent.speed = Speed;
    }


    private void Update()
    {
        Hp_bar_Set();
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
                break;
        }
        Rotate();
        Die_Check();
    }
  
    public void Spawn()
    {
        isDie = false;
        Hp = Max_Hp;
        Slime_State = (int)SLIME_STATE.Find;
        Hp_bar.SetActive(true);
        Hp_bar_Set();
    }


    #region 상태
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
            //추격
            agent.speed = Speed + 2;
            agent.SetDestination(Player.position);
            Attack_CoolTime_Count = 0.5f;
        }


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
    #endregion

    #region 회전관련

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

    #endregion

    #region 피격

    public void Slime_Hit_Base_Attack(float _Damage)
    {
        if (Hp <= 0)
            return;
        Hp -= _Damage;
        Blood.transform.forward = Player.position - Blood.transform.position;
        Blood.transform.Rotate(0, 90, 0);
        Blood.Play();
        //Hp -= _Damage;
        //죽었는지 확인
    }
    #endregion

    #region 공격



    private void OnTriggerEnter(Collider other)
    {
        //if (other.tag == "Player_Weapon")
        //{
        //    Debug.Log("스라임 피격");

        //}
    }

    #endregion

    #region 죽음
    void Die_Check()
    {
        if (!isDie && Hp <= 0)
        {
            StartCoroutine("Die_ani");
            Player_Event_Manager.Instance.Kill_Slime();
        }
    }

    IEnumerator Die_ani()
    {
        agent.ResetPath();
        agent.velocity = Vector3.zero;
        animator.SetTrigger("isDie");
        Slime_State = (int)SLIME_STATE.Die;
        isDie = true;

        yield return new WaitForSeconds(0.2f);
        Hp_bar.SetActive(false);
        yield return new WaitForSeconds(0.8f);
        gameObject.SetActive(false);

        yield break;
    }
    #endregion

    #region UI
    void Hp_bar_Set()
    {
        Hp_bar.transform.position = camera.WorldToScreenPoint(gameObject.transform.position + new Vector3(0, 1.5f, 0));
        Hp_bar_slider.value = Hp / Max_Hp;
    }
    #endregion

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
