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
        //base.OnInspectorGUI();
        DrawDefaultInspector();
    }

}
#endif

public class AudioHandler : MonoBehaviour
{
    [SerializeField] AudioSource _audioSource;
    [Header("Clips")]
    [SerializeField] AudioClip _start;
    [SerializeField] List<AudioClip> _reminders;

    int _counter = 0;

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
}
