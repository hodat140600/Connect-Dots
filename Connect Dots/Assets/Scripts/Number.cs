using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Number : MonoBehaviour
{
    [SerializeField]
    private Text text;

    [SerializeField]
    private Image image;

    public int Value
    {
        set
        {
            text.text = value.ToString();
        }
    }

    public Color Color
    {
        set
        {
            image.color = value;
        }
    }
    
    void Start()
    {
        
    }

}
