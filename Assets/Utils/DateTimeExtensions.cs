using System;


public static class DateTimeExtensions
{
    public static uint ToTimeStamp(this DateTime dateTime){
        var timespan2 = dateTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return (uint)timespan2.TotalSeconds;
    } 
}

