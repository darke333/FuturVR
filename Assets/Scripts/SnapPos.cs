using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class SnapPos : MonoBehaviour
{
    Transform CurrentAttahcedObj;
    Transform ReadyToAttachObj;
    FixedJoint fixedJoint;

    [SerializeField] Transform SnapTo;
    [SerializeField] Rigidbody ConnectTo;


    [SerializeField] float AttahedForce = 250;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!fixedJoint && CurrentAttahcedObj)
        {
            CurrentAttahcedObj = null;
            ReadyToAttachObj = null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.transform == ReadyToAttachObj)
        {
            AttachObj();
        }
    }

    void AttachObj()
    {
        if (CurrentAttahcedObj != ReadyToAttachObj)
        {
            CurrentAttahcedObj = ReadyToAttachObj;
            if (SnapTo)
            {
                CurrentAttahcedObj.position = SnapTo.position;
                CurrentAttahcedObj.rotation = SnapTo.rotation;
            }
            else
            {
                CurrentAttahcedObj.position = transform.position;
                CurrentAttahcedObj.rotation = transform.rotation;
            }
            fixedJoint = CurrentAttahcedObj.gameObject.AddComponent<FixedJoint>();
            if (ConnectTo)
            {
                fixedJoint.connectedBody = ConnectTo;
            }
            else
            {
                fixedJoint.connectedBody = GetComponent<Rigidbody>();
            }
            fixedJoint.breakForce = AttahedForce;
            
        }
    }

    public void ReadyToAttach(Transform attached)
    {
        ReadyToAttachObj = attached;
    }

    public void ChangeMesh(Mesh mesh)
    {
        //GetComponent<MeshFilter>().mesh = mesh;
    }
}
