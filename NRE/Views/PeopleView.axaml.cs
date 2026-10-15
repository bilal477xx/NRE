using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using NRE.Helpers;
using NRE.Models;
using NRE.Models.Abstracts;
using NRE.Models.Enums;
using NRE.Models.Wrapper;

namespace NRE.Views;

public partial class PeopleView : UserControl
{
    private DataStore _store = null!;
    private Person? _currentPerson;

    public DataStore Store
    {
        get => _store;
        set
        {
            _store = value;
            LbPeople.ItemsSource = _store.PeopleManager.ToStringArray();
            _store.PeopleManager.ListChanged += RefreshViewData;
        }
    }

    public PeopleView()
    {
        InitializeComponent();
        CmbPersonType.ItemsSource = TypeDiscovery.GetConcreteSubclasses<Person>();
        CmbCountry.ItemsSource = Enum.GetValues<Countries>();
    }
    
    private async void BtnAdd_OnClick(object? sender, RoutedEventArgs e)
    {
        if (_currentPerson == null) return;
        if (!_currentPerson.Validate())
        {
            await Dialogs.ShowErrorAsync("Make sure all fields are filled properly.");
            return;
        }

        Store.PeopleManager.Add(_currentPerson);
        ResetForm();
    }
    
    private async void BtnEdit_OnClick(object? sender, RoutedEventArgs e)
    {
        int index = LbPeople.SelectedIndex;
        if (!_store.PeopleManager.CheckIndex(index) || _currentPerson == null)
        {
            await Dialogs.ShowErrorAsync("No person selected.");
            return;
        }
        if (!_currentPerson.Validate())
        {
            await Dialogs.ShowErrorAsync("Make sure all fields are filled properly.");
            return;
        }
        

        // Re-assigning the item makes the list box redraw it with the edited values.
        Store.PeopleManager.ChangeAt(index, _currentPerson);
        ResetForm();
    }

    private async void BtnRemove_OnClick(object? sender, RoutedEventArgs e)
    {
        int index = LbPeople.SelectedIndex;
        if (!_store.PeopleManager.CheckIndex(index) )
        {
            await Dialogs.ShowErrorAsync("No person selected.");
            return;
        }

        Store.PeopleManager.DeleteAt(index);
        ResetForm();
    }
    
    private void LbPeople_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        int index = LbPeople.SelectedIndex;
        if (_store.PeopleManager.CheckIndex(index))
        {
            ShowPerson(_store.PeopleManager.GetAt(index));
            SetEditMode(true);
        }
        else
        {
            SetEditMode(false);
            ResetForm();
        }
    }
    
    private void CmbPersonType_OnSelectionChanged(object? sender, SelectionChangedEventArgs e) => ResetForm();
    
    /// <summary>
    /// Starts a new, empty person of the type selected in the type combo box.
    /// </summary>
    private void ResetForm()
    {
        if (CmbPersonType.SelectedItem is not TypeDisplayWrapper selectedType) return;

        ShowPerson((Person)Activator.CreateInstance(selectedType.ClassType)!);
    }
    
    private void ShowPerson(Person person)
    {
        _currentPerson = person;
        SpGeneralForm.DataContext = person;
        DynamicForm.Content = person;
    }

    private void SetEditMode(bool isEditing)
    {
        CmbPersonType.IsEnabled = !isEditing;
        SpEditButtons.IsVisible = isEditing;
        BtnAdd.IsVisible = !isEditing;
    }

    private void RefreshViewData()
    {
        var selected = LbPeople.SelectedItem;
        LbPeople.ItemsSource = null;
        LbPeople.ItemsSource = _store.PeopleManager.ToStringArray();
        LbPeople.SelectedItem = selected;
    }
    
}
