using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    public EnemyController[] enemies;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Vector3 getPositionOfEnemy(int index){
        return enemies[index].transform.position;
    }

    public float getSpeedAndDirectionOfEnemy(int index){
        return enemies[index].GetComponent<EnemyController>().currentDirection.x * enemies[index].GetComponent<EnemyController>().speed;
    }
}
