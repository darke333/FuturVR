using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Grabber : MonoBehaviour
{
    public ConfigurableJoint StrongGrip;
    public ConfigurableJoint WeakGrip;
    public FixedJoint FixedJoint;
    //GameObject AssetHand;
    //GameObject SteamVRHand;
    //GripController gripController;
    void Start()
    {
        //gripController = GetComponentInParent<GripController>();
        /*AssetHand = gripController.HandSkeleton.gameObject;
        if (gripController.Hand == SteamVR_Input_Sources.RightHand)
        {
            SteamVRHand = GameObject.FindGameObjectWithTag("HandModelRight");
        }
        else
        {
            SteamVRHand = GameObject.FindGameObjectWithTag("HandModelLeft");
        }*/
    }

    

    public List<GameObject> NearObjects = new List<GameObject>();

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<GrabPoint>())
        {
            //gripController.ChangeHands();
            //Debug.Log("It didn't crash!");
            if (!other.GetComponent<GrabPoint>().Gripped)
            {
                NearObjects.Add(other.gameObject);
            }
            
        }
        //Debug.Log(NearObjects);
    }
    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<GrabPoint>())
        {
            NearObjects.Remove(other.gameObject);

            //// Transfer from asset to normal SteamVr hands
            //gripController.ChangeHands();


        }
    }


    public void ChangeHands()
    {
        //SteamVRHand.SetActive(AssetHand.activeSelf);
    }

    public GameObject ClosestGrabbable()
    {
        GameObject ClosestGameObj = null;
        float Distance = float.MaxValue;
        if (NearObjects != null)
        {
            foreach (GameObject GameObj in NearObjects)
            {
                if (!GameObj.GetComponent<GrabPoint>().RestrictByRotation || GameObj.GetComponent<GrabPoint>().RotationLimit > Quaternion.Angle(transform.rotation, GameObj.transform.rotation))
                {
                    if ((GameObj.transform.position - transform.position).sqrMagnitude < Distance)
                    {
                        ClosestGameObj = GameObj;
                        Distance = (GameObj.transform.position - transform.position).sqrMagnitude;
                    }
                }
            }
        }
        return ClosestGameObj;
    }
}
