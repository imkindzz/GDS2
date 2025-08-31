using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRenderCamera : MonoBehaviour
{
    void Awake()
    {
        Canvas canvas = GetComponent<Canvas>();

        
            
        Camera mainCam = Camera.main;

                
                    canvas.worldCamera = mainCam;
                    
                    canvas.renderMode = RenderMode.ScreenSpaceCamera;
                
            
        
    }
}
