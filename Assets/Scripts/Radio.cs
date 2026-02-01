using System;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour
{
    public List<MusicChannel>  MusicChannels;
    AudioSource _radioMusicPlayer;
    AudioClip _currentClip;
    float _currentClipProgressTime;
    MusicChannel _currentMusicChannel;

    [Serializable]
    public class MusicChannel
    {
        public string Name;
        public List<AudioClip> MusicQueue;
    }

    private void Awake()
    {
        _radioMusicPlayer = GetComponent<AudioSource>();
        _currentMusicChannel = MusicChannels[0];
    }

    private void Start()
    {
        if (MusicChannels != null && MusicChannels.Count > 0)
        {
            if (MusicChannels[0] != null)
            {
                if (MusicChannels[0].MusicQueue != null && MusicChannels[0].MusicQueue.Count > 0)
                {
                    _currentClip = MusicChannels[0].MusicQueue[0];
                    _radioMusicPlayer.clip = _currentClip;
                    _radioMusicPlayer.Play();
                }
            }
        }
    }

    private void Update()
    {
        if (_currentClip != null)
        {
            _currentClipProgressTime += Time.deltaTime;
            if (_currentClipProgressTime >= _currentClip.length)
            {
                int indexOfCurrentClip = _currentMusicChannel.MusicQueue.IndexOf(_currentClip);
                if (_currentMusicChannel.MusicQueue.Count >= indexOfCurrentClip + 1)
                {
                    indexOfCurrentClip = 0;
                }
                else
                {
                    indexOfCurrentClip++;
                }

                _radioMusicPlayer.clip = _currentMusicChannel.MusicQueue[indexOfCurrentClip];
                _currentClipProgressTime = 0;
                _radioMusicPlayer.Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.R)) 
        {
            int channelIndex = MusicChannels.IndexOf(_currentMusicChannel);
            if (channelIndex + 1 >= MusicChannels.Count)
            {
                channelIndex = 0;
            }
            else
            {
                channelIndex++;
            }

            _currentMusicChannel = MusicChannels[channelIndex];
            _currentClip = _currentMusicChannel.MusicQueue[0];
            _radioMusicPlayer.clip = _currentClip;
            _currentClipProgressTime = 0;
            _radioMusicPlayer.Play();
        }
    }
}
