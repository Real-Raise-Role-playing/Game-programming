using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Curve : MonoBehaviour
{
    public AnimationCurve curve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
    Vector3 startPosition;
    Vector3 targetPosition;
    float timer;
    float duration;

    void Start()
    {
        timer = 0;
        duration = 5;
        startPosition = transform.localPosition;
        targetPosition = transform.localPosition + new Vector3(4, 6, 0);
    }

    void Update()
    {
        transform.localPosition = Vector3.Lerp(startPosition, targetPosition, curve.Evaluate(timer / duration));
        timer += Time.deltaTime;
    }
}
