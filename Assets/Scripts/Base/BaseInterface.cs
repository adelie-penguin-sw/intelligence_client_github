using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 인게임 Tab에 따라 켜지는 Application이 가져야 할 interface
/// </summary>
public interface IGameStateBasicModule
{
    void Init();
    void OnEnter();
    void AdvanceTime(float dt_sec);
    void LateAdvanceTime(float dt_sec);
    void OnExit();
    void Dispose();
}

/// <summary>
/// 유니티에서 제공하는 생명주기를 사용자 설정으로 구현한 interface
/// </summary>
public interface IGameBasicModule
{
    void Init();
    void Set();
    void AdvanceTime(float dt_sec);
    void LateAdvanceTime(float dt_sec);
    void Dispose();
}