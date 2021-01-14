#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OverdrawPass : ScriptableRenderPass
{
    const string m_ProfilerTag = "OverDrawProfile Pass";
    RenderTargetIdentifier m_Source;
    Material m_OverDrawProfileMaterial;
    RenderTexture m_ResultRT;
    RenderTargetIdentifier m_ResultRTID;
    TextureDimension m_TargetDimension;

    float anasysizeTime = 0;
    float anasysizeIntervalTime = 0.1f;

    public OverdrawPass(RenderPassEvent renderPassEvent, Material m )
    {
        this.renderPassEvent = renderPassEvent;
        m_OverDrawProfileMaterial = m;
    }

    /// <summary>
    /// Configure the pass
    /// </summary>
    /// <param name="baseDescriptor"></param>
    /// <param name="colorHandle"></param>
    public void Setup(RenderTextureDescriptor baseDescriptor, RenderTargetIdentifier srcRTIdentifier)
    {
        m_Source = srcRTIdentifier;
        m_TargetDimension = baseDescriptor.dimension;
        if (m_OverDrawProfileMaterial == null)
        {
            Shader sh = Shader.Find("OverdrawProfile//OverdrawPost");
            m_OverDrawProfileMaterial = new Material(sh);
        }
        if (m_ResultRT == null)
        {
            m_ResultRT = new RenderTexture(baseDescriptor.width, baseDescriptor.height, 0, RenderTextureFormat.BGRA32);
            m_ResultRTID = new RenderTargetIdentifier(m_ResultRT);
        }

    }

    void anasysizeRenderTexture(RenderTexture rt)
    {
        var oldRT = RenderTexture.active;

        var tex = new Texture2D(rt.width, rt.height);
        RenderTexture.active = rt;
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();

        //File.WriteAllBytes(pngOutPath, tex.EncodeToPNG());
        RenderTexture.active = oldRT;
        RenderBuffer colBuffer = rt.colorBuffer;
        int redCount = 0;
        int blueCount = 0;
        Color[] allCol = tex.GetPixels();
        for (int i = 0; i < allCol.Length; i++)
        {
            if (allCol[i].r >= 0.9f)
                redCount++;
            if (allCol[i].b >= 0.9f)
                blueCount++;
        }
        float warringPre1 = redCount / (float)allCol.Length;
        if (Application.isPlaying)//游戏模式，扩大点
        {
            if (warringPre1 > 0.10f)
                UnityEngine.Debug.LogError(string.Format(" 特效OverDraw大于75层 = {0}%面积", +warringPre1 * 100));
        }
        else//编辑模式严格点
        {
            if (warringPre1 > 0.05f)
                UnityEngine.Debug.LogError(string.Format(" 特效OverDraw大于75层 = {0}%面积", +warringPre1 * 100));
        }

        float warringPre2 = blueCount / (float)allCol.Length;
        if (warringPre2 > 0.25f)
            UnityEngine.Debug.LogError(string.Format(" 特效OverDraw大于50层 = {0}%面积", +warringPre2 * 100));

    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (m_OverDrawProfileMaterial == null)
        {
            Debug.LogErrorFormat("Missing {0}. {1} render pass will not execute. Check for missing reference in the renderer resources.", 
                m_OverDrawProfileMaterial, GetType().Name);
            return;
        }

            // Note: We need to get the cameraData.targetTexture as this will get the targetTexture of the camera stack.
            // Overlay cameras need to output to the target described in the base camera while doing camera stack.
        ref CameraData cameraData = ref renderingData.cameraData;
        Camera camera = cameraData.camera;
        RenderTargetIdentifier cameraTarget = (cameraData.targetTexture != null) ? new RenderTargetIdentifier(cameraData.targetTexture) : BuiltinRenderTextureType.CameraTarget;

        CommandBuffer cmd = CommandBufferPool.Get(m_ProfilerTag);

        RenderTextureDescriptor rtd = new RenderTextureDescriptor(Screen.width, Screen.height);
        int overDrawRT = Shader.PropertyToID("_OverDrawTex");
        cmd.GetTemporaryRT(overDrawRT, rtd, FilterMode.Bilinear);

        cmd.Blit(m_Source, overDrawRT);

        cmd.Blit(overDrawRT, m_Source, m_OverDrawProfileMaterial);

        anasysizeTime += Time.deltaTime;
        if (anasysizeTime > anasysizeIntervalTime)
        {
            anasysizeTime = 0;
            //camera.targetTexture = m_ResultRT;
            cmd.Blit(m_Source, m_ResultRT);

            anasysizeRenderTexture(m_ResultRT);
            //camera.targetTexture = null;
        }


        cmd.ReleaseTemporaryRT(overDrawRT);
        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    public override void FrameCleanup(CommandBuffer cmd)
    {
        //base.FrameCleanup(cmd);
    }

}
#endif