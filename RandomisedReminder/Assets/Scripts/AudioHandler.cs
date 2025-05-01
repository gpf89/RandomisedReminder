using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        EditorGUILayout.TextField(audioHandler.FormattedTime());
        //base.OnInspectorGUI();
        DrawDefaultInspector();
    }

}
#endif

public class AudioHandler : MonoBehaviour
{
    [SerializeField] AudioSource _audioSource;
    [SerializeField] int _duration;
    [Header("Clips")]
    [SerializeField] AudioClip _start;
    [SerializeField] List<AudioClip> _reminders;

    int _counter = 0;
    public float RemainingTime { get => _remainingTime;  }
    float _remainingTime;
    bool _isCountingDown;

    public void Start()
    {
        _remainingTime = _duration * 60f;
        _isCountingDown = true;
    }

    public void Update()
    {
        if (_remainingTime >0)
        {
            _remainingTime -= Time.deltaTime;
        }
        else
        {
            _isCountingDown = false;
        }
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

        return $"{minutes} : {seconds}";
    }
}
