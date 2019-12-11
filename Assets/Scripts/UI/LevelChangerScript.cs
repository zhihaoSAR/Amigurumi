using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChangerScript : MonoBehaviour
{
    public Animator animator;

    private int levelLoaded;

    public void Update()
    {
       
    }

    public void FadeToLevel(int indexLevel)
    {
        levelLoaded = indexLevel;
        animator.SetTrigger("fadeOut");
        Time.timeScale = 1;
    }

    public void OnFadeComplete()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(levelLoaded);
    }
}
