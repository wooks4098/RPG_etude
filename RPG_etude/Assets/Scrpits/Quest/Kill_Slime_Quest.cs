using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kill_Slime_Quest : MonoBehaviour
{
    int goal_Count = 5;
    int Kill_Count = 0;

    private void Start()
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
                Debug.Log("퀘스트 클리어");
        }
    }
}
