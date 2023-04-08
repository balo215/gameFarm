using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.3f;
    private Vector2 velocity = Vector2.zero;

    private void FixedUpdate(){
        Vector2 targetPosition = new Vector2(target.position.x, target.position.y);
        Vector2 smoothPosition = Vector2.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        transform.position = new Vector3(smoothPosition.x, smoothPosition.y, transform.position.z);
    }
}
