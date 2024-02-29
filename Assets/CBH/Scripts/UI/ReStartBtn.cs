using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReStartBtn : UIAnimations
{
    private void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(Restart);
        StartCoroutine(SlideUpC(0.2f,30f));
    }


    void Restart()
    {
        Debug.Log("restart");
        //
    }
}
