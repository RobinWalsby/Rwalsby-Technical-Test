using Technical_Test.Domain.ValueObjects;

namespace Domain.Tests.TimesheetValueObjects
{
    [TestClass]
    public class TimesheetValueObjectTests
    {
        [TestMethod]
        public void TimesheetHours_ShouldNotThrow_IfZero() => Assert.IsTrue(TimesheetHours.Create(0) == TimesheetHours.Create(0));

        [TestMethod]
        public void TimesheetHours_ShouldNotThrow_IfEmptyString() => Assert.IsTrue(TimesheetHours.Empty == TimesheetHours.Create(string.Empty));

        [TestMethod]
        public void TimesheetHours_ShouldThrowArgumentException_IfInvalidFractional() => Assert.ThrowsException<ArgumentException>(() => TimesheetHours.Create(6.3m));

        [TestMethod]
        public void TimesheetHours_ShouldThrowArgumentException_IfInvalidHoursCount() => Assert.ThrowsException<ArgumentException>(() => TimesheetHours.Create(25m));

        [TestMethod]
        public void TimesheetHours_ShouldNotThrow_IfValidFractional()
        {
            const decimal VALID_VALUE = 7.5m;

            Assert.AreEqual(VALID_VALUE, TimesheetHours.Create(VALID_VALUE).Value);
        }

        [TestMethod]
        public void TimesheetHours_ShouldBeTrue_EqualityOperator() => Assert.IsTrue(TimesheetHours.Create(5) == TimesheetHours.Create(5));


        [TestMethod]
        public void TimesheetDate_ShouldThrowArgumentException_IfTooFarInfuture() => Assert.ThrowsException<ArgumentException>(() => TimesheetDate.Create(DateOnly.FromDateTime(DateTime.Now.Date.AddYears(2).AddDays(1))));

        [TestMethod]
        public void TimesheetDate_ShouldNotThrow_IfValid()
        {
            DateOnly VALID_VALUE = DateOnly.FromDateTime(DateTime.Now);

            Assert.AreEqual(VALID_VALUE, TimesheetDate.Create(VALID_VALUE).Value);
        }
    }
}
