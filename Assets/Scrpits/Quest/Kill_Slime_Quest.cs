using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Kill_Slime_Quest : MonoBehaviour
{
    public int goal_Count = 5;
    public int Kill_Count = 0;
    public string Quest_Name = "슬라임을처치해라!!";
    public string Quest_info = "마을공터에 나타나는 슬라임 5마리를 처치해라";

    private Text Quest_Name_Text;
    private Text Quest_info_Text;
    private GameObject Quest_Accept_slot;

  
    Quest_Npc quest_Npc;


    private void Start()
    {
        quest_Npc = FindObjectOfType<Quest_Npc>();
    }

    public void Acceptance_Quest()
    {
        Player_Event_Manager.Instance.kill_Monster.AddListener(Kill_Slime_Check);
        Quest_Accept_slot.SetActive(true);

    }

    public void Kill_Slime_Check(string Name)
    {
        if(Name == "Slime")
        {
            Kill_Count++;
            Debug.Log(Kill_Count + " / " + goal_Count);
            Set_info();
            if (Kill_Count >= goal_Count)
            {
                Debug.Log("퀘스트 클리어");
                Player_Event_Manager.Instance.kill_Monster.RemoveListener(Kill_Slime_Check);
                quest_Npc.Clear_Quest("Kill_Slime_Quest");
            }
        }
    }


    public void Set_Quest_Accept_slot(GameObject _Slot, Text _Name, Text _info)
    {
        Quest_Accept_slot = _Slot;
        Quest_Name_Text = _Name;
        Quest_info_Text = _info;

        Quest_Name_Text.text = Quest_Name;
        Set_info();
        Quest_Accept_slot.SetActive(false);
    }

    public void Set_info()
    {
        Quest_info_Text.text = Kill_Count + " / " + goal_Count;
    }

}
