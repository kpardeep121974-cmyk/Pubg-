using System;
using System.Collections.Generic;
using UnityEngine;

public class GameServiceLocator : MonoBehaviour
{
    public static GameServiceLocator Instance { get; private set; }

    private readonly Dictionary<Type, MonoBehaviour> _services = new Dictionary<Type, MonoBehaviour>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Call this to register any manager (like EconomyManager, LobbyManager, etc.)
    public void RegisterService<T>(T service) where T : MonoBehaviour
    {
        Type type = typeof(T);
        if (!_services.ContainsKey(type))
        {
            _services.Add(type, service);
            Debug.Log($"[ServiceLocator] Registered: {type.Name}");
        }
    }

    // Call this from ANY script to grab a manager instantly
    public T GetService<T>() where T : MonoBehaviour
    {
        Type type = typeof(T);
        if (_services.TryGetValue(type, out MonoBehaviour service))
        {
            return (T)service;
        }

        Debug.LogError($"[ServiceLocator] Service of type {type} is not registered!");
        return null;
    }

    public void UnregisterService<T>() where T : MonoBehaviour
    {
        Type type = typeof(T);
        if (_services.ContainsKey(type))
        {
            _services.Remove(type);
        }
    }
}
