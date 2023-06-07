using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleMeatEnergy : MonoBehaviour
{
    public float energy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(energy / 8 + 10, energy / 8 + 10, 0);
        if (energy <= 0)
        {
            Destroy(gameObject);
        }
    }
}
