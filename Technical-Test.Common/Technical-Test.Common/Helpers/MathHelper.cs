namespace Technical_Test.Common.Helpers
{
    public static class MathHelper
    {
        public static decimal Fractional(decimal value)
        {
            return value - Math.Truncate(value);
        }
    }
}
