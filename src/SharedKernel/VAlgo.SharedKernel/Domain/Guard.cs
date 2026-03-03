namespace VAlgo.SharedKernel.Domain;

public static class Guard
{
    public static void AgainstNull(object? value, string name)
    {
        if (value is null)
            throw new ArgumentNullException(name);
    }

    public static void AgainstNullOrEmpty(string? value, string name)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{name} cannot be null or empty", name);
    }

    public static void AgainstOutOfRange(
        int value,
        int min,
        int max,
        string name)
    {
        if (value < min || value > max)
            throw new ArgumentOutOfRangeException(name, value,
                $"{name} must be between {min} and {max}");
    }
}
