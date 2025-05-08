using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(AudioHandler))]
class AudioHandlerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var audioHandler = (AudioHandler)target;
        if (audioHandler == null) return;

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Play clip"))
        {
            audioHandler.PlayClip();
        }
        if (GUILayout.Button("Pause clip"))
        {
            audioHandler.PauseClip();
        }
        if (GUILayout.Button("Stop clip"))
        {
            audioHandler.StopClip();
        }
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("Next clip"))
        {
            audioHandler.ChangeClip();
        }
        if (GUILayout.Button("Initialise"))
        {
            audioHandler.Initialise();
        }
        if (GUILayout.Button("BeginCountdown"))
        {
            audioHandler.BeginCountdown();
        }
        EditorGUILayout.TextField(audioHandler.FormattedTime());
        //base.OnInspectorGUI();
        DrawDefaultInspector();
    }

}
#endif

public class AudioHandler : MonoBehaviour
{
    [SerializeField] AudioSource _audioSource;
    [Range(0,60)]
    [SerializeField] float _duration; //float temporarily to allow short durations but will be int
    [Header("Clips")]
    [SerializeField] AudioClip _start;
    [SerializeField] List<AudioClip> _reminders;
    [SerializeField] TextMeshProUGUI _displayTime;
    
    int _counter = 0;
    public float RemainingTime { get => _remainingTime;  }
    float _remainingTime;
    bool _isCountingDown = false;

    float MINS_TO_SECS = 60f;
    // this will vary later
    private int _clipCount = 4;
    private float _timeToNextClip;

    public void Start()
    {
        _remainingTime = _duration * MINS_TO_SECS;
    }

    public void Update()
    {
        if (_isCountingDown == false) return;
        if (_remainingTime > 0f)
        {
            _remainingTime -= Time.deltaTime;
            _displayTime.text = FormattedTime();
            // dirty hack for now
            // before _remainingTime = 0f in the else block it displays -01:-01 
            if (_remainingTime < 0.1)
            {
                _remainingTime = 0f;
            }
        }
        else
        {
            Debug.Log(FormattedTime());
            _remainingTime = 0f;
            Debug.Log(FormattedTime());
            _isCountingDown = false;
        }


        _timeToNextClip -= Time.deltaTime;
        if (_timeToNextClip < 0)
        {
            Debug.Log("Reset now");
            Initialise();
        }
        
        
    }

    public void Initialise()
    {
        _timeToNextClip = _duration * MINS_TO_SECS / _clipCount;
    }

    public void BeginCountdown()
    {
        _isCountingDown = true;
    }

    public void PlayClip()
    {
        _audioSource.Play();
    }

    public void PauseClip()
    {
        _audioSource.Pause();
    }
    public void StopClip()
    {
        _audioSource.Stop();
    }

    public void ChangeClip()
    {
        _counter++;
        _audioSource.clip = _reminders[_counter % _reminders.Count];
    }

    public string FormattedTime()
    {
        int minutes = Mathf.FloorToInt(_remainingTime / 60);
        int seconds = Mathf.FloorToInt((_remainingTime % 60));

        return minutes.ToString("00") + " : " + seconds.ToString("00");
    }
}
