using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaPositions.Core.Services;
using AdaPositions.Core.Interfaces;
using AdaPositions.Core.Entities;
using AdaPositions.Core.Extensions;
using AdaPositions.Test.Entities;

namespace AdaPositions.Test.Integration {

  [TestClass]
  public class ConfigureIntegrationSettings
  {   
    private SettingsService _settingsService;

    public ConfigureIntegrationSettings() {

      var MsgLog = new LogMsg();
      _settingsService = new SettingsService(FileExts.SettingsTestFileName, MsgLog);

    }


    [TestMethod]
    public void LoadApiKeyIntoSettings() {               // To Configure your Blockfrost API key for the Integration Tests  
      var settings = _settingsService.Settings;
      settings[SettingCol.BlockFrostApiKey].Value = "";  // <-  Add your Blockfrost API key to left in quotes here. 
      _settingsService.Settings = settings;
      // _settingsService.Save();                        // Uncomment out lines to left to save key to test settings.     
    }


    [TestMethod]
    public void VerifyApiKeyInSettings() {
                                                         // Use this test to Verify API key is loaded in settings.
      var settings = _settingsService.Settings;   
      var apiKey = settings[SettingCol.BlockFrostApiKey].Value;
      Console.WriteLine(apiKey);                        

      Assert.IsTrue(apiKey.Length > 0);                 //  Remove you API key from above so that you dont check it in.
    }



  }
}
