using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleFoodEnergy : MonoBehaviour
{
    public int foodEnergyConstant = 100;
    public int foodEnergyVariability = 50;
    public float energy;

    // Start is called before the first frame update
    void Start()
    {
        //create a random amount of energy per food 
        //get a the energy constant (avarage) and from -variability to + variability pick random energy
        energy = Random.Range(
            foodEnergyConstant - foodEnergyVariability, 
            foodEnergyConstant + foodEnergyVariability);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(energy / 6, energy / 6, 0);
        if (energy <= 0)
        {
            Destroy(gameObject);
        }
    }
}
