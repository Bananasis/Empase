using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SettingsWindow : AlphaWindow
{
    [Inject] private GameManager _gameManager;
    [SerializeField] private List<Slider> _soundSliders = new List<Slider>();
    [SerializeField] private List<Toggle> _soundToggle = new List<Toggle>();
    [SerializeField] private Button delete;
    public override void Init()
    {
        base.Init();
        shortcuts[KeyCode.Escape] = () => _windowManager.CloseTopWindow();

        _soundSliders[0].value = _gameManager.MasterSoundVolume.val;
        _gameManager.MasterSoundVolume.OnChange.AddListener((v) => _soundSliders[0].value = v);
        _soundSliders[0].onValueChanged.AddListener((v) => _gameManager.MasterSoundVolume.val = v);

        _soundSliders[1].value = _gameManager.SoundVolume[TrackType.Ost];
        _gameManager.SoundVolume.GetEvent(TrackType.Ost).AddListener((v) => _soundSliders[1].value = v);
        _soundSliders[1].onValueChanged.AddListener((v) => _gameManager.SoundVolume[TrackType.Ost] = v);

        _soundSliders[2].value = _gameManager.SoundVolume[TrackType.SoundEffect];
        _gameManager.SoundVolume.GetEvent(TrackType.SoundEffect).AddListener((v) => _soundSliders[2].value = v);
        _soundSliders[2].onValueChanged.AddListener((v) => _gameManager.SoundVolume[TrackType.SoundEffect] = v);

        _soundSliders[3].value = _gameManager.SoundVolume[TrackType.UiEffect];
        _gameManager.SoundVolume.GetEvent(TrackType.UiEffect).AddListener((v) => _soundSliders[3].value = v);
        _soundSliders[3].onValueChanged.AddListener((v) => _gameManager.SoundVolume[TrackType.UiEffect] = v);

        _soundToggle[0].isOn = _gameManager.MasterSoundOn.val;
        _gameManager.MasterSoundOn.OnChange.AddListener((v) => _soundToggle[0].isOn = v);
        _soundToggle[0].onValueChanged.AddListener((v) => _gameManager.MasterSoundOn.val = v);

        _soundToggle[1].isOn = _gameManager.SoundOn[TrackType.Ost];
        _gameManager.SoundOn.GetEvent(TrackType.Ost).AddListener((v) => _soundToggle[1].isOn = v);
        _soundToggle[1].onValueChanged.AddListener((v) => _gameManager.SoundOn[TrackType.Ost] = v);

        _soundToggle[2].isOn = _gameManager.SoundOn[TrackType.SoundEffect];
        _gameManager.SoundOn.GetEvent(TrackType.SoundEffect).AddListener((v) => _soundToggle[2].isOn = v);
        _soundToggle[2].onValueChanged.AddListener((v) => _gameManager.SoundOn[TrackType.SoundEffect] = v);

        _soundToggle[3].isOn = _gameManager.SoundOn[TrackType.UiEffect];
        _gameManager.SoundOn.GetEvent(TrackType.UiEffect).AddListener((v) => _soundToggle[3].isOn = v);
        _soundToggle[3].onValueChanged.AddListener((v) => _gameManager.SoundOn[TrackType.UiEffect] = v);

        delete.onClick.AddListener(() => _gameManager.ResetSave());
    }
}