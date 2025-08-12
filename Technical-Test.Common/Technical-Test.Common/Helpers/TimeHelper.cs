namespace Technical_Test.Common.Helpers
{
    public static class TimeHelper
    {
        public static string HoursFractionalToHoursMinutes(decimal value)
        {
            var completeHours = Math.Truncate(value);
            var completeMinutes = MathHelper.Fractional(value) * 60;

            return $"{(int)completeHours}:{(int)completeMinutes:00}";
        }
    }
}
