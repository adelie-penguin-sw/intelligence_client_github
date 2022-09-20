using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

namespace TpTab
{
    /// <summary>
    /// TP 업그레이드 탭의 데이터를 가지고 있는 Model 클래스
    /// </summary>
    [Serializable]
    public class TpTabModel
    {
        [BoxGroup("TPU01")]
        [SerializeField] public TextMeshProUGUI TPU01NameText;
        [BoxGroup("TPU01")]
        [SerializeField] public TextMeshProUGUI TPU01EffectText;
        [BoxGroup("TPU01")]
        [SerializeField] public TextMeshProUGUI TPU01CostText;
        [BoxGroup("TPU02")]
        [SerializeField] public TextMeshProUGUI TPU02NameText;
        [BoxGroup("TPU02")]
        [SerializeField] public TextMeshProUGUI TPU02EffectText;
        [BoxGroup("TPU02")]
        [SerializeField] public TextMeshProUGUI TPU02CostText;
        [BoxGroup("TPU03")]
        [SerializeField] public TextMeshProUGUI TPU03NameText;
        [BoxGroup("TPU03")]
        [SerializeField] public TextMeshProUGUI TPU03EffectText;
        [BoxGroup("TPU03")]
        [SerializeField] public TextMeshProUGUI TPU03CostText;
        [BoxGroup("TPU04")]
        [SerializeField] public TextMeshProUGUI TPU04NameText;
        [BoxGroup("TPU04")]
        [SerializeField] public TextMeshProUGUI TPU04EffectText;
        [BoxGroup("TPU04")]
        [SerializeField] public TextMeshProUGUI TPU04CostText;
        [BoxGroup("TPU05")]
        [SerializeField] public TextMeshProUGUI TPU05NameText;
        [BoxGroup("TPU05")]
        [SerializeField] public TextMeshProUGUI TPU05EffectText;
        [BoxGroup("TPU05")]
        [SerializeField] public TextMeshProUGUI TPU05CostText;
        [BoxGroup("TPU06")]
        [SerializeField] public TextMeshProUGUI TPU06NameText;
        [BoxGroup("TPU06")]
        [SerializeField] public TextMeshProUGUI TPU06EffectText;
        [BoxGroup("TPU06")]
        [SerializeField] public TextMeshProUGUI TPU06CostText;
        [BoxGroup("TPU07")]
        [SerializeField] public TextMeshProUGUI TPU07NameText;
        [BoxGroup("TPU07")]
        [SerializeField] public TextMeshProUGUI TPU07EffectText;
        [BoxGroup("TPU07")]
        [SerializeField] public TextMeshProUGUI TPU07CostText;
        [BoxGroup("TPU08")]
        [SerializeField] public TextMeshProUGUI TPU08NameText;
        [BoxGroup("TPU08")]
        [SerializeField] public TextMeshProUGUI TPU08EffectText;
        [BoxGroup("TPU08")]
        [SerializeField] public TextMeshProUGUI TPU08CostText;
    }
}
