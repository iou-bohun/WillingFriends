using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeOverTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //To Do - 게임오버

        if (other.CompareTag("Player"))
        {
            Debug.Log("게임 오버!");
            GameManager.Instance.GameOver();
        }            
    }
}
