using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Brain : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro _textNum;
    [SerializeField]
    private BrainData _brainData;

    public void Init()
    {
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

    private void SetNumText(double num)
    {
        _textNum.text = num.ToString();
    }
}
