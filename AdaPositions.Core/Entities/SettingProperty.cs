using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPositions.Core.Entities {

  [MessagePackObject]
  public class SettingProperty {

    [Key(0)]
    public string Key { get; set; } = "";

    [Key(1)]
    public string Value { get; set; } = "";

  }

}
