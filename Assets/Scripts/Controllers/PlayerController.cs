using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    public Interactable focus;
    public LayerMask movementMask;
    public float yawSpeed = 100f;
    private float currentYaw = 0f;

    Camera cam;
    PlayerMotor motor;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        motor = GetComponent<PlayerMotor>();
    }

    // Update is called once per frame
    void Update()
    {
        float horInput = Input.GetAxis("Horizontal");
        float verInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horInput, 0f, verInput);
        Vector3 moveDestination = transform.position - movement;
        motor.MoveToPoint(moveDestination);

        // GetComponent<NavMeshAgent>().destination = moveDestination;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            motor.Run();
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            motor.Walk();
        }
    }
}
