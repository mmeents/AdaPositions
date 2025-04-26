using AdaPositions.Core.Entities;
using AdaPositions.Core.Extensions;
using AdaPositions.Core.Services;
using AdaPositions.Test.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPositions.Test.Integration {

  [TestClass]
  public class AssetsTests {
    private SettingsService _settingsService;
    private BlockfrostService _blockfrostService;

    public AssetsTests() {
      var Msglog = new LogMsg();
      _settingsService = new SettingsService(FileExts.SettingsTestFileName, Msglog);
      string ApiKey = _settingsService.Settings[SettingCol.BlockFrostApiKey].Value;
      _blockfrostService = new BlockfrostService(ApiKey, Msglog);
    }


    [TestMethod]
    public async Task LoadAssetsTest() {

      var assetsService = new AssetsService(_blockfrostService);
      await assetsService.LoadAssets();

      var asset = "008977011abb1d15bbba595c0418307f4cfc7bbc707b17522db4e7c44d59484d";
      var assetDetails = await _blockfrostService.GetAssetsAsync(asset).ConfigureAwait(false);

      Assert.IsNotNull(assetDetails);

      var assetEntity = assetsService.AddAsset(assetDetails);

      await assetsService.SaveAssetsAsync();

      var assetsService2 = new AssetsService(_blockfrostService);
      await assetsService2.LoadAssets();

      Assert.IsTrue(assetsService2.Assets.Count == 1);
      var assetTest = assetsService2.Assets.FindByTicker("MYHM");

      Assert.IsNotNull(assetTest);
    }

  }

}
