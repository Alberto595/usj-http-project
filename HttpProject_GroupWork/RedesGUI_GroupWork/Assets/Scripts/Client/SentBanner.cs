using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SentBanner : MonoBehaviour
{
    bool isMoving = false;
    public float maxTime = 0.5f;
    public float stopTime = 0.1f;
    public Vector2 startPos;
    public Vector2 middlePos;
    public Vector2 finalPos;
    public AnimationCurve enter;
    public AnimationCurve exit;
    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
    public void Appear()
    {
        if (isMoving) return;
        StartCoroutine(AppearCoroutine());
    }

    private IEnumerator AppearCoroutine()
    {
        isMoving = true;

        rect.anchoredPosition = startPos;

        float timer = 0.0f;
        while(timer <= maxTime)
        {
            timer += Time.deltaTime;
            
            rect.anchoredPosition = Vector2.Lerp(startPos, middlePos, enter.Evaluate(timer / maxTime));
            yield return null;
        }

        rect.anchoredPosition = middlePos;
        yield return new WaitForSeconds(stopTime);

        timer = 0.0f;
        while (timer <= maxTime)
        {
            timer += Time.deltaTime;

            rect.anchoredPosition = Vector3.Lerp(middlePos, finalPos, exit.Evaluate(timer / maxTime));
            yield return null;
        }

        rect.anchoredPosition = finalPos;
        isMoving = false;
    }

}
