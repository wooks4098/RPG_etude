using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool inventoryOpened = false; //인벤토리를 열었는지


    //필요한 컴포넌트
    [SerializeField]
    private GameObject Inventory_base;
    [SerializeField]
    private GameObject Etc_SlotsParent;
    //슬롯
    private Slot[] Etc_slots;
    private void Awake()
    {
        Etc_slots = Etc_SlotsParent.GetComponentsInChildren<Slot>();

    }
    private void Update()
    {
        TryOpenInventory();
    }

    void TryOpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I) && !Store_Npc.Store_NpcOpened)
        {
            inventoryOpened = !inventoryOpened;

            if (inventoryOpened)
                OpenInventory();
            else
                CloseInventory();


        }
    }

    //아이템 추가
    public bool AcquireItem(Item _item, int _count = 1)
    {
        //같은 아이템이 있다면 증가
        for (int i = 0; i < Etc_slots.Length; i++)
        {
            if (Etc_slots[i].item != null)
            {
                if (Etc_slots[i].item.itemName == _item.itemName)
                {
                    Etc_slots[i].SetSlotCount(_count);
                    return true;
                }
            }

        }

        //해당 아이템이 없다면 새로 등록
        for (int i = 0; i < Etc_slots.Length; i++)
        {
            if (Etc_slots[i].item == null)
            {
                Etc_slots[i].AddItem(_item, _count);
                return true;
            }
        }
        return false;
    }


    //인벤토리 열기
    void OpenInventory()
    {
        Inventory_base.SetActive(true);


    }

    //인벤토리 닫기
    void CloseInventory()
    {
        Inventory_base.SetActive(false);

    }
}
