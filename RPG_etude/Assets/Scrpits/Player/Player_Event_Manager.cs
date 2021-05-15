using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 
public class Player_Event_Manager : MonoBehaviour
{
    private static Player_Event_Manager instance = null;

    public UnityEvent onInputSpace;


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
            onInputSpace.Invoke();
        }
    }

    #region 이벤트

    #region 몬스터 처치
    public void Kill_Slime()
    {

    }
    #endregion


    #region 아이템 획득

    #endregion



    #endregion
}
