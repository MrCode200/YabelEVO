using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generateObject : MonoBehaviour
{
    public GameObject yabelPrefab;
    public int numOfYabels = 0;

    public GameObject foodPrefab;
    public int numOfFood = 0;

    public int spawnRadius = 50;
    public int spawnPoint = 0;

    // Start is called before the first frame update
    void Start()
    {
        //for numOfYabels create the amount of yabels in a spawnRadius radius
        for (int i = 0; i < numOfYabels; i++)
        {
            Instantiate(yabelPrefab,
                Random.insideUnitCircle * spawnRadius,
                Quaternion.Euler(0, 0, Random.Range(0, 360)));
        }
        //for numOfFood create the amount of food in a spawnRadius radius
        for (int i = 0; i < numOfFood; i++)
        {
            Instantiate(foodPrefab,
                Random.insideUnitCircle * spawnRadius,
                Quaternion.Euler(0, 0, Random.Range(0, 360)));
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

