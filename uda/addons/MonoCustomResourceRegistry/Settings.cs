using System.Collections.ObjectModel;
using Godot;
using Godot.Collections;
using Array = Godot.Collections.Array;

namespace UDA.addons.MonoCustomResourceRegistry;

public static class Settings
{
    public enum ResourceSearchType
    {
        Recursive = 0,
        Namespace = 1
    }

    public static string ClassPrefix => GetSettings(nameof(ClassPrefix)).AsString();
    public static ResourceSearchType SearchType => (ResourceSearchType)GetSettings(nameof(SearchType)).AsInt32();

    public static ReadOnlyCollection<string> ResourceScriptDirectories
    {
        get
        {
            var array = (Array)GetSettings(nameof(ResourceScriptDirectories)) ?? new Array();
            return new ReadOnlyCollection<string>(array.Select(v => v.AsString()).ToList());
        }
    }

    public static void Init()
    {
        AddSetting(nameof(ClassPrefix), Variant.Type.String, "");
        AddSetting(nameof(SearchType), Variant.Type.Int, ResourceSearchType.Recursive, PropertyHint.Enum,
            "Recursive,Namespace");
        AddSetting(nameof(ResourceScriptDirectories), Variant.Type.Array, new Array<string>(new[] { "res://" }));
    }

    private static Variant GetSettings(string title)
    {
        // I removed the global alias qualifier here because it was causing error CS8083
        return ProjectSettings.GetSetting($"{nameof(MonoCustomResourceRegistry)}/{title}");
    }

    private static void AddSetting<T>(string title, Variant.Type type, T value, PropertyHint hint = PropertyHint.None,
        string hintString = "")
    {
        title = SettingPath(title);
        if (!ProjectSettings.HasSetting(title))
            ProjectSettings.SetSetting(title, Variant.From(value));
        var info = new Dictionary
        {
            ["name"] = title,
            ["type"] = Variant.From(type),
            ["hint"] = Variant.From(hint),
            ["hint_string"] = hintString
        };
        ProjectSettings.AddPropertyInfo(info);
        GD.Print("Successfully added property: " + title);
    }

    private static string SettingPath(string title)
    {
        // I removed the global alias qualifier here because it was causing error CS8083
        return $"{nameof(MonoCustomResourceRegistry)}/{title}";
    }
}