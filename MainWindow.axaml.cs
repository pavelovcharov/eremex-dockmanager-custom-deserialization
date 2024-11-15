using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.ComponentModel;
using Eremex.AvaloniaUI.Controls.Common;
using Eremex.AvaloniaUI.Controls.Docking;
using Eremex.AvaloniaUI.Controls.Serialization;
using Eremex.AvaloniaUI.Controls.Utils;

namespace EremexAvaloniaApplication1;

public partial class MainWindow : MxWindow
{
    private ViewModel viewModel;
    private string serializationId;
    public MainWindow()
    {
        InitializeComponent();
        viewModel = new ViewModel();
        DataContext = viewModel;
        serializationId = Guid.NewGuid().ToString();
        
        dock.AddHandler(SerializationManager.StartDeserializingEvent, OnStartDeserializing);
        dock.AddHandler(SerializationManager.EndDeserializingEvent, OnEndDeserializing);
    }

    private void OnEndDeserializing(object? sender, RoutedEventArgs e)
    {
        dock.GetItems().Where(x => x.Tag != serializationId).ForEach(x => dock.Remove(x));
    }

    private void OnStartDeserializing(object? sender, RoutedEventArgs e)
    {
        dock.GetItems().ForEach(x => x.Tag = serializationId);
    }
    
    

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        var fs = File.OpenWrite("test.xml");
        dock.SaveLayout(fs);
        fs.Close();
    }

    private void Button_OnClick1(object? sender, RoutedEventArgs e)
    {
        var fs = File.OpenRead("test.xml");
        dock.RestoreLayout(fs);
        fs.Close();
    }

    private void Button_OnClick2(object? sender, RoutedEventArgs e)
    {
        viewModel.Panes.Add(new PaneViewModel(){ Header = Guid.NewGuid().ToString()});
    }
}

public class ViewModel : ObservableObject
{
    public ObservableCollection<PaneViewModel> Panes { get; } = new();
}

public partial class PaneViewModel : ObservableObject
{
    [ObservableProperty] private string header;
}