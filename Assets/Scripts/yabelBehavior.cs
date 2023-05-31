using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class yabelBehavior : MonoBehaviour
{
    #region Variables

    public float moveSpeed = 4f;
    [Range(1,360)]public float rotationSpeed = 45f;

    //randomIntFrequenzy is how many Frames it takes to change movement state
    public int randomIntFrequenzy = 60;
    //forwardFrequenzy is how often it doesn't turn
    public int forwardFrequenzy = 2;
    //used to check(count) at which frame the programm is
    private int rotationElapsed = 0;
    int randomIntMovement = 5;

    public float energy = 100f;
    public float energyConsumptionRate = 0.1f; //can be deleted probably
    public float foodConsumptionRate = 20f;

    public LayerMask FoodLayer;
    public float FieldOfViewRadius = 10; //change name to ...length or ...visionlength etc.
    [Range(1, 360)]public float FieldOfViewAngle = 45;

    private Transform transformFood;

    public bool CanSeeFood { get; private set; }


    #endregion

    #region Functions

    //function for movement
    private void movement()
    {
        // movement forward
        transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);

        //create randomMovement Number afte rotationFrequenzy frames
        if (rotationElapsed == randomIntFrequenzy)
        {
            randomIntMovement = Random.Range(0, 1 + forwardFrequenzy);
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
    //function for energy consumption
    private void energyMonitor() //rename to energyController or maybe sth else 
    {
        energy = energy - energyConsumptionRate * Mathf.RoundToInt(moveSpeed);
        if (energy <= 0)
        {
            Destroy(gameObject);
        }
    }

    //check for collision
    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Food")
        {
            if (collisionInfo.gameObject.GetComponent<HandleFoodEnergy>().energy>0)
            {
                energy += foodConsumptionRate;
                collisionInfo.gameObject.GetComponent<HandleFoodEnergy>().energy -= foodConsumptionRate;
            }
            else
            {
                Destroy(collisionInfo.gameObject);
            }            
        }
    }

    private void FOV()
    {
        //get all food collided in a circle around the yabel 
        Collider2D[] rangeCheck = Physics2D.OverlapCircleAll(transform.position, FieldOfViewRadius, FoodLayer);

        //if food has been found
        if (rangeCheck.Length > 0)
        {
            //get the component transform from food and get the directionOfFood in a Vector2
            transformFood = rangeCheck[0].transform;
            Vector2 directionOfFood = (transformFood.position - transform.position).normalized;
            Debug.Log(transformFood);

            //check if target is in the FOV angle
            if(Vector2.Angle(transform.up, directionOfFood) < FieldOfViewAngle/2)
            {
                float distanceToFood = Vector2.Distance(transform.position, transformFood.position);
                RaycastHit2D vision = Physics2D.Raycast(transform.forward, directionOfFood, distanceToFood, FoodLayer);

                //check from objects position to the direction of food within the distanceToTarget
                if (vision.collider != null)  //theoreticly can be removed
                {
                    CanSeeFood = true;
                    Debug.Log("hit");
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
        else if(CanSeeFood)
        {
            CanSeeFood = false;
        }
    }

    //draw field of view UI
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, FieldOfViewRadius);

        Vector3 angleNeg = DirectionFromAngle(-transform.eulerAngles.z, -FieldOfViewAngle / 2);
        Vector3 anglePos = DirectionFromAngle(transform.eulerAngles.z, FieldOfViewAngle / 2);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + angleNeg * FieldOfViewRadius);
        Gizmos.DrawLine(transform.position, transform.position + anglePos * FieldOfViewRadius);

        if(CanSeeFood)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transformFood.position);
        }
    }

    //for Ui create FOV
    private Vector2 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector2(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    private IEnumerator FOVcheck()
    {
        //run FOV check every 0.1second
        WaitForSeconds wait = new WaitForSeconds(0.1f);

        while (true)
        {
            yield return wait;
            FOV();
        }
    }

    #endregion

    void Start()
    {
        StartCoroutine(FOVcheck());
    }

    // Update is called once per frame
    void Update()
    {
        energyMonitor();
        if (CanSeeFood ==  false )
        {
            movement();
        }
    }
}
