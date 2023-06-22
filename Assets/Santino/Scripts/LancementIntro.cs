using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class LancementIntro : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] GameObject canvasVideo;
    [SerializeField] VideoPlayer video;

    public void Start()
    {
        video.loopPointReached += OnVideoEnd;
    }

    private void OnDestroy()
    {
        video.loopPointReached -= OnVideoEnd;
    }

    void OnVideoEnd(UnityEngine.Video.VideoPlayer vp)
    {
        canvasVideo.SetActive(false);
        animator.Play("TitleAnim");
    }
}
