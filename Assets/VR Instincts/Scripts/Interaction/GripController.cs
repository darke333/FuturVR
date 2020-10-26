using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using RootMotion.FinalIK;
using Valve.VR.InteractionSystem;

public class GripController : MonoBehaviour
{
    public SteamVR_Input_Sources Hand;
    public SteamVR_Action_Boolean ToggleGripButton;
    public SteamVR_Action_Pose position;
    public SteamVR_Behaviour_Skeleton HandSkeleton;
    public SteamVR_Behaviour_Skeleton PreviewSkeleton;
    public Grabber grabber;

    Valve.VR.InteractionSystem.Player player;
    private GameObject ConnectedObject;
    private Transform OffsetObject;
    private bool DomanantGrip;
    [SerializeField] Hand hand;
    //GameObject AssetHand;
    //GameObject SteamVRHand;
    //Transform ConnectPoint;
    VRIK VRIK;
    void Start()
    {
        // Invoke("FindHands", 2);
        if (!hand)
        {
            hand = GetComponent<Hand>();
        }
        VRIK = GameObject.FindObjectOfType<VRIK>();
        //Valve.VR.InteractionSystem.Player.instance.rightHand.mainRenderModel.onControllerLoaded += MainRenderModel_onControllerLoaded1; ;


    }

    /*void FindHands()
    {
        VRIK = GameObject.FindObjectOfType<VRIK>();
        //AssetHand = HandSkeleton.gameObject; ////
        if (Hand == SteamVR_Input_Sources.RightHand)
        {
            SteamVRHand = GameObject.FindGameObjectWithTag("HandModelRight");
            //ConnectPoint = GameObject.FindGameObjectWithTag("HandRightConnectPoint").transform;

        }
        else
        {
            SteamVRHand = GameObject.FindGameObjectWithTag("HandModelLeft");
            //ConnectPoint = GameObject.FindGameObjectWithTag("HandLeftConnectPoint").transform;

        }
    }

    private void MainRenderModel_onControllerLoaded()
    {
        print("left");
    }

    public void ChangeHands(bool IsAsetHands)
    {
        if (IsAsetHands)
        {
            SteamVRHand.SetActive(false);
            AssetHand.SetActive(true);
        }
        else
        {
            SteamVRHand.SetActive(true);
            AssetHand.SetActive(false);
        }


    }*/

    private void Update()
    {
        if (hand)
        {
            if (hand.mainRenderModel && !HandSkeleton)
            {
                //print("ready to take");
                //HandSkeleton = hand.mainRenderModel.handPrefab.GetComponent<SteamVR_Behaviour_Skeleton>();\
                HandSkeleton = hand.mainRenderModel.GetSkeleton();
                if (Hand == SteamVR_Input_Sources.RightHand)
                {
                    VRIK.solver.rightArm.target = GameObject.FindGameObjectWithTag("HandRightConnectPoint").transform;
                }
                else
                {
                    VRIK.solver.leftArm.target = GameObject.FindGameObjectWithTag("HandLeftConnectPoint").transform;
                }
                //VRIK.solver.leftArm.target = null;
            }
        }

        if (ConnectedObject != null )
        {
            if (DomanantGrip || !ConnectedObject.GetComponent<Interactable>().SecondGripped)
            {
                if (ConnectedObject.GetComponent<Interactable>().touchCount == 0 &&  !ConnectedObject.GetComponent<Interactable>().SecondGripped)
                {
                    grabber.FixedJoint.connectedBody = null;
                    grabber.StrongGrip.connectedBody = null;

                    ConnectedObject.transform.position = Vector3.MoveTowards(ConnectedObject.transform.position, transform.position - ConnectedObject.transform.rotation * OffsetObject.GetComponent<GrabPoint>().Offset, .25f);
                    ConnectedObject.transform.rotation = Quaternion.RotateTowards(ConnectedObject.transform.rotation, transform.rotation*Quaternion.Inverse( OffsetObject.GetComponent<GrabPoint>().RotationOffset), 100);

                    //ConnectedObject.transform.position = transform.position - ConnectedObject.transform.rotation * OffsetObject.GetComponent<GrabPoint>().Offset;
                    //ConnectedObject.transform.rotation = transform.rotation * Quaternion.Inverse(OffsetObject.GetComponent<GrabPoint>().RotationOffset);

                    /*
                    SteamVR_Skeleton_PoseSnapshot pose = grabber.ClosestGrabbable().GetComponent<SteamVR_Skeleton_Poser>().GetBlendedPose(GetComponent<Hand>().skeleton);

                    //snap the object to the center of the attach point
                    ConnectedObject.transform.position = transform.TransformPoint(pose.position);
                    ConnectedObject.transform.rotation = transform.rotation * pose.rotation;
                    */

                    grabber.FixedJoint.connectedBody = ConnectedObject.GetComponent<Rigidbody>();
                }
                else if (ConnectedObject.GetComponent<Interactable>().touchCount > 0|| ConnectedObject.GetComponent<Interactable>().SecondGripped)
                {

                    grabber.FixedJoint.connectedBody = null;
                    grabber.StrongGrip.connectedAnchor = OffsetObject.GetComponent<GrabPoint>().Offset;
                    grabber.StrongGrip.connectedBody = ConnectedObject.GetComponent<Rigidbody>();

                }else if(ConnectedObject.GetComponent<Interactable>().touchCount < 0)
                {
                    ConnectedObject.GetComponent<Interactable>().touchCount = 0;
                }
                
            }
            else
            {
                grabber.FixedJoint.connectedBody = null;
                grabber.StrongGrip.connectedBody = null;
                grabber.WeakGrip.connectedBody = ConnectedObject.GetComponent<Rigidbody>();
            }
            if (ToggleGripButton.GetStateUp(Hand))
            {
                Release();
            }
            if(PreviewSkeleton)
                PreviewSkeleton.transform.gameObject.SetActive(false);
        }
        else
        {
            if (grabber.ClosestGrabbable() && PreviewSkeleton)
            {
                PreviewSkeleton.transform.gameObject.SetActive(true);
                OffsetObject = grabber.ClosestGrabbable().transform;
                if (grabber.ClosestGrabbable().GetComponent<SteamVR_Skeleton_Poser>())
                {
                    if (!OffsetObject.GetComponent<GrabPoint>().Gripped)
                    {
                        PreviewSkeleton.transform.SetParent(OffsetObject, false);
                        PreviewSkeleton.BlendToPoser(OffsetObject.GetComponent<SteamVR_Skeleton_Poser>(), 0f);
                    }
                }
            }
            else
            {
                PreviewSkeleton.transform.gameObject.SetActive(false);
            }
            if (ToggleGripButton.GetStateDown(Hand))
            {
                Grip();
            }
        }
    }



    private void Grip()
    {
        GameObject NewObject = grabber.ClosestGrabbable();
        if (NewObject != null)
        {
            OffsetObject = grabber.ClosestGrabbable().transform;
            ConnectedObject = OffsetObject.GetComponent<GrabPoint>().ParentInteractable.gameObject;//find the Closest Grabbable and set it to the connected object
            ConnectedObject.GetComponent<Rigidbody>().useGravity = false;

            OffsetObject.GetComponent<GrabPoint>().Gripped = true;
           // ChangeHands(true);///////////////

            if (ConnectedObject.GetComponent<Interactable>().gripped)
            {
                ConnectedObject.GetComponent<Interactable>().SecondGripped = true;
                if (OffsetObject.GetComponent<GrabPoint>().HelperGrip)
                {
                    DomanantGrip = false;
                    grabber.WeakGrip.connectedBody = ConnectedObject.GetComponent<Rigidbody>();
                    grabber.WeakGrip.connectedAnchor = OffsetObject.GetComponent<GrabPoint>().Offset;
                }
                grabber.WeakGrip.connectedBody = ConnectedObject.GetComponent<Rigidbody>();
                grabber.WeakGrip.connectedAnchor = OffsetObject.GetComponent<GrabPoint>().Offset;

            }
            else
            {
                ConnectedObject.GetComponent<Interactable>().Hand = Hand;
                ConnectedObject.GetComponent<Interactable>().gripped = true;
                if (!OffsetObject.GetComponent<GrabPoint>().HelperGrip)
                {
                    DomanantGrip = true;
                    /*
                    SteamVR_Skeleton_PoseSnapshot pose = grabber.ClosestGrabbable().GetComponent<SteamVR_Skeleton_Poser>().GetBlendedPose(GetComponent<Hand>().skeleton);

                    //snap the object to the center of the attach point
                    ConnectedObject.transform.position = transform.TransformPoint(pose.position);
                    ConnectedObject.transform.rotation = transform.rotation * pose.rotation;
                    */

                    //ConnectedObject.transform.position = Vector3.MoveTowards(ConnectedObject.transform.position, transform.position - ConnectedObject.transform.rotation * OffsetObject.GetComponent<GrabPoint>().Offset, 1);
                    //ConnectedObject.transform.rotation = Quaternion.RotateTowards(ConnectedObject.transform.rotation, transform.rotation * Quaternion.Inverse(OffsetObject.GetComponent<GrabPoint>().RotationOffset), 100);

                    ConnectedObject.transform.position = transform.position - ConnectedObject.transform.rotation * OffsetObject.GetComponent<GrabPoint>().Offset;
                    ConnectedObject.transform.rotation = transform.rotation * Quaternion.Inverse(OffsetObject.GetComponent<GrabPoint>().RotationOffset);

                    ConnectedObject.GetComponent<Interactable>().GrippedBy = transform.parent.gameObject;
                }
            }
            if (OffsetObject.GetComponent<SteamVR_Skeleton_Poser>()&&HandSkeleton)
            {
                HandSkeleton.transform.SetParent(OffsetObject, false);
                HandSkeleton.BlendToPoser(OffsetObject.GetComponent<SteamVR_Skeleton_Poser>(), 0f);
            }


        }
    }
    private void Release()
    {
       // ChangeHands(false);//////////

        grabber.FixedJoint.connectedBody = null;
        grabber.StrongGrip.connectedBody = null;
        grabber.WeakGrip.connectedBody = null;
        
        ConnectedObject.GetComponent<Rigidbody>().velocity = position.GetVelocity(Hand);// + transform.parent.GetComponent<Rigidbody>().velocity;
        ConnectedObject.GetComponent<Rigidbody>().angularVelocity = position.GetAngularVelocity(Hand);//+ transform.parent.GetComponent<Rigidbody>().angularVelocity;
        ConnectedObject.GetComponent<Rigidbody>().useGravity = true;
        if (!ConnectedObject.GetComponent<Interactable>().SecondGripped)
        {
            
            ConnectedObject.GetComponent<Interactable>().gripped = false;

            ConnectedObject.GetComponent<Interactable>().GrippedBy =null;

        }
        else
        {
            ConnectedObject.GetComponent<Interactable>().SecondGripped = false;
        }
        
        ConnectedObject = null;
        if (OffsetObject.GetComponent<SteamVR_Skeleton_Poser>() && HandSkeleton)
        {
            HandSkeleton.transform.SetParent(transform, false);
            HandSkeleton.BlendToSkeleton();
        }
        OffsetObject.GetComponent<GrabPoint>().Gripped = false;
        OffsetObject = null;
    }
    
}
