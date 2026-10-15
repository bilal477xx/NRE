using System;
using System.Collections.Generic;
using NRE.Collections;
using NRE.Models.Abstracts;

namespace NRE.Managers;

public class EstateManager : ListManager<Estate>
{

    private Dictionary<string, List<Estate>> GroupByCity()
    {
        var groups = new Dictionary<string, List<Estate>>(StringComparer.OrdinalIgnoreCase);

        foreach (Estate estate in GetAll())
        {
            string city = estate.Address.City.Trim();

            if (!groups.TryGetValue(city, out var estatesInCity))
            {
                estatesInCity = new List<Estate>();
                groups[city] = estatesInCity;
            }
            estatesInCity.Add(estate);
        }
        
        return groups;
    }

    public Dictionary<string, List<Estate>>  GetByCity(string searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
            throw new ArgumentException("City must not be empty.", nameof(searchText));
        
        var matches = new Dictionary<string, List<Estate>>(StringComparer.OrdinalIgnoreCase);

        foreach (var (city, estates) in GroupByCity())
        {
            if (city.Contains(searchText.Trim(), StringComparison.OrdinalIgnoreCase))
                matches.Add(city, estates);
        }

        return matches;
    }
}