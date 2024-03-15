using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceWindowSize : MonoBehaviour
{
    public int width = 1920; // 设置期望的窗口宽度
    public int height = 1080; // 设置期望的窗口高度
    public bool fullScreen = false; // 是否全屏

    void Start()
    {
        // 设置窗口大小
        Screen.SetResolution(width, height, fullScreen);
    }
}
