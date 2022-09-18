using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpTabView : MonoBehaviour
{
    public async void OnClick_TpUpgrade(int upgrade)
    {
        TpUpgradeSingleNetworkRequest req = new TpUpgradeSingleNetworkRequest();
        req.upgrade = upgrade;
        req.upgradeCount = 1;
        await NetworkManager.Instance.API_TpUpgrade(req);
    }
}
