using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EggBehavior : MonoBehaviour
{
    public GameObject yabelPrefab;
    private GameObject currentYabel;

    private float IncubationTimer;
    public int IncubationTimeInSeconds;

    public float moveSpeed;
    public float rotationSpeed;
    public float randomIntFrequenzy;
    public float forwardFrequenzy;

    public float regeneration;
    public float damage;

    public float energy;
    public int energyUsageForEgg;
    public int energyCapEgg;
    public float foodConsumptionRate;

    public float agingSpeed;
    public float growSpeed;

    public float FieldOfViewRadius;
    public float FieldOfViewAngle;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //set timer
        IncubationTimer += Time.deltaTime;

        if (IncubationTimer >= IncubationTimeInSeconds)
        {
            currentYabel = Instantiate(yabelPrefab, transform.position, transform.rotation);
            Destroy(gameObject);

            //create mutation
            currentYabel.GetComponent<yabelBehavior>().moveSpeed = Mathf.Abs(moveSpeed + Random.Range(-0.5f, 0.5f));
            currentYabel.GetComponent<yabelBehavior>().rotationSpeed = Mathf.Abs(rotationSpeed + Random.Range(-5f,5f));
            currentYabel.GetComponent<yabelBehavior>().randomIntFrequenzy = randomIntFrequenzy + (Random.Range(-1f, 1f));
            currentYabel.GetComponent<yabelBehavior>().forwardFrequenzy = forwardFrequenzy + (Random.Range(-0.25f, 0.25f));

            currentYabel.GetComponent<yabelBehavior>().regeneration = Mathf.Abs(regeneration + Random.Range(-0.3f, 0.3f));
            currentYabel.GetComponent<yabelBehavior>().damage = Mathf.Abs(damage + Random.Range(-1f, 1f));

            currentYabel.GetComponent<yabelBehavior>().energy = energy;
            currentYabel.GetComponent<yabelBehavior>().energyUsageForEgg = energyUsageForEgg + Random.Range(-5, 5);
            currentYabel.GetComponent<yabelBehavior>().energyCapEgg = energyCapEgg + Random.Range(-5, 5);
            currentYabel.GetComponent<yabelBehavior>().foodConsumptionRate = Mathf.Abs(foodConsumptionRate + Random.Range(-1.5f, 1.5f));

            currentYabel.GetComponent<yabelBehavior>().agingSpeed = Mathf.Abs(agingSpeed + Random.Range(-0.1f, 0.1f));
            currentYabel.GetComponent<yabelBehavior>().growSpeed = growSpeed + Mathf.Abs(growSpeed + Random.Range(-0.002f, 0.002f));

            currentYabel.GetComponent<yabelBehavior>().FieldOfViewRadius = Mathf.Abs(FieldOfViewRadius + Random.Range(-1.5f, 1.5f));
            currentYabel.GetComponent<yabelBehavior>().FieldOfViewAngle = Mathf.Abs(FieldOfViewAngle + Random.Range(-1, 1));
        }
    }
}
