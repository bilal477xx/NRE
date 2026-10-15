using System;

namespace NRE.Helpers;

public static class Validation
{
    public static int EnsureNotNegative(int value, string propertyName)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(propertyName, $"{propertyName} cannot be negative.");
        return value;
    }

    public static double EnsureNotNegative(double value, string propertyName)
    {
        if (value < 0.0)
            throw new ArgumentOutOfRangeException(propertyName, $"{propertyName} cannot be negative.");
        return value;
    }
}
