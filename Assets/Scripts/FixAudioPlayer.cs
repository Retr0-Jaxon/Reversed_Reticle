using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class FixAudioPlayer:MonoBehaviour
{
    [SerializeField]
    private String audioName;

    private void Awake()
    {
        var video = GetComponent<VideoPlayer>();
        video.url= Application.streamingAssetsPath+"/"+audioName;
        video.Play();
        
    }
}







