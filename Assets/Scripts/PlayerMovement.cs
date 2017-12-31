using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float runMultiplier;

    private bool isRotateWithMouse = false;
    private bool isAllowMove = false;
    private Vector3 moveVector;
    private float rotationZ;
    
    // Use this for initialization
    void Awake () {
    }

    // Update is called once per frame
    void Update () {
        if (isRotateWithMouse)
        {
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
        }

    }

    private void FixedUpdate()
    {
        if (isAllowMove)
        {
            transform.position += moveVector;
        }
    }

    public void Rotate(float f)
    {
        rotationZ = f;
    }

    public void Move(Vector3 v) {
        moveVector = v * movementSpeed;
    }
    public void Move(Vector3 v, bool run)
    {
        if (run)
        {
            moveVector = v * movementSpeed * runMultiplier;
        }
    }

    public void AllowControl(bool b)
    {
        isAllowMove = b;
        isRotateWithMouse = b;
    }
}
