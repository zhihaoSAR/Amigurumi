using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    public float speed;
    public CharacterController controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        
        float horizontal = Input.GetAxis("Horizontal"); 
        float vertical = Input.GetAxis("Vertical");
        
        float moveY = 0, m_gravity = 10f;
        moveY -= m_gravity * Time.deltaTime;

        Vector3 movement = Quaternion.Euler(0, transform.eulerAngles.y, 0) *
                        new Vector3(horizontal, moveY, vertical);
        controller.Move(movement * speed * Time.deltaTime);

    }
}
