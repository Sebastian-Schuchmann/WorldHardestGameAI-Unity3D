using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class WHDAgent : Agent {

    private PlayerController playerController;
    public float viewRadius = 0.0f;

    void Start(){
        
    }

    public override void InitializeAgent()
    {
        playerController = GetComponent<PlayerController>();

    }

    public override void AgentReset()
    {
        playerController.Kill();
    }

    public override void CollectObservations()
    {
        Vector2[] rayDirections = {
            Vector2.left,
            Vector2.right,
            Vector2.up,
            Vector2.down
        };

        List<float> observations = new List<float>();
        foreach(Vector2 direction in rayDirections)
            observations.AddRange(CollectRayInformation(direction));

        AddVectorObs(observations);

    }

    public List<float> CollectRayInformation(Vector2 direction){

        List<float> observations = new List<float>();
        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);

        RaycastHit2D[] hits = Physics2D.CircleCastAll(currentPosition, viewRadius, direction);
        foreach(RaycastHit2D hit in hits){
            //Normalize Value between -1, 1
            observations.Add((Vector2.Distance(transform.position, hit.point)-viewRadius/2)/8);
            Debug.DrawLine(currentPosition, hit.point, Color.red);   
        }

        return observations;
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
       // playerController.HorizontalInput = vectorAction[0];
       // playerController.VerticalInput = vectorAction[1];
        switch((int)vectorAction[0]){
            case 0:
                playerController.VerticalInput = 1.0f;
                playerController.HorizontalInput = 0f;
                break;
            case 1:
                playerController.VerticalInput = -1.0f;
                playerController.HorizontalInput = 0f;
                break;
            case 2:
                playerController.HorizontalInput = 1.0f;
                playerController.VerticalInput = 0.0f;
                break;
            case 3:
                playerController.HorizontalInput = -1.0f;
                playerController.VerticalInput = 0.0f;
                break;
            default:
                playerController.VerticalInput = 0.0f;
                playerController.HorizontalInput = 0f;
                break;
                
        }
    }
}
