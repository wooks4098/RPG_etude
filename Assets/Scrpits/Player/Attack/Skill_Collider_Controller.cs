using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Collider_Controller : MonoBehaviour
{
    [SerializeField]
    private GameObject Skill_Q_Collider;
    [SerializeField]
    private GameObject Skill_W_Collider;
    [SerializeField]
    private GameObject Skill_E_Collider;
    //public TrailRenderer trailEffect;
    //public void Start_Attack()
    //{
    //    trailEffect.enabled = true;
    //}
    //public void End_Attack()
    //{
    //    trailEffect.enabled = false;
    //}
    public void Skill_Q_AttackCollision()
    {
        Skill_Q_Collider.SetActive(true);
    }
    public void Skill_W_AttackCollision()
    {
        Skill_W_Collider.SetActive(true);
    }
    public void Skill_E_AttackCollision()
    {
        Skill_E_Collider.SetActive(true);
    }

}
