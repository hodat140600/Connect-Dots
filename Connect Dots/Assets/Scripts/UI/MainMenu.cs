using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public enum ButtonType
    {
        New,
        Save,
        Load,
        Exit
    }

    public event System.Action<ButtonType> OnClick;

    [SerializeField]
    private CanvasGroup canvasGroup; 
    [SerializeField]
    private MenuButton newGameButton;
    [SerializeField]
    private MenuButton saveGameButton;
    [SerializeField]
    private MenuButton loadGameButton;
    [SerializeField]
    private MenuButton exitGameButton;

    public bool Active
    {
        get
        {
            return canvasGroup.alpha == 1f;
        }
        set
        {
            canvasGroup.alpha = value ? 1f : 0f;
            canvasGroup.interactable = value;
            canvasGroup.blocksRaycasts = value;
        }
    }

    public void EnableButton(ButtonType buttonType, bool enabled)
    {
        MenuButton button = null;
        switch (buttonType)
        {
            case ButtonType.Load:
                button = loadGameButton;
                break;
            case ButtonType.Save:
                button = saveGameButton;
                break;
            case ButtonType.New:
                button = newGameButton;
                break;
            case ButtonType.Exit:
                button = exitGameButton;
                break;
        }
        button?.Enable(enabled);
    }

    private void Awake()
    {
        Active = false;
    }

    void Start()
    {
        newGameButton.OnClick += NewGameButton_OnClick;

        saveGameButton.OnClick += SaveGameButton_OnClick;

        loadGameButton.OnClick += LoadGameButton_OnClick;

        exitGameButton.OnClick += ExitGameButton_OnClick;
    }

    void Update()
    {
        
    }

    private void LoadGameButton_OnClick(MenuButton button)
    {
        OnClick?.Invoke(ButtonType.Load);
    }

    private void SaveGameButton_OnClick(MenuButton button)
    {
        OnClick?.Invoke(ButtonType.Save);
    }

    private void NewGameButton_OnClick(MenuButton button)
    {
        OnClick?.Invoke(ButtonType.New);
    }

    private void ExitGameButton_OnClick(MenuButton button)
    {
        OnClick?.Invoke(ButtonType.Exit);
    }
}
