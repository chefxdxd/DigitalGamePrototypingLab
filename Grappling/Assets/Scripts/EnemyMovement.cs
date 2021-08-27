using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Rigidbody2D rb;
    float oscillation = 0f;
    public float oscillationRate = 1f;
    public float oscillationHeight = 1f;
    public float moveSpdX = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        oscillation += oscillationRate;
        if (oscillation >= 360f) oscillation -= 360f;
    }

    void FixedUpdate()
    {
        Vector2 finalPos = rb.position + CalculateMovement();
        rb.MovePosition(finalPos);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        moveSpdX *= -1f;
        oscillation -= 180f;
        if (oscillation < 0f) oscillation += 360f;
        oscillationRate *= -1;
    }

    private Vector2 CalculateMovement()
    {
        Vector2 movement;
        float moveSpdY = Mathf.Sin(oscillation * Mathf.Deg2Rad) * oscillationHeight * Mathf.Abs(oscillationRate);
        movement = new Vector2(moveSpdX * Time.deltaTime, moveSpdY * Time.deltaTime);

        return movement;
    }
}
