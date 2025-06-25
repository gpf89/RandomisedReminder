using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
        if (GUILayout.Button("EndCOuntdown"))
        {
            audioHandler.EndCountdown();
        }
        EditorGUILayout.TextField(audioHandler.FormattedTime(audioHandler.RemainingTime));
        //base.OnInspectorGUI();
        DrawDefaultInspector();
    }

}
#endif

public class AudioHandler : MonoBehaviour
{
    enum Density
    {
        SPARSE,
        MEH,
        FREQUENT
    }

    [Header("User Controlls")]
    [Range(5,60)]
    [SerializeField] private int _duration = 5;
    [SerializeField] private Density _density;
    [Header("Clips")]
    [SerializeField] private AudioClip _startClip;
    [SerializeField] private List<AudioClip> _reminderClips;
    [Header("Config")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private TextMeshProUGUI _displayTime;
    [SerializeField] private Slider _durationSetter;
    [SerializeField] private List<GameObject> _disableables;
    
    public float RemainingTime { get => _remainingTime;  }
    private float _remainingTime;
    private float _timeToNextClip;
    private bool _isCountingDown = false;

    private const float MINS_TO_SECS = 60f;
    private const float FRACTIONAL_VARIATION = 0.2f;

    //For Debug purposes only
    private int _clipCounter = 0;

    public void Start()
    {
        _durationSetter.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        Debug.Log(_duration);
        _remainingTime = _duration * MINS_TO_SECS;
        _displayTime.text = FormattedTime(_remainingTime);
    }

    public void Update()
    {
        if (_isCountingDown == false) return;
        SetUIActive(false);
        if (_remainingTime > 0f)
        {
            _remainingTime -= Time.deltaTime;
            _displayTime.text = FormattedTime(_remainingTime);
            // dirty hack for now
            // before _remainingTime = 0f in the else block it displays -01:-01 
            if (_remainingTime < 0.1)
            {
                _remainingTime = 0f;
            }
        }
        else
        {
            _isCountingDown = false;
            SetUIActive(true);
        }


        _timeToNextClip -= Time.deltaTime;
        if (_timeToNextClip < 0)
        {
            PlayNextClip();
            Initialise();
        }
    }

    #region UIControlls
    public void SetDensityToSparse()
    {
        _density = Density.SPARSE;
    }

    public void SetDensityToMeh()
    {
        _density = Density.MEH;
    }

    public void SetDensityToFrequent()
    {
        _density = Density.FREQUENT;
    }
    #endregion

    #region PlayControls
    public void BeginCountdown()
    {
        Initialise();
        _isCountingDown = true;
        _audioSource.clip = _startClip;
        _audioSource.Play();
    }

    public void EndCountdown()
    {
        _isCountingDown = false;
        _audioSource.Stop();
        _remainingTime = _duration * MINS_TO_SECS;
        _displayTime.text = FormattedTime(_remainingTime);
        SetUIActive(true);
    }

    private void SetUIActive(bool isActive)
    {
        foreach (var gameObject in _disableables)
        {
            gameObject.SetActive(isActive);
        }
    }

    private void PlayNextClip()
    {
        var clipIndex = UnityEngine.Random.Range(0,_reminderClips.Count);
        _audioSource.clip = _reminderClips[clipIndex];
        _audioSource.Play();
        Debug.Log($"Playing clip {_audioSource.clip.name}");
    }
    #endregion
    
    public void Initialise()
    {
        _remainingTime = _duration * MINS_TO_SECS;

        float baseInterval = 0f;
        switch(_density)
        {
            case Density.SPARSE:
                baseInterval = 360f;
                break;
            case Density.MEH:
                baseInterval = 240f;
                break;
            case Density.FREQUENT:
                baseInterval = 120f;
                break;
        }
        if (baseInterval == 0f)
            throw new ArgumentOutOfRangeException("interval between clips is 0 seconds");

        float intervalDeviation = UnityEngine.Random.Range(-0.5f * baseInterval * FRACTIONAL_VARIATION, 0.5f * baseInterval * FRACTIONAL_VARIATION);
        _timeToNextClip = baseInterval + intervalDeviation;
        Debug.Log($"Clip timer reset, time to next clip: {FormattedTime(_timeToNextClip)}, intervalDeviation: {intervalDeviation}");
    }

    private void ValueChangeCheck()
    {
        _duration = (int)_durationSetter.value + 5;
        _remainingTime = _duration * MINS_TO_SECS;
        _displayTime.text = FormattedTime(_remainingTime);
        Debug.Log($"Countdown timer set to " + _displayTime.text);
    }


    #region DebugButtons
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
        _clipCounter++;
        _audioSource.clip = _reminderClips[_clipCounter % _reminderClips.Count];
    }
    #endregion

    public string FormattedTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt((time % 60));

        return minutes.ToString("00") + " : " + seconds.ToString("00");
    }
}
