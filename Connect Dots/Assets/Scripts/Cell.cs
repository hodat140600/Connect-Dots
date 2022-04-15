using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField]
    private Color hightlightedColor;

    private Renderer renderer;
    private Color originalColor;

    public void Highlight(bool highlighted)
    {
        renderer.material.color = highlighted ? hightlightedColor : originalColor;
    }

    void Start()
    {
        renderer = GetComponent<Renderer>();
        originalColor = renderer.material.color;
    }

    
    void Update()
    {
        
    }
}
