using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 playerPos;
    Vector2 grapplePos;
    LineRenderer grapple;
    bool grappling = false;
    GameObject grappledEnemy = null;
    float grappleAcceleration = 0.5f;
    float grappleAccelerationRate = 5f;

    public float grappleLength = 5f;
    public float grappleSpd = 25f;

    ParticleHandler ParticleHandler;
    public ParticleSystem enemyExplosion;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        grapple = GetComponent<LineRenderer>();
        ParticleHandler = FindObjectOfType<ParticleHandler>();
    }

    void Update()
    {
        playerPos = new Vector2(rb.transform.position.x, rb.transform.position.y);

        if (Input.GetMouseButtonDown(0)) //LMB clicked
        {
            Vector2 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CalculateGrapple(clickPos);
        }

        if (Input.GetMouseButtonUp(0)) //LMB released
        {
            ResetGrapple();
        }

        if (grappling)
        {
            grapple.SetPosition(0, playerPos);

            if (grappledEnemy != null)
            {
                grapplePos = grappledEnemy.transform.position;
                grapple.SetPosition(1, grapplePos);
            }
        }

    }

    void FixedUpdate()
    {
        if (grappling)
        {
            Vector2 grappleDir = CalculateGrappleDirection(grapplePos);

            if (grappledEnemy != null)
            {
                rb.velocity = grappleDir * grappleSpd * grappleAcceleration;

                float xDist = grapplePos.x - transform.position.x;
                float yDist = grapplePos.y - transform.position.y;
                float currentGrappleLength = Mathf.Sqrt((xDist * xDist) + (yDist * yDist));
                float enemyRadius = grappledEnemy.GetComponent<Collider2D>().bounds.extents.x;

                if (currentGrappleLength < (rb.velocity.magnitude * Time.deltaTime) + enemyRadius) // We are going to hit the enemy
                {
                    ParticleHandler.CreateParticleSystem(enemyExplosion, grapplePos);
                    Destroy(grappledEnemy);
                    ResetGrapple();
                }
                else grappleAcceleration += grappleAccelerationRate * Time.deltaTime;
            }
            else rb.AddForce(grappleDir * grappleSpd * 5);
        }
    }

    private void CalculateGrapple(Vector2 clickPos)
    {
        Vector2 grappleDir = CalculateGrappleDirection(clickPos) * grappleLength;

        RaycastHit2D hit = Physics2D.Raycast(playerPos, grappleDir, grappleLength);

        Debug.DrawRay(playerPos, grappleDir, Color.red, 1);

        if (hit)
        {
            if (hit.collider.tag == "Enemy") grappledEnemy = hit.collider.gameObject;
            grapplePos = hit.point;
            grappling = true;

            grapple.SetPosition(1, hit.point);
            grapple.enabled = true;
        }
    }

    private Vector2 CalculateGrappleDirection(Vector2 endPos)
    {
        Vector2 grappleDir = endPos - playerPos;
        float angle = Mathf.Atan2(grappleDir.y, grappleDir.x);
        grappleDir.x = Mathf.Cos(angle);
        grappleDir.y = Mathf.Sin(angle);

        return grappleDir;
    }

    private void ResetGrapple()
    {
        grappling = false;
        grapple.enabled = false;
        grappledEnemy = null;
        grappleAcceleration = 0.5f;
    }
}
