using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_W : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine("AutoDisable");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Monster_Slime")
        {
            Slime slime = other.transform.gameObject.GetComponent<Slime>();
            slime.Slime_Hit_Base_Attack(1);
        }
    }

    private IEnumerator AutoDisable()
    {
        yield return new WaitForSeconds(0.3f);
        gameObject.SetActive(false);
    }
}
