using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    public event System.Action<MenuButton> OnClick;

    [SerializeField]
    private Button button;

    void Start()
    {
        button.onClick.AddListener(() => OnClick?.Invoke(this));
    }

    public void Enable(bool enabled) 
    {
        button.interactable = enabled;
    }
}
