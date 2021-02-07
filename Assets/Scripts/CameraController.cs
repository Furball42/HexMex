using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float NormalSpeed;
    public float FastSpeed;
    public float MovementSpeed;
    public float MovementTime;
    public float rotationAmount;
    public Vector3 NewPosition;
    public Quaternion NewRotation;

    // Start is called before the first frame update
    void Start()
    {
        NewPosition = transform.position;
        NewRotation = transform.rotation;

    }

    // Update is called once per frame
    void Update()
    {
        HandleMovementInput();
    }

    public void HandleMovementInput(){

        //check for fast speed
        if(Input.GetKey(KeyCode.LeftShift))
            MovementSpeed = FastSpeed;
        else
            MovementSpeed = NormalSpeed;

        //camera movement
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            NewPosition += (transform.forward * MovementSpeed);
        }

        if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            NewPosition += (transform.forward * -MovementSpeed);
        }

        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            NewPosition += (transform.right * MovementSpeed);
        }

        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            NewPosition += (transform.right * -MovementSpeed);
        }

        //camera rotation
        if(Input.GetKey(KeyCode.Q))
        {
            // Quaternion target = transform.rotation * Quaternion.AngleAxis(-45f, Vector3.up);
            // transform.rotation = Quaternion.Lerp(transform.rotation, target, Time.deltaTime * MovementTime);

            NewRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        }

        if(Input.GetKey(KeyCode.E))
        {
            // Quaternion target = transform.rotation * Quaternion.AngleAxis(45f, Vector3.up);
            // transform.rotation = Quaternion.Lerp(transform.rotation, target, Time.deltaTime * MovementTime);


            NewRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }

        //do actual transform manipulations
        transform.position = Vector3.Lerp(transform.position, NewPosition, Time.deltaTime * MovementTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, NewRotation, Time.deltaTime * MovementTime);
    }
}
