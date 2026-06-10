using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class FeatureWithLayersWizard : ScriptableWizard
{
    [Header("Название фичи")]
    [Tooltip("Например: BouncyWall")]
    public string featureName = "NewFeature";

    [Tooltip("Подпапка в Features/. Например: Walls (оставьте пустым для корня)")]
    public string featureGroup = "Group";

    [Tooltip("Подпапка в Scripts/. Обычно совпадает с featureGroup")]
    public string scriptsGroup = "Group";

    [Header("Пресеты")]
    public bool usePreset = true;

    public enum LayerPreset
    {
        Custom,             // то, что выбрано галочками
        FullFeature,        // Domain + Application + Contracts + Controllers + Presentation + View
        NoView,             // Domain + Application + Contracts + Controllers (без View/Presentation)
        LogicOnly,          // Domain + Application + Contracts
        Minimal             // Domain + Contracts
    }

    public LayerPreset preset = LayerPreset.FullFeature;
    
    [Header("Ресурсы (Features/)")]
    public bool createConfigs = true;
    public bool createAudio = true;
    public bool createAnimations;
    public bool createMaterials = true;
    public bool createTextures = true;

    [Header("Слои кода (Scripts/)")]
    public bool createDomain = true;
    public bool createApplication = true;
    public bool createContracts = true;
    public bool createControllers = true;
    public bool createPresentation;
    public bool createView;

    [Header("Создать заготовки .cs файлов?")]
    public bool createScriptFiles = true;
    

    [MenuItem("Tools/Architecture/Create Feature With Layers")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard<FeatureWithLayersWizard>(
            "Create Feature With Layers", "Create All");
    }

    void OnWizardUpdate()
    {
        if (!usePreset) return;

        switch (preset)
        {
            case LayerPreset.FullFeature:
                createDomain = true;
                createApplication = true;
                createContracts = true;
                createControllers = true;
                createPresentation = true;
                createView = true;
                break;

            case LayerPreset.NoView:
                createDomain = true;
                createApplication = true;
                createContracts = true;
                createControllers = true;
                createPresentation = false;
                createView = false;
                break;

            case LayerPreset.LogicOnly:
                createDomain = true;
                createApplication = true;
                createContracts = true;
                createControllers = false;
                createPresentation = false;
                createView = false;
                break;

            case LayerPreset.Minimal:
                createDomain = true;
                createApplication = false;
                createContracts = true;
                createControllers = false;
                createPresentation = false;
                createView = false;
                break;

            case LayerPreset.Custom:
                usePreset = false;
                break;
        }
    }

    void OnWizardCreate()
    {
        if (string.IsNullOrEmpty(featureName))
        {
            Debug.LogError("Feature name cannot be empty.");
            return;
        }

        // 1. Создаём папки в Features/
        CreateFeatureFolders();

        // 2. Создаём папки в Scripts/ по слоям
        CreateScriptFolders();

        // 3. Создаём заготовки .cs файлов
        if (createScriptFiles)
        {
            CreateScriptFiles();
        }

        AssetDatabase.Refresh();

        // Фокус на папке фичи в Features/
        var featureFolder = GetFeatureFolderPath();
        var obj = AssetDatabase.LoadAssetAtPath<Object>(featureFolder);
        if (obj != null)
        {
            EditorGUIUtility.PingObject(obj);
            Selection.activeObject = obj;
        }

        Debug.Log($"Feature '{featureName}' created with layers and folders.");
    }

    // ─── ПАПКИ В FEATURES/ ────────────────────────────

    private string GetFeatureFolderPath()
    {
        if (string.IsNullOrEmpty(featureGroup))
            return $"Assets/_Project/Features/{featureName}";
        return $"Assets/_Project/Features/{featureGroup}/{featureName}";
    }

    private void CreateFeatureFolders()
    {
        var featurePath = GetFeatureFolderPath();
        var folders = new List<string>();

        if (createConfigs)    folders.Add("Configs");
        if (createAudio)      folders.Add("Audio");
        if (createAnimations) folders.Add("Animations");
        if (createMaterials)  folders.Add("Materials");
        if (createTextures)   folders.Add("Textures");

        CreateFolderRecursive(featurePath);
        foreach (var folder in folders)
        {
            CreateFolderRecursive($"{featurePath}/{folder}");
        }
    }

    // ─── ПАПКИ В SCRIPTS/ ПО СЛОЯМ ────────────────────

    private string GetScriptsSubfolder()
    {
        if (string.IsNullOrEmpty(scriptsGroup))
            return featureName;
        return $"{scriptsGroup}/{featureName}";
    }

    private void CreateScriptFolders()
    {
        var subfolder = GetScriptsSubfolder();
        var scriptsRoot = "Assets/_Project/Scripts";

        var layers = new Dictionary<string, bool>
        {
            { "Domain",         createDomain },
            { "Application",    createApplication },
            { "Contracts",      createContracts },
            { "Controllers",    createControllers },
            { "Presentation",   createPresentation },
            { "View",           createView }
        };

        foreach (var layer in layers)
        {
            if (layer.Value)
            {
                var path = $"{scriptsRoot}/{layer.Key}/{subfolder}";
                CreateFolderRecursive(path);
            }
        }
    }

    // ─── ЗАГОТОВКИ .CS ФАЙЛОВ ─────────────────────────

    private void CreateScriptFiles()
    {
        var subfolder = GetScriptsSubfolder();
        var scriptsRoot = "Assets/_Project/Scripts";

        if (createDomain)
        {
            CreateScriptFile($"{scriptsRoot}/Domain/{subfolder}", 
                $"{featureName}Model.cs", 
                GetModelTemplate());
        }

        if (createApplication)
        {
            CreateScriptFile($"{scriptsRoot}/Application/{subfolder}", 
                $"{featureName}UseCase.cs", 
                GetUseCaseTemplate());
        }

        if (createContracts)
        {
            CreateScriptFile($"{scriptsRoot}/Contracts/{subfolder}", 
                $"I{featureName}View.cs", 
                GetInterfaceTemplate());

            CreateScriptFile($"{scriptsRoot}/Contracts/{subfolder}", 
                $"{featureName}ContactData.cs", 
                GetDTOTemplate());
        }

        if (createControllers)
        {
            CreateScriptFile($"{scriptsRoot}/Controllers/{subfolder}", 
                $"{featureName}Controller.cs", 
                GetControllerTemplate());
        }

        if (createPresentation)
        {
            CreateScriptFile($"{scriptsRoot}/Presentation/{subfolder}", 
                $"{featureName}Presenter.cs", 
                GetPresenterTemplate());
        }

        if (createView)
        {
            CreateScriptFile($"{scriptsRoot}/View/{subfolder}", 
                $"{featureName}View.cs", 
                GetViewTemplate());
        }
    }

    private void CreateScriptFile(string folderPath, string fileName, string content)
    {
        var fullPath = $"{folderPath}/{fileName}";
        
        // Не перезаписываем существующие файлы
        if (File.Exists(fullPath))
        {
            Debug.LogWarning($"File already exists, skipping: {fullPath}");
            return;
        }

        File.WriteAllText(fullPath, content, Encoding.UTF8);
    }

    // ─── ШАБЛОНЫ КОДА ─────────────────────────────────

    private string GetModelTemplate()
    {
        return $@"// Domain/{GetScriptsSubfolder()}/{featureName}Model.cs
// Богатая модель. Данные + логика над собой.

public class {featureName}Model
{{
    // public float Health {{ get; private set; }}
    
    // public void TakeDamage(float amount)
    // {{
    //     Health -= amount;
    // }}
}}
";
    }

    private string GetUseCaseTemplate()
    {
        return $@"// Application/{GetScriptsSubfolder()}/{featureName}UseCase.cs
// Сценарий. Оркестрирует взаимодействие модели с внешним миром.

public class {featureName}UseCase
{{
    // readonly {featureName}Model model;
    // readonly IPlayerMotor motor;

    // public void Execute(ContactData data)
    // {{
    //     // оркестрация
    // }}
}}
";
    }

    private string GetInterfaceTemplate()
    {
        return $@"// Contracts/{GetScriptsSubfolder()}/I{featureName}View.cs
// Интерфейс для View или Controller.

public interface I{featureName}View
{{
    // void HandleContact(ContactData data);
}}
";
    }

    private string GetDTOTemplate()
    {
        return $@"// Contracts/{GetScriptsSubfolder()}/{featureName}ContactData.cs
// DTO. Структура данных для передачи между слоями.

public struct {featureName}ContactData
{{
    // public Vector3 Point;
    // public Vector3 Normal;
}}
";
    }

    private string GetControllerTemplate()
    {
        return $@"// Controllers/{GetScriptsSubfolder()}/{featureName}Controller.cs
// Точка входа. Принимает внешний сигнал и вызывает Use Case.

using UnityEngine;

public class {featureName}Controller : MonoBehaviour
{{
    // {featureName}UseCase useCase;

    // [Inject]
    // void Construct({featureName}UseCase uc) => useCase = uc;

    // void OnTriggerEnter(Collider other)
    // {{
    //     useCase.Execute(data);
    // }}
}}
";
    }

    private string GetPresenterTemplate()
    {
        return $@"// Presentation/{GetScriptsSubfolder()}/{featureName}Presenter.cs
// Логика отображения. Работает с View через интерфейс.

public class {featureName}Presenter
{{
    // readonly I{featureName}View view;
    // readonly {featureName}Model model;

    // public void UpdateView()
    // {{
    //     view.SetSomething(model.Something);
    // }}
}}
";
    }

    private string GetViewTemplate()
    {
        return $@"// View/{GetScriptsSubfolder()}/{featureName}View.cs
// Компоненты Unity. Ссылки на Image, Animator, ParticleSystem и т.д.

using UnityEngine;

public class {featureName}View : MonoBehaviour
{{
    // [SerializeField] Image icon;
    // [SerializeField] ParticleSystem particles;

    // public void SetSomething(bool value)
    // {{
    //     // обновление UI/эффектов
    // }}
}}
";
    }

    // ─── УТИЛИТЫ ───────────────────────────────────────

    private void CreateFolderRecursive(string fullPath)
    {
        if (AssetDatabase.IsValidFolder(fullPath))
            return;

        fullPath = fullPath.Replace("\\", "/");

        var lastSlash = fullPath.LastIndexOf('/');
        if (lastSlash <= 0) return;

        var parentPath = fullPath[..lastSlash];
        var folderName = fullPath[(lastSlash + 1)..];

        CreateFolderRecursive(parentPath);
        AssetDatabase.CreateFolder(parentPath, folderName);
    }
}