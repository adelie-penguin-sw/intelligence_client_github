using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameTabBasicModule
{
    void Init();
    void OnEnter();
    void AdvanceTime(float dt_sec);
    void OnExit();
    void Dispose();
}

public interface IGameBasicModule
{
    void Init();
    void Set();
    void AdvanceTime(float dt_sec);
    void Dispose();
}