using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeOverTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("게임 오버!");
            GameManager.Instance.GameOver();
        }            
    }
}
