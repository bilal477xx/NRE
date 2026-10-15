using System;
using System.Linq;
using NRE.Managers;
using NRE.Models.Abstracts;
using NRE.Models.Entities;


namespace NRE.Models;

[Serializable]
public class DataStore
{
    public EstateManager EstatesManager { get; set; } = new();
    public PeopleManager PeopleManager { get; set; } = new();
    public TransactionsManager TransactionsManager { get; set; } = new();

    public NreData ToData() => new()
    {
        Estates =  EstatesManager.GetAll().ToList(),
        People =  PeopleManager.GetAll().ToList(),
        Transactions =  TransactionsManager.GetAll().ToList(),
    };

    public void LoadFrom(NreData data)
    {
        EstatesManager.DeleteAll();
        foreach (Estate estate in data.Estates)
        {
            EstatesManager.Add(estate);
        }
        
        PeopleManager.DeleteAll();
        foreach (Person person in data.People)
        {
            PeopleManager.Add(person);
        }

        TransactionsManager.DeleteAll();
        foreach (Transaction transaction in data.Transactions)
        {
            TransactionsManager.Add(transaction);
        }
    }
    
    public void ClearData()
    {
        EstatesManager.DeleteAll();
        PeopleManager.DeleteAll();
        TransactionsManager.DeleteAll();
    }
}

