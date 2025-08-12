using System;

namespace UserManagement.Services.Extensions;

public static class DateTimeExtensions
{
    public static bool IsWithinDatePeriod(this DateTime dateTime,DateOnly startDate, DateOnly endDate)
    {
        var dateonlyConversion = DateOnly.FromDateTime(dateTime);
        return dateonlyConversion >= startDate & dateonlyConversion <= endDate;
    }
}
