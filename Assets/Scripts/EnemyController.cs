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

    public Vector2 currentDirection;
    public float speed = 0.118f;
    public float minX = -3.76f;
    public float maxX = 3.76f;

    public MovementDirection movementDirection;

    // Use this for initialization
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

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.x < minX)
            switchDirection();
        if (transform.position.x > maxX)
            switchDirection();


        transform.position = transform.position + new Vector3(currentDirection.x * speed, currentDirection.y * speed, 0);
    }

    private void switchDirection()
    {
        if (currentDirection == Vector2.left)
            currentDirection = Vector2.right;
        else if (currentDirection == Vector2.right)
            currentDirection = Vector2.left;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 9)
        {

        }
    }
}
