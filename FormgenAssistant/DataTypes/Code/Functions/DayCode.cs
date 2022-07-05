﻿namespace FormgenAssistant.DataTypes.Code.Functions
{
    internal class DayCode : CodeBase
    {
        public DayCode()
        {
            Name = "Day";
            Prefix = "DAY";
            Description = "Extract the numeric day from a date";
            AddInput("Date Field");
        }
    }
}
