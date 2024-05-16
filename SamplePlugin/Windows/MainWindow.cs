using System;
using System.IO;
using System.Numerics;
using Dalamud.Interface.Internal;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using ImGuiNET;

namespace SamplePlugin.Windows;

public class MainWindow : Window, IDisposable
{
    public Configuration Configuration { get; }
    public ITextureProvider TextureProvider { get; }
    public DalamudPluginInterface PluginInterface { get; }
    public ConfigWindow ConfigWindow { get; }
    private IDalamudTextureWrap? GoatImage;

    // We give this window a hidden ID using ##
    // So that the user will see "My Amazing Window" as window title,
    // but for ImGui the ID is "My Amazing Window##With a hidden ID"
    public MainWindow(Configuration configuration, ITextureProvider textureProvider, DalamudPluginInterface pluginInterface, ConfigWindow configWindow)
        : base("My Amazing Window##With a hidden ID", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        Configuration = configuration;
        TextureProvider = textureProvider;
        PluginInterface = pluginInterface;
        ConfigWindow = configWindow;
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        var file = new FileInfo(Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "goat.png"));
        GoatImage = textureProvider.GetTextureFromFile(file);
    }

    public void Dispose() { }

    public override void Draw()
    {
        ImGui.Text($"The random config bool is {Configuration.SomePropertyToBeSavedAndWithADefault}");

        if (ImGui.Button("Show Settings"))
        {
            ConfigWindow.Toggle();
        }

        ImGui.Spacing();

        ImGui.Text("Have a goat:");
        if (GoatImage != null)
        {
            ImGuiHelpers.ScaledIndent(55f);
            ImGui.Image(GoatImage.ImGuiHandle, new Vector2(GoatImage.Width, GoatImage.Height));
            ImGuiHelpers.ScaledIndent(-55f);
        }
        else
        {
            ImGui.Text("Image not found.");
        }
    }
}
