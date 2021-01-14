using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EditorHelper : EditorWindow
{
    [MenuItem("Tools/特效OverDraw观察")]
    private static void OpenOrCloseOverDrawProfile()
    {
        GameObject go = GameObject.FindGameObjectWithTag("OverDrawCamera");
        if (go == null)//打开
        {
            UniversalRenderPipelineAsset urpAsset = GraphicsSettings.renderPipelineAsset as UniversalRenderPipelineAsset;
            if(urpAsset != null)
            {
                Camera cam = Camera.main;
                UniversalAdditionalCameraData cameraData = cam.GetUniversalAdditionalCameraData();
                if(cameraData != null)
                {
                    //urpAsset.m_RendererDataList
                    //urpAsset.GetRenderer()
                    //cameraData.SetRenderer()
                }
            }

            GameObject prefab = UnityEngine.Resources.Load("OverdrawCameraPrefab") as GameObject;
            go = GameObject.Instantiate(prefab);
            if (Application.isPlaying)
                GameObject.DontDestroyOnLoad(go);
        }
        else //关闭
        {
            GameObject.DestroyImmediate(go);
        }
    }
}
