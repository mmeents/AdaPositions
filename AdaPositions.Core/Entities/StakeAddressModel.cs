using MessagePack;

namespace AdaPositions.Core.Entities {

  [MessagePackObject]
  public class StakeAddressModel {
    [Key(0)]
    public long Id { get; set; } = 0;

    [Key(1)]
    public string StakeAddress { get; set; } = "";

    [IgnoreMember]
    public Addresses? Addresses { get; set; } 

  }


}