using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPositions.Core.Entities {

  [MessagePackObject]
  public class AddressModel {

    [Key(0)]
    public long Id { get; set; } = 0;

    [Key(1)]
    public long StakeAddressModelId { get; set; } = 0;

    [Key(2)]
    public string Address { get; set; } = "";

    [IgnoreMember]
    public Amounts AddressAmounts { get; set; } = new Amounts();

  }
}
