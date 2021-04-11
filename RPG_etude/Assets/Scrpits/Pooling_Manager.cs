using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooling_Manager : MonoBehaviour
{
    public GameObject[] Slime;
    public GameObject Slime_Prefab;

    private void Awake()
    {
        Slime = new GameObject[20];


        Setobject();        //로딩?
    }

    //오브젝트 미리 생성
    void Setobject()
    {
        for(int i = 0; i< Slime.Length; i++)
        {
            Slime[i] = Instantiate(Slime_Prefab);
            Slime[i].SetActive(false);
        }

    }
}
