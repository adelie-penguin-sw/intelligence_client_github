using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Channel : MonoBehaviour
{
    [SerializeField]
    private Transform _trStart;
    [SerializeField]
    private Transform _trEnd;
    [SerializeField]
    private LineRenderer _lineRenderer;
    [SerializeField]
    private Material _material;

    private void Start()
    {
        _material.SetFloat("width", 0.5f);
        _material.SetFloat("heigth", 0.5f);
    }
    private void Update()
    {
        Init();
    }

    public void Init()
    {
        _lineRenderer.SetPosition(0, _trStart.position);
        _lineRenderer.SetPosition(1, _trEnd.position);
    }

    public void Set()
    {
    }
    public void AdvanceTime(float dt_sec)
    {
    }

    public void Dispose()
    {
    }
}
