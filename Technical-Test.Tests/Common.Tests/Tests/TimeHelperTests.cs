using Technical_Test.Common.Helpers;

namespace Common.Tests
{
    [TestClass]
    public class TimeHelperTests
    {
        [TestMethod]
        public void HoursFractionalToHoursMinute_ShouldBeTrue_ValidateValue() => Assert.IsTrue(TimeHelper.HoursFractionalToHoursMinutes(23.5m) == "23:30");

        [TestMethod]
        public void HoursFractionalToHoursMinute_ShouldBeFalse_ValidateValue() => Assert.IsFalse(TimeHelper.HoursFractionalToHoursMinutes(0.0m) == "1:00");
    }
}
