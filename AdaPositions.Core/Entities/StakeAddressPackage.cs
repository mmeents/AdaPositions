using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPositions.Core.Entities {

  [MessagePackObject]
  public class StakeAddressPackage {

    [Key(0)]
    public IEnumerable<StakeAddressModel> StakeAddresses { get; set; } = new List<StakeAddressModel>();

    [Key(1)]
    public IEnumerable<AddressModel> Addresses { get; set; } = new List<AddressModel>();

    [Key(2)]
    public IEnumerable<AmountModel> AddressAmounts { get; set; } = new List<AmountModel>();

  }
}
