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
    public float CurrentCriuchHeight;

    public float jumpSpeed = 3;
    public float speed = 1;
    public float gravity = Physics.gravity.y;

    float directionY;
    CharacterController characterController;
    [SerializeField] Transform Robot;
    Vector3 StartRobotPos;
    bool IsCrouch = false;
    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartRobotPos = Robot.localPosition;
        characterController = GetComponent<CharacterController>();
        CrouchButton.onStateDown += CrouchButton_onStateDown;
        CrouchButton.onChange += CrouchButton_onChange;
        CrouchButton.onStateUp += CrouchButton_onStateUp;
        
        JumphButton.onStateDown += JumphButton_onStateDown;
    }

    private void CrouchButton_onChange(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        //IsCrouch = !IsCrouch;
    }

    private void JumphButton_onStateDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Jump();
    }

    private void CrouchButton_onStateUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        //Robot.transform.position -= new Vector3(0, CrouchHeightDifference, 0);
        //transform.position += new Vector3(0, CrouchHeightDifference * 2, 0);
        //LeanTween.moveY(Robot.gameObject, Robot.gameObject.transform.position.y + CrouchHeightDifference, 1);
        IsCrouch = false;
        timer = 1;
        
    }

    private void CrouchButton_onStateDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        IsCrouch = true;
        timer = 0f;
        //LeanTween.moveY(Robot.gameObject, Robot.gameObject.transform.position.y - CrouchHeightDifference, 1);

        //Robot.transform.position += new Vector3(0, CrouchHeightDifference, 0);

    }

    // Update is called once per frame
    void Update()
    {
        Transform CameraTr = Valve.VR.InteractionSystem.Player.instance.hmdTransform;
        
        Vector3 direction = CameraTr.TransformDirection(new Vector3(input.axis.x, 0, input.axis.y));
        directionY -= gravity * Time.deltaTime;
        Mathf.Clamp(directionY, -9.8f, jumpSpeed);
        //print()
        characterController.Move((speed * Vector3.ProjectOnPlane(direction, Vector3.up) + new Vector3(0, directionY,0)) * Time.deltaTime);
        Crouch();
        //characterController.height = Player.instance.eyeHeight;
        //characterController.center = CameraTr.position - Player.instance.transform.position - new Vector3(0,characterController.height / 2,0);
        characterController.center = CameraTr.position - transform.position - new Vector3(0, characterController.height / 2, 0);

    }

    void Crouch()
    {
        if (CrouchButton.state)
        {
            CurrentCriuchHeight += CrouchHeightDifference * Time.deltaTime * CrouchUpSpeed;
            if (CurrentCriuchHeight < CrouchHeightDifference)
            {
                Robot.transform.position += new Vector3(0, CrouchHeightDifference, 0) * Time.deltaTime * CrouchUpSpeed ;
            }
        }
        else
        {
            CurrentCriuchHeight -= CrouchHeightDifference * Time.deltaTime * CrouchUpSpeed;
            if (CurrentCriuchHeight > 0)
            {
                transform.position += new Vector3(0, CurrentCriuchHeight, 0) * Time.deltaTime * CrouchUpSpeed * 2;
                Robot.transform.position -= new Vector3(0, CrouchHeightDifference, 0) * Time.deltaTime * CrouchUpSpeed;

            }
        }
        CurrentCriuchHeight = Mathf.Clamp(CurrentCriuchHeight, 0, CrouchHeightDifference);

        

        characterController.height = Valve.VR.InteractionSystem.Player.instance.eyeHeight - CurrentCriuchHeight;

        if (IsCrouch)
        {
            //timer += Time.deltaTime;
            //timer = Mathf.Clamp(timer, 0,1);

            //CurrentCriuchHeight = CrouchHeightDifference * timer;
            //characterController.height = Valve.VR.InteractionSystem.Player.instance.eyeHeight - CurrentCriuchHeight;
            //Robot.transform.localPosition = StartRobotPos - new Vector3(0, CrouchHeightDifference, 0) * timer;
        }
        else
        {
            //timer -= Time.deltaTime;
            //timer = Mathf.Clamp(timer, 0, 1);

            //characterController.height = Valve.VR.InteractionSystem.Player.instance.eyeHeight - CurrentCriuchHeight * timer;
            //transform.position += new Vector3(0, CurrentCriuchHeight, 0) * Time.deltaTime;
            //Robot.transform.localPosition = StartRobotPos - new Vector3(0, CrouchHeightDifference, 0) * timer;

        }
    }

    void Jump()
    {
        directionY = jumpSpeed;
    }


}
