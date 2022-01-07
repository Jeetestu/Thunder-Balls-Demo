using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolutionFix : MonoBehaviour
{
    Vector3 cameraPos;
    float defaultHeight;
    public float minWidth;



    // Start is called before the first frame update
    void Start()
    {
        cameraPos = Camera.main.transform.position;

        defaultHeight = Camera.main.orthographicSize;

        updateCamera();
        //the width of the camera is equal to the height (size) * the camera aspect ratio 
        //defaultWidth = Camera.main.orthographicSize * Camera.main.aspect;
    }

    /*
    private void Update()
    {
        updateCamera();
    }

    */

    // Update is called once per frame
    void updateCamera()
    {
        float width = Camera.main.orthographicSize * Camera.main.aspect;
        if (width < minWidth || width > minWidth)
        {
            Camera.main.orthographicSize = Mathf.Max(defaultHeight, minWidth / Camera.main.aspect);

            //Camera.main.transform.position = new Vector3(cameraPos.x, -1 * (defaultHeight - Camera.main.orthographicSize),-10);
        }
    }
}
