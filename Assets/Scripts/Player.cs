using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float cordura,energia;
    public Slider barra_cordura;
    private bool recibiendoDano = false;
    //valor entre 0-1
    public float hMin, hMax, wMin, wMax;
    private Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        camera = transform.GetChild(0).GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    void FixedUpdate()
    {

        if (!recibiendoDano)
        {
            cordura = cordura >= 995 ? 1000 : cordura+5;
            barra_cordura.value = cordura;
        }
        recibiendoDano = false;
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
}
