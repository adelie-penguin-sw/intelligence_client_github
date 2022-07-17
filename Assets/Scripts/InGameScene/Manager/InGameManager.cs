using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//develop 브랜치 추가
public class InGameManager : MonoBehaviour, IDisposable
{
    void Awake()
    {
        
    }

    void Start()
    {
        InitHandlers();
        ChangeState(EGameState.MAINTAB);
    }

    void Update()
    {
        if (_currentState != EGameState.UNKNOWN)
        {
            GetStateHandler(_currentState).AdvanceTime(Time.deltaTime);
        }
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    private bool _disposed;
    protected virtual void Dispose(bool disposing)
    {
        if (this._disposed) return;
        if (disposing)
        {
            // IDisposable ???????????? ???????? ???????? ?????? ??????????.
            foreach (EGameState state in _handlers.Keys)
            {
                _handlers[state].Dispose();
            }
        }
        // .NET Framework?? ?????? ???????? ???? ???? ?????????? ?????? ??????????.
        this._disposed = true;
    }


    private Dictionary<EGameState, IGameStateBasicModule> _handlers = new Dictionary<EGameState, IGameStateBasicModule>();
    private EGameState _currentState = EGameState.UNKNOWN;
    private GameObject _goTemp;
    private void InitHandlers()
    {
        _handlers.Clear();

        _goTemp = PoolManager.Instance.GrabPrefabs(EPrefabsType.TAP_APPLICATION, "MainTabApp", transform);
        _handlers.Add(EGameState.MAINTAB, _goTemp.GetComponent<MainTab.MainTabApplication>());

        foreach (EGameState state in _handlers.Keys)
        {
            _handlers[state].Init();
        }
    }

    private void ChangeState(EGameState nextState)
    {
        if (nextState != EGameState.UNKNOWN && nextState != _currentState)
        {
            EGameState prevState = _currentState;
            _currentState = nextState;
            IGameStateBasicModule leaveHandler = GetStateHandler(prevState);
            if (leaveHandler != null)
            {
                leaveHandler.OnExit();
            }
            IGameStateBasicModule enterHandler = GetStateHandler(_currentState);
            if (enterHandler != null)
            {
                enterHandler.OnEnter();
            }
        }
    }

    /// <summary>
    /// 한글주석테스트입니다.
    /// 
    /// </summary>
    /// <param name="EGameState"></param>
    /// <returns>return ?? ????? ?? null, ??? ?? ?? ??? ??</returns>
    private IGameStateBasicModule GetStateHandler(EGameState state)
    {
        if (_handlers.ContainsKey(state))
        {
            return _handlers[state];
        }
        return null;
    }
}

public enum EGameState
{
    UNKNOWN,
    MAINTAB,
}