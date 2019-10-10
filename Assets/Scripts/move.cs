using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    public float speed,speedH,speedV,limitUp,limitDown;
    private float yaw = 0, pitch = 0;   
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");
        if(pitch < limitDown)
        {
            pitch = limitDown;
        }
        else if(pitch > limitUp)
        {
            pitch = limitUp;
        }

        transform.eulerAngles = new Vector3(pitch, yaw, 0);
        transform.GetChild(0).transform.eulerAngles = new Vector3(-pitch, 0, 0);

        //float dir = Vector3.Angle(tr.position, Vector3.forward);

        transform.position += Quaternion.Euler(0,transform.eulerAngles.y,0)* (movement * speed);
        transform.position = transform.position;

    }

}
