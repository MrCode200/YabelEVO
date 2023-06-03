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
    public int forwardFrequenzy;

    public float foodConsumptionRate;

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
            currentYabel.GetComponent<yabelBehavior>().forwardFrequenzy = forwardFrequenzy + Mathf.RoundToInt(Random.Range(0, 0.6f));

            currentYabel.GetComponent<yabelBehavior>().foodConsumptionRate = Mathf.Abs(foodConsumptionRate + Random.Range(-1.5f, 1.5f));
            
            currentYabel.GetComponent<yabelBehavior>().FieldOfViewRadius = Mathf.Abs(FieldOfViewRadius + Random.Range(-1.5f, 1.5f));
            currentYabel.GetComponent<yabelBehavior>().FieldOfViewAngle = Mathf.Abs(FieldOfViewAngle + Random.Range(-1, 1));
        }
    }
}
