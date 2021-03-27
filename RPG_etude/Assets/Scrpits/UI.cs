using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI : MonoBehaviour
{
    public static UI instance = null;


    public Image Skill_Dodge_Cooltime_Image;
    private bool Use_Skill = false;
    public float Skill_CoolTime;
    float Skill_CoolTime_Count = 0;

    void Awake()
    {
        
        if (null == instance)
        {
            instance = this;


            DontDestroyOnLoad(this.gameObject);
        }
        else
        {

            Destroy(this.gameObject);
        }

        //Skill_Dodge_Cooltime_Image = GameObject.Find("Dodge_Skill_Cooltime").GetComponent<Image>();
    }
    private void Update()
    {
        if (Use_Skill)
        {
            Skill_CoolTime_Count += Time.deltaTime;
            Skill_Dodge_Cooltime_Image.fillAmount = Mathf.Lerp(1, 0, Skill_CoolTime_Count / Skill_CoolTime);

        }
        if (Skill_CoolTime_Count >= Skill_CoolTime)
            Use_Skill = false;
    }

    public void Skill_Use_Dodge(float cooltime)
    {
        Skill_Dodge_Cooltime_Image.fillAmount = 1;
        Use_Skill = true;
        Skill_CoolTime_Count = 0;
        Skill_CoolTime = cooltime;
    }
}
