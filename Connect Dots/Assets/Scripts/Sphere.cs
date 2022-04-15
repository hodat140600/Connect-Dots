using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    public event System.Action<Sphere> OnMoveComplete;

    private List<Vector3> path;
    private int index;

    private IEnumerator Move()
    {
        while(index != path.Count)
        {
            transform.position = path[index];

            yield return new WaitForSeconds(0.15f);

            index++;
        }

        OnMoveComplete?.Invoke(this);
    }

    public void Move(List<Vector3> path)
    {
        this.path = path;
        index = 0;

        StartCoroutine(Move());
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
