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

    public void Skill_Use_Dodge(float cooltime)
    {
        Skill_Dodge_Cooltime_Image.fillAmount = 1;
        Use_Doge = true;
        Skill_Dodge_CoolTime_Count = 0;
        Skill_Dodge_CoolTime = cooltime;
    }
}
