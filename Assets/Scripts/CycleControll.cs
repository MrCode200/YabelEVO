using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class CycleControll : MonoBehaviour
{
    public int Cycle = 0; //add later read only
    public float CycleInSeconds;
    private float CycleSeconds = 0.0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CycleSeconds += Time.deltaTime;
        Cycle = Mathf.FloorToInt(CycleSeconds / CycleInSeconds);

        if (Input.GetKey(KeyCode.RightBracket) && Time.timeScale < 6f)
        {
            Time.timeScale = Time.timeScale + 0.5f;
            Debug.Log(Time.timeScale);
        }
        else if (Input.GetKey(KeyCode.LeftBracket) && Time.timeScale > 0f)
        {
            Time.timeScale = Time.timeScale - 0.5f;
            Debug.Log(Time.timeScale);
        }
    }
}
