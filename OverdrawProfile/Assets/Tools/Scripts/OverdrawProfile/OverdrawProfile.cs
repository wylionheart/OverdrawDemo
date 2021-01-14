#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace MobileGameDemo {

    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    [DefaultExecutionOrder(100)]
    class OverdrawProfile : MonoBehaviour
    {        
        public Camera MasterCamera = null;

        Camera thisCamera_ = null;
        protected Camera ThisCamera
        {
            get { return thisCamera_ = thisCamera_ ?? GetComponent<Camera>(); }
        }

        RenderTargetHandle m_CameraColorAttachment;

        void OnEnable()
        {
            if (MasterCamera == null)
            {
                MasterCamera = Camera.main;
            }

            if (ThisCamera != null)
            {
                ThisCamera.clearFlags = CameraClearFlags.SolidColor;
                ThisCamera.backgroundColor = Color.clear;   // clear: (0,0,0,0)
            }

            RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
            RenderPipelineManager.endCameraRendering += OnEndCameraRendering;

        }

        internal void OnDisable()
        {
            RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
            RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
        }

        void OnBeginCameraRendering(ScriptableRenderContext context, Camera camera)
        {
            OnPreRender();
        }
        
        void OnEndCameraRendering(ScriptableRenderContext context, Camera camera)
        {

#if UNITY_EDITOR
            OnPostRender();  
#endif
        }


        void OnPreRender()
        {
            if (ThisCamera != null && MasterCamera != null)
            {
                ThisCamera.transform.position = MasterCamera.transform.position;
                ThisCamera.transform.rotation = MasterCamera.transform.rotation;
                ThisCamera.transform.localScale = MasterCamera.transform.localScale;
                //ThisCamera.rect = MasterCamera.rect;
                ThisCamera.fieldOfView = MasterCamera.fieldOfView;
                ThisCamera.nearClipPlane = MasterCamera.nearClipPlane;
                ThisCamera.farClipPlane = MasterCamera.farClipPlane;
            }
        }

#if UNITY_EDITOR
        private void OnPostRender()
        {

        }

#endif
    }
} // namespace
#endif