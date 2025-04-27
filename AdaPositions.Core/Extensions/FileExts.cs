using AdaPositions.Core.Entities.Assets;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPositions.Core.Extensions {
  public static class FileExts {

    public const string CommonPathAdd = "\\PrompterFiles";
    public const string SettingsAdd = "\\AdaPositionsSettings.sft";
    public const string SettingsAddTest = "\\AdaPositionsSettingsTest.sft";
    public const string AssetsAdd = "\\AdaPositionsAssets.msgpk";
    public const string StakeAddressAdd = "\\AdaPositionsStakeAddress.sft";
    public static string DefaultPath {
      get {
        var DefaultDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + FileExts.CommonPathAdd;
        if (!Directory.Exists(DefaultDir)) {
          Directory.CreateDirectory(DefaultDir);
        }
        return DefaultDir;
      }
    }
    public static string SettingsFileName { get { return DefaultPath + SettingsAdd; } }
    public static string SettingsTestFileName { get { return DefaultPath + SettingsAddTest; } }
    public static string AssetsFileName { get { return DefaultPath + AssetsAdd; } }
    public static string StakeAddressFileName { get { return DefaultPath + StakeAddressAdd; } }


    /// <summary>
    /// async read file from file system into string
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static async Task<string> ReadAllTextAsync(this string filePath) {
      using (var streamReader = new StreamReader(filePath)) {
        return await streamReader.ReadToEndAsync();
      }
    }

    /// <summary>
    /// async write content to fileName location on file system. 
    /// </summary>
    /// <param name="Content"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static async Task<int> WriteAllTextAsync(this string Content, string fileName) {
      using (var streamWriter = new StreamWriter(fileName)) {
        await streamWriter.WriteAsync(Content);
      }
      return 1;
    }

   

    public static string AsStr1P(this decimal x) {
      return String.Format(CultureInfo.InvariantCulture, "{0:0.0}", x);
    }
    


    public static long AsLong(this string s) {
      long result = 0;
      if (!long.TryParse(s, out result)) {
        if (decimal.TryParse(s, out decimal d)) {
          result = Convert.ToInt64(d);
        }
      }
      return result;
    }

    public static string SanitizeUnit(this string input) {
      if (string.IsNullOrEmpty(input)) return string.Empty;

      // Remove non-printable characters
      var sanitized = new string(input.Where(c => !char.IsControl(c)).ToArray());

      // Optionally, trim leading/trailing whitespace
      return sanitized.Trim();
    }

  }
}
