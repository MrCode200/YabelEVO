using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generateObject : MonoBehaviour
{
    public CycleControll CycleControll;
    private int lastCycle = 0;

    private GameObject currentYabel;
    public GameObject yabelPrefab;
    public int numOfYabels = 0;

    public GameObject foodPrefab;
    public int numOfFood = 0;
    public int FoodPerCycle = 0;

    public int spawnRadius = 50;
    public int spawnPoint = 0;


    //for numOfYabels create the amount of yabels in a spawnRadius radius
    public void generateYabel()
    {
        for (int i = 0; i<numOfYabels; i++)
        {
            currentYabel = Instantiate(yabelPrefab,
                Random.insideUnitCircle* spawnRadius,
                Quaternion.Euler(0, 0, Random.Range(0, 360)));

            currentYabel.GetComponent<yabelBehavior>().moveSpeed = Random.Range(3, 10);
            currentYabel.GetComponent<yabelBehavior>().rotationSpeed = Random.Range(30, 60);
            currentYabel.GetComponent<yabelBehavior>().forwardFrequenzy = Random.Range(0, 5);

            currentYabel.GetComponent<yabelBehavior>().foodConsumptionRate = Random.Range(1, 20);

            currentYabel.GetComponent<yabelBehavior>().FieldOfViewRadius = Random.Range(1, 20);
            currentYabel.GetComponent<yabelBehavior>().FieldOfViewAngle = Random.Range(1, 90);
        }
    }

    //for numOfFood create the amount of food in a spawnRadius radius
    public void generateFood(int numOfGeneration)
    {
        for (int i = 0; i < numOfGeneration; i++)
        {
            Instantiate(foodPrefab,
                Random.insideUnitCircle * spawnRadius,
                Quaternion.Euler(0, 0, 0));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        generateFood(numOfFood);
        generateYabel();
    }

    // Update is called once per frame
    void Update()
    {
        if (CycleControll.Cycle > lastCycle)
        {
            generateFood(FoodPerCycle);
            lastCycle++;
        }
    }
}

