using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Plugins.Options;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Core;



public class Sphere : MonoBehaviour
{
    public event System.Action<Sphere> OnMoveComplete;

    private TweenerCore<Vector3, Path, PathOptions> moveTween;
    private TweenerCore<Vector3, Vector3, VectorOptions> scaleTween;

    public void Move(List<Vector3> path)
    {
        moveTween = transform.DOPath(path.ToArray(), path.Count * 0.1f)
            .SetEase(Ease.Linear)
            .OnComplete(() => OnMoveComplete?.Invoke(this));
    }

    public void Destroy()
    {
        scaleTween = transform.DOScale(0.1f, 0.25f).OnComplete(() =>
        {
            GameObject.Destroy(gameObject);
        });
    }
    void Start()
    {
        
    }

    private void OnDestroy()
    {
        moveTween?.Kill();
        scaleTween.Kill();
    }
}
