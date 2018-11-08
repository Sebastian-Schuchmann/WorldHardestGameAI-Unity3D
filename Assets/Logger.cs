using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleFirebaseUnity;
using System.Linq;

public class WorldHardestGameLogData
{
    public float Dist;
    public float X;
    public float Y;

    public WorldHardestGameLogData(float closest_Distance_To_Goal, Vector3 position)
    {
        Dist = closest_Distance_To_Goal;
        //Death Position X&Y
        X = position.x;
        Y = position.y;
    }
}


//I setup up a Logger for Realtime Database in Firebase to log my Data when
//training in the cloud. For local training you really dont need this. 
public class Logger : MonoBehaviour {

    public string fireBaseAdress = "https://whg-ai.firebaseio.com/";
    Firebase firebase;

    void Start()
    {  
        firebase = Firebase.CreateNew("https://whg-ai.firebaseio.com/");
    }

    public void LogData(WorldHardestGameLogData worldHardestGameLog)
    {
        firebase.Child("WorldHardestGame" + SystemInfo.deviceName).Push(JsonUtility.ToJson(worldHardestGameLog), true);

        firebase.OnDeleteSuccess += (Firebase sender, DataSnapshot snapshot) => {
            Debug.Log("[OK] Delete from " + sender.Endpoint + ": " + snapshot.RawJson);
        };

        firebase.OnUpdateFailed += UpdateFailedHandler;
        // Method signature: void UpdateFailedHandler(Firebase sender, FirebaseError err)

        firebase.GetValue("print=pretty");
    }
    void UpdateFailedHandler(Firebase sender, FirebaseError err)
    {
        Debug.Log(err);
    }
}

