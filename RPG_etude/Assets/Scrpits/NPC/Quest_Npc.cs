using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest_Npc : MonoBehaviour
{
    public static bool Ques_NpcOpened = false; //퀘스트을 열었는지

    [SerializeField] GameObject Contents;
    [SerializeField] GameObject QuestBase;
    [SerializeField] GameObject Quest_prefab;
    Quest_Slot[] quest_slot;
    private GameObject[] questt_obj;
    [SerializeField] int Quest_Count;
    //퀘스트
    [SerializeField] Kill_Slime_Quest Quest_kill_slime;

    private void Awake()
    {
        questt_obj = new GameObject[Quest_Count];
        quest_slot = new Quest_Slot[Quest_Count];
        for (int i = 0; i < Quest_Count; i++)
        {
            questt_obj[i] = Instantiate(Quest_prefab, Contents.transform);
            quest_slot[i] = questt_obj[i].GetComponent<Quest_Slot>();
        }
        quest_slot[0].Return_Button().onClick.AddListener(Quest_kill_slime.Acceptance_Quest);
        quest_slot[0].Set_Quest_Slot(Quest_kill_slime.Quest_Name, Quest_kill_slime.Quest_info);
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && QuestBase.activeSelf)
            Close_Quest();
    }

    public void Show_Quest()
    {
        Ques_NpcOpened = true;
        QuestBase.SetActive(true);
    }

    public void Close_Quest()
    {
        Ques_NpcOpened = false;
        QuestBase.SetActive(false);

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
}
