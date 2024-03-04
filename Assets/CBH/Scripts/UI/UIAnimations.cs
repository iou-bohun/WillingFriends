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


    protected IEnumerator SlideUpC(float duration, Vector2 endPos)
    {
        float elapsedTime = 0f;
        float percent = 0;
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 startPos = transform.localPosition;

        while (percent < 1)
        {
            elapsedTime += Time.deltaTime;
            percent = elapsedTime/ duration;

            rectTransform.anchoredPosition = Vector3.Lerp(startPos, endPos, percent);

            yield return null;
        }
    }
}
