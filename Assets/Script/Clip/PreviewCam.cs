using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PreviewCam : MonoBehaviour
{
    public Shader shader;
    private Camera cam;
    
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        cam.SetReplacementShader(shader, "RenderType");
    }

    private void OnDestroy()
    {
        cam.ResetReplacementShader();
    }
}
