using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menupausa : MonoBehaviour
{
    public Canvas menu,ui;
    private bool pausaEnable = true;
    CanvasGroup uiGroup;
    // Start is called before the first frame update
    void Start()
    {
        menu.enabled = false;
        ui.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        uiGroup = ui.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!menu.enabled)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                pausa();
            }
        }

    }

    public void pausa()
    {
        if (pausaEnable)
        {
            menu.enabled = !menu.enabled;
            ui.enabled = !ui.enabled;
            pausaEnable = false;
            if (menu.enabled)
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                Time.timeScale = 0;
            }
            else
            {

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1;
            }

            StartCoroutine("hacerAnimacion", 1);
        }
       
    }

    public void cinematicMode(bool enter)
    {
        if (enter)
        {
            pausaEnable = false;
            StartCoroutine("fadeOutUI", 1f);
        }
        else
        {
            StartCoroutine("fadeInUI", 1f);
        }
        

    }
    IEnumerator fadeInUI(float second)
    {
        float time = 0;
        while (time < second)
        {
            time += Time.deltaTime;
            uiGroup.alpha = 1 * ( time / second);
            yield return null;
        }
        pausaEnable = true;
    }

    IEnumerator fadeOutUI(float second)
    {
        float time = 0;
        while (time < second)
        {
            time += Time.deltaTime;
            uiGroup.alpha = 1 * (second - time / second);
            yield return null;
        }

    }

    private IEnumerator hacerAnimacion(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        pausaEnable = true;
    }
}
