using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Slot_info : MonoBehaviour
{
    [SerializeField]
    private GameObject Slot_info_base;

    [SerializeField]
    private Text ItemName;
    [SerializeField]
    private Text ItemType;
    [SerializeField]
    private Text ItemInfo;


    public void  Show_Slot_info(Item _item, Vector3 _Pos)
    {
        Slot_info_base.SetActive(true);

        _Pos += new Vector3(Slot_info_base.GetComponent<RectTransform>().rect.width * 0.8f, -Slot_info_base.GetComponent<RectTransform>().rect.height * 0.8f, 0f);
        Slot_info_base.transform.position = _Pos;

        ItemName.text = _item.itemName;
        ItemType.text = ItemType_set((int)_item.itemtype);
        ItemInfo.text = _item.item_info;

    }

    public void Close_Slot_info()
    {
        Slot_info_base.SetActive(false);
    }

    string ItemType_set(int _item_type)
    {
        switch(_item_type)
        {
            case (int)Item.ITEMTYPE.Using:
                return "물약";
        }

        return "";
    }

}
