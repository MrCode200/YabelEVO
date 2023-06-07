using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generateObject : MonoBehaviour
{
    public CycleControll CycleControll;
    private int lastCycle = 0;

    public GameObject Barrier;
    private GameObject currentBarrier;

    private GameObject currentYabel;

    public GameObject yabelPrefab;
    public int numOfYabels = 0;

    public GameObject yabelPrefabVariant;
    public int numOfYabelsVariant = 0;

    public GameObject foodPrefab;
    public int numOfFood = 0;
    public int FoodPerCycle = 0;

    public bool spawnInCicrle;
    private Vector2 spawnArea;
    public int spawnRadius = 50;
    public int spawnPoint = 0;


    //for numOfYabels create the amount of yabels in a spawnRadius radius
    void generateYabel()
    {
        for (int i = 0; i<numOfYabels; i++)
        {
            currentYabel = Instantiate(yabelPrefab,
                Random.insideUnitCircle* spawnRadius,
                Quaternion.Euler(0, 0, Random.Range(0, 360)));

            currentYabel.GetComponent<yabelBehavior>().moveSpeed = Random.Range(3, 10);
            currentYabel.GetComponent<yabelBehavior>().rotationSpeed = Random.Range(30, 60);
            currentYabel.GetComponent<yabelBehavior>().forwardFrequenzy = Random.Range(0, 5);

            currentYabel.GetComponent<yabelBehavior>().regeneration = Random.Range(0f, 1f);
            currentYabel.GetComponent<yabelBehavior>().damage = Random.Range(1f, 20f);

            currentYabel.GetComponent<yabelBehavior>().energyCapEgg = Random.Range(100, 250);
            currentYabel.GetComponent<yabelBehavior>().energyUsageForEgg = Random.Range(40, currentYabel.GetComponent<yabelBehavior>().energyCapEgg - 40);
            currentYabel.GetComponent<yabelBehavior>().foodConsumptionRate = Random.Range(1, 20);

            currentYabel.GetComponent<yabelBehavior>().agingSpeed = Random.Range(0.1f, 4);
            currentYabel.GetComponent<yabelBehavior>().growSpeed = Random.Range(0.02f, 0.13f);

            currentYabel.GetComponent<yabelBehavior>().FieldOfViewRadius = Random.Range(1, 20);
            currentYabel.GetComponent<yabelBehavior>().FieldOfViewAngle = Random.Range(1, 200);
        }

        for (int i = 0; i < numOfYabelsVariant; i++)
        {
            currentYabel = Instantiate(yabelPrefabVariant,
                Random.insideUnitCircle * spawnRadius,
                Quaternion.Euler(0, 0, Random.Range(0, 360)));

            currentYabel.GetComponent<yabelBehavior>().moveSpeed = Random.Range(3, 10);
            currentYabel.GetComponent<yabelBehavior>().rotationSpeed = Random.Range(30, 60);
            currentYabel.GetComponent<yabelBehavior>().forwardFrequenzy = Random.Range(0, 5);

            currentYabel.GetComponent<yabelBehavior>().regeneration = Random.Range(0f, 1f);
            currentYabel.GetComponent<yabelBehavior>().damage = Random.Range(1f, 20f);

            currentYabel.GetComponent<yabelBehavior>().energyCapEgg = Random.Range(100, 250);
            currentYabel.GetComponent<yabelBehavior>().energyUsageForEgg = Random.Range(40, currentYabel.GetComponent<yabelBehavior>().energyCapEgg - 40);
            currentYabel.GetComponent<yabelBehavior>().foodConsumptionRate = Random.Range(1, 20);

            currentYabel.GetComponent<yabelBehavior>().agingSpeed = Random.Range(0.1f, 4);
            currentYabel.GetComponent<yabelBehavior>().growSpeed = Random.Range(0.02f, 0.13f);

            currentYabel.GetComponent<yabelBehavior>().FieldOfViewRadius = Random.Range(1, 20);
            currentYabel.GetComponent<yabelBehavior>().FieldOfViewAngle = Random.Range(1, 200);
        }
    }

    //for numOfFood create the amount of food in a spawnRadius radius
    void generateFood(int numOfGeneration)
    {
        for (int i = 0; i < numOfGeneration; i++)
        {
            if (spawnInCicrle)
            {
                spawnArea = Random.insideUnitCircle * spawnRadius;
            }
            else
            {
                spawnArea = new Vector2(Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius));
            }

            Instantiate(foodPrefab,
                spawnArea,
                Quaternion.Euler(0, 0, 0));
        }
    }

    void generateBorder()
    {
        if (!spawnInCicrle)
        {
            currentBarrier = Instantiate(Barrier, new Vector3(spawnRadius + 5, 0, 0), Quaternion.Euler(0, 0, 90));
            currentBarrier.transform.localScale = new Vector3((spawnRadius + 10) * 2, 2);

            currentBarrier = Instantiate(Barrier, new Vector3(-spawnRadius - 5, 0, 0), Quaternion.Euler(0, 0, 90));
            currentBarrier.transform.localScale = new Vector3((spawnRadius + 10) * 2, 2);

            currentBarrier = Instantiate(Barrier, new Vector3(0, spawnRadius + 5, 0), Quaternion.Euler(0, 0, 90));
            currentBarrier.transform.localScale = new Vector3(2, (spawnRadius + 10) * 2);

            currentBarrier = Instantiate(Barrier, new Vector3(0, -spawnRadius - 5, 0), Quaternion.Euler(0, 0, 90));
            currentBarrier.transform.localScale = new Vector3(2, (spawnRadius + 10) * 2);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        generateBorder();
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

