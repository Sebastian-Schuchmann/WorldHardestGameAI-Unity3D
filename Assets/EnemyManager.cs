using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    public EnemyController[] enemies;

    public int GetLength(){
        return enemies.Length;
    }

    public Vector3 GetPositionOfEnemy(int index){
        return enemies[index].transform.localPosition;
    }


    public float GetSpeedAndDirectionOfEnemy(int index){
        return enemies[index].GetComponent<EnemyController>().GetCurrentDirection().x * enemies[index].GetComponent<EnemyController>().speed;
    }
}
