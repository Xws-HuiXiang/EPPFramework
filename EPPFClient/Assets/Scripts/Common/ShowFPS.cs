using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 用于显示当前的帧率。使用方法：随便挂到一个物体上
/// </summary>
public class ShowFPS : MonoBehaviour
{
    public int fpsTarget;
    public float updateInterval = 0.5f;
    private float lastInterval;
    private int frames = 0;
    private float fps;

    private void Start()
    {
        if(fpsTarget != 0)
        {
            //设置帧率
            Application.targetFrameRate = fpsTarget;
        }
        lastInterval = Time.realtimeSinceStartup;
        frames = 0;
    }

    private void Update()
    {
        ++frames;
        float timeNow = Time.realtimeSinceStartup;
        if (timeNow >= lastInterval + updateInterval)
        {
            fps = frames / (timeNow - lastInterval);
            frames = 0;
            lastInterval = timeNow;
        }
    }
    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.normal.background = null;
        style.normal.textColor = new Color(1.0f, 0.5f, 0.0f);
        style.fontSize = 24;

        GUI.Label(new Rect(200, 40, 100, 30), fps.ToString(), style);
    }
}
