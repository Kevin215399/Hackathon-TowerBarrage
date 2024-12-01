using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeContent : MonoBehaviour
{
    public float ratio = 0;
    /*

    public GameObject content;
    private Camera mainCamera;

    private void Update()
    {
        mainCamera = Camera.main;
        Vector2 screenResolution = new Vector2(Screen.width, Screen.height);
        float height = Screen.height;
        float width = Screen.width;

        float screenAspect = width / height;

        mainCamera.aspect = screenAspect;
        
                float cameraHeight = 100f * mainCamera.orthographicSize * 2f;
                float cameraWidth = cameraHeight * screenAspect;
                Debug.Log("width: " + cameraWidth + " height: " + cameraHeight);

                SpriteRenderer contentSR = content.GetComponent<SpriteRenderer>();
                float contentHeight = contentSR.bounds.size.y;
                float contentWidth = contentSR.bounds.size.x;

                Debug.Log("cH " + contentWidth + " cW " + contentHeight);


                float contentScaleRatioHeight = cameraHeight / contentHeight;
                float contentScaleRatioWidth = cameraWidth / contentWidth;

                content.transform.localScale = new Vector3(contentScaleRatioWidth, contentScaleRatioHeight, 1);
            
    }*//*
    private Camera mainCamera;

    void Update()
    {
        mainCamera = Camera.main;
        transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, 0);
        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(Vector3.zero);
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(mainCamera.rect.width, mainCamera.rect.height));
        Vector3 screenSize = topRight - bottomLeft;
        float screenRatio = screenSize.x / screenSize.y;
        float desiredRatio = transform.localScale.x / transform.localScale.y;

        if (screenRatio > desiredRatio)
        {
            float height = screenSize.y;
            transform.localScale = new Vector3(height * desiredRatio, height);
        } else
        {
            float width = screenSize.x;
            transform.localScale = new Vector3(width, width / desiredRatio);
        }


        ratio = desiredRatio;
    }*/
    private bool grow = true;
    [SerializeField] private Anchor[] anchors = new Anchor[2];
    private void Start(){
        Camera.main.orthographicSize = 0.1f;
    }
    private void Update(){
        grow = false;
        foreach(var anchor in anchors){
            if(!anchor.isOnScreen){
                grow = true;
            }
        }
        if(grow){
            Camera.main.orthographicSize += 0.1f;
        }
    }



}
