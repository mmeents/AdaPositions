using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPositions.Core.Extensions {
  public static class FileExts {

    public const string CommonPathAdd = "\\PrompterFiles";
    public const string SettingsAdd = "\\AdaPositionsSettings.sft";
    public const string SettingsAddTest = "\\AdaPositionsSettingsTest.sft";
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

  }
}
