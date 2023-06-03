using System.Collections;
using System.Collections.Generic;
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

    public float moveSpeed;
    [Range(1,360)]public float rotationSpeed;

    //randomIntFrequenzy is how many Frames it takes to change movement state
    public int randomIntFrequenzy = 60;
    //forwardFrequenzy is how often it doesn't turn
    public int forwardFrequenzy = 2;
    //used to check(count) at which frame the programm is
    private int rotationElapsed = 0;
    int randomIntMovement;

    public float energy = 200f;
    public float energyConsumptionRate = 0.01f; //can be deleted probably
    public float foodConsumptionRate;

    public LayerMask FoodLayer;
    public float FieldOfViewRadius; //change name to ...length or ...visionlength etc.
    [Range(1, 360)]public float FieldOfViewAngle;



    private Vector3 positionFood;
    private float AngleToFood;

    public bool CanSeeFood { get; private set; }
    public bool CollidedFood { get; private set; }


    #endregion

    #region Functions

    //function for movement
    private void movement()
    {
        // movement forward
        transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);


        //create randomMovement Number afte rotationFrequenzy frames
        if (!CanSeeFood)
        {
            if (rotationElapsed == randomIntFrequenzy)
            {
                randomIntMovement = Random.Range(0, 3 + forwardFrequenzy);
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
            if (transform.rotation.z != Mathf.Round(90 - AngleToFood))
            {
                //if food is to the right rotate right else left
                if (transform.rotation.z == Mathf.Round(90 - AngleToFood) +- 10)
                {
                    transform.Rotate(0, 0, AngleToFood);
                }
                else if (AngleToFood < 90) 
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
    private void energyMonitor() //rename to energyController or maybe sth else 
    {
        //use energy per frame
        energy = energy - Time.timeScale * energyConsumptionRate * 
            (Mathf.RoundToInt(moveSpeed) + rotationSpeed / 10 + foodConsumptionRate / 2 + FieldOfViewRadius/25 * FieldOfViewAngle/75);

        if (energy <= 0)
        {
            Destroy(gameObject);
        }
        //check if enough energy for egg
        else if(energy >= energyCapEgg)
        {
            currentEgg = Instantiate(YabelEgg, transform.position, Quaternion.Euler(0, 0, 0));
            currentEgg.GetComponent<EggBehavior>().moveSpeed = moveSpeed;
            currentEgg.GetComponent<EggBehavior>().foodConsumptionRate = foodConsumptionRate;
            currentEgg.GetComponent<EggBehavior>().rotationSpeed = rotationSpeed;
            currentEgg.GetComponent<EggBehavior>().FieldOfViewRadius = FieldOfViewRadius;
            currentEgg.GetComponent<EggBehavior>().FieldOfViewAngle = FieldOfViewAngle;

            energy -= energyUsageForEgg;
        }
    }
    void OnCollisionStay2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Food")
        {
            if (collisionInfo.gameObject.GetComponent<HandleFoodEnergy>().energy > 0)
            {
                energy += foodConsumptionRate;
                collisionInfo.gameObject.GetComponent<HandleFoodEnergy>().energy -= foodConsumptionRate;
            }
        }
    }


    #region FOV
    private void FOV()
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
            if (AngleToFood >= 90 - FieldOfViewAngle / 2 && AngleToFood <= 90 + FieldOfViewAngle / 2)
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
    private void OnDrawGizmos()
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
    private Vector2 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector2(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    #endregion
    #endregion

    void Start()
    {
        randomIntMovement = Random.Range(0, 1 + forwardFrequenzy);
    }

    // Update is called once per frame
    void Update()
    {
        energyMonitor();
        FOV();
        movement();
    }
}
