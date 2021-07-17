using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{

    private float forwardInput;
    private float horizontalInput;
    private float turnSpeed = 21.0f;
    public Camera mainCamera;
    public Camera seatCamera;
    public KeyCode switchKey = KeyCode.V;

    // speedometer
    [SerializeField] float speed;
    [SerializeField] float rpm;
    [SerializeField] GameObject centerOfMass;
    [SerializeField] private float horsePower = 0;
    [SerializeField] TextMeshProUGUI speedometerText;
    [SerializeField] TextMeshProUGUI rpmText;
    private Rigidbody playerRb;

    [SerializeField] List<WheelCollider> allWeels;
    [SerializeField] int wheelsOnGround;


    // Multiplayer
    public string inputID;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerRb.centerOfMass = centerOfMass.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        horizontalInput = Input.GetAxis("Horizontal" + inputID);
        forwardInput = Input.GetAxis("Vertical" + inputID);

        if(isOnGround())
        {
            // transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput);
            playerRb.AddRelativeForce(Vector3.forward * forwardInput * horsePower);
            transform.Rotate(Vector3.up, Time.deltaTime * turnSpeed * horizontalInput);

            if (Input.GetKeyDown(switchKey))
            {
                mainCamera.enabled = !mainCamera.enabled;
                seatCamera.enabled = !seatCamera.enabled;
            }

            speed = Mathf.Round(playerRb.velocity.magnitude * 3.6f);
            speedometerText.SetText("Speed: " + speed + "km");

            rpm = Mathf.Round((speed % 30) * 40);
            rpmText.SetText("RPM: " + rpm);
        }
    }

    bool isOnGround()
    {
        wheelsOnGround = 0;
        foreach( WheelCollider wheel in allWeels)
        {
            if(wheel.isGrounded){
                wheelsOnGround++;
            }
        }

        if(wheelsOnGround == 4)
        {
            return true;
        }

        return false;
    }
}
