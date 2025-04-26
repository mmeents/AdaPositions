using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaPositions.Core.Services;


namespace AdaPositions.Core.Entities {

  public class Addresses : ConcurrentDictionary<long, AddressModel> {
    private readonly object _lock = new object();
    private readonly AddressService _addressService;
    public Addresses(AddressService addressService) : base() {
      _addressService = addressService;
    }
    public Addresses(AddressService addressService, IEnumerable<AddressModel> asList) : base() {
      _addressService = addressService;
      AsList = asList;
    }

    public virtual new AddressModel this[long Id] { 
      get {
        lock (_lock) {
          if (!ContainsKey(Id)) base[Id] = new AddressModel() { Id = Id, StakeAddressModelId = 0, Address = "" };
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

    public void Add(AddressModel item) {
      lock (_lock) {
        if (item.Id == 0) {
          item.Id = GetNextId();
        }
        base[item.Id] = item;
        item.AddressAmounts = new Amounts(_addressService.GetAmountsOfAddress(item.Id));
      }
    }

    public AddressModel? AddressModel(string address) {
      lock (_lock) {
        if (this.Values.Any(x => x.Address == address)) {
          return this.Values.First(x => x.Address == address);
        }
        return null;
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

    public IEnumerable<AddressModel> AsList { 
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
