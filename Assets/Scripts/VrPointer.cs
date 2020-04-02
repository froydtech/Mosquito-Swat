using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using System;
using UnityEngine.Events;


namespace Zenva.VR
{
    public class VrPointer : MonoBehaviour
    {
        static readonly Dictionary<string, InputFeatureUsage<float>> availableFeatures = new Dictionary<string, InputFeatureUsage<float>>
        {
            { "trigger", CommonUsages.trigger },
            { "grip", CommonUsages.grip },
            { "indexTouch", CommonUsages.indexTouch },
            { "thumbTouch", CommonUsages.thumbTouch },
            { "indexFinger", CommonUsages.indexFinger },
            { "middleFinger", CommonUsages.middleFinger },
            { "ringFinger", CommonUsages.ringFinger },
            { "pinkyFinger", CommonUsages.pinkyFinger },
        };
        public enum FeatureOptions
        {
            trigger,
            grip,
            indexTouch,
            thumbTouch,
            indexFinger,
            middleFinger,
            ringFinger,
            pinkyFinger
        };

        //keep devices that are detected
        List<InputDevice> devices;
        [Tooltip("Input Device Role (left/right hand)")]
        public InputDeviceRole deviceRole;

        [Tooltip("Sensitivity of the axis")]
        [Range(0.0f, 1.0f)]
        public float threshold;
        //button press value
        float inputValue;
        //selected feature object
        InputFeatureUsage<float> selectedFeature;

        [Tooltip("Select an input feature")]
        public FeatureOptions feature;

        [Tooltip("Event when the button starts being pressed")]
        public UnityEvent OnPress;

        [Tooltip("Event when the button is released")]
        public UnityEvent OnRelease;

        [Tooltip("Maximum Interaction Distance")]
        public float maxDistance;

        [Tooltip("Shwo ray for debugging")]
        public bool showRay = false;

        //Keep track of interactables
        Interactable currInteractable;
        Interactable prevInteractable;

        //checking to see if btton is pressed

        bool isPressed = false;


        public Vector3 EndPosition { get; private set; }

        // Update is called once per frame

        void Awake()
        {
            devices = new List<InputDevice>();

            //get the label selected by the user

            string featureLabel = Enum.GetName(typeof(FeatureOptions), feature);



            //find the dictionary entry

            availableFeatures.TryGetValue(featureLabel, out selectedFeature);

        }
        void Update()
        {
            InputDevices.GetDevicesWithRole(deviceRole, devices);

            // go through the devices
            for (int i = 0; i < devices.Count; i++)
            {
                //check whether button is pressed
                //1. Check to see if we can read the state of the button
                //2. The buttons value should be true

                if (devices[i].TryGetFeatureValue(selectedFeature, out inputValue) && Math.Round(inputValue, 2) >= threshold)
                {
                    // check if already being pressed
                    if (!isPressed)
                    {
                        isPressed = true;
                    }

                }
                else if (isPressed)
                {
                    //update flag
                    isPressed = false;
                }
            }
            RayCast();
        }

        void RayCast()
        {
            RaycastHit target;
            //found an object
            if (Physics.Raycast(transform.position, transform.forward, out target, maxDistance))
            {

                EndPosition = target.point;
                currInteractable = target.transform.GetComponent<Interactable>();

                //call selection method
                if (currInteractable)
                {
                    currInteractable.Over();
                    PressButton();
                }

            }

            //no object found
            else
            {                
                EndPosition = transform.position + transform.forward * maxDistance;
                //call unselection method
                if (currInteractable)
                {
                    currInteractable.Out();
                }
            }

            //check that selection chagned at all
            if (currInteractable != prevInteractable)
            {
                prevInteractable?.Out();
            }
            prevInteractable = currInteractable;

            if (showRay)
            {
                Debug.DrawRay(transform.position, EndPosition - transform.position, Color.blue);
            }
        }

        public void PressButton()
        {
            if (isPressed)
                currInteractable?.ButtonDown();           
        }
    }
}
