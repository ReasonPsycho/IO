using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecyklingGameScript : MonoBehaviour
{
    public PointsCounter pointsCounter;
    public Timer gameTimer;
    public int initialNumberOfTrash = 10;
    public int trashCollected = 0;
    public int currentTrash;
    public int addedTrash = 0;
    public GameObject trashPrefab;
    void Start()
    {
        currentTrash = initialNumberOfTrash;
        gameTimer.timeLeft = 90;
        gameTimer.timerOn = true;

        for(int i = 0; i < initialNumberOfTrash; i++)
        {
            SpawnTrash();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(gameTimer.timeLeft > 0)
        {
            currentTrash = initialNumberOfTrash - trashCollected + addedTrash;
            if(currentTrash < initialNumberOfTrash)
            {
                SpawnTrash();
                addedTrash++;
            }
        }
        else
        {
            Debug.Log("Time has run out!");
            gameTimer.timeLeft = 0;
            gameTimer.timerOn = false;
            Debug.Log("Points: " + pointsCounter.GetPoints());
            

            //tutaj bedzie GetResult(pointsCounter.GetPoints())
        }
    }

    private void SpawnTrash()
    {
        GameObject trash = Instantiate(trashPrefab);
        trash.transform.position = new Vector3(Random.Range(-150, 250), Random.Range(0, 200), 0);
        trash.GetComponent<TrashInstance>().trashType = (TrashType)Random.Range(0, 6);
    }
}