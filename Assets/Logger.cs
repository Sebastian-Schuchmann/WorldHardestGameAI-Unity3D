using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleFirebaseUnity;
using System.Linq;

public class WorldHardestGameLogData
{
 //   public List<float> ProgressInDistanceCollection;
 //   public float Average_Distance_From_Goal;
    public float Closest_Distance_To_Goal;
    public float Death_Position_X;
    public float Death_Position_Y;
    public string TimeStamp;

    public WorldHardestGameLogData(float closest_Distance_To_Goal, Vector3 position)
    {
        Closest_Distance_To_Goal = closest_Distance_To_Goal;
        Death_Position_X = position.x;
        Death_Position_Y = position.y;
        TimeStamp = System.DateTime.Now.ToString();
//        Debug.Log(Death_Position_X + " " +  Death_Position_Y);
    }



    /*  public void calculateAverageProgess(){
          Average_Distance_From_Goal = ProgressInDistanceCollection.Average();
          ProgressInDistanceCollection.Clear();
      }

      public void AddProgess(float Progess){
          ProgressInDistanceCollection.Add(Progess);
      }*/
}


public class Logger : MonoBehaviour {

    Firebase firebase;
   // public WorldHardestGameLogData logData;




    void Update()
    {

        //LOG DATA
    }

    void Start()
    {
        
        firebase = Firebase.CreateNew("https://basketball-tf.firebaseio.com/");
       // logData = new WorldHardestGameLogData();
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

