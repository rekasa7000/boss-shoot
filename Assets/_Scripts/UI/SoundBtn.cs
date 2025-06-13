using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundBtn : MonoBehaviour
{
    [SerializeField]
    private AudioMixerSnapshot _defaultAudioMixedSnapshot, _mutedAudioMixerSnapshot;

    [SerializeField]
    private Sprite _soundOffIcon, _soundOnIcon;

    [SerializeField]
    private Image _buttonIconImage;

    private bool _soundToggle = true;

    public void ToggleMusic()
    {
        _soundToggle = !_soundToggle;
        _buttonIconImage.sprite = _soundToggle ? _soundOffIcon : _soundOnIcon;
        if (_soundToggle)
            _defaultAudioMixedSnapshot.TransitionTo(0.1f);
        else
            _mutedAudioMixerSnapshot.TransitionTo(0.1f);
    }
}
