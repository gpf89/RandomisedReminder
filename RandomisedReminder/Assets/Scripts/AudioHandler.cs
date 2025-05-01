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

        if (GUILayout.Button("Play clip"))
        {
            audioHandler.PlayClip();
        }
        //base.OnInspectorGUI();
        DrawDefaultInspector();
    }

}
#endif

public class AudioHandler : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    public void PlayClip()
    {
        audioSource.Play();
    }
}
