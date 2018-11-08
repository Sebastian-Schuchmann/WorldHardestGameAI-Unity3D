using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementDirection
{
    left,
    right,
    up,
    down
}

public class EnemyController : MonoBehaviour
{
    [Header("Settings")]
    public MovementDirection movementDirection;
    public float speed = 0.129f;
    //LEFT AND RIGHT
    public float minX = -3.76f;
    public float maxX = 3.76f;
    //UP AND DOWN
    public float minY = -3.76f;
    public float maxY = 3.76f;

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
        if (movementDirection == MovementDirection.up)
        {
            currentDirection = Vector2.up;
        }
        else if (movementDirection == MovementDirection.down)
        {
            currentDirection = Vector2.down;
        }
    }

    void FixedUpdate()
    {
        if (movementDirection == MovementDirection.left || movementDirection == MovementDirection.right)
        {
            if (transform.position.x < minX)
                SwitchDirection();
            if (transform.position.x > maxX)
                SwitchDirection();
        }

        if (movementDirection == MovementDirection.up || movementDirection == MovementDirection.down)
        {
            if (transform.position.y < minY)
                
                SwitchDirection();
            if (transform.position.y > maxY)
                SwitchDirection();
        }
        
        transform.position = transform.position + new Vector3(currentDirection.x * speed, currentDirection.y * speed, 0);
    }

    private void SwitchDirection()
    {
        if (currentDirection == Vector2.left)
            currentDirection = Vector2.right;
        else if (currentDirection == Vector2.right)
            currentDirection = Vector2.left;
        else if (currentDirection == Vector2.up)
            currentDirection = Vector2.down;
        else if (currentDirection == Vector2.down)
            currentDirection = Vector2.up;
    }

    public Vector2 GetCurrentDirection(){
        return currentDirection;
    }

}
