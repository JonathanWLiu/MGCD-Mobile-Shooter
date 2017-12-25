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

    private Rigidbody2D rb;
    // Use this for initialization
    void Awake () {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update () {
        if (isRotateWithMouse)
        {
            Vector3 mouseIn = Input.mousePosition;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(mouseIn.x, mouseIn.y, 10));
            Vector3 difference = mousePos - transform.position;
            float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
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

    public void AllowControl()
    {
        isAllowMove = true;
        isRotateWithMouse = true;
    }
}
