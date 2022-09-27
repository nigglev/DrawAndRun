using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsRomb : MonoBehaviour, IDisposable
{
    private int _points;
    private bool _is_disposed;
    
    private void Awake()
    {
        _is_disposed = false;
        _points = 50;
    }

    public void OnDestroy()
    {
        if (IsDisposed())
            return;
        CGameManager.Instance.AddPoints(_points);
        
    }

    public bool IsDisposed()
    {
        return _is_disposed;
    }

    public void Dispose()
    {
        _is_disposed = true;
    }
}
