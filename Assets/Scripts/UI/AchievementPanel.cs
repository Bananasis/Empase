using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(Button))]
public class AchievementPanel : MonoBehaviour
{
    [Inject] private GameManager _gameManager;
    [SerializeField] Image _image;
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _description;
    private Canvas _graphicRaycaster;
    private Button button;
    public float gray = 0;

    private void Awake()
    {
        _image.material = new Material(_image.material);
        button = GetComponent<Button>();
        _graphicRaycaster = GetComponent<Canvas>();
    }

    public void Set(Sprite sprite, string title, string description, int order, Achievement achievement)
    {
        _image.sprite = sprite;
        button.interactable = _gameManager.Achievements[achievement];
        _gameManager.Achievements.GetEvent(achievement).AddListener((b) => button.interactable = b);
        _title.text = title;
        _description.text = description;
        _graphicRaycaster.sortingOrder = order;
    }

    public void Update()
    {
        _image.material.SetFloat("Gray", gray);
    }
}