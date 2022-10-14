using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupCanvas : MonoBehaviour
{
    [SerializeField] private GameObject _normalGroup;
    [SerializeField] private GameObject _importantGroup;
    [SerializeField] private Canvas _canvas;

    public GameObject NomalGroup { get { return _normalGroup; } }
    public GameObject ImportantGroup { get { return _importantGroup; } }
    public Canvas Canvas { get { return _canvas; } }
}
