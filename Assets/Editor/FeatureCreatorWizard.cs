using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class FeatureCreatorWizard : ScriptableWizard
{
    [Tooltip("Путь относительно Features/. Например: Walls/BouncyWall или UI/SettingsPopup")]
    public string featurePath = "NewFeature/MyItem";

    [Header("Выберите нужные папки:")]
    public bool createConfigs = true;
    public bool createAudio = true;
    public bool createAnimations;
    public bool createMaterials = true;
    public bool createTextures = true;
    public bool createPrefabs;      // иногда префаб кладут сразу в корень фичи, но можно и папку
    public bool createScripts;      // если решим хранить скрипты внутри фичи

    // Дополнительные папки (через запятую)
    [Tooltip("Дополнительные папки через запятую: Lightmaps, Timelines, VFX")]
    public string customFolders = "";

    // Пресеты для быстрого выбора
    [Header("Быстрые пресеты:")]
    public bool usePreset = true;

    public enum Preset
    {
        Custom,         // то, что выбрано галочками
        Default,        // Configs, Audio, Materials, Textures
        WithAnimations, // Default + Animations
        Simple,         // Configs, Textures
        UIElement,      // Configs, Audio, Textures, Animations
        ScriptableOnly, // Configs
        Full            // всё включая Scripts и Prefabs
    }

    public Preset preset = Preset.Default;

    [MenuItem("Tools/Architecture/Create Feature Folder Structure")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard<FeatureCreatorWizard>(
            "Create New Feature", "Create");
    }

    // При изменении пресета — автоматом проставляем галочки
    void OnWizardUpdate()
    {
        if (!usePreset) return;

        switch (preset)
        {
            case Preset.Default:
                createConfigs = true;
                createAudio = true;
                createAnimations = false;
                createMaterials = true;
                createTextures = true;
                createPrefabs = false;
                createScripts = false;
                break;

            case Preset.WithAnimations:
                createConfigs = true;
                createAudio = true;
                createAnimations = true;
                createMaterials = true;
                createTextures = true;
                createPrefabs = false;
                createScripts = false;
                break;

            case Preset.Simple:
                createConfigs = true;
                createAudio = false;
                createAnimations = false;
                createMaterials = false;
                createTextures = true;
                createPrefabs = false;
                createScripts = false;
                break;

            case Preset.UIElement:
                createConfigs = true;
                createAudio = true;
                createAnimations = true;
                createMaterials = false;
                createTextures = true;
                createPrefabs = false;
                createScripts = false;
                break;

            case Preset.ScriptableOnly:
                createConfigs = true;
                createAudio = false;
                createAnimations = false;
                createMaterials = false;
                createTextures = false;
                createPrefabs = false;
                createScripts = false;
                break;

            case Preset.Full:
                createConfigs = true;
                createAudio = true;
                createAnimations = true;
                createMaterials = true;
                createTextures = true;
                createPrefabs = true;
                createScripts = true;
                break;

            case Preset.Custom:
                usePreset = false;
                break;
        }
    }

    void OnWizardCreate()
    {
        if (string.IsNullOrEmpty(featurePath))
        {
            Debug.LogError("Feature path cannot be empty.");
            return;
        }

        var folders = new List<string>();

        if (createConfigs)    folders.Add("Configs");
        if (createAudio)      folders.Add("Audio");
        if (createAnimations) folders.Add("Animations");
        if (createMaterials)  folders.Add("Materials");
        if (createTextures)   folders.Add("Textures");
        if (createPrefabs)    folders.Add("Prefabs");
        if (createScripts)    folders.Add("Scripts");

        // Добавляем кастомные папки
        if (!string.IsNullOrWhiteSpace(customFolders))
        {
            var customs = customFolders.Split(',');
            foreach (var folder in customs)
            {
                var trimmed = folder.Trim();
                if (!string.IsNullOrEmpty(trimmed))
                    folders.Add(trimmed);
            }
        }

        CreateFeatureStructure(featurePath, folders);
        AssetDatabase.Refresh();

        // Фокус на созданной папке
        var finalPath = $"Assets/_Project/Features/{featurePath}";
        var obj = AssetDatabase.LoadAssetAtPath<Object>(finalPath);
        if (obj != null)
        {
            EditorGUIUtility.PingObject(obj);
            Selection.activeObject = obj;
        }

        Debug.Log($"Feature '{featurePath}' created with {folders.Count} folders.");
    }

    private void CreateFeatureStructure(string path, List<string> folders)
    {
        var basePath = $"Assets/_Project/Features/{path}";

        // Создаём корневую папку фичи
        CreateFolderRecursive(basePath);

        // Создаём подпапки
        foreach (var folder in folders)
        {
            CreateFolderRecursive($"{basePath}/{folder}");
        }
    }

    private void CreateFolderRecursive(string fullPath)
    {
        if (AssetDatabase.IsValidFolder(fullPath))
            return;

        // Нормализуем путь
        fullPath = fullPath.Replace("\\", "/");

        // Находим родительскую папку
        var lastSlash = fullPath.LastIndexOf('/');
        if (lastSlash <= 0) return; // корень Assets — не создаём

        var parentPath = fullPath[..lastSlash];
        var folderName = fullPath[(lastSlash + 1)..];

        // Рекурсивно создаём родителя
        CreateFolderRecursive(parentPath);

        // Создаём папку
        AssetDatabase.CreateFolder(parentPath, folderName);
    }
}