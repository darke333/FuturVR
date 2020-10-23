using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class TouchpadLocomotion : MonoBehaviour
{

    public SteamVR_Action_Vector2 input;
    public SteamVR_Action_Boolean CrouchButton;
    public SteamVR_Action_Boolean JumphButton;

    [SerializeField] float CrouchHeightDifference = 0.3f;
    [SerializeField] float CrouchUpSpeed = 1;
    float CurrentCrouchHeight;

    public float jumpSpeed = 3;
    public float speed = 1;
    public float gravity = Physics.gravity.y;

    float directionY;
    CharacterController characterController;
    [SerializeField] Transform Robot;

    // Start is called before the first frame update
    void Start()
    {

        characterController = GetComponent<CharacterController>();
        
        JumphButton.onStateDown += JumphButton_onStateDown;
    }

    private void JumphButton_onStateDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Jump();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Transform CameraTr = Valve.VR.InteractionSystem.Player.instance.hmdTransform;
        Move(CameraTr);
        Crouch();
        CalculateCenter(CameraTr);

    }

    void Move(Transform CameraTr)
    {
        Vector3 direction = CameraTr.TransformDirection(new Vector3(input.axis.x, 0, input.axis.y));
        directionY -= gravity * Time.deltaTime;
        Mathf.Clamp(directionY, -9.8f, jumpSpeed);
        characterController.Move((speed * Vector3.ProjectOnPlane(direction, Vector3.up) + new Vector3(0, directionY, 0)) * Time.deltaTime);
    }

    void CalculateCenter(Transform CameraTr)
    {
        /*
        Vector3 cameraRelative = CameraTr.position - transform.position - new Vector3(0, characterController.height / 2, 0);

        float oldX = cameraRelative.x;
        float oldY = cameraRelative.y;
        float oldZ = cameraRelative.z;
        float angle = transform.eulerAngles.y;

        float newX = oldX * Mathf.Cos(angle) - oldZ * Mathf.Sin(angle);
        float newY = oldY;
        float newZ = oldZ * Mathf.Sin(angle) + oldZ * Mathf.Cos(angle);

        Vector3 center = new Vector3(newX, newY, newZ);

        //characterController.center = center;
        CameraSphere.localPosition = cameraRelative;
        ChangedSphere.localPosition = center;
        */

        Vector3 cameraRelative = transform.InverseTransformPoint(CameraTr.position);
        characterController.center = cameraRelative - new Vector3(0, characterController.height / 2, 0);
    }

    void Crouch()
    {
        if (CrouchButton.state)
        {
            CurrentCrouchHeight += CrouchHeightDifference * Time.deltaTime * CrouchUpSpeed;
            if (CurrentCrouchHeight < CrouchHeightDifference)
            {
                Robot.transform.position += new Vector3(0, CrouchHeightDifference, 0) * Time.deltaTime * CrouchUpSpeed ;
            }
        }
        else
        {
            CurrentCrouchHeight -= CrouchHeightDifference * Time.deltaTime * CrouchUpSpeed;
            if (CurrentCrouchHeight > 0)
            {
                transform.position += new Vector3(0, CurrentCrouchHeight, 0) * Time.deltaTime * CrouchUpSpeed * 2;
                Robot.transform.position -= new Vector3(0, CrouchHeightDifference, 0) * Time.deltaTime * CrouchUpSpeed;

            }
        }
        CurrentCrouchHeight = Mathf.Clamp(CurrentCrouchHeight, 0, CrouchHeightDifference);

        

        characterController.height = Valve.VR.InteractionSystem.Player.instance.eyeHeight - CurrentCrouchHeight;
        
    }

    void Jump()
    {
        directionY = jumpSpeed;
    }


}
