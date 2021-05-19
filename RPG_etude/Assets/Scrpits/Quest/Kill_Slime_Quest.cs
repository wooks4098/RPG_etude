using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kill_Slime_Quest : MonoBehaviour
{
    [SerializeField] int goal_Count = 5;
    [SerializeField] int Kill_Count = 0;
    public string Quest_Name = "슬라임을처치해라!!";
    public string Quest_info = "마을공터에 나타나는 슬라임 5마리를 처치해라";
    Quest_Npc quest_Npc;

    private void Start()
    {
        quest_Npc = FindObjectOfType<Quest_Npc>();
    }

    public void Acceptance_Quest()
    {
        Player_Event_Manager.Instance.kill_Monster.AddListener(Kill_Slime_Check);
    }

    public void Kill_Slime_Check(string Name)
    {
        if(Name == "Slime")
        {
            Kill_Count++;
            Debug.Log(Kill_Count + " / " + goal_Count);
            if (Kill_Count >= goal_Count)
            {
                Debug.Log("퀘스트 클리어");
                Player_Event_Manager.Instance.kill_Monster.RemoveListener(Kill_Slime_Check);
                quest_Npc.Clear_Quest("Kill_Slime_Quest");
            }
        }
    }
}
