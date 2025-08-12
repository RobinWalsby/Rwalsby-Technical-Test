using Technical_Test.Domain.ValueObjects;

namespace Domain.Tests.IdValueObjects
{
    [TestClass]
    public class IdValueObjectTests
    {
        [TestMethod]
        public void UserId_ShouldThrowArgumentException_IfEmpty() => Assert.ThrowsException<ArgumentException>(() => UserId.Create(Guid.Empty));

        [TestMethod]
        public void UserId_ShouldNotThrow_IfValid()
        {
            Guid VALID_USER_ID = Guid.NewGuid();

            Assert.AreEqual(VALID_USER_ID, UserId.Create(VALID_USER_ID).Value);
        }

        [TestMethod]
        public void UserId_ShouldBeTrue_EqualityOperator() => Assert.IsTrue(UserId.Create(Guid.Parse("7a565017-2dcc-460f-92bf-59c8d051baba")) == UserId.Create(Guid.Parse("7a565017-2dcc-460f-92bf-59c8d051baba")));


        [TestMethod]
        public void ProjectId_ShouldThrowArgumentException_IfEmpty() => Assert.ThrowsException<ArgumentException>(() => ProjectId.Create(Guid.Empty));

        [TestMethod]
        public void ProjectId_ShouldBeTrue_EqualityOperator() => Assert.IsTrue(ProjectId.Create(Guid.Parse("aa935873-b876-45be-9d1b-5c73187d03c9")) == ProjectId.Create(Guid.Parse("aa935873-b876-45be-9d1b-5c73187d03c9")));

        [TestMethod]
        public void ProjectId_ShouldNotThrow_IfValid()
        {
            Guid VALID_PROJECT_ID = Guid.NewGuid();

            Assert.AreEqual(VALID_PROJECT_ID, ProjectId.Create(VALID_PROJECT_ID).Value);
        }

        [TestMethod]
        public void TimesheetId_ShouldThrowArgumentException_IfEmpty() => Assert.ThrowsException<ArgumentException>(() => TimesheetId.Create(Guid.Empty));

        [TestMethod]
        public void TimesheetId_ShouldBeTrue_EqualityOperator() => Assert.IsTrue(TimesheetId.Create(Guid.Parse("9cd7da70-7d50-4b3c-83d7-51d0c95c79a0")) == TimesheetId.Create(Guid.Parse("9cd7da70-7d50-4b3c-83d7-51d0c95c79a0")));

        [TestMethod]
        public void TimesheetId_ShouldNotThrow_IfValid()
        {
            Guid VALID_PROJECT_ID = Guid.NewGuid();

            Assert.AreEqual(VALID_PROJECT_ID, TimesheetId.Create(VALID_PROJECT_ID).Value);
        }
    }
}
