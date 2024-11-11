using AdaPositions.Core.Entities;
using AdaPositions.Core.Extensions;
using AdaPositions.Core.Interfaces;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPositions.Core.Services {

  public class SettingsService : ISettingsService {

    private readonly ILogMsg _form1;
    public string FileName { get; set; }

    private bool _FileLoaded = false;
    public bool FileLoaded { get { return _FileLoaded; } }
    public SettingsPackage Package { get; set; }
    public SettingsService(string fileName, ILogMsg form1) {
      _form1 = form1;
      FileName = fileName;

      Package = new SettingsPackage {
        Name = FileName
      };
      if (File.Exists(FileName)) {
        Load();
      }
    }

    private Settings GetSettings() {
      return new Settings(Package.SettingsList);
    }

    private void SetSettings(Settings value) {
      Package.SettingsList = value.AsList;
    }

    public Settings Settings { get { return GetSettings(); } set { SetSettings(value); } }

    public void Load() {
      Task.Run(async () => await this.LoadAsync().ConfigureAwait(false)).GetAwaiter().GetResult();
    }
    public async Task LoadAsync() {
      if (File.Exists(FileName)) {
        var mark = DateTime.Now;
        var encoded = await FileName.ReadAllTextAsync();
        var decoded = Convert.FromBase64String(encoded.Replace('?', '='));
        this.Package = MessagePackSerializer.Deserialize<SettingsPackage>(decoded);
        _FileLoaded = true;
        var finish = DateTime.Now;
        var diff = (finish - mark).TotalMilliseconds;
        _form1.LogMsg($"{DateTime.Now} {diff}ms loaded: {FileName}");
      } else {
        _form1.LogMsg($"{DateTime.Now} ms skipped(no file): {FileName}");
      }
    }

    public void Save() {
      Task.Run(async () => await this.SaveAsync().ConfigureAwait(false)).GetAwaiter().GetResult();
    }
    public async Task SaveAsync() {
      var mark = DateTime.Now;
      byte[] WirePacked = MessagePackSerializer.Serialize(this.Package);
      string encoded = Convert.ToBase64String(WirePacked);
      await encoded.WriteAllTextAsync(FileName);
      var finish = DateTime.Now;
      var diff = (finish - mark).TotalMilliseconds;
      _form1.LogMsg($"{DateTime.Now} {diff}ms Saved: {FileName}");
    }


  }
}
