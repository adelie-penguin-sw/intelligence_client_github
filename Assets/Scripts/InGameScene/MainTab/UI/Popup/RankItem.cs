using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MainTab;

public class RankItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _rankNumText;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _meText;
    [SerializeField] private TextMeshProUGUI _tierText;
    [SerializeField] private TextMeshProUGUI _highestCoreIntellectText;

    public void Init(ViewSingleLeaderboard item, bool isMe)
    {
        UpArrowNotation maxCoreIntellect = new UpArrowNotation(item.maximumCoreIntellect.top3Coeffs[0],
                                                               item.maximumCoreIntellect.top3Coeffs[1],
                                                               item.maximumCoreIntellect.top3Coeffs[2],
                                                               item.maximumCoreIntellect.operatorLayerCount);

        _rankNumText.text = item.rank.ToString();
        _nameText.text = item.email;
        _tierText.text = item.resetCount.ToString();
        _highestCoreIntellectText.text = maxCoreIntellect.ToString();
        _meText.gameObject.SetActive(isMe);
    }

    public void Dispose()
    {
        PoolManager.Instance.DespawnObject(EPrefabsType.RANK_ITEM, gameObject);
    }
}
