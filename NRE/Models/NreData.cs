using System.Collections.Generic;
using NRE.Models.Abstracts;
using NRE.Models.Entities;

namespace NRE.Models;

public class NreData
{
    public List<Estate> Estates { get; set; }
    public List<Person> People { get; set; }
    public List<Transaction> Transactions { get; set; }
}