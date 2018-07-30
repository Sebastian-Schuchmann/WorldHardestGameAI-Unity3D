using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAgents;

public class PlayerController : MonoBehaviour
{
    private const int ENEMY_LAYER = 10;
    private const int GOAL_LAYER = 8;

    public WHDAgent agent; 
    public float speed = 0.07f;
    public float minStepToImprove = 0.02f;
    public Text progressText;
    public Text failCounterText;
    private float lastProgress = 0.0f;
    private int failCounter = 0;

    public float HorizontalInput = 0.0f;
    public float VerticalInput = 0.0f;

    private Vector3 startPosition;

    // Use this for initialization
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        float movementX = HorizontalInput * speed;
        float movementY = VerticalInput * speed;

        transform.position = transform.position + new Vector3(movementX, movementY, 0);
        progressText.text = getProgress().ToString();
        failCounterText.text = failCounter.ToString();

        //Negative Reward for Time
        agent.AddReward(-0.001f);

        //Reward for moving towards the Goal
        if(getProgress() > lastProgress+minStepToImprove){
            agent.AddReward(0.02f);
            lastProgress = getProgress();
        } else if(getProgress() < lastProgress - minStepToImprove){
            agent.AddReward(-0.02f);
            lastProgress = getProgress();
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == ENEMY_LAYER){
            Kill();
            agent.AddReward(-1.0f);
            failCounter++;
        }
            
        if (collision.gameObject.layer == GOAL_LAYER){
            //WIN
            failCounter = 0;
            agent.AddReward(1.0f);
            Kill();
        }
            
    }

    public void Kill(){
        transform.position = startPosition;
    }

    public float getProgress(){
        float progress = (transform.position.x - startPosition.x)/11.5f;
        return Mathf.Clamp01(progress);
    }
}



