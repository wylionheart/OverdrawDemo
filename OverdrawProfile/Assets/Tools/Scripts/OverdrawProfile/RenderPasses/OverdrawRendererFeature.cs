#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class OverdrawRendererFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class BlitSettings
    {
        public RenderPassEvent Event = RenderPassEvent.AfterRenderingTransparents;
        public Material overdrawProfileMaterial = null;
    }
    public BlitSettings settings = new BlitSettings();
    OverdrawPass mPass;

    public override void Create()
    {
        mPass = new OverdrawPass(settings.Event, settings.overdrawProfileMaterial);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
            mPass.Setup(renderingData.cameraData.cameraTargetDescriptor, renderer.cameraColorTarget);
            renderer.EnqueuePass(mPass);  
    }



}
#endif