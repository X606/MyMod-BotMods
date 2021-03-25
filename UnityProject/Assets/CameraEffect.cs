using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffect : MonoBehaviour
{
    void OnValidate()
    {
        Camera camera = GetComponent<Camera>();

        camera.depthTextureMode = DepthTextureMode.Depth;

#if UNITY_EDITOR

        foreach (Camera camera1 in UnityEditor.SceneView.GetAllSceneCameras())
        {
            camera1.depthTextureMode = DepthTextureMode.Depth;
        }
#endif
    }
}
