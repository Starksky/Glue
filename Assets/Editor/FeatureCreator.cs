using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Editor {
    public static class FeatureCreator
    {
        [MenuItem("Tools/Architecture/Create Feature")]
        static void CreateFeature()
        {
            var window = EditorWindow.GetWindow<FeatureCreatorWindow>("Create Feature");
            window.Show();
        }
    }

    public class FeatureCreatorWindow : EditorWindow
    {
        string _featureName = "NewFeature";
    
        // Слои
        bool _createDomain = true;
        bool _createApplication = true;
        bool _createPresentation = true;
        bool _createView = true;
        bool _createContracts = true;
        bool _createControllers = true;
        bool _createInfrastructure = true;
    
        // Ресурсы
        bool _createPrefabs = true;
        bool _createConfigs = true;
        bool _createAudio = true;

        void OnGUI()
        {
            GUILayout.Label("Create New Feature", EditorStyles.boldLabel);

            _featureName = EditorGUILayout.TextField("Feature Name", _featureName);

            GUILayout.Space(10);
            GUILayout.Label("Layers (Scripts + Asmdef):", EditorStyles.boldLabel);
            _createDomain = EditorGUILayout.Toggle("Domain", _createDomain);
            _createApplication = EditorGUILayout.Toggle("Application", _createApplication);
            _createPresentation = EditorGUILayout.Toggle("Presentation", _createPresentation);
            _createView = EditorGUILayout.Toggle("View", _createView);
            _createContracts = EditorGUILayout.Toggle("Contracts", _createContracts);
            _createControllers = EditorGUILayout.Toggle("Controllers", _createControllers);
            _createInfrastructure = EditorGUILayout.Toggle("Infrastructure", _createInfrastructure);

            GUILayout.Space(10);
            GUILayout.Label("Resources:", EditorStyles.boldLabel);
            _createPrefabs = EditorGUILayout.Toggle("Prefabs", _createPrefabs);
            _createConfigs = EditorGUILayout.Toggle("Configs", _createConfigs);
            _createAudio = EditorGUILayout.Toggle("Audio", _createAudio);

            GUILayout.Space(20);

            if (GUILayout.Button("Create", GUILayout.Height(30)))
            {
                EnsureCommonExists();
                CreateFeatureStructure(_featureName);
                Close();
            }
        }

        // ─── Common ────────────────────────────────────────

        void EnsureCommonExists()
        {
            var CommonPath = "Assets/_Project/_Common";
            var CommonScriptsPath = $"{CommonPath}/Scripts";

            // Common asmdef-ы
            CreateCommonAsmdef($"{CommonScriptsPath}/Signals", "Common.Signals", Array.Empty<string>());
            CreateCommonAsmdef($"{CommonScriptsPath}/Contracts", "Common.Contracts", Array.Empty<string>());
            CreateCommonAsmdef($"{CommonScriptsPath}/Utils", "Common.Utils", Array.Empty<string>());
            CreateCommonAsmdef($"{CommonScriptsPath}/Infrastructure", "Common.Infrastructure", 
                new [] {"Common.Contracts", "Common.Utils", "Common.Signals"});
        }

        void CreateCommonAsmdef(string path, string name, string[] references)
        {
            CreateFolderRecursive(path);

            var asmdefPath = $"{path}/{name}.asmdef";
            if (File.Exists(asmdefPath)) return;

            var json = $@"{{
            ""name"": ""{name}"",
            ""references"": {JsonArray(references)},
            ""includePlatforms"": [],
            ""excludePlatforms"": [],
            ""allowUnsafeCode"": false,
            ""overrideReferences"": false
        }}";

            File.WriteAllText(asmdefPath, json);
        }

        // ─── Feature ───────────────────────────────────────

        void CreateFeatureStructure(string featureName)
        {
            var basePath = $"Assets/_Project/Features/{featureName}";
            var scriptsPath = $"{basePath}/Scripts";

            // Папки ресурсов
            if (_createPrefabs) CreateFolderRecursive($"{basePath}/Prefabs");
            if (_createConfigs) CreateFolderRecursive($"{basePath}/Configs");
            if (_createAudio) CreateFolderRecursive($"{basePath}/Audio");

            // Скрипты + asmdef
            if (_createDomain) CreateLayer(scriptsPath, featureName, "Domain");
            if (_createApplication) CreateLayer(scriptsPath, featureName, "Application");
            if (_createPresentation) CreateLayer(scriptsPath, featureName, "Presentation");
            if (_createView) CreateLayer(scriptsPath, featureName, "View");
            if (_createContracts) CreateLayer(scriptsPath, featureName, "Contracts");
            if (_createControllers) CreateLayer(scriptsPath, featureName, "Controllers");
            if (_createInfrastructure) CreateLayer(scriptsPath, featureName, "Infrastructure");

            // README
            CreateReadme(basePath, featureName);

            AssetDatabase.Refresh();
        
            // Выделяем созданную папку
            var obj = AssetDatabase.LoadAssetAtPath<Object>(basePath);
            if (obj != null)
            {
                EditorGUIUtility.PingObject(obj);
                Selection.activeObject = obj;
            }

            Debug.Log($"Feature '{featureName}' created at {basePath}");
        }

        void CreateLayer(string scriptsPath, string featureName, string layer)
        {
            // Папка слоя: Scripts/Domain/ (без названия фичи)
            var layerPath = $"{scriptsPath}/{layer}";

            CreateFolderRecursive(layerPath);
            CreateAsmdef(layerPath, featureName, layer);
        }

        void CreateAsmdef(string path, string featureName, string layer)
        {
            var asmdefName = $"{featureName}.{layer}";
            var asmdefPath = $"{path}/{asmdefName}.asmdef";

            if (File.Exists(asmdefPath)) return;

            var references = GetReferences(featureName, layer);

            var json = $@"{{
            ""name"": ""{asmdefName}"",
            ""references"": {JsonArray(references)},
            ""includePlatforms"": [],
            ""excludePlatforms"": [],
            ""allowUnsafeCode"": false,
            ""overrideReferences"": false
        }}";

            File.WriteAllText(asmdefPath, json);
        }

        string[] GetReferences(string featureName, string layer)
        {
            var refs = new List<string>();

            switch (layer)
            {
                case "Domain":
                    refs.Add($"{featureName}.Contracts");
                    refs.Add("Common.Contracts");
                    break;

                case "Application":
                    refs.Add($"{featureName}.Contracts");
                    refs.Add("Common.Signals");
                    refs.Add("Common.Utils");
                    refs.Add("Common.Contracts");
                    break;

                case "Presentation":
                    refs.Add($"{featureName}.Contracts");
                    refs.Add("Common.Signals");
                    refs.Add("Common.Utils");
                    refs.Add("Common.Contracts");
                    break;

                case "View":
                    refs.Add($"{featureName}.Contracts");
                    refs.Add("Common.Contracts");
                    refs.Add("Common.Signals");
                    refs.Add("Common.Utils");
                    break;

                case "Contracts":
                    refs.Add("Common.Contracts");
                    refs.Add("Common.Utils");
                    break;

                case "Controllers":
                    refs.Add($"{featureName}.Contracts");
                    refs.Add("Common.Signals");
                    refs.Add("Common.Utils");
                    refs.Add("Common.Contracts");
                    break;

                case "Infrastructure":
                    refs.Add($"{featureName}.Domain");
                    refs.Add($"{featureName}.Application");
                    refs.Add($"{featureName}.Presentation");
                    refs.Add($"{featureName}.View");
                    refs.Add($"{featureName}.Contracts");
                    refs.Add("Common.Signals");
                    refs.Add("Common.Contracts");
                    refs.Add("Common.Utils");
                    break;
            }

            return refs.ToArray();
        }

    
        //Создает болванку скрипта
        void CreateSampleScript(string path, string featureName, string layer)
        {
            var scriptPath = $"{path}/{featureName}{layer}.cs";
            if (File.Exists(scriptPath)) return;

            var content = $@"namespace {featureName}.{layer}
            {{
                public class {featureName}{layer}
                {{
                    // TODO: Implement
                }}
            }}";

            File.WriteAllText(scriptPath, content);
        }

        // ─── Утилиты ──────────────────────────────────────

        void CreateFolderRecursive(string path)
        {
            if (AssetDatabase.IsValidFolder(path))
                return;

            var parts = path.Replace("\\", "/").Split('/');
            var currentPath = parts[0];  // "Assets"

            for (int i = 1; i < parts.Length; i++)
            {
                var nextPath = $"{currentPath}/{parts[i]}";
                if (!AssetDatabase.IsValidFolder(nextPath))
                {
                    AssetDatabase.CreateFolder(currentPath, parts[i]);
                }
                currentPath = nextPath;
            }
        }

        string JsonArray(string[] items)
        {
            if (items.Length == 0) return "[]";
            var quoted = System.Array.ConvertAll(items, i => $"\"{i}\"");
            return $"[{string.Join(", ", quoted)}]";
        }

        void CreateReadme(string basePath, string featureName)
        {
            var readmePath = $"{basePath}/README.md";
            if (File.Exists(readmePath)) return;

            var content = $@"# {featureName}

            ## Правила взаимодействия слоёв

            ✅ РАЗРЕШЕНО:
              Controllers → View (через I{featureName}View)
              View → Presenter (через I{featureName}Presenter)
              Presenter → UseCase (через I{featureName}UseCase)
              UseCase → Model

            ❌ ЗАПРЕЩЕНО:
              View → UseCase (прыжок через слой)
              View → Model
              Presenter → Model (пропущен UseCase)
              Controller → Presenter (пропущен View)

            ## Внешнее взаимодействие
              С другими фичами — только через сигналы (Common.Signals)
              View-контакты — через Common.Contracts

            ## Структура
              Scripts/
              ├── Domain/
              ├── Application/
              ├── Presentation/
              ├── View/
              ├── Contracts/
              ├── Controllers/
              └── Infrastructure/
            ";

            File.WriteAllText(readmePath, content);
        }
    }
}