using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour , IPointerClickHandler , IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 orginPos;


    public Item item; //획득한 아이템
    public int itemCount; //획득한 아이템의 개수
    public Image itemImage; //아이템의 이미지
    [SerializeField]
    private Text text_Count;
    [SerializeField]
    private GameObject text_Count_obj;
    [SerializeField]
    private Item_Use item_Use;
    [SerializeField]
    private Slot_info slot_info;
    private bool IS_Slot_info_Open; //Slot_info오픈가능한지 (마우스가 올라와있으면 true)
     private float Slot_info_Open_timeCount;
    private float Slot_info_Open_time = 0.7f;
    void awake()
    {

    }

    void Start()
    {
        orginPos = transform.position;
        item_Use = FindObjectOfType<Item_Use>();
        slot_info = FindObjectOfType<Slot_info>();
        Slot_info_Open_timeCount = 0;
    }

    void Update()
    {
        if(IS_Slot_info_Open)
        {
            Slot_info_Open_timeCount += Time.deltaTime;
            if(Slot_info_Open_timeCount >= Slot_info_Open_time)
            {
                slot_info.Show_Slot_info(item, this.transform.position);
                Slot_info_Open_timeCount = 0f;
                IS_Slot_info_Open = false;
            }
        }
    }

    //이미지 투명도 조절
    private void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    //아이템 획득
    public void AddItem(Item _item, int _Count = 1)
    {
        item = _item;
        itemCount = _Count;
        itemImage.sprite = item.itemSprite;
        text_Count_obj.SetActive(true);
        text_Count.text = itemCount.ToString();

        SetColor(1);
    }
    //아이템 개수 조정
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        text_Count.text = itemCount.ToString();
        if (itemCount <= 0)
            ClearSlot();
    }

    //슬롯 초기화
    private void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);

        text_Count.text = "0";
        text_Count_obj.SetActive(false);
    }
    #region 마우스 이벤트

    //마우스 클릭
    public void OnPointerClick(PointerEventData eventData)
    {
        if (item != null)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {//우클릭
                item_Use.Show_Item_Use((int)item.itemtype, this.transform.position);
                IS_Slot_info_Open = false;
                Slot_info_Open_timeCount = 0f;
                slot_info.Close_Slot_info();
            }
        }
    }

    //드래그 시작
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            if (eventData.button == PointerEventData.InputButton.Left
                && Item_Use.Item_UseOpened == false)
            {
                DragSlot.instance.dragSlot = this;
                DragSlot.instance.DragSetImage(itemImage);
                DragSlot.instance.transform.position = eventData.position;
            }
        }
    }

    //드래그 중
    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (item != null)
                DragSlot.instance.transform.position = eventData.position;
        }

    }

    //드래그 끝
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            DragSlot.instance.dragSlot = null;
            DragSlot.instance.SetColor(0);
        }

    }

    //자신 위에서 마우스 클릭이 끝났을 때
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (DragSlot.instance.dragSlot != null)
            {
                ChangeSlot();
            }
        }
    }

    void ChangeSlot()
    {
        Item _tempItem = item;
        int _tempItemCount = itemCount;

        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);
        if (_tempItem != null)
            DragSlot.instance.dragSlot.AddItem(_tempItem, _tempItemCount);
        else
            DragSlot.instance.dragSlot.ClearSlot();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null && Item_Use.Item_UseOpened == false)
        {
            //slot_info.Show_Slot_info(item);
            IS_Slot_info_Open = true;
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (item != null)
        {
            IS_Slot_info_Open = false;
            Slot_info_Open_timeCount = 0f;
            slot_info.Close_Slot_info();
        }
    }

    #endregion
}
