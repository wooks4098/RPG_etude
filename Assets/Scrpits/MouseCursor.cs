using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    private Vector2 mousePos;
    public float MoveSpeed;
    public GameObject Player;


    void Update()
    {
        CameraMove();
        CameraPosReset();


    }

    void CameraMove()
    {
        mousePos.x = Input.mousePosition.x;
        mousePos.y = Input.mousePosition.y;

        if (mousePos.x < 0)
        {
            gameObject.transform.position += Vector3.left * MoveSpeed * Time.deltaTime;
        }
        if (mousePos.x > 1920)
        {
            gameObject.transform.position += Vector3.right * MoveSpeed * Time.deltaTime;

        }
        if (mousePos.y < 0)
        {
            gameObject.transform.position += new Vector3(0, 0, -1) * MoveSpeed * Time.deltaTime;

        }
        if (mousePos.y > 1080)
        {
            gameObject.transform.position += new Vector3(0, 0, 1) * MoveSpeed * Time.deltaTime;

        }
    }
    void CameraPosReset()
    {
        if (Input.GetKey(KeyCode.Space))
            gameObject.transform.position = new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z - 8.03f);
    }

}
