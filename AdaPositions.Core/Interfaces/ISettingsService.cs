using AdaPositions.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPositions.Core.Interfaces {
  public interface ISettingsService {
    string FileName { get; set; }
    bool FileLoaded { get; }
    SettingsPackage Package { get; set; }
    Settings Settings { get; set; }

    void Load();
    Task LoadAsync();
    void Save();
    Task SaveAsync();
  }

}
