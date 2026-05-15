using System;
using System.Collections.Generic;
using UnityEngine;

public class GameServiceLocator : MonoBehaviour
{
    public static GameServiceLocator Instance { get; private set; }

    private readonly Dictionary<Type, MonoBehaviour> _services = new Dictionary<Type, MonoBehaviour>();

    private void Awake()
    {
        // Ensure there is only one instance of the Service Locator
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Call this in the Awake() method of your individual managers
    public void RegisterService<T>(T service) where T : MonoBehaviour
    {
        Type type = typeof(T);
        if (!_services.ContainsKey(type))
        {
            _services.Add(type, service);
        }
    }

    // Call this in any script when you need to access a manager
    public T GetService<T>() where T : MonoBehaviour
    {
        Type type = typeof(T);
        if (_services.TryGetValue(type, out MonoBehaviour service))
        {
            return (T)service;
        }

        Debug.LogError($"Service of type {type} is not registered!");
        return null;
    }

    // Call this if a manager is destroyed or scene changes
    public void UnregisterService<T>() where T : MonoBehaviour
    {
        Type type = typeof(T);
        if (_services.ContainsKey(type))
        {
            _services.Remove(type);
        }
    }
}
