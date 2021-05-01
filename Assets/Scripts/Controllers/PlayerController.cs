using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{

    public Interactable focus;
    public LayerMask movementMask;

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
        //GetComponent<NavMeshAgent>().destination = moveDestination;
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, movementMask))
            {
                motor.MoveToPoint(hit.point);

                RemoveFocus();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                // check if we hit an interactable
                Interactable interactable = hit.collider.GetComponent<Interactable>();

                if (interactable != null)
                {
                    SetFocus(interactable);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            motor.Run();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            motor.Walk();
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space should make jump");
           //jump
        }
    }

    void SetFocus(Interactable newFocus)
    {
        if (newFocus != focus)
        {
            if (focus != null)
                focus.OnDefocused();
            focus = newFocus;
            motor.FollowTarget(newFocus);
        }
        newFocus.OnFocused(transform);
    }

    void RemoveFocus()
    {
        if (focus != null)
            focus.OnDefocused();
        focus = null;
        motor.StopFollowingTarget();

    }
}
