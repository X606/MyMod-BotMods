using ModLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomLava
{
    [MainModClass]
    public class Main : Mod
    {

        protected override void OnModEnabled()
        {
            Camera[] cameras = GameObject.FindObjectsOfType<Camera>();

            foreach (Camera camera in cameras)
            {
                camera.gameObject.AddComponent<CameraEffector>();
            }
        }

        bool _inResourcesLoad = false;
        protected override UnityEngine.Object OnResourcesLoad(string path)
        {
            if (_inResourcesLoad)
                return null;

            _inResourcesLoad = true;
            try
            {
                if (path == "Prefabs/LevelObjects/Hazards/Lava_Cube")
                {
                    Material mat = AssetLoader.GetObjectFromFile<Material>("customlava", "LavaMaterial");

                    GameObject lava = Resources.Load<GameObject>(path);
                    Renderer renderer = lava.GetComponent<Renderer>();
                    
                    renderer.material = mat;
                }

                return null;
            }
            finally
            {
                _inResourcesLoad = false;
            }
        }

        
    }

    public class CameraEffector : MonoBehaviour
    {
        void Awake()
        {
            Camera camera = GetComponent<Camera>();

            camera.depthTextureMode = DepthTextureMode.Depth;
        }
    }
}
