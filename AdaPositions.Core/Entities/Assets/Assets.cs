using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagePack;

namespace AdaPositions.Core.Entities.Assets {

  
  public class Assets : ConcurrentDictionary<long, AssetEntity> {  

    public long Nonce = 0;

    public Assets() : base() { }

    public AssetEntity AddAsset(AssetEntity asset) {
      if (asset.Id != 0) {
        base[asset.Id] = asset;
      } else {
        Nonce++;
        asset.Id = Nonce;
        base[Nonce] = asset;
      }      
      return asset;
    }

    public void RemoveAsset(long aKey) {
      if (ContainsKey(aKey)) {
        TryRemove(aKey, out _);
      }
    }
  }


}
