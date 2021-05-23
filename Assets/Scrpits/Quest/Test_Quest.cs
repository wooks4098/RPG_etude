using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Quest : MonoBehaviour
{
    int Kill_Monster = 0;
    int Max_Kill = 5;
    private void Start()
    {
        var p = FindObjectOfType<Player_Event_Manager>();
        //등록
        //p.onInputSpace.AddListener(SlimeCheck);
        //제거
        //p.onInputSpace.RemoveListener(SlimeCheck);

    }
    public void SlimeCheck()
    {
        Kill_Monster++;
        Debug.Log(Kill_Monster + " / " + Max_Kill);
    }
}
