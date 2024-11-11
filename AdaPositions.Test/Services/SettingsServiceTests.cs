using Microsoft.VisualStudio.TestTools.UnitTesting;
using AdaPositions.Core.Entities;
using AdaPositions.Core.Interfaces;
using AdaPositions.Core.Services;
using AdaPositions.Core.Extensions;
using Moq;
using MessagePack;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AdaPositions.Test.Services
{
    [TestClass]
    public class SettingsServiceTests
    {
        private Mock<ILogMsg> _mockLogMsg;
        private string _testFileName;
        private SettingsService _settingsService;

        [TestInitialize]
        public void Setup()
        {
            _mockLogMsg = new Mock<ILogMsg>();
            _testFileName = FileExts.SettingsTestFileName;
            _settingsService = new SettingsService(_testFileName, _mockLogMsg.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(_testFileName))
            {
                File.Delete(_testFileName);
            }
        }

        [TestMethod]
        public void TestLoadSettings_FileExists()
        {
            // Arrange
            var settingsPackage = new SettingsPackage
            {
                Name = _testFileName,
                SettingsList = new List<SettingProperty>
                {
                    new SettingProperty { Key = "TestKey", Value = "TestValue" }
                }
            };
            var encoded = Convert.ToBase64String(MessagePackSerializer.Serialize(settingsPackage));
            File.WriteAllText(_testFileName, encoded);

            // Act
            _settingsService.Load();

            // Assert
            Assert.IsTrue(_settingsService.FileLoaded);
            Assert.AreEqual("TestValue", _settingsService.Settings["TestKey"].Value);
        }

        [TestMethod]
        public async Task TestLoadSettingsAsync_FileExists()
        {
            // Arrange
            var settingsPackage = new SettingsPackage
            {
                Name = _testFileName,
                SettingsList = new List<SettingProperty>
                {
                    new SettingProperty { Key = "TestKey", Value = "TestValue" }
                }
            };
            var encoded = Convert.ToBase64String(MessagePackSerializer.Serialize(settingsPackage));
            await File.WriteAllTextAsync(_testFileName, encoded);

            // Act
            await _settingsService.LoadAsync();

            // Assert
            Assert.IsTrue(_settingsService.FileLoaded);
            Assert.AreEqual("TestValue", _settingsService.Settings["TestKey"].Value);
        }

        [TestMethod]
        public void TestSaveSettings()
        {
            // Arrange
            var settings = new Settings
            {
                ["TestKey"] = new SettingProperty { Key = "TestKey", Value = "TestValue" }
            };
            _settingsService.Settings = settings;

            // Act
            _settingsService.Save();

            // Assert
            Assert.IsTrue(File.Exists(_testFileName));
            var encoded = File.ReadAllText(_testFileName);
            var decoded = Convert.FromBase64String(encoded.Replace('?', '='));
            var loadedPackage = MessagePackSerializer.Deserialize<SettingsPackage>(decoded);
            Assert.AreEqual("TestValue", loadedPackage.SettingsList.First(s => s.Key == "TestKey").Value);
        }

        [TestMethod]
        public async Task TestSaveSettingsAsync()
        {
            // Arrange
            var settings = new Settings
            {
                ["TestKey"] = new SettingProperty { Key = "TestKey", Value = "TestValue" }
            };
            _settingsService.Settings = settings;

            // Act
            await _settingsService.SaveAsync();

            // Assert
            Assert.IsTrue(File.Exists(_testFileName));
            var encoded = await File.ReadAllTextAsync(_testFileName);
            var decoded = Convert.FromBase64String(encoded.Replace('?', '='));
            var loadedPackage = MessagePackSerializer.Deserialize<SettingsPackage>(decoded);
            Assert.AreEqual("TestValue", loadedPackage.SettingsList.First(s => s.Key == "TestKey").Value);
        }

        [TestMethod]
        public void TestSettingsProperty()
        {
            // Arrange
            var settings = new Settings
            {
                ["TestKey"] = new SettingProperty { Key = "TestKey", Value = "TestValue" }
            };

            // Act
            _settingsService.Settings = settings;
            var retrievedSettings = _settingsService.Settings;

            // Assert
            Assert.AreEqual("TestValue", retrievedSettings["TestKey"].Value);
        }
    }
}
