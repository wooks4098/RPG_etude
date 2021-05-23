using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest_Npc : MonoBehaviour
{
    public static bool Ques_NpcOpened = false; //퀘스트을 열었는지

    [SerializeField] GameObject Quest_List_Contents;
    [SerializeField] GameObject Quest_List_Base;

    [SerializeField] GameObject Quest_Accept_Contents;
    [SerializeField] GameObject Quest_Accept_Base;

    [SerializeField] int Quest_Count;
    //퀘스트 목록
    [SerializeField] GameObject Quest_prefab;
    Quest_Slot[] quest_slot;
    private GameObject[] questt_obj;
    //퀘스트 진행도
    [SerializeField] GameObject accept_Quest_Slot_Prefab;
    private Accept_Quest_Slot[] accept_Quest_Slot;
    private GameObject[] accept_Quest_obj;
    private int accept_Quest_Slot_Count = 0;
    //퀘스트
    [SerializeField] Kill_Slime_Quest Quest_kill_slime;

    private void Awake()
    {
        Quest_Set();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Quest_List_Base.activeSelf)
            Close_Quest_List();
    }

    public void Show_Quest_List()
    {
        Ques_NpcOpened = true;
        Quest_List_Base.SetActive(true);
    }

    public void Close_Quest_List()
    {
        Ques_NpcOpened = false;
        Quest_List_Base.SetActive(false);

    }


    public void Clear_Quest(string _Quest_Name)
    {
        int Quest_Number = Return_Quest_Number(_Quest_Name);
        quest_slot[Quest_Number].Clear_Quest();
    }

    int Return_Quest_Number(string _Quest_Name)
    {
        switch (_Quest_Name)
        {
            case "Kill_Slime_Quest":
                return 0;

        }



        return -1;
    }

    public void EndCheck(string _Quest_Name)
    { 
        for(int i = 0;i <Quest_Count; i++)
        {
            if(_Quest_Name == accept_Quest_Slot[i].Return_Name().ToString())
            {
                accept_Quest_obj[i].SetActive(false);
            }
        }
    }

    public void Quest_Set()
    {
        questt_obj = new GameObject[Quest_Count];
        quest_slot = new Quest_Slot[Quest_Count];
        accept_Quest_Slot = new Accept_Quest_Slot[Quest_Count];
        accept_Quest_obj = new GameObject[Quest_Count];

        for (int i = 0; i < Quest_Count; i++)
        {
            questt_obj[i] = Instantiate(Quest_prefab, Quest_List_Contents.transform);
            quest_slot[i] = questt_obj[i].GetComponent<Quest_Slot>();
            quest_slot[i].EndCheck.AddListener(EndCheck);

            accept_Quest_obj[i] = Instantiate(accept_Quest_Slot_Prefab, Quest_Accept_Contents.transform);
            accept_Quest_Slot[i] = accept_Quest_obj[i].GetComponent<Accept_Quest_Slot>();

        }
        quest_slot[0].Return_Button().onClick.AddListener(Quest_kill_slime.Acceptance_Quest);
        quest_slot[0].Set_Quest_Slot(Quest_kill_slime.Quest_Name, Quest_kill_slime.Quest_info);
        Quest_kill_slime.Set_Quest_Accept_slot(accept_Quest_obj[0], accept_Quest_Slot[0].Return_Name(), accept_Quest_Slot[0].Return_info());

    }
}
