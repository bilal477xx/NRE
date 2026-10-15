using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using NRE.Helpers;
using NRE.Models;
using NRE.Models.Abstracts;
using NRE.Models.Entities;
using NRE.Models.Entities.People;
using NRE.Models.Wrapper;

namespace NRE.Views;

public partial class PaymentsView : UserControl
{
    private readonly List<TypeDisplayWrapper> _paymentMethods = TypeDiscovery.GetConcreteSubclasses<Payment>();
    private DataStore _store = null!;
    private Transaction? _currentTransaction;

    public DataStore Store
    {
        get => _store;
        set
        {
            _store = value;
            LbTransactions.ItemsSource = _store.TransactionsManager.ToStringArray();
            CmbEstates.ItemsSource = _store.EstatesManager.ToStringArray();
            _store.TransactionsManager.ListChanged += UpdateTransactionsView;
            _store.PeopleManager.ListChanged += UpdatePeopleComboBoxes;
            _store.EstatesManager.ListChanged += UpdateEstatesComboBox;
            
            UpdatePeopleComboBoxes();
        }
    }

    public PaymentsView()
    {
        InitializeComponent();
        CmbPaymentMethods.ItemsSource = _paymentMethods;
        ResetForm();
    }
    
    private async void BtnAdd_OnClick(object? sender, RoutedEventArgs e)
    {
        if (_currentTransaction == null) return;
        if (!_currentTransaction.Validate())
        {
            await Dialogs.ShowAsync("Incomplete", "Select an estate, a buyer and a payment method.");
            return;
        }
        
        Store.TransactionsManager.Add(_currentTransaction);
        ResetForm();
    }
    
    private async void BtnEdit_OnClick(object? sender, RoutedEventArgs e)
    {
        int index = LbTransactions.SelectedIndex;
        if (!_store.TransactionsManager.CheckIndex(index) || _currentTransaction == null)
        {
            await Dialogs.ShowErrorAsync("No transaction selected.");
            return;
        }
        
        Store.TransactionsManager.ChangeAt(index, _currentTransaction);
        ResetForm();
    }

    private async void BtnRemove_OnClick(object? sender, RoutedEventArgs e)
    {
        int index = LbTransactions.SelectedIndex;
        if (!_store.TransactionsManager.CheckIndex(index) || _currentTransaction == null)
        {
            await Dialogs.ShowErrorAsync("No transaction selected.");
            return;
        }

        Store.TransactionsManager.DeleteAt(index);
        ResetForm();
    }
    
    /// <summary>
    /// Starts a new transaction, keeping the currently selected payment method.
    /// </summary>
    private void ResetForm()
    {
        _currentTransaction = new Transaction();

        if (CmbPaymentMethods.SelectedItem is TypeDisplayWrapper selectedMethod)
            _currentTransaction.Payment = (Payment)Activator.CreateInstance(selectedMethod.ClassType)!;

        SpGeneralForm.DataContext = _currentTransaction;
        ShowPayment(_currentTransaction.Payment);
        CmbSeller.IsEnabled = true;
        CmbBuyer.IsEnabled = true;
    }

    private void UpdateTransactionsView()
    {
        LbTransactions.ItemsSource = _store.TransactionsManager.ToStringArray();
        UpdatePeopleComboBoxes();
        UpdateEstatesComboBox();
    }

    private void UpdatePeopleComboBoxes()
    {
        var selectedSeller = CmbSeller.SelectedItem;
        var selectedBuyer = CmbBuyer.SelectedItem;

        CmbSeller.ItemsSource = _store.PeopleManager.GetAll().OfType<Seller>();
        CmbBuyer.ItemsSource = _store.PeopleManager.GetAll().OfType<Buyer>();

        CmbSeller.SelectedItem = selectedSeller;
        CmbBuyer.SelectedItem = selectedBuyer;
        
        SpGeneralForm.DataContext = _currentTransaction;
    }
    
    private void UpdateEstatesComboBox()
    {
        var selectedEstate = CmbEstates.SelectedItem;

        CmbEstates.ItemsSource = _store.EstatesManager.GetAll();

        CmbEstates.SelectedItem = selectedEstate;
    }


    private void ShowPayment(Payment? payment)
    {
        SpPaymentCommon.DataContext = payment;
        SpPaymentCommon.IsEnabled = payment != null;
        DynamicForm.Content = payment;
    }

    private void SetEditMode(bool isEditing)
    {
        SpEditButtons.IsVisible = isEditing;
        BtnAdd.IsVisible = !isEditing;
    }
    
    private void LbTransactions_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        int index = LbTransactions.SelectedIndex;
        if (Store.TransactionsManager.CheckIndex(index))
        {
            _currentTransaction = Store.TransactionsManager.GetAt(index);
            CmbPaymentMethods.SelectedItem = _paymentMethods
                .FirstOrDefault(m => m.ClassType == Store.TransactionsManager.GetAt(index).Payment?.GetType());
            SpGeneralForm.DataContext = _currentTransaction;
            ShowPayment(_currentTransaction.Payment);
            SetEditMode(true);
        }
        else
        {
            SetEditMode(false);
            ResetForm();
        }
    }

    /// <summary>
    /// Swaps the payment to the chosen method, carrying over amount and date.
    /// </summary>
    private void CmbPaymentMethods_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (_currentTransaction == null ||
            CmbPaymentMethods.SelectedItem is not TypeDisplayWrapper selectedMethod) return;

        var oldPayment = _currentTransaction.Payment;
        if (oldPayment?.GetType() == selectedMethod.ClassType) return;

        var newPayment = (Payment)Activator.CreateInstance(selectedMethod.ClassType)!;
        if (oldPayment != null)
        {
            newPayment.Amount = oldPayment.Amount;
            newPayment.Date = oldPayment.Date;
        }

        _currentTransaction.Payment = newPayment;
        ShowPayment(newPayment);
    }

    /// <summary>
    /// Pre-fills and locks seller/buyer if the chosen estate already has them.
    /// </summary>
    private void CmbEstates_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (CmbEstates.SelectedItem is not Estate selectedEstate) return;

        CmbSeller.IsEnabled = selectedEstate.Seller == null;
        if (selectedEstate.Seller != null)
            CmbSeller.SelectedItem = selectedEstate.Seller;

        CmbBuyer.IsEnabled = selectedEstate.Buyer == null;
        if (selectedEstate.Buyer != null)
            CmbBuyer.SelectedItem = selectedEstate.Buyer;
    }
}
