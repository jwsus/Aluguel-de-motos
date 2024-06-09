[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
sealed class RentalPlanAttribute : Attribute
{
    public int DurationInDays { get; }
    public decimal DailyRate { get; }

    public RentalPlanAttribute(int durationInDays, decimal dailyRate)
    {
        DurationInDays = durationInDays;
        DailyRate = dailyRate;
    }
}
