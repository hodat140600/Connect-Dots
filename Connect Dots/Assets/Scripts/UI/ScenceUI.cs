using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenceUI : MonoBehaviour
{
    [SerializeField]
    private MainMenu mainMenu;

    [SerializeField]
    private Point point;

    public Point Point
    {
        get
        {
            return point;
        }
    }

    public MainMenu MainMenu
    {
        get
        {
            return mainMenu;
        }
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
