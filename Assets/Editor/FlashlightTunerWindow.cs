#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class FlashlightTunerWindow : EditorWindow
{
    FlashlightConfigSO config;
    SerializedObject so;
    SerializedProperty maxEnergy, drainPerSecond, rechargePerSecond, spotAngle, range, intensity;

    [MenuItem("SignalLost/Tools/Flashlight Tuner")]
    static void Open() => GetWindow<FlashlightTunerWindow>("Flashlight Tuner");

    void OnEnable()
    {
        var guids = AssetDatabase.FindAssets("t:FlashlightConfigSO");
        if (guids.Length > 0)
            config = AssetDatabase.LoadAssetAtPath<FlashlightConfigSO>(AssetDatabase.GUIDToAssetPath(guids[0]));
        SetupSO();
    }

    void OnGUI()
    {
        if (!config)
        {
            if (GUILayout.Button("Create FlashlightConfig.asset"))
            {
                System.IO.Directory.CreateDirectory("Assets/Data");
                config = ScriptableObject.CreateInstance<FlashlightConfigSO>();
                AssetDatabase.CreateAsset(config, "Assets/Data/FlashlightConfig.asset");
                AssetDatabase.SaveAssets();
                SetupSO();
            }
            return;
        }

        so.Update();
        EditorGUILayout.PropertyField(maxEnergy);
        EditorGUILayout.PropertyField(drainPerSecond);
        EditorGUILayout.PropertyField(rechargePerSecond);
        EditorGUILayout.Space(6);
        EditorGUILayout.PropertyField(spotAngle);
        EditorGUILayout.PropertyField(range);
        EditorGUILayout.PropertyField(intensity);

        if (GUILayout.Button("Save"))
        {
            so.ApplyModifiedProperties();
            EditorUtility.SetDirty(config);
            AssetDatabase.SaveAssets();
        }
    }

    void SetupSO()
    {
        if (!config) return;
        so = new SerializedObject(config);
        maxEnergy        = so.FindProperty("maxEnergy");
        drainPerSecond   = so.FindProperty("drainPerSecond");
        rechargePerSecond= so.FindProperty("rechargePerSecond");
        spotAngle        = so.FindProperty("spotAngle");
        range            = so.FindProperty("range");
        intensity        = so.FindProperty("intensity");
    }
}
#endif
