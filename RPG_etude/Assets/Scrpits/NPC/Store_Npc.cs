using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store_Npc : MonoBehaviour
{
    public static bool Store_NpcOpened = false; //상점을 열었는지

    [SerializeField] Item[] item;
    [SerializeField] GameObject Store_item_prefab;
    private Store_Item_Slot[] Item_Slot;
    private GameObject[] Item_Slot_obj;

    [SerializeField] GameObject Contents;
    [SerializeField] GameObject StoreBase;

    private void Awake()
    {
        Item_Slot_obj = new GameObject[item.Length];
        Item_Slot = new Store_Item_Slot[item.Length];

        for (int i = 0; i< item.Length; i++)
        {
            Item_Slot_obj[i] = Instantiate(Store_item_prefab, Contents.transform);
            Item_Slot[i] = Item_Slot_obj[i].GetComponent<Store_Item_Slot>();
            Item_Slot[i].Store_Item_Slot_Set(item[i]);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && StoreBase.activeSelf)
            Close_Store();
    }

    public void Show_Store()
    {
        Store_NpcOpened = true;
        StoreBase.SetActive(true);
    }

    public void Close_Store()
    {
        Store_NpcOpened = false;
        StoreBase.SetActive(false);

    }
}
