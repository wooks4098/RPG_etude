using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_UI : MonoBehaviour
{
    public Image Skill_Dodge_Cooltime_Image;
    private bool Use_Doge = false;
    public float Skill_Dodge_CoolTime;
    float Skill_Dodge_CoolTime_Count = 0;

    public Image Skill_SkillQ_Cooltime_Image;
    private bool Use_SkillQ = false;
    public float Skill_SkillQ_CoolTime;
    float Skill_SkillQ_CoolTime_Count = 0;

    public Image Skill_SkillW_Cooltime_Image;
    private bool Use_SkillW = false;
    public float Skill_SkillW_CoolTime;
    float Skill_SkillW_CoolTime_Count = 0;

    public Image Skill_SkillE_Cooltime_Image;
    private bool Use_SkillE = false;
    public float Skill_SkillE_CoolTime;
    float Skill_SkillE_CoolTime_Count = 0;

    void Awake()
    {
    }
    private void Update()
    {
        if (Use_SkillQ)
        {
            Skill_SkillQ_CoolTime_Count += Time.deltaTime;
            Skill_SkillQ_Cooltime_Image.fillAmount = Mathf.Lerp(1, 0, Skill_SkillQ_CoolTime_Count / Skill_SkillQ_CoolTime);

        }
        if (Skill_SkillQ_CoolTime_Count >= Skill_SkillQ_CoolTime)
            Use_SkillQ = false;

        if (Use_SkillW)
        {
            Skill_SkillW_CoolTime_Count += Time.deltaTime;
            Skill_SkillW_Cooltime_Image.fillAmount = Mathf.Lerp(1, 0, Skill_SkillW_CoolTime_Count / Skill_SkillW_CoolTime);

        }
        if (Skill_SkillW_CoolTime_Count >= Skill_SkillW_CoolTime)
            Use_SkillW = false;

        if (Use_SkillE)
        {
            Skill_SkillE_CoolTime_Count += Time.deltaTime;
            Skill_SkillE_Cooltime_Image.fillAmount = Mathf.Lerp(1, 0, Skill_SkillE_CoolTime_Count / Skill_SkillE_CoolTime);

        }
        if (Skill_SkillE_CoolTime_Count >= Skill_SkillE_CoolTime)
            Use_SkillE = false;

        if (Use_Doge)
        {
            Skill_Dodge_CoolTime_Count += Time.deltaTime;
            Skill_Dodge_Cooltime_Image.fillAmount = Mathf.Lerp(1, 0, Skill_Dodge_CoolTime_Count / Skill_Dodge_CoolTime);

        }
        if (Skill_Dodge_CoolTime_Count >= Skill_Dodge_CoolTime)
            Use_Doge = false;
    }

    public void Skill_Use_SkillQ(float cooltime)
    {
        Skill_SkillQ_Cooltime_Image.fillAmount = 1;
        Use_SkillQ = true;
        Skill_SkillQ_CoolTime_Count = 0;
        Skill_SkillQ_CoolTime = cooltime;
    }

    public void Skill_Use_SkillW(float cooltime)
    {
        Skill_SkillW_Cooltime_Image.fillAmount = 1;
        Use_SkillW = true;
        Skill_SkillW_CoolTime_Count = 0;
        Skill_SkillW_CoolTime = cooltime;
    }

    public void Skill_Use_SkillE(float cooltime)
    {
        Skill_SkillE_Cooltime_Image.fillAmount = 1;
        Use_SkillE = true;
        Skill_SkillE_CoolTime_Count = 0;
        Skill_SkillE_CoolTime = cooltime;
    }
    public void Skill_Use_Dodge(float cooltime)
    {
        Skill_Dodge_Cooltime_Image.fillAmount = 1;
        Use_Doge = true;
        Skill_Dodge_CoolTime_Count = 0;
        Skill_Dodge_CoolTime = cooltime;
    }
}
