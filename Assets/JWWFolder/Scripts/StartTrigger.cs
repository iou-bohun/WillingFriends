using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Managers.Instance.CallInitEvent();
        //GameManager.Instance.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}