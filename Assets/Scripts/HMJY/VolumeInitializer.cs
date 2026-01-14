using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class VolumeInitializer : MonoBehaviour
{
    public Volume globalVolume;
    public VolumeProfile targetProfile;

    void Start()
    {
        StartCoroutine(InitVolume());
    }

    private System.Collections.IEnumerator InitVolume()
    {
        yield return null; // 延迟一帧

        if (globalVolume != null && targetProfile != null)
        {
            globalVolume.profile = null;           // 强制触发刷新
            globalVolume.profile = targetProfile;  // 设置目标 Profile
            globalVolume.weight = 1f;

            Debug.Log("Global Volume 初始化成功！");
        }
        else
        {
            Debug.LogWarning("Global Volume 或 Profile 未设置！");
        }
    }
}

