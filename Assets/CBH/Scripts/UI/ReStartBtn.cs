using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReStartBtn : UIAnimations
{
    private Animator animator;
    private void Start()
    {        
        animator = GetComponent<Animator>();    
        Button button = GetComponent<Button>();
        button.onClick.AddListener(Restart);
        StartCoroutine(SlideUpC(0.2f, Vector2.zero));
    }

    void Restart()
    {
        Debug.Log("restart");
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void SelectAnim()
    {
        animator.SetTrigger("Selected");
    }
}
