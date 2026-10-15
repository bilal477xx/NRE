using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using NRE.Helpers;
using NRE.Models;
using NRE.Models.Abstracts;
using NRE.Models.Entities.People;
using NRE.Models.Enums;
using NRE.Models.Wrapper;

namespace NRE.Views;

public partial class EstatesView : UserControl
{
    private DataStore _store = null!;
    private Estate? _currentEstate;
    
    public DataStore Store
    {
        get => _store;
        set
        {
            _store = value;
            _store.EstatesManager.ListChanged += UpdateEstatesView;
            _store.PeopleManager.ListChanged += UpdatePeopleComboBoxes;
            UpdateEstatesView();
        }
    }

    public EstatesView()
    {
        InitializeComponent();
        CmbEstateType.ItemsSource = TypeDiscovery.GetConcreteSubclasses<Estate>();
        CmbCountry.ItemsSource = Enum.GetValues<Countries>();
        CmbLegalForm.ItemsSource = Enum.GetValues<LegalForm>();
        
    }
    
    private async void BtnAdd_OnClick(object? sender, RoutedEventArgs e)
    {
        if (_currentEstate == null) return;

        if (!_currentEstate.Validate())
        {
            await Dialogs.ShowErrorAsync("Make sure all fields are filled properly.");
            return;
        }

        Store.EstatesManager.Add(_currentEstate);
        ResetForm();
    }
    
    private async void BtnEdit_OnClick(object? sender, RoutedEventArgs e)
    {
        int index = LbEstates.SelectedIndex;
        
        if (!_store.EstatesManager.CheckIndex(index) || _currentEstate == null)
        {
            await Dialogs.ShowErrorAsync("No estate selected.");
            return;
        }
        
        Store.EstatesManager.ChangeAt(LbEstates.SelectedIndex, _currentEstate);
        LbEstates.SelectedItem = null;
        ResetForm();
    }
    
    private async void BtnRemove_OnClick(object? sender, RoutedEventArgs e)
    {
        int index = LbEstates.SelectedIndex;
        if (!_store.EstatesManager.CheckIndex(index))
        {
            await Dialogs.ShowErrorAsync("No estate selected.");
            return;
        }

        Store.EstatesManager.DeleteAt(LbEstates.SelectedIndex);
        ResetForm();
    }
    
    private void CmbEstateType_OnSelectionChanged(object? sender, SelectionChangedEventArgs e) => ResetForm();
    
    private async void BtnChooseImage_OnClick(object? sender, RoutedEventArgs e)
        {
            if (_currentEstate == null) return;
    
            var topLevel = TopLevel.GetTopLevel(this);
            if (topLevel == null) return;
    
            var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Choose an image",
                AllowMultiple = false,
                FileTypeFilter = [FilePickerFileTypes.ImageAll]
            });
    
            var path = files.FirstOrDefault()?.TryGetLocalPath();
            if (path == null) return;
    
            _currentEstate.ImagePath = CopyToAppFolder(path);
            ShowImage(_currentEstate.ImagePath);
        }
    
    private void LbEstates_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        int index = LbEstates.SelectedIndex;
        if (_store.EstatesManager.CheckIndex(index))
        {
            ShowEstate(_store.EstatesManager.GetAt(index));
            SetEditMode(true);
        }
        else
        {
            SetEditMode(false);
            ResetForm();
        }
    }
    
    /// <summary>
    /// Starts a new, empty estate of the type selected in the type combo box.
    /// </summary>
    private void ResetForm()
    {
        if (CmbEstateType.SelectedItem is not TypeDisplayWrapper selectedType) return;

        ShowEstate((Estate)Activator.CreateInstance(selectedType.ClassType)!);
    }

    /// <summary>
    /// Updates the content of LbEstate
    /// </summary>
    private void UpdateEstatesView()
    {
        LbEstates.ItemsSource = _store.EstatesManager.ToStringArray();
        UpdatePeopleComboBoxes();
    }

    private void UpdatePeopleComboBoxes()
    {
        var selectedSeller = CmbSeller.SelectedItem;
        var selectedBuyer = CmbBuyer.SelectedItem;

        CmbSeller.ItemsSource = _store.PeopleManager.GetAll().OfType<Seller>();
        CmbBuyer.ItemsSource = _store.PeopleManager.GetAll().OfType<Buyer>();

        CmbSeller.SelectedItem = selectedSeller;
        CmbBuyer.SelectedItem = selectedBuyer;
    }
    
    private void ShowEstate(Estate estate)
    {
        _currentEstate = estate;
        SpGeneralForm.DataContext = estate;
        DynamicEstateCategory.Content = estate;
        DynamicForm.Content = estate;
        ShowImage(estate.ImagePath);
    }
    
    private void SetEditMode(bool isEditing)
    {
        CmbEstateType.IsEnabled = !isEditing;
        SpEditButtons.IsVisible = isEditing;
        BtnAdd.IsVisible = !isEditing;
    }
    
    private void ShowImage(string? path)
    {
        (ImgEstate.Source as IDisposable)?.Dispose(); // free the previous image
        ImgEstate.Source = !string.IsNullOrEmpty(path) && File.Exists(path)
            ? new Bitmap(path)
            : null;
    }

    /// <summary>
    /// Copies the chosen image into the app's data folder so the estate keeps
    /// its image even if the original file is moved or deleted.
    /// </summary>
    private static string CopyToAppFolder(string sourcePath)
    {
        var folder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "NRE", "images");
        Directory.CreateDirectory(folder);

        var destination = Path.Combine(folder, $"{Guid.NewGuid()}{Path.GetExtension(sourcePath)}");
        File.Copy(sourcePath, destination);
        return destination;
    }

    private void TxtCitySearch_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TxtCitySearch.Text))
        {
            UpdateEstatesView();
            return;
        }

        var matches = _store.EstatesManager.GetByCity(TxtCitySearch.Text);
        LbEstates.ItemsSource = matches.SelectMany(m => m.Value).ToList();
    }
}
