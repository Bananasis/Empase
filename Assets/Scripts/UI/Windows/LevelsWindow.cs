using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LevelsWindow : AlphaWindow
{
    [SerializeField] private PagedScrollController pageController;
    [SerializeField] private List<Button> pageButtons;
    [SerializeField] private List<GameObject> pages;
    [SerializeField] private Button backPageButton;
    [SerializeField] private Button forwardPageButton;
    [Inject] private GameManager _gameManager;

    public override void Init()
    {
        base.Init();

        pageController.OnPageChange.AddListener((p) => pageButtons[p].Select());

        UpdatePages(_gameManager.LastOpenZone.val);
        _gameManager.LastOpenZone.OnChange.AddListener(UpdatePages);
        OnWindowOpen.AddListener(() => pageController.GoToItem(_gameManager.CurZone.val));
        pageController.OnPageChange.AddListener((p) => _gameManager.CurZone.val = p);
        backPageButton.onClick.AddListener(() => pageController.currentHeroIndex--);
        forwardPageButton.onClick.AddListener(() => pageController.currentHeroIndex++);
        shortcuts[KeyCode.Escape] = () => _windowManager.CloseTopWindow();
        shortcuts[KeyCode.LeftArrow] = () => backPageButton.onClick.Invoke();
        shortcuts[KeyCode.RightArrow] = () => forwardPageButton.onClick.Invoke();
        for (var i = 0; i < pageButtons.Count; i++)
        {
            var capture = i;
            pageButtons[i].onClick.AddListener(() => pageController.GoToItem(capture));
        }
    }

    private void UpdatePages(int lastZone)
    {
        pageController.childCount = lastZone + 2;

        pages[pages.Count - 1].SetActive(lastZone != 0);
        backPageButton.gameObject.SetActive(lastZone != 0);
        forwardPageButton.gameObject.SetActive(lastZone != 0);
        for (var i = 0; i < pageButtons.Count; i++)
        {
            pageButtons[i].gameObject.SetActive(lastZone >= i);
        }

        for (var i = 0; i < pages.Count - 1; i++)
        {
            pages[i].SetActive(lastZone >= i);
        }

        pageButtons[0].gameObject.SetActive(lastZone != 0);
    }
}