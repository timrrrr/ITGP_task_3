using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController Controller;
    public Transform cam;
    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    public float moveSmoothTime = 0.5f;



    private Vector3 lastMoveDir; //used to move towards this direction with inertia
    private float turnSmoothVelocity;
    private float moveSmoothVelocity;
    [FormerlySerializedAs("actualSpeed")] public float moveSpeed = 0f;
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        
        // local direction depending on input WASD
        Vector3 LocalDir = new Vector3(horizontal, 0f, vertical).normalized; 
        Debug.Log(LocalDir.magnitude);


        if (LocalDir.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(LocalDir.x, LocalDir.z) * Mathf.Rad2Deg + cam.eulerAngles.y; 
            //todo why exactly?
            float facingAngle = Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, facingAngle, 0f);

            Vector3 moveDir = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized;
            lastMoveDir = moveDir;

            float currentSpeed = Controller.velocity.magnitude;
            moveSpeed = Mathf.SmoothDamp(currentSpeed, speed, ref moveSmoothVelocity, moveSmoothTime);
            Controller.Move(moveDir * (moveSpeed * Time.deltaTime));
            
        }
        else if (Controller.velocity.magnitude != 0f)// if it still moves we need to smoothly stop our character
        {
            float currentSpeed = Controller.velocity.magnitude;
            moveSpeed = Mathf.SmoothDamp(currentSpeed, 0f, ref moveSmoothVelocity, moveSmoothTime);
            Controller.Move(lastMoveDir * (moveSpeed * Time.deltaTime));
        }
        

    }
}
