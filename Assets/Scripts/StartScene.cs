using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class StartScene : MonoBehaviour
{
    private VideoPlayer _player;
    void Start()
    {
        _player = GetComponent<VideoPlayer>();
    }
    
    void Update()
    {
        if (!_player.isPrepared) return;
        //print(_player.isPlaying);
        if (!_player.isPlaying)
        {
            SceneManager.LoadScene(2);
        }
    }
}
