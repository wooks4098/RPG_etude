using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Base : MonoBehaviour
{
    public enum PLAYER_STATE { Idle, ClickMove, Base_Attack, Dodge, SkillQ, SkillW, SkillE, SkillR, GetItem }


    [SerializeField] protected float Damage;                   //플레이어 데미지
    [SerializeField] protected float Speed;                     //이동 속도


    [SerializeField] protected float GetItem_Range;            //아이템 획득 범위

    [SerializeField] protected float BaseAttack_Range;          //기본공격 거리
    [SerializeField] protected float BaseAttack_CoolTime;       //기본공격 쿨타임

    [SerializeField] protected float Skill_Dodge_CoolTime;      //회피 스킬 쿨타임
    [SerializeField] protected float Skill_Q_CoolTime;          //Q스킬 쿨타임
    [SerializeField] protected float Skill_W_CoolTime;          //w스킬 쿨타임
    [SerializeField] protected float Skill_E_CoolTime;          //E스킬 쿨타임
    [SerializeField] protected float Skill_R_CoolTime;          //R스킬 쿨타임


}
