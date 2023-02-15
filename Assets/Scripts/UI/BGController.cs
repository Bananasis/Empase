using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


public class BGController : MonoBehaviour
{

    [Inject] private IWindowManager _windowManager;
    // Start is called before the first frame update
    void Start()
    {
        _windowManager.OnWindowChanged.AddListener(
            (item) =>
            {
                gameObject.SetActive(item.Item2 != WindowType.None && item.Item2 != WindowType.GameplayUI && item.Item2 != WindowType.Pause&& item.Item2 != WindowType.Loading);
            });

    }
}