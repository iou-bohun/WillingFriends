using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Managers.Instance.CallInitEvent();
        //GameManager.Instance.Initialize();
        Destroy(gameObject);
    }
}
