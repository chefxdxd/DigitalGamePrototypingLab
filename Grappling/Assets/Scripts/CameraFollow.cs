using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Rigidbody2D target;
    public Collider2D myCollider;
    public Vector2 focusAreaSize;
    public float verticalOffset;
    public float lookAheadDistX;
    public float lookSmoothTimeX;
    public float verticalSmoothTime;

    FocusArea focusArea;

    float currentLookAheadX;
    float targetLookAheadX;
    float lookAheadDirX;
    float smoothLookVelocityX;
    float smoothVelocityY;
    bool lookAheadStopped;

    void Start()
    {
        focusArea = new FocusArea(myCollider.bounds, focusAreaSize);
    }

    void LateUpdate()
    {
        focusArea.Update(myCollider.bounds);

        Vector2 focusPosition = focusArea.centre + Vector2.up * verticalOffset;

        if (focusArea.velocity.x != 0)
        {
            lookAheadDirX = Mathf.Sign(focusArea.velocity.x);
            if ((Mathf.Sign(target.velocity.x) == Mathf.Sign(focusArea.velocity.x)) && target.velocity.x != 0)
            {
                targetLookAheadX = lookAheadDirX * lookAheadDistX;
                lookAheadStopped = false;
            }
            else
            {
                if (!lookAheadStopped)
                {
                    targetLookAheadX = currentLookAheadX + (lookAheadDirX * lookAheadDistX - currentLookAheadX) / 4f;
                    lookAheadStopped = true;
                }
            }
        }

        currentLookAheadX = Mathf.SmoothDamp(currentLookAheadX, targetLookAheadX, ref smoothLookVelocityX, lookSmoothTimeX);
        focusPosition += Vector2.right * currentLookAheadX;
        focusPosition.y = Mathf.SmoothDamp(transform.position.y, focusPosition.y, ref smoothVelocityY, verticalSmoothTime);

        transform.position = (Vector3)focusPosition + Vector3.forward * -10;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(focusArea.centre, focusAreaSize);
    }

    struct FocusArea
    {
        public Vector2 centre;
        public Vector2 velocity;
        float left, right, top, bottom;

        public FocusArea(Bounds targetBounds, Vector2 size)
        {
            left = targetBounds.center.x - size.x / 2;
            right = targetBounds.center.x + size.x / 2;
            bottom = targetBounds.min.y;
            top = targetBounds.min.y + size.y;

            velocity = Vector2.zero;
            centre = new Vector2((left + right) / 2, (top + bottom) / 2);
        }

        public void Update(Bounds targetBounds)
        {
            float shiftX = 0;
            if (targetBounds.min.x < left) shiftX = targetBounds.min.x - left;
            else if (targetBounds.max.x > right) shiftX = targetBounds.max.x - right;
            left += shiftX;
            right += shiftX;

            float shiftY = 0;
            if (targetBounds.min.y < bottom) shiftY = targetBounds.min.y - bottom;
            else if (targetBounds.max.y > top) shiftY = targetBounds.max.y - top;
            top += shiftY;
            bottom += shiftY;

            centre = new Vector2((left + right) / 2, (top + bottom) / 2);
            velocity = new Vector2(shiftX, shiftY);
        }
    }
}