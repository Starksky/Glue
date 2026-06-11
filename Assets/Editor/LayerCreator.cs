using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public static class LayerCreator
    {
        // ─── Меню по правому клику на ПАПКЕ ─────────────────

        [MenuItem("Assets/Create/Feature Layer/Contracts", false, 99)]
        static void AddContracts() => AddLayer("Contracts");
        static void AddContracts(bool isDebug) => AddLayer("Contracts", isDebug);
        
        [MenuItem("Assets/Create/Feature Layer/Domain", false, 100)]
        static void AddDomain()
        {
            AddContracts(false);
            AddLayer("Domain");
        }

        [MenuItem("Assets/Create/Feature Layer/Application", false, 101)]
        static void AddApplication() 
        {
            AddContracts(false);
            AddLayer("Application");
        }

        [MenuItem("Assets/Create/Feature Layer/Presentation", false, 102)]
        static void AddPresentation()
        {
            AddContracts(false);
            AddLayer("Presentation");
        }

        [MenuItem("Assets/Create/Feature Layer/View", false, 103)]
        static void AddView()
        {
            AddContracts(false);
            AddLayer("View");
        }

        [MenuItem("Assets/Create/Feature Layer/Controllers", false, 104)]
        static void AddControllers()
        {
            AddContracts(false);
            AddLayer("Controllers");
        }

        [MenuItem("Assets/Create/Feature Layer/Infrastructure", false, 105)]
        static void AddInfrastructure()
        {
            AddContracts(false);
            AddLayer("Infrastructure");
        }

        // ─── Показывать меню только для папок внутри Features ───

        [MenuItem("Assets/Create/Feature Layer/Domain", true)]
        [MenuItem("Assets/Create/Feature Layer/Application", true)]
        [MenuItem("Assets/Create/Feature Layer/Presentation", true)]
        [MenuItem("Assets/Create/Feature Layer/View", true)]
        [MenuItem("Assets/Create/Feature Layer/Contracts", true)]
        [MenuItem("Assets/Create/Feature Layer/Controllers", true)]
        [MenuItem("Assets/Create/Feature Layer/Infrastructure", true)]
        static bool ValidateAddLayer()
        {
            var selectedPath = GetSelectedPath();
            if (string.IsNullOrEmpty(selectedPath)) return false;
        
            // Показываем меню только если мы внутри Features
            return selectedPath.Contains("Features/");
        }

        // ─── Логика добавления слоя ─────────────────────────

        static void AddLayer(string layer, bool isDebug = true)
        {
            var selectedPath = GetSelectedPath();
            if (string.IsNullOrEmpty(selectedPath))
            {
                if (!isDebug)
                    return;
                
                Debug.LogError("No folder selected.");
                return;
            }

            var (featureName, scriptsPath) = FindFeatureContext(selectedPath);
        
            if (featureName == null)
            {
                if (!isDebug)
                    return;
                
                Debug.LogError($"Cannot find feature root. Selected: {selectedPath}");
                EditorUtility.DisplayDialog("Error", 
                    "Cannot determine feature name.\nSelect a folder inside a feature (e.g., Features/Player/Scripts).", 
                    "OK");
                return;
            }

            // Путь для нового слоя
            var layerPath = $"{scriptsPath}/{layer}";

            // Проверяем, существует ли уже
            if (AssetDatabase.IsValidFolder(layerPath))
            {
                if (!isDebug)
                    return;
                
                Debug.LogWarning($"Layer '{layer}' already exists in feature '{featureName}'");
                EditorUtility.DisplayDialog("Warning", 
                    $"Layer '{layer}' already exists in feature '{featureName}'!", 
                    "OK");
                return;
            }

            // Создаём
            CreateFolderRecursive(layerPath);
            CreateAsmdef(layerPath, featureName, layer);
            //CreateSampleScript(layerPath, featureName, layer);

            AssetDatabase.Refresh();

            // Выделяем созданную папку
            var obj = AssetDatabase.LoadAssetAtPath<Object>(layerPath);
            if (obj != null)
            {
                EditorGUIUtility.PingObject(obj);
                Selection.activeObject = obj;
            }

            if (!isDebug)
                return;
            
            Debug.Log($"Layer '{layer}' added to feature '{featureName}' at {layerPath}");
        }

        // ─── Поиск контекста фичи ──────────────────────────

        static (string featureName, string scriptsPath) FindFeatureContext(string selectedPath)
        {
            var parts = selectedPath.Replace("\\", "/").Split('/');

            // Ищем "Features" в пути
            int featuresIndex = -1;
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i] == "Features" && i + 1 < parts.Length)
                {
                    featuresIndex = i;
                    break;
                }
            }

            if (featuresIndex < 0) return (null, null);

            // Ищем папку Scripts
            int scriptsIndex = -1;
            for (int i = featuresIndex + 1; i < parts.Length; i++)
            {
                if (parts[i] == "Scripts")
                {
                    scriptsIndex = i;
                    break;
                }
            }

            string featurePath;
            string scriptsPath;

            if (scriptsIndex >= 0)
            {
                // Scripts найден — берём путь от Features до Scripts
                var rootParts = parts.Skip(featuresIndex + 1).Take(scriptsIndex - featuresIndex - 1);
                featurePath = string.Join("/", rootParts);
                scriptsPath = string.Join("/", parts.Take(scriptsIndex + 1));
            }
            else
            {
                // Scripts не найден — используем выбранную папку как корень фичи
                var rootParts = parts.Skip(featuresIndex + 1);
                featurePath = string.Join("/", rootParts);
                scriptsPath = $"{string.Join("/", parts)}/Scripts";
            }

            // Имя фичи = путь с точками (для namespace)
            var featureName = featurePath.Replace("/", ".");

            return (featureName, scriptsPath);
        }

        static string GetSelectedPath()
        {
            var selected = Selection.activeObject;
            if (selected == null) return null;

            var path = AssetDatabase.GetAssetPath(selected);
            if (string.IsNullOrEmpty(path)) return null;

            // Если выбран файл — берём папку файла
            if (!AssetDatabase.IsValidFolder(path))
                path = Path.GetDirectoryName(path).Replace("\\", "/");

            return path;
        }

        // ─── Создание asmdef ────────────────────────────────

        static void CreateAsmdef(string path, string featureName, string layer)
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

        static string[] GetReferences(string featureName, string layer)
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
                    refs.Add($"{featureName}.Controllers");
                    refs.Add("Common.Signals");
                    refs.Add("Common.Contracts");
                    refs.Add("Common.Utils");
                    refs.Add("Common.Infrastructure");
                    break;
            }

            return refs.ToArray();
        }

        // ─── Создание заготовки скрипта ────────────────────

        static void CreateSampleScript(string path, string featureName, string layer)
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

        // ─── Утилиты ────────────────────────────────────────

        static void CreateFolderRecursive(string path)
        {
            if (AssetDatabase.IsValidFolder(path))
                return;

            var parts = path.Replace("\\", "/").Split('/');
            var currentPath = parts[0];

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

        static string JsonArray(string[] items)
        {
            if (items.Length == 0) return "[]";
            var quoted = System.Array.ConvertAll(items, i => $"\"{i}\"");
            return $"[{string.Join(", ", quoted)}]";
        }
    }
}