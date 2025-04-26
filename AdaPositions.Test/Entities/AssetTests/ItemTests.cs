using Microsoft.VisualStudio.TestTools.UnitTesting;
using AdaPositions.Core.Entities.Operations;

namespace AdaPositions.Test.Entities.Operations
{
    [TestClass]
    public class ItemTests
    {
        [TestMethod]
        public void Constructor_Default_ShouldInitializeIdToZero()
        {
            // Arrange & Act
            var item = new Item();

            // Assert
            Assert.AreEqual(0, item.Id);
        }

        [TestMethod]
        public void Constructor_WithId_ShouldInitializeIdCorrectly()
        {
            // Arrange
            long expectedId = 123;

            // Act
            var item = new Item(expectedId);

            // Assert
            Assert.AreEqual(expectedId, item.Id);
        }

        [TestMethod]
        public void Id_Property_ShouldGetAndSetIdCorrectly()
        {
            // Arrange
            var item = new Item();
            long expectedId = 456;

            // Act
            item.Id = expectedId;

            // Assert
            Assert.AreEqual(expectedId, item.Id);
        }
    }
}
