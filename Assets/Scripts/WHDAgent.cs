using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using UnityEngine.UI;
using System;

public class WHDAgent : MonoBehaviour
{
    public event System.Action dying;

    [Header("Data Logging")]
    public bool LogDataToFirebase;
    public Logger Logger;

    [Header("Agent Settings")]
    public float speed = 0.055f; //0.055 is as close to the original speed I could get, should be pretty accurate
    [Range(1.0f, 120.0f)]
    public float timeToDie = 30.0f;
    public bool trainWithCamera;
    public bool debugMode;

    [Header("References")]
    public GameObject goalArea;
    public EnemyManager enemyManager;
    public Text progressText;
    public Text failCounterText;

    double[] CurrentInputs;

    public bool moveEnabled = true;

    int failCounter;
    float lastProgress;
    float highestDist;
    float timeToLive;
    Vector3 startPosition;

    Vector2[] directions = {
            Vector2.left,
            Vector2.right,
            Vector2.up,
            Vector2.down,
            new Vector2(0.5f, 0.5f),
            new Vector2(-0.5f, 0.5f),
            new Vector2(0.5f, -0.5f),
            new Vector2(-0.5f, -0.5f)};

    LayerMask rayObservationMask;


   public void Awake()
  {
        timeToLive = timeToDie;
        highestDist = GetProgress();

        startPosition = transform.position;

        //The raycast used in observations shouldnt hit the player or enemies
        //They are just used to detect walls
        rayObservationMask = ~(LayerMask.GetMask("Player"));
    }


    public void AgentReset()
    {
        dying();
        highestDist = 10000.0f;
        timeToLive = Time.time + timeToDie;
        resetPosition();
    }

    public void resetPosition(){
        transform.position = startPosition;  
    }

    public void restart()
    {
        moveEnabled = true;
    }


    public double[] CollectObservations()
{
        
        double[] obs = new double[11];
        obs[0] = transform.localPosition.x / 14.0f;
        obs[1] = transform.localPosition.y / 14.0f;
        obs[2] = Vector2.Distance(transform.localPosition, goalArea.transform.localPosition) / 14.0f;

        double[] rays = CollectRayInformation();
        for (int i = 3; i < 11; i++){
           obs[i] = rays[i - 3];
        }
        return obs;
}

    public void Stop(){
        moveEnabled = false;
    }

    //Shots out rays and collects distance to the hit points
    public double[] CollectRayInformation()
    {
        double[] observations = new double[directions.Length];

        for (int i = 0; i < directions.Length; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directions[i], Mathf.Infinity, rayObservationMask);
            observations[i] = hit.distance;
           // Debug.Log("Hitpoint!" + hit.point.x);
          //  Debug.DrawLine(transform.position, new Vector3(hit.point.x, hit.point.y, 0 ), Color.blue);
        
        }

        return observations;
    }

    public double[] getCurrentInputs(){
        return CurrentInputs;
    }

    public void SetInputs(double[] input)
    {
        if(moveEnabled){

            //if (timeToLive < Time.time)
            //{
            //    failCounter++;
            //    AddReward(-1.0f);
            //    AgentReset();
            //}
            
            ////Negative Reward for Time
            //AddReward(-0.01f/timeToDie);
            CurrentInputs = input;
            
            double HorizontalInput = input[0];
            double VerticalInput = input[1];
            
            //Apply movement
            double movementX = HorizontalInput * speed;
            double movementY = VerticalInput * speed;
            transform.position = transform.position + new Vector3((float)movementX, (float)movementY, 0);
            
            //Update last Progress and highest Distance
            UpdateProgress();
        }
    }

    void UpdateProgress()
    {
        if (GetProgress() > lastProgress)
        {
            lastProgress = GetProgress();
        }
        else if (GetProgress() < highestDist)
        {
            highestDist = GetProgress();
            lastProgress = GetProgress();
        }
        else
        {
            lastProgress = GetProgress();
        }

//        failCounterText.text = failCounter.ToString();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
          //  AddReward(-1.0f);
            AgentReset();
            failCounter++;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Goal"))
        {
            failCounter = 0;
         //   AddReward(1.0f);
            AgentReset();
        }
    }

    public float GetProgress()
    {
        
        return Vector2.Distance(transform.position, goalArea.transform.position);
    }
}
