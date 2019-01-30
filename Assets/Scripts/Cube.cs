using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{

    public Material rightPosMat; //green color

    Material startMat; // shape of gray
    Renderer renderer;
    Vector3 startPos;


    private void Start()
    {
        startPos = transform.localPosition;
        renderer = transform.GetComponent<Renderer>();
        startMat = renderer.material;
    }

    private void Update()
    {
        if (startPos == transform.localPosition) //Check if cube is on right place
        {
            renderer.material = rightPosMat;

            //Debug.Log("Color changed!");
        }
        else
            renderer.material = startMat;
    }

    public bool OnRightPlace()
    {
        return startPos == transform.localPosition;

    }
}
