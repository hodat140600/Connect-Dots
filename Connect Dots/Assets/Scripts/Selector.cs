using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Selector : MonoBehaviour
{
    public event System.Action<Sphere, Cell> OnSelected;

    [SerializeField]
    private Camera camera;

    [SerializeField]
    private EventSystem eventSystem;

    private Sphere selected;

    public bool Locked { get; set; }


    private T Raycast<T> () where T : Component
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit))
        {
            return hit.transform.GetComponent<T>();
        }
        return null;
    }

    void Start()
    {
        
    }


    void Update()
    {
        if (Locked) return;

        if (Input.GetMouseButtonUp(0))
        {
            if (eventSystem.IsPointerOverGameObject()) return;

            var sphere = Raycast<Sphere>();

            if(sphere != null)
            {
                selected = sphere;
            }
            else
            {
                if(selected != null)
                {
                    var cell = Raycast<Cell>();

                    if(cell != null){
                        OnSelected?.Invoke(selected, cell);
                    }

                    selected = null;
                }
            }
        }
    }
}
