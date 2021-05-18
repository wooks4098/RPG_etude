using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

[System.Serializable]
public class Kill_Monster : UnityEvent<string>
{

}

public class Player_Event_Manager : MonoBehaviour
{
    private static Player_Event_Manager instance = null;

    public Kill_Monster kill_Monster;

    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }

    public static Player_Event_Manager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //onInputSpace.Invoke();
        }
    }

    #region 이벤트

    #region 몬스터 처치
    public void Kill_Slime()
    {
        kill_Monster.Invoke("Slime");
        Debug.Log("슬라임 처치");
    }

    public void Kill_otherMonster()
    {
        kill_Monster.Invoke("otherMonster");
    }
    #endregion


    #region 아이템 획득

    #endregion



    #endregion
}
