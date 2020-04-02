using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;

public class mosquitocontroller : MonoBehaviour
{

    //interactive item
    VRInteractiveItem vrIntItem;

    Vector3 skeeterTarget;

    Rigidbody skeeterRigidBody;

    //speed
    public float speed = 0.5f;

    //min distance
    public float minDistance = .5f;

    //flag to track mosquito moving
    bool isMoving = true;
    bool isDead = false;

    void Awake()
    {
        //grab the interactive component
        vrIntItem = GetComponent<VRInteractiveItem>();

        skeeterRigidBody = GetComponent<Rigidbody>();

        //make skeeter look at us
        transform.LookAt(Camera.main.transform.position);

        //target for mosquito
        skeeterTarget = Camera.main.transform.position;
    }

    void Update()
    {
        //check to see if the skeeter is alive
        skeeterTarget = Camera.main.transform.position;

        if (isMoving)
        {
            //calculate the distance from target\
            float distance = Vector3.Distance(transform.position, skeeterTarget);
            if ((distance <= minDistance) && isDead)
            {
                isMoving = false;
            }
            else
            {
                //calculate movement speed
                float movementStep = speed * Time.deltaTime;

                //move that step
                transform.position = Vector3.MoveTowards(transform.position, skeeterTarget, movementStep);
            }
        }    
    }
    private void OnEnable()
    {
        vrIntItem.OnClick += HandleClick;
    }

    //when our game object is disabled
    private void OnDisable()
    {
        vrIntItem.OnClick -= HandleClick;
    }

    //called when mosquito is clicked on
    public void HandleClick()
    {
        if (isMoving && !isDead) {
            //rotate 180 around z axis
            transform.Rotate(new Vector3(0, 0, 180));
            

            //Turn off iskinematic so it falls down
            skeeterRigidBody.isKinematic = false;

            //stops it moving
            isMoving = false;
            isDead = true;
        }

    }


}

