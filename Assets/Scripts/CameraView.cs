using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraView : MonoBehaviour
{
    public Camera Camera;

    public float zoomSpeed = 1f;
    public float moveSpeed = 1f;

    // Update is called once per frame
    void Update()
    {
        // Move left when key "a" is pressed
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * moveSpeed * Camera.main.orthographicSize * Time.deltaTime);
        }

        // Move right when key "d" is pressed
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * moveSpeed * Camera.main.orthographicSize * Time.deltaTime);
        }

        // Move up when key "w" is pressed
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.up * moveSpeed * Camera.main.orthographicSize * Time.deltaTime);
        }

        // Move down when key "s" is pressed
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.down * moveSpeed * Camera.main.orthographicSize * Time.deltaTime);
        }

        if (Input.mouseScrollDelta.y > 0)
        {
            Camera.orthographicSize += zoomSpeed;
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            Camera.orthographicSize -= zoomSpeed;
        }
    }
}
