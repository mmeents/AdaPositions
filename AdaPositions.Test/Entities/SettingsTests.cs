using Microsoft.VisualStudio.TestTools.UnitTesting;
using AdaPositions.Core.Entities;
using System.Collections.Generic;

namespace AdaPositions.Test.Entities
{
    [TestClass]
    public class SettingsTests
    {
        [TestMethod]
        public void TestAddAndRetrieveSetting()
        {
            // Arrange
            var settings = new Settings();
            var setting = new SettingProperty { Key = "TestKey", Value = "TestValue" };

            // Act
            settings["TestKey"] = setting;
            var retrievedSetting = settings["TestKey"];

            // Assert
            Assert.IsNotNull(retrievedSetting);
            Assert.AreEqual("TestKey", retrievedSetting.Key);
            Assert.AreEqual("TestValue", retrievedSetting.Value);
        }

        [TestMethod]
        public void TestUpdateSetting()
        {
            // Arrange
            var settings = new Settings();
            var setting = new SettingProperty { Key = "TestKey", Value = "TestValue" };
            settings["TestKey"] = setting;

            // Act
            var updatedSetting = new SettingProperty { Key = "TestKey", Value = "UpdatedValue" };
            settings["TestKey"] = updatedSetting;
            var retrievedSetting = settings["TestKey"];

            // Assert
            Assert.IsNotNull(retrievedSetting);
            Assert.AreEqual("TestKey", retrievedSetting.Key);
            Assert.AreEqual("UpdatedValue", retrievedSetting.Value);
        }

        [TestMethod]
        public void TestRemoveSetting()
        {
            // Arrange
            var settings = new Settings();
            var setting = new SettingProperty { Key = "TestKey", Value = "TestValue" };
            settings["TestKey"] = setting;

            // Act
            settings.Remove("TestKey");
            var containsKey = settings.Contains("TestKey");

            // Assert
            Assert.IsFalse(containsKey);
        }

        [TestMethod]
        public void TestCloneSettings()
        {
            // Arrange
            var settings = new Settings();
            var setting = new SettingProperty { Key = "TestKey", Value = "TestValue" };
            settings["TestKey"] = setting;

            // Act
            var clonedSettings = settings.Clone();
            var retrievedSetting = clonedSettings["TestKey"];

            // Assert
            Assert.IsNotNull(retrievedSetting);
            Assert.AreEqual("TestKey", retrievedSetting.Key);
            Assert.AreEqual("TestValue", retrievedSetting.Value);
        }

        [TestMethod]
        public void TestAsListProperty()
        {
            // Arrange
            var settings = new Settings();
            var setting1 = new SettingProperty { Key = "Key1", Value = "Value1" };
            var setting2 = new SettingProperty { Key = "Key2", Value = "Value2" };
            settings["Key1"] = setting1;
            settings["Key2"] = setting2;

            // Act
            var asList = settings.AsList;

            // Assert
            Assert.AreEqual(2, asList.Count);
            Assert.IsTrue(asList.Contains(setting1));
            Assert.IsTrue(asList.Contains(setting2));
        }

        [TestMethod]
        public void TestInitializeWithCollection()
        {
            // Arrange
            var setting1 = new SettingProperty { Key = "Key1", Value = "Value1" };
            var setting2 = new SettingProperty { Key = "Key2", Value = "Value2" };
            var initialList = new List<SettingProperty> { setting1, setting2 };

            // Act
            var settings = new Settings(initialList);
            var asList = settings.AsList;

            // Assert
            Assert.AreEqual(2, asList.Count);
            Assert.IsTrue(asList.Contains(setting1));
            Assert.IsTrue(asList.Contains(setting2));
        }
    }
}
