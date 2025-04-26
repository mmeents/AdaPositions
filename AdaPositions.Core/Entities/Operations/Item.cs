using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPositions.Core.Entities.Operations
{

    public interface IItem
    {
        long Id { get; set; }
    }

  [MessagePackObject]
  public class Item {
    public Item() { }
    public Item(long id) {
      Id = id;
    }

    [Key(0)]
    public long Id { get; set; }

  }
}
