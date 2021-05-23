using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeManager : MonoBehaviour
{
    [SerializeField]
    private float Spawn_time;
    [SerializeField]
    private float time;
    [SerializeField]
    private float Spawn_Range;
    [SerializeField]
    private Pooling_Manager pooling_Manager;

    private void Awake()
    {
        time = 0f;
    }

    private void Update()
    {
        time += Time.deltaTime;

        if(time >= Spawn_time)
        {
            Spawn();
            time = 0; 
        }
    }

    void Spawn()
    {
        GameObject slime = pooling_Manager.ReturnObject("슬라임");
        if (slime == null)
            return;
        slime.SetActive(true);
        slime.GetComponent<Slime>().Spawn();
        slime.transform.position = Spawn_Position();
    }
    Vector3 Spawn_Position()
    {
        float x = Random.Range(-Spawn_Range, Spawn_Range);
        float Max_Y = Mathf.Sqrt(Mathf.Pow(Spawn_Range, 2) - Mathf.Pow(x, 2));
        float y = Random.Range(-Max_Y, Max_Y);

        return new Vector3(transform.position.x + x, 0.5f, transform.position.z + y);
    }

    private void OnDrawGizmosSelected()
    {
        //생성 범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Spawn_Range);
    }
}
