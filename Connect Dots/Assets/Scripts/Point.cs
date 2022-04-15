using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Point : MonoBehaviour
{
    [SerializeField]
    private Text text;

    private int value;

    public int Value
    {
        get
        {
            return value;
        }
        set
        {
            this.value = value;
            text.text = this.value.ToString();
        }
    }


    void Start()
    {
        Value = 0;
    }

    
    void Update()
    {
        
    }
}
