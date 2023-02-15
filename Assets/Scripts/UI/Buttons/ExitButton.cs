using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Button))]
public class ExitButton : MonoBehaviour
{
    [Inject] private IWindowManager _windowManager;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image icon;
    [SerializeField] private Sprite exit;
    [SerializeField] private Sprite back;

    // Start is called before the first frame update
    void Start()
    {
        _windowManager.OnWindowChanged.AddListener(
            (item) =>
            {
                gameObject.SetActive(item.Item2 != WindowType.None && item.Item2 != WindowType.GameplayUI &&
                                     item.Item2 != WindowType.Loading);
                text.text = item.Item2 == WindowType.MainMenu ? "Exit" : "Back\n'ESC'";
                icon.sprite = item.Item2 == WindowType.MainMenu ? exit : back;
            });
        GetComponent<Button>().onClick.AddListener(() =>
        {
            switch (_windowManager.TopWindowType())
            {
                case WindowType.MainMenu:
                    Application.Quit();
                    break;
                default:
                    _windowManager.CloseTopWindow();
                    break;
            }
        });
    }
}