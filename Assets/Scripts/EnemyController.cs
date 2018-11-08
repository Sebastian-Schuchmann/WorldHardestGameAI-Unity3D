using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementDirection
{
    left,
    right
}

public class EnemyController : MonoBehaviour
{
    [Header("Settings")]
    public MovementDirection movementDirection;
    public float speed = 0.118f;
    public float minX = -3.76f;
    public float maxX = 3.76f;

    Vector2 currentDirection;

    void Start()
    {
        if (movementDirection == MovementDirection.left)
        {
            currentDirection = Vector2.left;
        }
        else if (movementDirection == MovementDirection.right)
        {
            currentDirection = Vector2.right;
        }
    }

    void FixedUpdate()
    {
        if (transform.position.x < minX)
            SwitchDirection();
        if (transform.position.x > maxX)
            SwitchDirection();
        
        transform.position = transform.position + new Vector3(currentDirection.x * speed, currentDirection.y * speed, 0);
    }

    private void SwitchDirection()
    {
        if (currentDirection == Vector2.left)
            currentDirection = Vector2.right;
        else if (currentDirection == Vector2.right)
            currentDirection = Vector2.left;
    }

    public Vector2 GetCurrentDirection(){
        return currentDirection;
    }

}
