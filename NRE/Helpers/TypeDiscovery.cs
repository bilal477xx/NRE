using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using NRE.Models.Wrapper;

namespace NRE.Helpers;

public static class TypeDiscovery
{
    /// <summary>
    /// Finds every concrete subclass of <typeparamref name="TBase"/> in its assembly,
    /// paired with the name from its [Description] attribute (or the class name).
    /// </summary>
    public static List<TypeDisplayWrapper> GetConcreteSubclasses<TBase>() =>
        typeof(TBase).Assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(TBase)))
            .Select(t => new TypeDisplayWrapper
            {
                ClassType = t,
                DisplayName = GetDisplayName(t),
            })
            .ToList();

    private static string GetDisplayName(Type type) =>
        type.GetCustomAttribute<DescriptionAttribute>()?.Description ?? type.Name;
}
