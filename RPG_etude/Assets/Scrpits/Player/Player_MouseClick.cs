using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_MouseClick : MonoBehaviour
{
    private RaycastHit hitInfo;     //충동체 정보저장
    private RaycastHit Item_Info;     //아이템 충돌체 정보저장

    //아이템 레이어를 담을 레이어 변수
    [SerializeField]
    private LayerMask Item_LayerMask;

    private Camera camera;


    //스크립트
    [SerializeField] private Player_Fsm player;
    [SerializeField] private Store_Npc store_Npc;


    private void Awake()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        ClickCheck();
    }

    void ClickCheck()
    {
        if (Input.GetMouseButton(1))
        {
            if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hitInfo))
            {
                if (hitInfo.transform.tag == "Item")
                {   //아이템 획득
                    Item_Info = hitInfo;
                    player.ChangeMouseHit(Item_Info);
                    player.ChangeState((int)PLAYER_STATE.GetItem);
                }
                else if (hitInfo.transform.tag == "Monster_Slime")
                {   //기본공격
                    player.ChangeMouseHit(hitInfo);
                    player.ChangeState((int)PLAYER_STATE.Base_Attack);
                }
                else if(hitInfo.transform.tag == "Store_Npc" && !Inventory.inventoryOpened)
                {
                    store_Npc.Show_Store();
                }
                else
                {   //이동
                    player.ChangeState((int)PLAYER_STATE.ClickMove);
                    player.ChangeMouseHit(hitInfo);
                    player.Click_Move();

                }
            }
        }
    }
}
