using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooling_Manager : MonoBehaviour
{
    public GameObject[] Slime;
    public GameObject Slime_Prefab;
    public int Slime_Poolling_Count;



    private void Awake()
    {
        Slime = new GameObject[Slime_Poolling_Count];


        Setobject();
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

    public GameObject ReturnObject(string _Name)
    {
        switch (_Name)
        {
            case "슬라임":
                return findObject(Slime);

        }
        return null; //사용가능한 오브젝트가 없음
    }

    //사용가능한 오브젝트 탐색
    GameObject findObject(GameObject[] _obj)
    {
        for (int i = 0; i < _obj.Length; i++)
        {
            if (_obj[i].activeSelf == false)
                return _obj[i];
        }
        return null;
    }

}
