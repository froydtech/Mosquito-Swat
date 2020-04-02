using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : MonoBehaviour
{

    //minimum and amximum scale values
    public float minScale = .7f;
    public float maxScale = 2.5f;

    // Start is called before the first frame update
    void Start()
    {
        //Set the Y position to ground level
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        //obtain a random scale
        float scale = Random.Range(minScale, maxScale);

        //change scale
        transform.localScale *= scale;

        //rotate about the y axis for the trees

        //random rotation value y
        float rotationY = Random.Range(0, 360);

        transform.Rotate(0, rotationY, 0, Space.World);
    }

    
}
