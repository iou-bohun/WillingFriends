using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimations : MonoBehaviour
{
    protected IEnumerator SlideRightC()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        while (true)
        {
            transform.position += Vector3.right * 0.5f;
            yield return null;
        }
    }


    protected IEnumerator SlideUpC(float duration, float speed)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            transform.position += Vector3.up * speed;
            yield return null;
        }
    }
}
