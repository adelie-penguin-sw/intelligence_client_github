using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    private bool disposed;
    protected virtual void Dispose(bool disposing)
    {
        if (this.disposed) return;
        if (disposing)
        {
            // IDisposable �������̽��� �����ϴ� ������� ���⼭ �����մϴ�.
            foreach (EGameState state in _handlers.Keys)
            {
                _handlers[state].Dispose();
            }
        }
        // .NET Framework�� ���Ͽ� �������� �ʴ� �ܺ� ���ҽ����� ���⼭ �����մϴ�.
        this.disposed = true;
    }


    private Dictionary<EGameState, IGameBasicModule> _handlers = new Dictionary<EGameState, IGameBasicModule>();
    private EGameState _currentState = EGameState.UNKNOWN;
    private GameObject _goTemp;
    private void InitHandlers()
    {
        _handlers.Clear();

        _goTemp = PoolManager.Instance.GrabPrefabs(EPrefabsType.TabApplication, "MainTabApp", transform);
        _handlers.Add(EGameState.MAINTAB, _goTemp.GetComponent<MainTabApplication>());

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
            IGameBasicModule leaveHandler = GetStateHandler(prevState);
            if (leaveHandler != null)
            {
                leaveHandler.OnExit();
            }
            IGameBasicModule enterHandler = GetStateHandler(_currentState);
            if (enterHandler != null)
            {
                enterHandler.OnEnter();
            }
        }
    }

    private IGameBasicModule GetStateHandler(EGameState state)
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