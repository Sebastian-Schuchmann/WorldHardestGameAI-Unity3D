using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public float speed = 1.0f;
    public float minStepToImprove = 0.02f;
    public Text progressText;

    private Vector3 startPosition;
    private const int ENEMY_LAYER = 10;
    // Use this for initialization
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float movementX = Input.GetAxis("Horizontal") * speed;
        float movementY = Input.GetAxis("Vertical") * speed;

        transform.position = transform.position + new Vector3(movementX, movementY, 0);
        progressText.text = getProgress().ToString();
        //GetComponent<Rigidbody2D>().AddForce(new Vector2(movementX, movementY));

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == ENEMY_LAYER)
            Kill();
    }

    public void Kill(){
        transform.position = startPosition;
    }

    public float getProgress(){
        float progress = (transform.position.x - startPosition.x)/11.5f;
        return Mathf.Clamp01(progress);
    }
}
