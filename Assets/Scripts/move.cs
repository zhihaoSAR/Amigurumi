using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    public float speed,speedH,speedV;
    public GameObject light;
    private float yaw = 0, pitch = 0;   
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(pitch, yaw, 0);
        light.transform.eulerAngles = transform.eulerAngles;

        //float dir = Vector3.Angle(tr.position, Vector3.forward);

        transform.position += Quaternion.Euler(0,transform.eulerAngles.y,0)* (movement * speed);
        light.transform.position = transform.position;

    }

}
