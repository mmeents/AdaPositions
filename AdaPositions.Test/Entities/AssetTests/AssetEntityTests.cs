using Microsoft.VisualStudio.TestTools.UnitTesting;
using AdaPositions.Core.Entities.Assets;

namespace AdaPositions.Test.Entities.Assets
{
    [TestClass]
    public class AssetEntityTests
    {
        [TestMethod]
        public void Constructor_Default_ShouldInitializePropertiesToDefaultValues()
        {
            // Arrange & Act
            var assetEntity = new AssetEntity();

            // Assert
            Assert.AreEqual(0, assetEntity.Id);
            Assert.AreEqual("", assetEntity.Asset);
            Assert.AreEqual("", assetEntity.PolicyId);
            Assert.AreEqual("", assetEntity.AssetName);
            Assert.AreEqual("", assetEntity.FingerPrint);
            Assert.AreEqual("", assetEntity.Quantity);
            Assert.AreEqual(0, assetEntity.Decimals);
            Assert.AreEqual("", assetEntity.Name);
            Assert.AreEqual("", assetEntity.Ticker);
            Assert.AreEqual("", assetEntity.Desc);
            Assert.AreEqual("", assetEntity.Url);
            Assert.AreEqual("", assetEntity.Logo);
        }

        [TestMethod]
        public void Constructor_WithId_ShouldInitializeIdCorrectly()
        {
            // Arrange
            long expectedId = 123;

            // Act
            var assetEntity = new AssetEntity(expectedId);

            // Assert
            Assert.AreEqual(expectedId, assetEntity.Id);
        }

        [TestMethod]
        public void Properties_ShouldGetAndSetValuesCorrectly()
        {
            // Arrange
            var assetEntity = new AssetEntity();
            long expectedId = 456;
            string expectedAsset = "Asset1";
            string expectedPolicyId = "Policy1";
            string expectedAssetName = "AssetName1";
            string expectedFingerPrint = "FingerPrint1";
            string expectedQuantity = "1000";
            int expectedDecimals = 2;
            string expectedName = "Name1";
            string expectedTicker = "Ticker1";
            string expectedDesc = "Description1";
            string expectedUrl = "http://example.com";
            string expectedLogo = "Logo1";

            // Act
            assetEntity.Id = expectedId;
            assetEntity.Asset = expectedAsset;
            assetEntity.PolicyId = expectedPolicyId;
            assetEntity.AssetName = expectedAssetName;
            assetEntity.FingerPrint = expectedFingerPrint;
            assetEntity.Quantity = expectedQuantity;
            assetEntity.Decimals = expectedDecimals;
            assetEntity.Name = expectedName;
            assetEntity.Ticker = expectedTicker;
            assetEntity.Desc = expectedDesc;
            assetEntity.Url = expectedUrl;
            assetEntity.Logo = expectedLogo;

            // Assert
            Assert.AreEqual(expectedId, assetEntity.Id);
            Assert.AreEqual(expectedAsset, assetEntity.Asset);
            Assert.AreEqual(expectedPolicyId, assetEntity.PolicyId);
            Assert.AreEqual(expectedAssetName, assetEntity.AssetName);
            Assert.AreEqual(expectedFingerPrint, assetEntity.FingerPrint);
            Assert.AreEqual(expectedQuantity, assetEntity.Quantity);
            Assert.AreEqual(expectedDecimals, assetEntity.Decimals);
            Assert.AreEqual(expectedName, assetEntity.Name);
            Assert.AreEqual(expectedTicker, assetEntity.Ticker);
            Assert.AreEqual(expectedDesc, assetEntity.Desc);
            Assert.AreEqual(expectedUrl, assetEntity.Url);
            Assert.AreEqual(expectedLogo, assetEntity.Logo);
        }
    }
}
