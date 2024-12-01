using EnumCentricStatusManagement.Core;
using Xunit;

namespace EnumCentricStatusManagement.Tests
{
    public enum testStatus
    {
        [Status("New Record Created.", StatusType.Success)]
        NewRecord,

        [Status("Registration Updated.", StatusType.Success)]
        UpdatedRecord,

        [Status("Record Deleted.", StatusType.Success)]
        DeletedRecord,

        [Status("User Information Could Not Be Verified", StatusType.Error)]
        UserInformationCouldNotBeVerified,
    }

    public class CustomModel
    {
        public testStatus Status { get; set; }
    }

    public class EnumExtensionsTests
    {
        [Theory]
        [InlineData(testStatus.NewRecord, "New Record Created.", StatusType.Success)]
        [InlineData(testStatus.UpdatedRecord, "Registration Updated.", StatusType.Success)]
        [InlineData(testStatus.DeletedRecord, "Record Deleted.", StatusType.Success)]
        [InlineData(testStatus.UserInformationCouldNotBeVerified, "User Information Could Not Be Verified", StatusType.Error)]
        public void TestStatusAttributes(testStatus status, string expectedMessage, StatusType expectedType)
        {
            // Arrange
            var getEnumStatus = status.GetEnumStatus();

            // Act & Assert
            Assert.NotNull(getEnumStatus);
            Assert.Equal(expectedMessage, getEnumStatus.Message);
            Assert.Equal(expectedType, getEnumStatus.Type);
        }
    }
}
