using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class yabelBehavior : MonoBehaviour
{
    #region Variables

    public GameObject YabelEgg;
    private GameObject currentEgg;

    //maybe later make private
    //at which energy egg is layed
    public int energyCapEgg;

    //how much energy it loses
    public int energyUsageForEgg;

    public Rigidbody2D rg;
    public float throwBack;
    public int maxLife;
    public float life;
    public float regeneration;
    public float damage;

    public float moveSpeed;
    private float moveSpeedMax;
    [Range(1, 360)] public float rotationSpeed;

    //randomIntFrequenzy is how many Frames it takes to change movement state
    public float randomIntFrequenzy = 60;

    //forwardFrequenzy is how often it doesn't turn
    public float forwardFrequenzy = 2;

    //used to check(count) at which frame the programm is
    private int rotationElapsed = 0;
    int randomIntMovement;

    public int age;
    public float agingSpeed;
    public float growSpeed;
    public float time;

    public GameObject meat;
    private GameObject constantMeat;
    public int meatChunks;

    public float energy = 200f;
    public float energyConsumptionRate = 0.01f; //can be deleted probably
    public float foodConsumptionRate;

    public LayerMask FoodLayer;
    public float FieldOfViewRadius; //change name to ...length or ...visionlength etc.
    private float FieldOfViewRadiusGrowth;
    [Range(1, 360)] public float FieldOfViewAngle;

    private Vector3 positionFood;
    private float AngleToFood;

    public bool CanSeeFood { get; private set; }
    public bool CollidedFood { get; private set; }


    #endregion

    #region Functions

    //function for movement
    void movement()
    {

        transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);


        //create randomMovement Number afte rotationFrequenzy frames
        if (!CanSeeFood)
        {
            if (rotationElapsed >= randomIntFrequenzy)
            {
                randomIntMovement = Random.Range(0, 3 + Mathf.RoundToInt(forwardFrequenzy));
                rotationElapsed = 0;
            }
            else
            {
                rotationElapsed = rotationElapsed + 1;
            }

            //depending on randomMovement rotate and set rotationElapsed = 0
            if (randomIntMovement == 0)
            {
                // Rotate to the right
                transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
            }
            else if (randomIntMovement == 1)
            {
                // Rotate to the left
                transform.Rotate(Vector3.forward * -rotationSpeed * Time.deltaTime);
            }
        }
        else
        {

            //check if it has rotated towards food
            if (transform.rotation.z != (90 - AngleToFood) % rotationSpeed * Time.deltaTime)
            {
                //if food is to the right rotate right else left
                if (AngleToFood < 90)
                {
                    transform.Rotate(Vector3.forward * -rotationSpeed * Time.deltaTime);
                }
                else
                {
                    transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
                }
            }
        }

    }
    //function for energy consumption
    void energyMonitor() //rename to energyController or maybe sth else 
    {
        //use energy per frame
        if (energy > 0)
        {
            energy = energy - Time.timeScale * energyConsumptionRate *
            (Mathf.RoundToInt(moveSpeed) + 
            rotationSpeed / 30 + 
            foodConsumptionRate / 1.5f + 
            FieldOfViewRadius / 4 * FieldOfViewAngle / 65 + 
            damage/2 +
            transform.localScale.x * transform.localScale.x);

        }
        if (energy <= 0 || life <= 0)
        {
            life -= 0.1f;
            if (life <= 0)
            {
                for (int i = 0; i < meatChunks; i++)
                {
                    constantMeat = Instantiate(meat, transform.position + new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0), Quaternion.Euler(0, 0, 0));
                    constantMeat.GetComponent<HandleMeatEnergy>().energy = transform.localScale.x * 1.5f * Random.Range(0.1f, 1f) / meatChunks;
                }
                Destroy(gameObject);
            }
        }
        else if (life < maxLife)
        {
            energy -= regeneration * 0.8f;
            life += regeneration;
        }
    }

    void reproduction()
    {
        //check if enough energy for egg
        if (age > 22 && energy >= energyCapEgg && energy > energyUsageForEgg + energyUsageForEgg * 0.1f && life >= maxLife * 0.75)
        {
            currentEgg = Instantiate(YabelEgg, transform.position, Quaternion.Euler(0, 0, 0));

            if (energy > energyUsageForEgg + energyUsageForEgg * 0.1f)
            {
                currentEgg.GetComponent<EggBehavior>().energy = energyUsageForEgg;

                energy -= energyUsageForEgg + energyUsageForEgg * 0.1f;
            }
            else
            {
                currentEgg.GetComponent<EggBehavior>().energy = energy;

                energy = 0;
            }

            currentEgg.GetComponent<EggBehavior>().energyUsageForEgg = energyUsageForEgg;
            currentEgg.GetComponent<EggBehavior>().energyCapEgg = energyCapEgg;
            currentEgg.GetComponent<EggBehavior>().moveSpeed = moveSpeedMax;
            currentEgg.GetComponent<EggBehavior>().foodConsumptionRate = foodConsumptionRate;
            currentEgg.GetComponent<EggBehavior>().regeneration = regeneration;
            currentEgg.GetComponent<EggBehavior>().damage = damage;
            currentEgg.GetComponent<EggBehavior>().rotationSpeed = rotationSpeed;
            currentEgg.GetComponent<EggBehavior>().growSpeed = growSpeed;
            currentEgg.GetComponent<EggBehavior>().agingSpeed = agingSpeed;
            currentEgg.GetComponent<EggBehavior>().randomIntFrequenzy = randomIntFrequenzy;
            currentEgg.GetComponent<EggBehavior>().forwardFrequenzy = forwardFrequenzy;
            currentEgg.GetComponent<EggBehavior>().FieldOfViewRadius = FieldOfViewRadius;
            currentEgg.GetComponent<EggBehavior>().FieldOfViewAngle = FieldOfViewAngle;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Food")
        {
            if (collision.gameObject.GetComponent<HandleFoodEnergy>().energy > 0)
            {
                energy += foodConsumptionRate;
                collision.gameObject.GetComponent<HandleFoodEnergy>().energy -= foodConsumptionRate;
            }
        }
        else if (collision.gameObject.tag == "Meat")
        {
             if (collision.gameObject.GetComponent<HandleMeatEnergy>().energy > 0)
             {
                energy += foodConsumptionRate;
                collision.gameObject.GetComponent<HandleMeatEnergy>().energy -= foodConsumptionRate;
             }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Yabel")
        {
            collision.gameObject.GetComponent<yabelBehavior>().life -= damage;

            // Berechnen der Abprallrichtung basierend auf der Kollisionsnormalen
            Vector2 bounceDirection = collision.contacts[0].normal;

            // Anwenden der Kraft für den Abprall
            rg.AddForce(bounceDirection * throwBack, ForceMode2D.Force);
        }
    }

    void controllAge()
    {
        time += Time.deltaTime;
        if (time >= agingSpeed)
        {
            age += 1;
            time = 0;

            if (age <= 22)
            {
                transform.localScale += new Vector3(growSpeed, growSpeed, 0);
                energy -= growSpeed * 1.5f;
            }
            if (age >= 50)
            {
                if (moveSpeed > 0)
                {
                    // movement forward but slower because of age
                    moveSpeed -= 0.02f;
                }
            }
            if (age <= 19)
            {
                //devide stes FOV and FOA to normal
                //times creates new FOV and FOA
                FieldOfViewRadius += FieldOfViewRadiusGrowth;
            }
        }
    }

    #region FOV
    void FOV()
    {
        //get all food collided in a circle around the yabel 
        Collider2D[] rangeCheck = Physics2D.OverlapCircleAll(transform.position, FieldOfViewRadius, FoodLayer);

        if (rangeCheck.Length > 0)
        {

            //get distance to food and normalize it
            if (!CanSeeFood)
            {
                positionFood = rangeCheck[0].transform.position;
            }
            Vector3 directionToFood = transform.InverseTransformPoint(positionFood);

            //check where food is 
            AngleToFood = Mathf.Atan2(directionToFood.y, directionToFood.x) * Mathf.Rad2Deg;

            //check if food is in FOV
            if (AngleToFood >= 90 - (FieldOfViewAngle - (transform.localScale.x / 6)) / 2 && 
                AngleToFood <= 90 + (FieldOfViewAngle + (transform.localScale.x / 6)) / 2)
            {
                CanSeeFood = true;
            }
            else
            {
                CanSeeFood = false;
            }
        }
        else
        {
            CanSeeFood = false;
        }
    }

    //draw field of view UI
    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, FieldOfViewRadius);

        Vector3 angle01 = DirectionFromAngle(-transform.eulerAngles.z, -FieldOfViewAngle / 2);
        Vector3 angle02 = DirectionFromAngle(-transform.eulerAngles.z, FieldOfViewAngle / 2);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + angle01 * FieldOfViewRadius);
        Gizmos.DrawLine(transform.position, transform.position + angle02 * FieldOfViewRadius);

        if (CanSeeFood)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, positionFood);
        }
    }

    //for Ui create FOV
    Vector2 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector2(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    #endregion
    #endregion

    void Start()
    {
        randomIntMovement = Random.Range(0, 1 + Mathf.RoundToInt(forwardFrequenzy));
        meatChunks = Random.Range(1, 4);
        life = maxLife;

        FieldOfViewRadiusGrowth = FieldOfViewRadius / 22;

        moveSpeedMax = moveSpeed;
        FieldOfViewRadius = 0;
        FieldOfViewRadius += FieldOfViewRadiusGrowth * 3;
    }

    // Update is called once per frame
    void Update()
    {
        controllAge();
        energyMonitor();
        reproduction();
        FOV();
        movement();
    }
}
