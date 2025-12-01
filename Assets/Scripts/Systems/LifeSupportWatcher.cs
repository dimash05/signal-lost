using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class LifeSupportWatcher : MonoBehaviour
{
    [Tooltip("Если оставить пустым, возьмётся с этого же объекта")]
    [SerializeField] private MonoBehaviour lifeSupport; 
    [SerializeField] private float checkInterval = 0.2f;
    [SerializeField] private float zeroEpsilon = 0.0001f;

    private Func<float> _oxygenGetter;  
    private bool _depleted;
    private float _nextCheckTime;

    private void Start()
    {
        if (!lifeSupport) lifeSupport = GetComponent<MonoBehaviour>();

        if (!lifeSupport)
        {
            Debug.LogWarning("[LSWatcher] lifeSupport component not set/found.");
            return;
        }

        var t = lifeSupport.GetType();
        string[] deathEventNames = { "OnDeath", "Died", "OnOxygenEmpty", "OnDepleted" };
        foreach (var en in deathEventNames)
        {
            var ev = t.GetEvent(en, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (ev != null)
            {
                var method = GetType().GetMethod(nameof(HandleDeath), BindingFlags.Instance | BindingFlags.NonPublic);
                var handler = Delegate.CreateDelegate(ev.EventHandlerType, this, method);
                ev.AddEventHandler(lifeSupport, handler);
                Debug.Log("[LSWatcher] Subscribed to event: " + en);
                return; 
            }
        }

        _oxygenGetter = BuildOxygenGetter(t, lifeSupport);
        if (_oxygenGetter != null)
        {
            Debug.Log("[LSWatcher] Using polling for oxygen");
        }
        else
        {
            Debug.LogWarning("[LSWatcher] Oxygen member not found. Nothing to watch.");
        }
    }

    private void Update()
    {
        if (_depleted || _oxygenGetter == null) return;
        if (Time.time < _nextCheckTime) return;
        _nextCheckTime = Time.time + checkInterval;

        float o2 = _oxygenGetter();
        if (o2 <= zeroEpsilon)
        {
            HandleDeath();
        }
    }

    private void HandleDeath()
    {
        if (_depleted) return;
        _depleted = true;

        var ui = SimpleEndGameUI.Instance;
        if (ui != null) ui.ShowDeath();
        else Debug.LogWarning("[LSWatcher] SimpleEndGameUI not found to show DEATH");
    }

    private static Func<float> BuildOxygenGetter(Type t, object instance)
    {
        bool NameOk(string n)
        {
            string s = n.ToLowerInvariant();
            return s.Contains("oxygen") && !s.Contains("max") && !s.Contains("recharge") && !s.Contains("drain");
        }

        var prop = t.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .FirstOrDefault(p => p.CanRead &&
                                         (p.PropertyType == typeof(float) || p.PropertyType == typeof(double)) &&
                                         NameOk(p.Name));
        if (prop != null)
        {
            if (prop.PropertyType == typeof(float))
                return () => (float)prop.GetValue(instance);
            else
                return () => (float)(double)prop.GetValue(instance);
        }

      
        var field = t.GetFields(BindingFlags.Instance | BindingFlags.Public)
                     .FirstOrDefault(f => (f.FieldType == typeof(float) || f.FieldType == typeof(double)) &&
                                          NameOk(f.Name));
        if (field != null)
        {
            if (field.FieldType == typeof(float))
                return () => (float)field.GetValue(instance);
            else
                return () => (float)(double)field.GetValue(instance);
        }

        return null;
    }
}
