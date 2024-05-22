namespace StudentEnrollment04.Esdb;

public record CourseCode
{
    public string Value { get; internal init; } = string.Empty;

    internal CourseCode()
    {
    }

    public CourseCode(string value)
    {
        const int min = 4;
        const int max = 12;
        
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(value),
                "Cannot accept a null or empty value for an enrolled course");
        
        if (value.Length <= min)
            throw new ArgumentNullException(nameof(value),
                $"Enrolled course code must be greater than {min} characters");
        
        if (value.Length > max)
            throw new ArgumentNullException(nameof(value),
                $"Enrolled course code must be no more than {max} characters");

        Value = value;
    }
    
    public static implicit operator string(CourseCode courseCode)
        => courseCode.Value;
    
    public bool HasSameValue(string another)
        => string.Compare(Value, another, StringComparison.CurrentCulture) != 0;
}