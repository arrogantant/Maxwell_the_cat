using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GradientEffect : MonoBehaviour
{
    public Gradient gradient;
    public float gradientSpeed = 1.0f;
    private SpriteRenderer spriteRenderer;
    private float gradientTime;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Repeat the gradient over time
        gradientTime += Time.deltaTime * gradientSpeed;
        if (gradientTime >= 1.0f)
            gradientTime -= 1.0f;

        spriteRenderer.color = gradient.Evaluate(gradientTime);
    }
}