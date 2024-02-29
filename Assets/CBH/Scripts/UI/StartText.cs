using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StartText : MonoBehaviour
{
    public void OnMove()
    {

        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        while (true)
        {
            transform.position += Vector3.right*0.5f;
            yield return null;
        }
    }
}
