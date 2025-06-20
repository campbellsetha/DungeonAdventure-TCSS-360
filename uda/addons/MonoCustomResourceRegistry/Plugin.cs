using System.Reflection;
using System.Text;
using Godot;
using MonoCustomResourceRegistry;
using FileAccess = Godot.FileAccess;

// Originally written by wmigor
// Edited by Atlinx to recursively search for files.
// Edited by bls220 to update for Godot 4.0
// wmigor's Public Repo: https://github.com/wmigor/godot-mono-custom-resource-register
namespace UDA.addons.MonoCustomResourceRegistry;
#if TOOLS
[Tool]
#nullable enable
public partial class Plugin : EditorPlugin
{
    // We're not going to hijack the Mono Build button since it actually takes time to build
    // and we can't be sure how long that is. I guess we have to leave refreshing to the user for now.
    // There isn't any automation we can do to fix that.
    // private Button MonoBuildButton => GetNode<Button>("/root/EditorNode/@@580/@@581/@@589/@@590/Button");
    private readonly List<string> customTypes = new();
    private Button? refreshButton;

    public override void _EnterTree()
    {
        refreshButton = new Button();
        refreshButton.Text = "CCR";

        AddControlToContainer(CustomControlContainer.Toolbar, refreshButton);
        refreshButton.Icon = EditorInterface.Singleton.GetBaseControl().GetThemeIcon("Reload", "EditorIcons");
        refreshButton.Pressed += OnRefreshPressed;

        Settings.Init();
        RefreshCustomClasses();
        GD.PushWarning(
            "You may change any setting for MonoCustomResourceRegistry in Project -> ProjectSettings -> General -> MonoCustomResourceRegistry");
    }

    public override void _ExitTree()
    {
        UnregisterCustomClasses();
        RemoveControlFromContainer(CustomControlContainer.Toolbar, refreshButton);
        refreshButton?.QueueFree();
    }

    public void RefreshCustomClasses()
    {
        GD.Print("\nRefreshing Registered Resources...");
        UnregisterCustomClasses();
        RegisterCustomClasses();
    }

    private void RegisterCustomClasses()
    {
        customTypes.Clear();

        foreach (var type in GetCustomRegisteredTypes())
            if (type.IsSubclassOf(typeof(Resource)))
                AddRegisteredType(type, nameof(Resource));
            else
                AddRegisteredType(type, nameof(Node));
    }
    
    private void AddRegisteredType(Type type, string defaultBaseTypeName)
    {
        var attribute = Attribute.GetCustomAttribute(type, typeof(RegisteredTypeAttribute)) as RegisteredTypeAttribute;
        var path = FindClassPath(type);
        if (path == null && !FileAccess.FileExists(path))
            return;
        var script = GD.Load<Script>(path);
        if (script == null)
            return;
        var baseType = defaultBaseTypeName;
        if (attribute is not null && attribute.baseType != "")
            baseType = attribute.baseType;
        ImageTexture? icon = null;
        if (attribute is not null && attribute.iconPath != "")
        {
            if (FileAccess.FileExists(attribute.iconPath))
            {
                var rawIcon = ResourceLoader.Load<Texture2D>(attribute.iconPath);
                if (rawIcon != null)
                {
                    var image = rawIcon.GetImage();
                    var length = (int)Mathf.Round(16 * EditorInterface.Singleton.GetEditorScale());
                    image.Resize(length, length);
                    icon = ImageTexture.CreateFromImage(image);
                }
                else
                {
                    GD.PushError(
                        $"Could not load the icon for the registered type \"{type.FullName}\" at path \"{path}\".");
                }
            }
            else
            {
                GD.PushError(
                    $"The icon path of \"{path}\" for the registered type \"{type.FullName}\" does not exist.");
            }
        }

        AddCustomType($"{Settings.ClassPrefix}{type.Name}", baseType, script, icon);
        customTypes.Add($"{Settings.ClassPrefix}{type.Name}");
        GD.Print($"Registered custom type: {type.Name} -> {path}");
    }
    
    private static string? FindClassPath(Type type)
    {
        switch (Settings.SearchType)
        {
            case Settings.ResourceSearchType.Recursive:
                return FindClassPathRecursive(type);
            case Settings.ResourceSearchType.Namespace:
                return FindClassPathNamespace(type);
            default:
                throw new Exception($"ResourceSearchType {Settings.SearchType} not implemented!");
        }
    }

    private static string? FindClassPathNamespace(Type type)
    {
        foreach (var dir in Settings.ResourceScriptDirectories)
        {
            StringBuilder builder = new(dir);
            if (!dir.EndsWith('/')) builder.Append('/');
            if (type.Namespace is not null)
                builder
                    .Append(type.Namespace.Replace(".", "/"))
                    .Append('/');
            builder
                .Append(type.Name)
                .Append(".cs");
            var filePath = builder.ToString();
            if (FileAccess.FileExists(filePath))
                return filePath;
        }

        return null;
    }

    private static string? FindClassPathRecursive(Type type)
    {
        foreach (var directory in Settings.ResourceScriptDirectories)
        {
            var fileFound = FindClassPathRecursiveHelper(type, directory);
            if (fileFound != null)
                return fileFound;
        }

        return null;
    }

    private static string? FindClassPathRecursiveHelper(Type type, string directory)
    {
        var dir = DirAccess.Open(directory);

        if (DirAccess.GetOpenError() == Error.Ok)
        {
            dir.ListDirBegin();

            while (true)
            {
                var fileOrDirName = dir.GetNext();

                // Skips hidden files like .
                if (fileOrDirName == "")
                    break;
                if (fileOrDirName.StartsWith("."))
                    continue;
                if (dir.CurrentIsDir())
                {
                    var foundFilePath = FindClassPathRecursiveHelper(type, dir.GetCurrentDir() + "/" + fileOrDirName);
                    if (foundFilePath != null)
                    {
                        dir.ListDirEnd();
                        return foundFilePath;
                    }
                }
                else if (fileOrDirName == $"{type.Name}.cs")
                {
                    return dir.GetCurrentDir() + "/" + fileOrDirName;
                }
            }
        }

        return null;
    }

    private static IEnumerable<Type> GetCustomRegisteredTypes()
    {
        var assembly = Assembly.GetAssembly(typeof(Plugin));
        return assembly?.GetTypes().Where(t => !t.IsAbstract
                                               && Attribute.IsDefined(t, typeof(RegisteredTypeAttribute))
                                               && (t.IsSubclassOf(typeof(Node)) || t.IsSubclassOf(typeof(Resource)))
        ) ?? Enumerable.Empty<Type>();
    }

    private void UnregisterCustomClasses()
    {
        foreach (var script in customTypes)
        {
            RemoveCustomType(script);
            GD.Print($"Unregister custom resource: {script}");
        }

        customTypes.Clear();
    }

    private void OnRefreshPressed()
    {
        RefreshCustomClasses();
    }
}
#endif