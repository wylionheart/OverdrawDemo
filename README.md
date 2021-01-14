# OverdrawDemo

Current support : 2019.3.0f6 or later with UniversalRP 7.2.0

This is a Demo that uses overdraw to analyze the performance of ParticleSystem. 

Used:renderFeature and customRenderPass

![](Readme/img1.png)

the way to Integrate your own project:
1.drag OverdrawCameraPrefab to hierarchy
You can see Overdraw Scene on left bottom. But no red or green warring.

![](Readme/img4.png)

2.Then add OverDrawRenderPipelineAsset_Renderer to your UniversalRenderPipelineAsset<br>
![](Readme/img4.png)<br>

3.Select OverdrawCameraPrefab : change renderer to OverDrawRenderPipelineAsset_Renderer<br>
![](Readme/img4.png)<br>
Then you can see 
green:overdraw  >  50 
Red:overdraw  >  75
