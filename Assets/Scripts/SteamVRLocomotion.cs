using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;


public class SteamVRLocomotion : MonoBehaviour
{
    [SerializeField] float sensitivity = 0.1f;
    [SerializeField] float MaxSpeed = 1;

    [SerializeField] SteamVR_Action_Vector2 MoveValue;

    float speed = 0;
    CharacterController characterController;
    Transform CameraRig;
    //Transform Head;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        CameraRig = Valve.VR.InteractionSystem.Player.instance.hmdTransform;
        //Head = SteamVR_Render.Top().;
    }

    // Update is called once per frame
    void Update()
    {
        HandleHead();
        HandleHeight();
        CalculateMovement();
    }

    void HandleHead()
    {
        //Store Current
        Vector3 oldPosition = CameraRig.position;
        Quaternion oldRotation = CameraRig.rotation;

        // Rotation
        //transform.eulerAngles = new Vector3(0, Head.rotation.eulerAngles.y, 0);

        // Restore
        CameraRig.position = oldPosition;
        CameraRig.rotation = oldRotation;
    }

    void CalculateMovement()
    {
        Vector3 orientationEuler = new Vector3(0, transform.eulerAngles.y, 0);
        Quaternion orientation = Quaternion.Euler(orientationEuler);
        Vector3 movement = Vector3.zero;

        speed += MoveValue.axis.y * sensitivity;
        speed = Mathf.Clamp(speed, -MaxSpeed, MaxSpeed);

        movement += orientation * (speed * Vector3.forward) * Time.deltaTime;
        print(movement);
        characterController.Move(movement);
    }

    void HandleHeight()
    {
        // Get the head in local space
        //float headHeight = Mathf.Clamp(Head.localPosition.y, 1, 2);
        //characterController.height = Head.localPosition.y;
    }

}