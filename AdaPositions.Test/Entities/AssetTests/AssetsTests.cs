using Microsoft.VisualStudio.TestTools.UnitTesting;
using AdaPositions.Core.Entities.Assets;

namespace AdaPositions.Test.Entities.Assets 
{ 

  [TestClass]
  public class AssetsTests
  {
      // Existing code...

      [TestMethod]
      public void AddAsset_ShouldAddAssetWithNewId()
      {
          // Arrange
          var assets = new Core.Entities.Assets.Assets();
          var asset = new AssetEntity();

          // Act
          var addedAsset = assets.AddAsset(asset);

          // Assert
          Assert.AreEqual(1, addedAsset.Id);
          Assert.AreEqual(1, assets.Count());
          Assert.IsTrue(assets.ContainsKey(1));
      }

      [TestMethod]
      public void AddAsset_ShouldAddAssetWithExistingId()
      {
          // Arrange
          var assets = new Core.Entities.Assets.Assets();
          var asset = new AssetEntity(2);

          // Act
          var addedAsset = assets.AddAsset(asset);

          // Assert
          Assert.AreEqual(2, addedAsset.Id);
          Assert.AreEqual(1, assets.Count());
          Assert.IsTrue(assets.ContainsKey(2));
      }

      [TestMethod]
      public void RemoveAsset_ShouldRemoveAssetById()
      {
          // Arrange
          var assets = new Core.Entities.Assets.Assets();
          var asset = new AssetEntity();
          assets.AddAsset(asset);

          // Act
          assets.RemoveAsset(1);

          // Assert
          Assert.AreEqual(0, assets.Count());
          Assert.IsFalse(assets.ContainsKey(1));
      }
  }
}