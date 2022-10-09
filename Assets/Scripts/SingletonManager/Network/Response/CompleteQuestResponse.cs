using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CompleteQuestResponse
{
    public AnsEquation NP;
    public List<QuestAttributes> questAttributes;
    public int statusCode;
}
