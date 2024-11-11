using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPositions.Core.Entities {

  [MessagePackObject]
  public class SettingsPackage {

    [Key(0)]
    public string Name { get; set; } = "";

    [Key(1)]
    public ICollection<SettingProperty> SettingsList { get; set; } = new List<SettingProperty>();

  }

}
