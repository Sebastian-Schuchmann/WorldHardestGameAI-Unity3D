using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour {
    
    public Color WallColor;
    public Color PlayerColor;
    public Color EnemyColor;
    public Color GoalColor;
    public Color BackgroundColor;

    public bool writeTexture = false;

    public Dictionary<int, Color> layerToColorDic;

    public int resolution;
    private int observeWidth;
    private int observeHeight;
    public Texture2D texture;

    int resolutionWidth;
    int resolutionHeight;

	// Use this for initialization
	void Start () 
    {
        layerToColorDic = new Dictionary<int, Color>()
        {
            {8, GoalColor},
            {9, WallColor},
            {10, EnemyColor},
            {11, PlayerColor},
            {0, BackgroundColor}
        };

        resolutionHeight = Camera.main.scaledPixelHeight;
        resolutionWidth = Camera.main.scaledPixelWidth;

        float dividend = resolutionWidth / (float)resolutionHeight;
        Debug.Log(dividend);
        observeHeight = resolution;
        observeWidth = Mathf.RoundToInt(resolution * dividend);

        texture = new Texture2D(observeWidth, observeHeight);
        Debug.Log(observeWidth + " " + observeHeight);

        Observe();
	}

    private void Observe()
    {
        Vector3 currentPointOnScreen = new Vector3(0, 0, 0);
        int stepsToMovePointWidth = resolutionWidth / observeWidth;
        int stepsToMovePointHeight = resolutionHeight / observeHeight;

        for (int i = 0; i < resolutionWidth; i += stepsToMovePointWidth)
        {
            for (int j = 0; j < resolutionHeight; j += stepsToMovePointHeight)
            {
                currentPointOnScreen = new Vector3(i, j, 0);
                int layer = 0;
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(currentPointOnScreen), Vector2.zero);
                if (hit.collider != null)
                {
                    layer = hit.collider.gameObject.layer;
                    //Debug.Log("Target Position: " + hit.collider.gameObject.layer);
                }

                if(writeTexture)
                texture.SetPixel(i / stepsToMovePointWidth, j / stepsToMovePointHeight, layerToColorDic[layer]); 
            }
            if(writeTexture)
            texture.Apply();
        }


    }

    // Update is called once per frame
    void FixedUpdate () {

        Observe();
        //RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(currentPointOnScreen), Vector2.zero);
            //if (hit.collider != null)
            //{
            //    Debug.Log("Target Position: " + hit.collider.gameObject.name);
            //}

	}
}
