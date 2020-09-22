
using UnityEngine;

/// <summary>
/// Monobehaviour singleton helper courtesy of Freya Holmér
/// </summary>
/// <typeparam name="T"></typeparam>
public class MonoBehaviorSingleton<T> : MonoBehaviour where T : MonoBehaviorSingleton<T>
{
    private static T instance;
    public static T Instance => instance == null ? instance = FindObjectOfType<T>() : instance;
    protected void RegisterSingleton() => instance = (T)this; 
}

