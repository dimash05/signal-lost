#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class FlashlightTunerWindow : EditorWindow
{
    private FlashlightConfigSO config;
    private PlayerFlashlight[] sceneFlashlights;

    [MenuItem("SignalLost/Flashlight Tuner")]
    public static void Open() => GetWindow<FlashlightTunerWindow>("Flashlight Tuner");

    void OnEnable()
    {
        if (!config)
        {
            var guids = AssetDatabase.FindAssets("t:FlashlightConfigSO");
            if (guids != null && guids.Length > 0)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[0]);
                config = AssetDatabase.LoadAssetAtPath<FlashlightConfigSO>(path);
            }
        }

        RefreshSceneRefs();
    }

    void RefreshSceneRefs()
    {
        sceneFlashlights = FindObjectsOfType<PlayerFlashlight>(true);
    }

    void OnGUI()
    {
        EditorGUILayout.Space();
        config = (FlashlightConfigSO)EditorGUILayout.ObjectField("Config", config, typeof(FlashlightConfigSO), false);

        if (!config)
        {
            EditorGUILayout.HelpBox("Config is absent. Create new.", MessageType.Info);
            if (GUILayout.Button("Create Config Asset"))
            {
                var path = "Assets/Settings";
                if (!AssetDatabase.IsValidFolder(path))
                    AssetDatabase.CreateFolder("Assets", "Settings");

                config = ScriptableObject.CreateInstance<FlashlightConfigSO>();
                var assetPath = AssetDatabase.GenerateUniqueAssetPath(path + "/FlashlightConfig.asset");
                AssetDatabase.CreateAsset(config, assetPath);
                AssetDatabase.SaveAssets();
                EditorGUIUtility.PingObject(config);
            }
            return;
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Energy", EditorStyles.boldLabel);
        config.capacity          = EditorGUILayout.FloatField("Max Energy", config.capacity);
        config.drainPerSecond    = EditorGUILayout.FloatField("Drain Per Second", config.drainPerSecond);
        config.rechargePerSecond = EditorGUILayout.FloatField("Recharge Per Second", config.rechargePerSecond);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Light", EditorStyles.boldLabel);
        config.spotAngle          = EditorGUILayout.FloatField("Spot Angle", config.spotAngle);
        config.maxRange           = EditorGUILayout.FloatField("Range", config.maxRange);
        config.maxIntensity       = EditorGUILayout.FloatField("Intensity", config.maxIntensity);
        config.minRangeFactor     = EditorGUILayout.Slider("Min Range Factor", config.minRangeFactor, 0f, 1f);
        config.minIntensityFactor = EditorGUILayout.Slider("Min Intensity Factor", config.minIntensityFactor, 0f, 1f);

        EditorGUILayout.Space();

        using (new EditorGUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Save"))
            {
                SaveConfigOnly();
            }

            if (GUILayout.Button("Apply to Scene"))
            {
                SaveConfigOnly();
                ApplyToScene();
            }
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("Re-scan Scene"))
            RefreshSceneRefs();

        EditorGUILayout.LabelField($"Scene flashlights: {(sceneFlashlights != null ? sceneFlashlights.Length : 0)}");
    }

    private void SaveConfigOnly()
    {
        if (!config) return;
        EditorUtility.SetDirty(config);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("[FlashlightTuner] Config saved.");
    }

    private void ApplyToScene()
    {
        if (!config) return;

        var all = FindObjectsOfType<PlayerFlashlight>(true);
        foreach (var f in all)
        {
            Undo.RecordObject(f, "Apply Flashlight Config");
            f.ApplyConfig(config);
            f.SetConfigRef(config);
            EditorUtility.SetDirty(f);

            var light = f.GetLight();
            if (light)
            {
                Undo.RecordObject(light, "Apply Light values");
                light.spotAngle = config.spotAngle;
                EditorUtility.SetDirty(light);
            }
        }

        Debug.Log($"[FlashlightTuner] Applied to {all.Length} flashlight(s).");
    }
}
#endif
