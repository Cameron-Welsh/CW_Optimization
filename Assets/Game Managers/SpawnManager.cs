using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameState;

public class SpawnManager : MonoBehaviour
{
    #region Variables

    public int screenWidth;
    public int screenHeight;

    public Vector3[] spawnLocations;
    public GameObject Lawyer;
    int index = 0;

    #endregion

    #region Unity Callbacks

    private void Awake()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
    }

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnLawyer(index);
            index++;
            if (index == 10)
            {
                index = 0;
            }
        }
    }

    #endregion

    #region Functions

    public void SpawnLawyer(int i)
    {
        Instantiate(Lawyer, spawnLocations[i], Quaternion.identity);
    }

    public void UpdateSpawnLocations(GameStateManager.Arena arena)
    {
        switch (arena)
        {
            case GameStateManager.Arena.Courtroom:
                spawnLocations[0].x = -12.0f;
                spawnLocations[0].y = 0.0f;
                spawnLocations[1].x = -14.0f;
                spawnLocations[1].y = -1.0f;
                spawnLocations[2].x = -14.0f;
                spawnLocations[2].y = -2.0f;
                spawnLocations[3].x = -14.0f;
                spawnLocations[3].y = -3.0f;
                spawnLocations[4].x = -12.0f;
                spawnLocations[4].y = -4.0f;
                spawnLocations[5].x = 12.0f;
                spawnLocations[5].y = 0.0f;
                spawnLocations[6].x = 14.0f;
                spawnLocations[6].y = -1.0f;
                spawnLocations[7].x = 14.0f;
                spawnLocations[7].y = -2.0f;
                spawnLocations[8].x = 14.0f;
                spawnLocations[8].y = -3.0f;
                spawnLocations[9].x = 12.0f;
                spawnLocations[9].y = -4.0f;
                break;
            case GameStateManager.Arena.DefendantsHall:
                spawnLocations[0].x = -12.0f;
                spawnLocations[0].y = 0.0f;
                spawnLocations[1].x = -14.0f;
                spawnLocations[1].y = -1.0f;
                spawnLocations[2].x = -14.0f;
                spawnLocations[2].y = -2.0f;
                spawnLocations[3].x = -14.0f;
                spawnLocations[3].y = -3.0f;
                spawnLocations[4].x = -12.0f;
                spawnLocations[4].y = -4.0f;
                spawnLocations[5].x = 12.0f;
                spawnLocations[5].y = 0.0f;
                spawnLocations[6].x = 14.0f;
                spawnLocations[6].y = -1.0f;
                spawnLocations[7].x = 14.0f;
                spawnLocations[7].y = -2.0f;
                spawnLocations[8].x = 14.0f;
                spawnLocations[8].y = -3.0f;
                spawnLocations[9].x = 12.0f;
                spawnLocations[9].y = -4.0f;
                break;
            case GameStateManager.Arena.ProsecutorHall:
                spawnLocations[0].x = -22.0f;
                spawnLocations[0].y = 0.0f;
                spawnLocations[1].x = -24.0f;
                spawnLocations[1].y = -1.0f;
                spawnLocations[2].x = -24.0f;
                spawnLocations[2].y = -2.0f;
                spawnLocations[3].x = -24.0f;
                spawnLocations[3].y = -3.0f;
                spawnLocations[4].x = -22.0f;
                spawnLocations[4].y = -4.0f;
                spawnLocations[5].x = 22.0f;
                spawnLocations[5].y = 0.0f;
                spawnLocations[6].x = 24.0f;
                spawnLocations[6].y = -1.0f;
                spawnLocations[7].x = 24.0f;
                spawnLocations[7].y = -2.0f;
                spawnLocations[8].x = 24.0f;
                spawnLocations[8].y = -3.0f;
                spawnLocations[9].x = 22.0f;
                spawnLocations[9].y = -4.0f;
                break;
            default:
                Debug.Log("Unrecognized Arena");
                break;
        }

    }

    #endregion
}