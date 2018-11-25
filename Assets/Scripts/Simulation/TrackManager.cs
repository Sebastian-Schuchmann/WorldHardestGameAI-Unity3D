/// Author: Samuel Arzt
/// Date: March 2017

#region Includes
using System;
using UnityEngine;
using System.Collections.Generic;
#endregion

/// <summary>
/// Singleton class managing the current track and all cars racing on it, evaluating each individual.
/// </summary>
public class TrackManager : MonoBehaviour
{
    #region Members
    public static TrackManager Instance
    {
        get;
        private set;
    }


    private Checkpoint[] checkpoints;

    /// <summary>
    /// Car used to create new cars and to set start position.
    /// </summary>
    public CarController PrototypeCar;
    // Start position for cars
    private Vector3 startPosition;
    private Quaternion startRotation;

    // Struct for storing the current cars and their position on the track.
    private class RaceCar
    {
        public RaceCar(CarController car = null, uint checkpointIndex = 1)
        {
            this.Car = car;
            this.CheckpointIndex = checkpointIndex;
        }
        public CarController Car;
        public uint CheckpointIndex;
    }
    private List<RaceCar> cars = new List<RaceCar>();

    /// <summary>
    /// The amount of cars currently on the track.
    /// </summary>
    public int CarCount
    {
        get { return cars.Count; }
    }

    #region Best and Second best
    private CarController bestCar = null;
    /// <summary>
    /// The current best car (furthest in the track).
    /// </summary>
    public CarController BestCar
    {
        get { return bestCar; }
        private set
        {
            if (bestCar != value)
            {

                //Update appearance
                if (BestCar != null)
                    BestCar.SpriteRenderer.sprite = NormalCarSprite;
                if (value != null)
                    value.SpriteRenderer.sprite = BestCarSprite;

                //Set previous best to be second best now
                CarController previousBest = bestCar;
                bestCar = value;
                BestCarChanged?.Invoke(bestCar);

                SecondBestCar = previousBest;
            }
        }
    }
    /// <summary>
    /// Event for when the best car has changed.
    /// </summary>
    public event System.Action<CarController> BestCarChanged;

    private CarController secondBestCar = null;
    /// <summary>
    /// The current second best car (furthest in the track).
    /// </summary>
    public CarController SecondBestCar
    {
        get { return secondBestCar; }
        private set
        {
            if (SecondBestCar != value)
            {

                secondBestCar = value;
                SecondBestCarChanged?.Invoke(SecondBestCar);
            }
        }
    }
    /// <summary>
    /// Event for when the second best car has changed.
    /// </summary>
    public event System.Action<CarController> SecondBestCarChanged;
    #endregion

    

    /// <summary>
    /// The length of the current track in Unity units (accumulated distance between successive checkpoints).
    /// </summary>
    public float TrackLength
    {
        get;
        private set;
    }
    public Sprite NormalCarSprite;
    public Sprite BestCarSprite;
    #endregion

    #region Constructors
    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Mulitple instance of TrackManager are not allowed in one Scene.");
            return;
        }

        Instance = this;

        //Get all checkpoints
        checkpoints = GetComponentsInChildren<Checkpoint>();

        //Set start position and hide prototype
        startPosition = PrototypeCar.transform.position;
        startRotation = PrototypeCar.transform.rotation;
        PrototypeCar.gameObject.SetActive(false);


    }

    void Start()
    {
        //Hide checkpoints
        foreach (Checkpoint check in checkpoints)
            check.IsVisible = false;
    }
    #endregion

    #region Methods
    // Unity method for updating the simulation
    void Update()
    {
        //Update reward for each enabled car on the track
        for (int i = 0; i < cars.Count; i++)
        {
            RaceCar car = cars[i];
            if (car.Car.enabled)
            {
                car.Car.CurrentCompletionReward = GetCompletePerc(car.Car);

                //Update best
                if (BestCar == null || car.Car.CurrentCompletionReward >= BestCar.CurrentCompletionReward)
                    BestCar = car.Car;
                else if (SecondBestCar == null || car.Car.CurrentCompletionReward >= SecondBestCar.CurrentCompletionReward)
                    SecondBestCar = car.Car;
            }
        }
    }

    public void SetCarAmount(int amount)
    {
        //Check arguments
        if (amount < 0) throw new ArgumentException("Amount may not be less than zero.");

        if (amount == CarCount) return;

        if (amount > cars.Count)
        {
            //Add new cars
            for (int toBeAdded = amount - cars.Count; toBeAdded > 0; toBeAdded--)
            {
                GameObject carCopy = Instantiate(PrototypeCar.gameObject);
                carCopy.transform.position = startPosition;
                carCopy.transform.rotation = startRotation;
                CarController controllerCopy = carCopy.GetComponent<CarController>();
                cars.Add(new RaceCar(controllerCopy, 1));
                carCopy.SetActive(true);
            }
        }
        else if (amount < cars.Count)
        {
            //Remove existing cars
            for (int toBeRemoved = cars.Count - amount; toBeRemoved > 0; toBeRemoved--)
            {
                RaceCar last = cars[cars.Count - 1];
                cars.RemoveAt(cars.Count - 1);

                Destroy(last.Car.gameObject);
            }
        }
    }

    /// <summary>
    /// Restarts all cars and puts them at the track start.
    /// </summary>
    public void Restart()
    {
        foreach (RaceCar car in cars)
        {
            car.Car.transform.position = startPosition;
            car.Car.transform.rotation = startRotation;
            car.Car.Restart();
            car.CheckpointIndex = 1;
        }

        BestCar = null;
        SecondBestCar = null;
    }

    /// <summary>
    /// Returns an Enumerator for iterator through all cars currently on the track.
    /// </summary>
    public IEnumerator<CarController> GetCarEnumerator()
    {
        for (int i = 0; i < cars.Count; i++)
            yield return cars[i].Car;
    }



    // Calculates the completion percentage of given car with given completed last checkpoint.
    // This method will update the given checkpoint index accordingly to the current position.
    private float GetCompletePerc(CarController car)
    {

//        Debug.Log(14.0f - car.Movement.GetProgress() / 14.0f);
        return 14.0f - car.Movement.GetProgress() / 14.0f;
    }
    #endregion

}
