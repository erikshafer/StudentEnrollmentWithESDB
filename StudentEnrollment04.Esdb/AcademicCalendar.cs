namespace StudentEnrollment04.Esdb;

public static class AcademicCalendar
{
    public static string Create(AcademicYear year, AcademicSemester semester)
    {
        if (year is AcademicYear.Unset)
            throw new ArgumentOutOfRangeException(nameof(year));
        
        if (semester is AcademicSemester.Unset)
            throw new ArgumentOutOfRangeException(nameof(semester));

        var schoolYear = (int)year;
        var schoolSemester = AcademicSemesterValues.Translate(semester);
        
        return $"{schoolYear}{schoolSemester}";
    }
}

public static class AcademicSemesterValues
{
    public const string FALL = "FALL";
    public const string SPRING = "SPRING";
    public const string SUMMER = "SUMMER";

    public static string Translate(AcademicSemester semester)
    {
        switch (semester)
        {
            case AcademicSemester.Fall:
                return FALL;
            case AcademicSemester.Spring:
                return SPRING;
            case AcademicSemester.Summer:
                return SUMMER;
            case AcademicSemester.Unset:
            default:
                throw new ArgumentOutOfRangeException(nameof(semester), semester, null);
        }
    }
}

public enum AcademicYear
{
    Unset = 0,
    Y2022 = 2022,
    Y2023 = 2023,
    Y2024 = 2024,
    Y2025 = 2025,
    Y2026 = 2026,
    Y2027 = 2027
}

public enum AcademicSemester
{
    Unset = 0,
    Fall = 1,
    Spring = 2,
    Summer = 3
}