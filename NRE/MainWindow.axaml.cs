using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using NRE.Models;
using NRE.Services;

namespace NRE;

public partial class MainWindow : Window
{
    private readonly string _defaultPath = Path.Combine(AppContext.BaseDirectory, "NreDataV1.json");
    private string _savedSnapshot = string.Empty;
    private string _currentFilePath = string.Empty;
    
    private readonly DataStore _store = new();
    private bool _closeConfirmed;
    
    public MainWindow()
    {
        InitializeComponent();
        
        EstatesTab.Store = _store;
        PeopleTab.Store = _store;
        PaymentsTab.Store = _store;
    }

    private async void Window_OnClosing(object? sender, WindowClosingEventArgs e)
    {
        if (_closeConfirmed || !HasUnsavedChanges()) return;
        e.Cancel = true;
        
        var result = await MessageBoxManager
            .GetMessageBoxStandard("Save", "Do you want to save the changes?", ButtonEnum.YesNo)
            .ShowWindowDialogAsync(this);
        

        if (result == ButtonResult.Yes)
        {
            if (!await Save()) return;
        }

        _closeConfirmed = true;
        Close();
    }
    
    private async void TopLevel_OnOpened(object? sender, EventArgs e)
    {
        try
        {
            if (File.Exists(_defaultPath))
            {
                var data = FileService.Load<NreData>(_defaultPath);
                if (data != null) _store.LoadFrom(data);
            }
            else
            {
                FileService.Save(_defaultPath, _store.ToData());
            }
            
            _currentFilePath = _defaultPath;
            UpdatedSavedSnapshot();
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException
                                       or NotSupportedException or JsonException or ArgumentException)
        {
            await MessageBoxManager
                .GetMessageBoxStandard("Error", $"The file could not be loaded:\n{ex.Message}")
                .ShowWindowDialogAsync(this);
        }
    }
    
    private async void MnuNew_OnClick(object? sender, RoutedEventArgs e)
    {
        if (HasUnsavedChanges())
        {
            var result = await MessageBoxManager
                .GetMessageBoxStandard("Unsaved Data",
                    "There are unsaved changes\nAre you sure you want to proceed without saving?", ButtonEnum.YesNo)
                .ShowWindowDialogAsync(this);

            if (result != ButtonResult.Yes)
                return;
        }
        
        _store.ClearData();
        _currentFilePath = string.Empty;
        UpdatedSavedSnapshot();
    }
    
    private async void MnuSave_OnClick(object? sender, RoutedEventArgs e)
    {
        if (await Save())
        {
            await MessageBoxManager
                .GetMessageBoxStandard("Saved", "The file has been saved.")
                .ShowWindowDialogAsync(this);
        }
    }
    
    private async void MnuSaveAsJson_OnClick(object? sender, RoutedEventArgs e)
    {
        if (await SaveAsJson())
        {
            await MessageBoxManager
                .GetMessageBoxStandard("Saved", "The file has been saved.")
                .ShowWindowDialogAsync(this);
        }
    }
    
    private async void MnuSaveAsXml_OnClick(object? sender, RoutedEventArgs e)
    {
        if (await SaveAsXml())
        {
            await MessageBoxManager
                .GetMessageBoxStandard("Saved", "The file has been saved.")
                .ShowWindowDialogAsync(this);
        }
    }
    
    private void UpdatedSavedSnapshot() =>
        _savedSnapshot = FileService.ToJson(_store.ToData());
        
    private bool HasUnsavedChanges() =>
        FileService.ToJson(_store.ToData()) != _savedSnapshot;
    
    private async Task<bool> Save()
    {
        if (_currentFilePath == string.Empty)
            return await SaveAsJson();

        if (Path.GetExtension(_currentFilePath) == ".json")
        {
            return await SaveToFile(_currentFilePath, "json");
        }
        else
        {
            return await SaveToFile(_currentFilePath, "xml");
        }
        
    }
    
    private async Task<bool> SaveAsJson()
    {
        try
        {
            var file = await this.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = "Save as",
                SuggestedFileName = "NreData.json",
                DefaultExtension =  "json",
                ShowOverwritePrompt = true,
                FileTypeChoices = [
                    new FilePickerFileType("Json file") { Patterns = ["*.json"]}
                ]
            });
            
            var path = file?.TryGetLocalPath();
            if (path == null) return false;
            
            if (!await SaveToFile(path, "json")) return false;
            _currentFilePath = path;
            return true;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or NotSupportedException)
        {
            await MessageBoxManager
                .GetMessageBoxStandard("Error", $"The file could not be saved:\n{ex.Message}")
                .ShowWindowDialogAsync(this);
            return false;
        }
    }
    
    private async Task<bool> SaveAsXml()
    {
        try
        {
            var file = await this.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = "Save as",
                SuggestedFileName = "NreData.xml",
                DefaultExtension =  "xml",
                ShowOverwritePrompt = true,
                FileTypeChoices = [
                    new FilePickerFileType("XML file") { Patterns = ["*.xml"]}
                ]
            });
            
            var path = file?.TryGetLocalPath();
            if (path == null) return false;
            
            if (!await SaveToFile(path, "xml")) return false;
            _currentFilePath = path;
            return true;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or NotSupportedException)
        {
            await MessageBoxManager
                .GetMessageBoxStandard("Error", $"The file could not be saved:\n{ex.Message}")
                .ShowWindowDialogAsync(this);
            return false;
        }
    }
    
    private async Task<bool> SaveToFile(string path, string fileType)
    {
        try
        {
            if (fileType == "xml")
            {
                FileService.SaveAsXml(path, _store.ToData());
            }
            else
            {
                FileService.Save(path, _store.ToData());
            }
            
            UpdatedSavedSnapshot();
            return true;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or NotSupportedException
                                       or SerializationException or InvalidDataContractException)
        {
            await MessageBoxManager
                .GetMessageBoxStandard("Error", $"The file could not be saved:\n{ex.Message}")
                .ShowWindowDialogAsync(this);
            return false;
        }
    }


    private async void MnuLoad_OnClick(object? sender, RoutedEventArgs e)
    {
        var files = await this.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open file",
            AllowMultiple = false,
            FileTypeFilter = [
                new FilePickerFileType("NRE data") { Patterns = ["*.json", "*.xml"] },
                new FilePickerFileType("JSON file") { Patterns = ["*.json"] },
                new FilePickerFileType("XML file") { Patterns = ["*.xml"] }
            ]
        });
            
        var path = files.Count > 0 ? files[0].TryGetLocalPath() : null;
        if (path == null) return;
        
        try
        {
            NreData? data;
            if (Path.GetExtension(path) == ".json")
                data = FileService.Load<NreData>(path);
            else 
                data = FileService.LoadFromXml<NreData>(path);

            if (data == null)
            {
                await MessageBoxManager
                    .GetMessageBoxStandard("Error", "The file contains no data.")
                    .ShowWindowDialogAsync(this);
                return;
            }

            _store.LoadFrom(data);
            _currentFilePath = path;
            UpdatedSavedSnapshot();
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or NotSupportedException
                                       or JsonException or SerializationException or ArgumentException)
        {
            await MessageBoxManager
                .GetMessageBoxStandard("Error", $"The file could not be loaded:\n{ex.Message}")
                .ShowWindowDialogAsync(this);
        }
    }

    private void MnuExit_OnClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}