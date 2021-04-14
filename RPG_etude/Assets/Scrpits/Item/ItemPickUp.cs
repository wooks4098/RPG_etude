using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPickUp : MonoBehaviour
{
    [SerializeField]
    private GameObject Show_Item_Name_obj;
    [SerializeField]
    private Text Show_Item_Name_text;

    //아이템 레이어를 담을 레이어 변수
    [SerializeField]
    private LayerMask Item_LayerMask;

    private bool PickUpActivate = false;        //습득 가능할 시 true;
    private RaycastHit hitInfo;     //충동체 정보저장



    [SerializeField]
    private Inventory inventory;
    private Camera camera;

    private void Awake()
    {
        camera = Camera.main;


    }
    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 20, Color.red);
        if (Physics.Raycast(ray.origin, ray.direction * 20, out hitInfo, Item_LayerMask))
        {
            if (hitInfo.transform.tag == "Item")
                Item_Name_Text_Appear(hitInfo);
            else
                Item_Name_Text_Disappear();
        }
    }

    public void Get_Item(RaycastHit _hitInfo)
    {
        if (_hitInfo.transform != null)
        {
            if (inventory.AcquireItem(_hitInfo.transform.GetComponent<Item_Info>().item))
            {
                _hitInfo.transform.gameObject.SetActive(false);
                Item_Name_Text_Disappear();
            }
            else
            {
                Debug.Log("인벤토리가 꽉찼습니다.");
                //StartCoroutine("Item_Full_Text_Show");
            }

        }
    }


    //아이템 이름 보여주기
    public void Item_Name_Text_Appear(RaycastHit _hitInfo)
    {
        hitInfo = _hitInfo;
        PickUpActivate = true;
        Show_Item_Name_obj.SetActive(true);
        Show_Item_Name_obj.gameObject.transform.position = new Vector3(Camera.main.WorldToScreenPoint(hitInfo.transform.position).x,
        Camera.main.WorldToScreenPoint(hitInfo.transform.position).y - 100f, Camera.main.WorldToScreenPoint(hitInfo.transform.position).z);
        Show_Item_Name_text.text = hitInfo.transform.GetComponent<Item_Info>().item.itemName;

    }

    //아이템 이름 숨기기
    public void Item_Name_Text_Disappear()
    {
        PickUpActivate = false;
        Show_Item_Name_obj.gameObject.SetActive(false);
    }
}
