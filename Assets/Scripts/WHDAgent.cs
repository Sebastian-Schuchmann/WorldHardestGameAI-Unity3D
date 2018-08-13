using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using UnityEngine.UI;

public class WHDAgent : Agent {

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

        AddVectorObs((transform.position.x - startPosition.x)/10.0f);
        AddVectorObs((transform.position.y - startPosition.y)/10.0f);
        // Debug.Log(transform.position.x - startPosition.x);
        //Position vom Ziel
        AddVectorObs(Vector2.Distance(transform.position, goalArea.transform.position) / 14.0f);

    }

    public List<float> CollectRayInformation(){

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
        Debug.DrawLine(transform.position, goalArea.transform.position, Color.blue);

        LayerMask player = LayerMask.NameToLayer("Wall");

        for (int i = 0; i < directions.Length; i++){
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directions[i]);
            observations.Add(hit.distance);
            if(hit.collider != null){
//                Debug.Log(hit.distance);
//                Debug.DrawLine(transform.position, hit.point, Color.red); 
            }
        }

        /*RaycastHit2D[] hits = Physics2D.CircleCastAll(currentPosition, viewRadius, direction);
        foreach(RaycastHit2D hit in hits){
            Debug.Log(hit.transform.name);
            //Normalize Value between -1, 1
            observations.Add((Vector2.Distance(transform.position, hit.point)-viewRadius/2)/8);
            Debug.DrawLine(currentPosition, hit.point, Color.red);   
        }*/

        return observations;
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        if(currentTime < Time.time){
            failCounter++;
            //Distanz abhängig?
            AddReward(-Vector2.Distance(transform.position, goalArea.transform.position)/14.0f);
            AgentReset();
        }
            
       // playerController.HorizontalInput = vectorAction[0];
       // playerController.VerticalInput = vectorAction[1];
        switch((int)vectorAction[0]){
            case 0:
                VerticalInput = 1.0f;
                HorizontalInput = 0f;
                break;
            case 1:
                VerticalInput = -1.0f;
                HorizontalInput = 0f;
                break;
            case 2:
                HorizontalInput = 1.0f;
                VerticalInput = 0.0f;
                break;
            case 3:
                HorizontalInput = -1.0f;
                VerticalInput = 0.0f;
                break;
            default:
                VerticalInput = 0.0f;
                HorizontalInput = 0f;
                break;
                
        }

        float movementX = HorizontalInput * speed;
        float movementY = VerticalInput * speed;

        transform.position = transform.position + new Vector3(movementX, movementY, 0);
        progressText.text = getProgress().ToString();
        failCounterText.text = failCounter.ToString();

        //Negative Reward for Time
       // AddReward(-0.01f);

        //Reward for moving towards the Goal
        if (getProgress() > lastProgress + minStepToImprove)
        {
            Debug.Log("BAD");
            AddReward(-0.002f);
            lastProgress = getProgress();
        }
        else if (getProgress() < highestDist - minStepToImprove)
        {
            Debug.Log("REWARD");
           AddReward(+0.005f);
            highestDist = getProgress();
            lastProgress = getProgress();
        }
        else{
            lastProgress = getProgress();
        }

    //    Monitor.Log("Reward", );
    }



    // Use this for initialization
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
   

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == ENEMY_LAYER)
        {
            lastProgress = getProgress();
            Debug.Log("VERY BAD");
            AddReward(-1.0f);
            AgentReset();
            failCounter++;
        }

        if (collision.gameObject.layer == GOAL_LAYER)
        {
            lastProgress = getProgress();
            Debug.Log("VERY GOOD");
            //WIN
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
