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
    public float speed, runSpeed;
    float horizontal, vertical, m_gravity = 10f;
    private CharacterController controller;
    public bool controllable = true;
    string lastAnimation = null,lastNivel = "parado";
    public Animator animator;
    bool enPie = false;

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
    public Text text_Interactuar;

    //variable para controlar empujar
    bool limited;
    Vector3 rail,limitMin,limitMax; 
    float pushSpeed;
    Rigidbody pushObj;
    Vector3 startPos;

    //valor entre 0-1
    //si el amigurumi esta dentro de (wMin,hMin) (wMax,hMax) recibe dano
    public float hMin, hMax, wMin, wMax;



    void Start()
    {
        controller = GetComponent<CharacterController>();
    }


    void FixedUpdate()
    {
        if (state.Equals(Estado.MOVE))
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
                    text_Interactuar.enabled = true;
                    if (Input.GetButtonDown("interactuar") && state.Equals(Estado.MOVE))
                    {
                        if (func == null)
                        {
                            func = obj.GetComponent<Interactuable>();
                        }
                        //calcular donde empieza a empujar
                        Vector3 dir = (hit.point - transform.position).normalized;
                        startPos = hit.point - dir*2f;
                        startPos.y = transform.position.y;
                        func.OnInteraction();
                    }
                }

            }
            else
            {
                func = null;
                text_Interactuar.enabled = false;
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
        
        

        if (controllable)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            //controlar la gravedad
            float moveY = 0;
            moveY -= m_gravity * Time.deltaTime;


            //comprobar estado
            if (state.Equals(Estado.MOVE))
            {


                Vector3 movement = Quaternion.Euler(0, transform.eulerAngles.y, 0) *
                                new Vector3(horizontal, moveY, vertical);


                if (Input.GetButton("correr"))
                {
                    if (!enPie)
                    {
                        ponerEnPie(true);
                    }
                    enPie = true;
                }
                else
                {
                    if (enPie)
                    {
                        ponerEnPie(false);
                    }
                    enPie = false;

                }

                

                //si hay movimiento
                if (!Mathf.Approximately(horizontal, 0) || !Mathf.Approximately(vertical, 0))
                {
                    if (enPie)
                    {
                        setAnimation("enPie","caminar");
                        movement *= runSpeed;
                    }
                    else
                    {
                        setAnimation("parado","gatear");
                    }

                }
                else
                {
                    if (enPie)
                    {
                        setAnimation("enPie", null);
                    }
                    else
                    {
                        setAnimation("parado", null);
                    }
                    
                }
                //rotacion del player
                
                camera.transform.eulerAngles = new Vector3(pitch, transform.eulerAngles.y, 0);
                transform.eulerAngles=new Vector3(0, yaw, 0);

                //posicion del player
                controller.Move(movement * speed * Time.deltaTime);
            }
            else if(state.Equals(Estado.PUSH))
            {
                camera.transform.eulerAngles = new Vector3(pitch, yaw, 0);
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
                        setAnimation("enPie","tirar");
                        animator.speed = 1;
                    }
                    else
                    {
                        setAnimation("enPie","empujar");
                        animator.speed = 1;
                    }
                    Vector3 moveStep = movement * pushSpeed * Time.deltaTime;
                    Vector3 objetivo;
                    if (limited)
                    {

                        moveStep = Vector3.Project(moveStep, rail);
                        
                        objetivo = pushObj.position + moveStep;
                        if (isGreaterOrEqual(objetivo, limitMax))
                        {
                            objetivo = limitMax;
                        }
                        else if (isGreaterOrEqual(limitMin, objetivo))
                        {
                            objetivo = limitMin;
                        }

                    }
                    else
                    {
                        objetivo = pushObj.position + moveStep;
                        
                    }

                    Vector3 lastPosicion = pushObj.position;
                    pushObj.MovePosition(objetivo);
                    controller.Move(pushObj.position - lastPosicion);
                }
                else
                {
                    controllable = false;
                    setAnimation("parado",null);
                    animator.speed = 1;
                    state = Estado.MOVE;
                    StartCoroutine(goToStartPos(0.2f,transform.position));
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
    public void setAnimation(string nivel,string animation)
    {
        if(nivel != lastNivel)
        {
            if (!lastNivel.Equals("parado"))
            {
                animator.SetBool(lastNivel, false);
            }
            if (!nivel.Equals("parado"))
            {
                animator.SetBool(nivel, true);
            }
        }
        if (animation != lastAnimation)
        {
            if(lastAnimation != null)
            {
                animator.SetBool(lastAnimation, false);
            }
            if(animation != null)
            {
                animator.SetBool(animation, true);
            }
        }
        lastNivel = nivel;
        lastAnimation = animation;

    }

    public void empujar(Rigidbody obj,Transform limitMin,Transform limitMax,float pushSpeed)
    {
        if(limitMax)
        {
            limited = true;
            rail = limitMax.position - limitMin.position;
            this.limitMax = limitMax.position;
            this.limitMin = limitMin.position;
        }
        else
        {
            limited = false;
        }
        
        
        this.pushSpeed = pushSpeed;
        pushObj = obj;
        if (!enPie)
        {
            enPie = true;
            ponerEnPie(true);
        }
        
        state = Estado.PUSH;
        setAnimation("enPie","empujar");
        StartCoroutine(goToStartPos( 0.5f, startPos));
    }
    IEnumerator goToStartPos( float seconds, Vector3 pos)
    {
        float time = 0;
        controllable = false;
        Vector3 movement = Vector3.zero;
        movement = pos - transform.position;
        movement.y = 0;



        while (time < seconds)
        {
            time += Time.deltaTime;
            controller.Move(movement * (Time.deltaTime / seconds));

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

    //0: gater 1: en pie
    void setCollider(int forma)
    {
        if(forma == 1)
        {

            
            controller.height = 3.5f;
            controller.center = new Vector3(0, -3f, 0f);
            
            

        }
        else
        {
            
            controller.height = 1.7f;
            controller.center = new Vector3(0, -3.8f, 3f);


        }
    }

    //true: cambia la formar de idlegatear a idledepie
    void ponerEnPie(bool enpie)
    {
        if (enpie)
        {
            Vector3 deltaPos = Quaternion.Euler(0, transform.eulerAngles.y, 0) * new Vector3(0, 0, 3.5f);
            //controller.Move(deltaPos);
            setAnimation("enPie", null);
            StartCoroutine(ponerEnPie(0.4f, 1,deltaPos));
            
   
        }
        else
        {
            Vector3 deltaPos = Quaternion.Euler(0, transform.eulerAngles.y, 0) * new Vector3(0, 0, -3.5f);
            //controller.Move(deltaPos);
            setAnimation("parado", null);
            StartCoroutine(ponerEnPie(0.4f,0,deltaPos));

    
        }
    }

    IEnumerator ponerEnPie(float seconds,int forma,Vector3 deltaPos)
    {
        controllable = false;
        float time = 0;
        Vector3 now,offset;
        deltaPos.y = -10;
        //float deltaHeight;
        if(forma == 0)
        {
            controller.height = 1.7f;
            now = new Vector3(0, -3f, 0f);
            offset = new Vector3(0, -3.8f, 3f) - now;
        }
        else
        {
            controller.height = 3.5f;
            now = new Vector3(0, -3.8f, 3f);
            offset = new Vector3(0, -3f, 0f) - now;
        }

        while (time < seconds)
        {
            time += Time.deltaTime;
            float porcion = time / seconds;

            controller.Move(deltaPos * (Time.deltaTime/seconds));
            //controller.height += deltaHeight * Time.deltaTime;
            controller.center = now + offset * porcion;


            yield return null;
        }
        if (forma == 1)
        {
            controller.center = new Vector3(0, -3f, 0f);
        }
        else
        {
            controller.center = new Vector3(0, -3.8f, 3f);
        }

        controllable = true;
    }
}
