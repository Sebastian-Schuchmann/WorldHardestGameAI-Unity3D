using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementDirection{
    left,
    right
}

public class EnemyController : MonoBehaviour {

    private Vector2 currentDirection;
    public float speed = 0.118f;

    public MovementDirection movementDirection;

	// Use this for initialization
	void Start () {
        if(movementDirection == MovementDirection.left){
            currentDirection = Vector2.left;
        } else if(movementDirection == MovementDirection.right){
            currentDirection = Vector2.right;
        }
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Input.GetKeyDown(KeyCode.U))
            transform.position = new Vector3(0, 0, -1);

        transform.position = transform.position + new Vector3(currentDirection.x*speed, currentDirection.y*speed, 0);
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 9)
        {
            if (currentDirection == Vector2.left)
                currentDirection = Vector2.right;
            else if (currentDirection == Vector2.right)
                currentDirection = Vector2.left;
        }
    }
}
