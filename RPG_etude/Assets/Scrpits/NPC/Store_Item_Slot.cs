using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Store_Item_Slot : MonoBehaviour
{
    private Item item; //획득한 아이템
    [SerializeField] private Image itemImage; //아이템의 이미지
    [SerializeField] [TextArea] private Text Item_info;
    [SerializeField] Inventory inventory;

    private void Awake()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    public void Store_Item_Slot_Set(Item _item)
    {
        ItemSet(_item);
        ImageSet();
        ItemInfoSet();
    }

    void ImageSet()
    {
        itemImage.sprite = item.itemSprite;
    }

    void ItemSet(Item _item)
    {
        item = _item;
    }

    void ItemInfoSet()
    {
        Item_info.text = item.item_info;
    }

    public void Buy()
    {
        inventory.AcquireItem(item);
    }



}
