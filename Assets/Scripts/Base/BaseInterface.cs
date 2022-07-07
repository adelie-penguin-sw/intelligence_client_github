using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameBasicModule
{
    void Init();
    void OnEnter();
    void AdvanceTime(float dt_sec);
    void OnExit();
    void Dispose();
}