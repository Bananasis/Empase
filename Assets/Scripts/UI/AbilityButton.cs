using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;


[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Animator))]
public class AbilityButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button _button;
    private Animator _animator;

    private readonly UnityEvent select = new UnityEvent();
    private readonly UnityEvent onHover = new UnityEvent();
    [Inject] private GameManager _gameManager;
    [Inject] private EventManager _eventManager;

    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI abilityName;

    // Start is called before the first frame update
    void Awake()
    {
        _button = GetComponent<Button>();
        _animator = GetComponent<Animator>();
    }

    public void SetUp(ShiftType type)
    {
        _button = GetComponent<Button>();
        _animator = GetComponent<Animator>();
        _button.onClick.AddListener(() => _gameManager.CurShiftAbility.val = type);
        _gameManager.ShiftAbilities.GetEvent(type).Subscribe((b) => { _button.interactable = b; });
        _button.interactable = _gameManager.ShiftAbilities[type];
        _animator.SetBool("SelectedAbility", _gameManager.CurShiftAbility.val == type);
        _gameManager.CurShiftAbility.OnChange.AddListener((t) =>
            _animator.SetBool("SelectedAbility", t == type)
        );
        select.AddListener(() => _animator.SetBool("SelectedAbility", _gameManager.CurShiftAbility.val == type));
        onHover.AddListener(() =>
        {
            var data = _eventManager.GetShiftData(type);
            description.text = _button.interactable
                ? data.description
                : data.lockedDescription;
            abilityName.text = data.name;
        });
    }

    public void SetUp(PropulsionType type)
    {
        _button = GetComponent<Button>();
        _animator = GetComponent<Animator>();
        _button.onClick.AddListener(() => _gameManager.CurMouseAbility.val = type);
        _gameManager.MouseAbilities.GetEvent(type).Subscribe((b) => { _button.interactable = b; });
        _button.interactable = _gameManager.MouseAbilities[type];
        _animator.SetBool("SelectedAbility", _gameManager.CurMouseAbility.val == type);
        _gameManager.CurMouseAbility.OnChange.AddListener((t) => { _animator.SetBool("SelectedAbility", t == type); });
        select.AddListener(() => _animator.SetBool("SelectedAbility", _gameManager.CurMouseAbility.val == type));
        onHover.AddListener(() =>
        {
            var data = _eventManager.GetPropulsionData(type);
            description.text = _button.interactable
                ? data.description
                : data.lockedDescription;
            abilityName.text = data.name;
        });
    }


    private void OnEnable()
    {
        select.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onHover.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        description.text = "";
        abilityName.text = "";
    }
}