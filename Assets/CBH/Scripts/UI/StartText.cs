using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class StartText : UIAnimations
{
    public void OnMove()
    {
        StartCoroutine(SlideRightC());
        Destroy(gameObject, 3f);
    }

}
