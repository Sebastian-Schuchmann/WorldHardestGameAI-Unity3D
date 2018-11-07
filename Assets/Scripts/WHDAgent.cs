﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using UnityEngine.UI;

public class WHDAgent : Agent
{
    public bool LogData = false;
    public Logger Logger;
    public float viewRadius = 0.0f;
    private const int ENEMY_LAYER = 10;
    private const int GOAL_LAYER = 8;
    public float speed = 0.07f;
    public float minStepToImprove = 0.02f;
    public Text progressText;
    public Text failCounterText;
    public GameObject goalArea;
    private float lastProgress = 0.0f;
    private int failCounter = 0;
    private float highestDist = 0.0f;
    private float currentTime = 0.0f;
    public float deathTime = 30.0f;

    public EnemyManager enemyManager;

    float HorizontalInput = 0.0f;
    float VerticalInput = 0.0f;

    private Vector3 startPosition;

    public override void InitializeAgent()
    {
        currentTime = deathTime;
        highestDist = getProgress();
        startPosition = transform.position;
    }

    public override void AgentReset()
    {
        if (LogData)
        {
            WorldHardestGameLogData logData = new WorldHardestGameLogData(highestDist, transform.position);
            Logger.LogData(logData);
        }

        highestDist = 10000.0f;
        currentTime = Time.time + deathTime;
        transform.position = startPosition;

    }

    public override void CollectObservations()
    {
        
        Vector2[] rayDirections = {
            Vector2.left,
            Vector2.right,
            Vector2.up,
            Vector2.down
        };

        AddVectorObs(CollectRayInformation());

        AddVectorObs((transform.position.x - startPosition.x) / 10.0f);
        AddVectorObs((transform.position.y - startPosition.y) / 10.0f);

        //Position vom Ziel
        AddVectorObs(Vector2.Distance(transform.position, goalArea.transform.position) / 14.0f);

        //Enemy observation
        //AddVectorObs(Vector2.Distance(transform.position, enemyManager.getPositionOfEnemy(0)) / 13.0f);
        //AddVectorObs(Vector2.Distance(transform.position, enemyManager.getPositionOfEnemy(1)) / 13.0f);
        //AddVectorObs(Vector2.Distance(transform.position, enemyManager.getPositionOfEnemy(2)) / 13.0f);
        //AddVectorObs(Vector2.Distance(transform.position, enemyManager.getPositionOfEnemy(3)) / 13.0f);
        //AddVectorObs(Vector2.Distance(transform.position, enemyManager.getPositionOfEnemy(4)) / 13.0f);

        //  Debug.Log("Angle" + Vector2.Angle(transform.position, enemyManager.getPositionOfEnemy(2))/ 180f);
        //AddVectorObs(Vector2.Angle(transform.position, enemyManager.getPositionOfEnemy(0)) / 180f);
        //AddVectorObs(Vector2.Angle(transform.position, enemyManager.getPositionOfEnemy(1)) / 180f);
        //AddVectorObs(Vector2.Angle(transform.position, enemyManager.getPositionOfEnemy(2)) / 180f);
        //AddVectorObs(Vector2.Angle(transform.position, enemyManager.getPositionOfEnemy(3)) / 180f);
        //AddVectorObs(Vector2.Angle(transform.position, enemyManager.getPositionOfEnemy(4)) / 180f);

        ////  Debug.Log("SPEED" + enemyManager.getSpeedAndDirectionOfEnemy(0));
        //AddVectorObs(enemyManager.getSpeedAndDirectionOfEnemy(0));
        //AddVectorObs(enemyManager.getSpeedAndDirectionOfEnemy(1));
        //AddVectorObs(enemyManager.getSpeedAndDirectionOfEnemy(2));
        //AddVectorObs(enemyManager.getSpeedAndDirectionOfEnemy(3));
        //AddVectorObs(enemyManager.getSpeedAndDirectionOfEnemy(4));

    }

    public List<float> CollectRayInformation()
    {

        List<float> observations = new List<float>();
        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);

        Vector2[] directions = {
            Vector2.left,
            Vector2.right,
            Vector2.up,
            Vector2.down,
            new Vector2(0.5f, 0.5f),
            new Vector2(-0.5f, 0.5f),
            new Vector2(0.5f, -0.5f),
            new Vector2(-0.5f, -0.5f)};

        LayerMask LayerMask = ~(LayerMask.GetMask("Player") + LayerMask.GetMask("Enemy"));

        for (int i = 0; i < directions.Length; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directions[i], Mathf.Infinity, LayerMask);
            observations.Add(hit.distance);

            if (hit.collider != null)
            {
                Debug.DrawLine(transform.position, hit.point, Color.red);
            }
        }

        return observations;
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        if (currentTime < Time.time)
        {
            failCounter++;
            AddReward(-1.0f);
            AgentReset();
        }

        switch ((int)vectorAction[0])
        {
            case 1:
                VerticalInput = 1.0f;
                break;
            case 2:
                VerticalInput = -1.0f;            
                break;
            case 0:
                VerticalInput = 0.0f;
                break;
        }

        switch ((int)vectorAction[1])
        {
            case 1:
                HorizontalInput = -1.0f;               
                break;
            case 2:
                HorizontalInput = 1.0f;
                break;
            case 0:
                HorizontalInput = 0.0f;
                break;
        }

            
            
          

        float movementX = HorizontalInput * speed;
        float movementY = VerticalInput * speed;

        transform.position = transform.position + new Vector3(movementX, movementY, 0);
        progressText.text = getProgress().ToString();
        failCounterText.text = failCounter.ToString();

        //Negative Reward for Time
        AddReward(-0.01f);


        if (getProgress() > lastProgress)
        {
            lastProgress = getProgress();
        }
        else if (getProgress() < highestDist)
        {
            highestDist = getProgress();
            lastProgress = getProgress();
        }
        else
        {
            lastProgress = getProgress();
        }

      
    }



    // Use this for initialization
    void Start()
    {
        startPosition = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            AddReward(-1.0f);
            AgentReset();
            failCounter++;
        }

        if (collision.gameObject.layer == GOAL_LAYER)
        {
            failCounter = 0;
            AddReward(1.0f);
            AgentReset();

        }

    }


    public float getProgress()
    {
        float progress = Vector2.Distance(transform.position, goalArea.GetComponent<Transform>().position);
        //progress -= Vector2.Distance(startPosition, goalArea.GetComponent<Transform>().position);
        //        Debug.Log(progress);
        return progress;
    }
}
