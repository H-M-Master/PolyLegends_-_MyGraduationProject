using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioSource audioSource;

    [Header("攻击音效列表")]
    public AudioClip[] attackClips;

    // 动画事件调用
    public void PlayAttack()
    {
        if (attackClips.Length > 0 && audioSource != null)
        {
            int index = Random.Range(0, attackClips.Length);
            audioSource.PlayOneShot(attackClips[index]);
        }
    }
}