using AdaPositions.Core.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPositions.Core.Entities {
  public class StakeAddresses : ConcurrentDictionary<long, StakeAddressModel> {
    private readonly object _lock = new object();
    private readonly AddressService _addressService;
    private ConcurrentDictionary<string, StakeAddressModel> _byStake = new ConcurrentDictionary<string, StakeAddressModel>();
    public StakeAddresses(AddressService addressService) : base() {      
      _addressService = addressService;
    }

    public StakeAddresses(AddressService addressService, IEnumerable<StakeAddressModel> asList) : base() {
      _addressService = addressService;
      AsList = asList;
    }    

    public virtual new StakeAddressModel this[long key] {
      get {
        lock (_lock) {
          if (!ContainsKey(key)) base[key] = new StakeAddressModel() { Id = key, StakeAddress = "" };
          return base[key];
        }
      }
      set {
       
          if (value != null) {
            Add(value);
          } else {
            if (ContainsKey(key)) {
              _ = base.TryRemove(key, out _);
            }
          }
       
      }
    }

    public StakeAddressModel? StakeAddressModel(string stakeAddress) {
      lock (_lock) {
        if (_byStake.ContainsKey(stakeAddress)) {
          return _byStake[stakeAddress];
        }
        return null;
      }
    }

    public void Add(StakeAddressModel item) {
      lock (_lock) {  
        if (item.Id == 0) {
          item.Id = GetNextId();
        }
        base[item.Id] = item;
        _byStake[item.StakeAddress] = item;
        IEnumerable<AddressModel> stakeAddresses = _addressService.GetAddressOfStake(item.Id);
        item.Addresses = new Addresses(_addressService, stakeAddresses );
      }
    }

    public long GetNextId() {
      long max = 0;
      if (this.Keys.Count > 0) {
        max = this.Select(x => x.Value).Max(x => x.Id);
      }
      return max + 1;
    }

    public virtual void Remove(long key) {
      lock (_lock) {
        if (_byStake.ContainsKey(base[key].StakeAddress)) {
          _ = _byStake.TryRemove(base[key].StakeAddress, out _);
        }
        if (ContainsKey(key)) {
          _ = base.TryRemove(key, out _);
        }        
      }
    }

    public IEnumerable<StakeAddressModel> AsList {
      get {
        lock (_lock) {
          return base.Values.ToList();
        }
      }
      set {
        lock (_lock) {
          base.Clear();
          _byStake.Clear();
          foreach (var item in value) {
            Add( item);
          }
        }
      }
    }


  } 
}
