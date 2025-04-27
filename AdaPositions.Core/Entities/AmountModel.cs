using MessagePack;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaPositions.Core.Extensions;

namespace AdaPositions.Core.Entities {

  [MessagePackObject]
  public class AmountModel {

    [Key(0)]
    public long Id { get; set; } = 0;

    [Key(1)]
    public long AddressModelId { get; set; } = 0;

    [Key(2)]
    public string Unit { get; set; } = "";

    [Key(3)]
    public string Quantity { get; set; } = "";

    [Key(4)]
    public DateTime DateUpdatedUtc { get; set; } = DateTime.UtcNow;

    [Key(5)]
    public DateTime DateCreatedUtc { get; set; } = DateTime.UtcNow;

  }


  public class Amounts : ConcurrentDictionary<long, AmountModel> {
    private readonly object _lock = new object();
    public Amounts() : base() { }
    public Amounts(IEnumerable<AmountModel> asList) : base() {
      AsList = asList;
    }

    public bool Any() {
      lock (_lock) {
        foreach(var item in base.Values) {
          if (item.Quantity.AsLong() > 0) {
            return true;
          }          
        }
        return false;
      }
    }
    public virtual new AmountModel this[long Id] {
      get {
        lock (_lock) {
          if (!ContainsKey(Id)) base[Id] = new AmountModel() { Id = Id };
          return base[Id];
        }
      }
      set {
        lock (_lock) {
          if (value != null) {
            base[Id] = value;
          } else {
            if (ContainsKey(Id)) {
              _ = base.TryRemove(Id, out _);
            }
          }
        }
      }
    }

    public void Add(AmountModel item) {
      lock (_lock) {
        if (item.Id == 0) {

          var existingAmount = this.Values.FirstOrDefault(x => x.Unit == item.Unit);
          if (existingAmount != null) { 
            long quantity = item.Quantity.AsLong();
            long existingQuantity = existingAmount.Quantity.AsLong();
            existingAmount.Quantity = (quantity + existingQuantity).ToString();
          } else {
            item.Id = GetNextId();
            base[item.Id] = item;
          }          
        } else {
          base[item.Id] = item;
        }        
      }
    }

    public string GetTotalAda() {
      lock (_lock) {
        decimal total = 0;
        foreach (var item in base.Values) {
          if (item.Unit == "lovelace") {
            total += item.Quantity.AsLong() / 1000000m;
          }
        }
        return string.Format("{0:F6}", total);
      }
    }

    public long GetNextId() {
      long max = 0;
      if (this.Keys.Count > 0) {
        max = this.Select(x => x.Value).Max(x => x.Id);
      }
      return max + 1;
    }

    public virtual void Remove(long Id) {
      lock (_lock) {
        if (ContainsKey(Id)) {
          _ = base.TryRemove(Id, out _);
        }
      }
    }

    public IEnumerable<AmountModel> AsList {
      get {
        lock (_lock) {
          return base.Values.ToList();
        }
      }
      set {
        lock (_lock) {
          base.Clear();
          if (value == null) return;
          foreach (var item in value) {
            Add(item);
          }
        }
      }
    }

  }



}
