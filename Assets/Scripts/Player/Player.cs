using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Estado state = Estado.MOVE;

    //player
    public float cordura,energia;
    public Slider barra_cordura; // slider del ui para mostrar la cantidad de cordura
    private bool recibiendoDano = false;
    public Camera camera;

    // mover
    public float speed, runSpeed,horizontal, vertical,m_gravity = 10f;
    private CharacterController controller;
    public bool controllable = true;
    string lastAnimation = "parado";
    public Animator animator;
    

    //moverCamara
    public float speedH, speedV, //variable que controla la sensibilidad
                    limitUp, limitDown; // para controlar limite de la rotacion x 
    private float yaw = 0, pitch = 0;

    //controlar interaccion
    Ray ray;
    RaycastHit hit;
    GameObject obj;
    public float distance;
    Interactuable func;

    //variable para controlar empujar
    Vector3 limitMin, limitMax;
    float pushSpeed;
    Rigidbody pushObj;

    //valor entre 0-1
    //si el amigurumi esta dentro de (wMin,hMin) (wMax,hMax) recibe dano
    public float hMin, hMax, wMin, wMax;



    void Start()
    {
        controller = GetComponent<CharacterController>();
    }


    void FixedUpdate()
    {
        //obtener que ha visto el jugador
        ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, distance))
        {
            obj = hit.collider.gameObject;

            if (obj.CompareTag("KeyObject"))
            {
                if (Input.GetButtonDown("interactuar"))
                {
                    if (func == null)
                    {
                        func = obj.GetComponent<Interactuable>();
                    }
                    
                    if (func.interactuable())
                    {

                    }
                    Destroy(obj);
                }
            }
            else if (obj.CompareTag("Interactable"))
            {
                if (Input.GetButtonDown("interactuar") && state.Equals(Estado.MOVE))
                {
                    if (func == null)
                    {
                        func = obj.GetComponent<Interactuable>();
                    }
                    func.OnInteraction();
                }
            }
            else
            {
                func = null;
            }
        }



        //controlar la rotacion de la camara
        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");
        if (pitch < limitDown)
        {
            pitch = limitDown;
        }
        else if (pitch > limitUp)
        {
            pitch = limitUp;
        }

        camera.transform.eulerAngles = new Vector3(pitch, transform.eulerAngles.y, 0);

        if (controllable)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            //controlar la gravedad
            float moveY = 0;
            moveY -= m_gravity * Time.deltaTime;

            if (state.Equals(Estado.MOVE))
            {
                
                

                Vector3 movement = Quaternion.Euler(0, transform.eulerAngles.y, 0) *
                                new Vector3(horizontal, moveY, vertical);
                //si hay movimiento
                if (!Mathf.Approximately(horizontal, 0) || !Mathf.Approximately(vertical, 0))
                {
                    if (Input.GetButton("correr"))
                    {
                        setAnimation("caminar");
                        movement *= runSpeed;
                    }
                    else
                    {
                        setAnimation("gatear");
                    }

                }
                else
                {
                    setAnimation("parado");
                }
                //rotacion del player
                transform.eulerAngles = new Vector3(0, yaw, 0);
                //posicion del player
                controller.Move(movement * speed * Time.deltaTime);
            }
            else
            {
                if (Input.GetButton("interactuar"))
                {
                    Vector3 movement = Quaternion.Euler(0, transform.eulerAngles.y, 0) *
                                new Vector3(0, moveY, vertical);
                    if (Mathf.Approximately(vertical, 0))
                    {
                        animator.speed = 0;
                    }
                    else if (vertical < 0)
                    {
                        setAnimation("estirar");
                        animator.speed = 1;
                    }
                    else
                    {
                        setAnimation("empujar");
                        animator.speed = 1;
                    }
                    Vector3 moveStep = movement * pushSpeed * Time.deltaTime;
                    Vector3 objetivo = pushObj.transform.position + moveStep;
                    if (isGreaterOrEqual(objetivo, limitMin) && !isGreaterOrEqual(objetivo, limitMax))
                    {
                        controller.Move(moveStep);
                        pushObj.MovePosition(objetivo);
                    }
                }
                else
                {
                    controllable = false;
                    setAnimation("parado");
                    animator.speed = 1;
                    state = Estado.MOVE;
                    StartCoroutine(goToStartPos(transform.position, 0.2f));
                }
                
                
            }
            
        }

        //recupera corduro cuando no ve amigurumi
        if (!recibiendoDano)
        {
            cordura = cordura >= 995 ? 1000 : cordura+5;
            barra_cordura.value = cordura;
        }
        recibiendoDano = false;
    }

    bool isGreaterOrEqual(Vector3 v1, Vector3 v2)
    {
        return v1.x >= v2.x && v1.z >= v2.z;
    }

    public void recibirDano(Vector3 ePos)
    {
        Vector3 pos = camera.WorldToViewportPoint(ePos);
        if (pos.x < wMax && pos.x > wMin && pos.y > hMin && pos.y < hMax && pos.z >= 0)
        {
            cordura -= 150* Time.deltaTime;
            barra_cordura.value = cordura;
            recibiendoDano = true;
            if (cordura <= 0)
            {
                Debug.Log("GAME OVER");
            }
        }

    }

    //funcion para controlar la animacion
    public void setAnimation(string animation)
    {
        if (lastAnimation != "parado")
        {
            animator.SetBool(lastAnimation, false);
        }
        if (animation != "parado")
        {
            animator.SetBool(animation, true);
            lastAnimation = animation;
        }

    }

    public void empujar(Vector3 startPos,Rigidbody obj,Vector3 limitMin,Vector3 limitMax,float pushSpeed)
    {
        this.limitMin = limitMin;
        this.limitMax = limitMax;
        this.pushSpeed = pushSpeed;
        pushObj = obj;
        state = Estado.PUSH;
        controllable = false;
        setAnimation("empujar");
        StartCoroutine(goToStartPos(startPos, 0.2f));
    }
    IEnumerator goToStartPos(Vector3 pos,float seconds)
    {
        float time = 0;
        Vector3 movement = pos - transform.position;
        movement.y = 0;
        while(time < seconds)
        {
            time += Time.deltaTime;
            controller.Move(movement*(Time.deltaTime/seconds));
            yield return null;
        }
        controllable = true;
    }

    public void OnVerticalChanged(float v)
    {
        speedV = v;
    }
    public void OnHorizontalChanged(float v)
    {
        speedH = v;
    }
}
