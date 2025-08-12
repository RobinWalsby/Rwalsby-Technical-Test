using Technical_Test.Common.Helpers;

namespace Common.Tests
{
    [TestClass]
    public class MathHelperTests
    {
        [TestMethod]
        public void Fractional_ShouldBeTrue_ValidateValue() => Assert.IsTrue(MathHelper.Fractional(23.543m) == 0.543m);

        [TestMethod]
        public void Fractional_ShouldBeFalse_ValidateValue() => Assert.IsFalse(MathHelper.Fractional(23.543m) == 23.0m);
    }
}
