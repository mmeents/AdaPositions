using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AdaPositions.Core.Entities;
using AdaPositions.Core.Entities.Assets; 
using AdaPositions.Core.Extensions;
using Blockfrost.Api;
using MessagePack;

namespace AdaPositions.Core.Services
{
    public class AssetsService
    {
        private readonly BlockfrostService _blockfrostService;
        private Assets _assets = new Assets();

        public Assets Assets {
          get {
            return _assets;
          }
          set {
            _assets = value;
          }
        }

        public AssetsService(BlockfrostService blockfrostService)
        {
          _blockfrostService = blockfrostService;

          Task.Run(async () => await LoadAssets().ConfigureAwait(false)).Wait();      
        }

        public async Task LoadAssets() {
          if (File.Exists(FileExts.AssetsFileName)) {
            _assets = await AssetExt.LoadAssetsAsync(FileExts.AssetsFileName).ConfigureAwait(false);
          } 
          else {
            _assets = new Assets();
          } 
        }
        
        public void SaveAssets() { 
          Task.Run(async () => await SaveAssetsAsync().ConfigureAwait(false)).GetAwaiter().GetResult();
        }

        public async Task SaveAssetsAsync() {
          await AssetExt.SaveAssetsAsync(_assets, FileExts.AssetsFileName).ConfigureAwait(false);
        }

        public AssetEntity AddAsset(AssetResponse asset) {

          var toAddAsset = asset.ToAssetEntry();
          var lookupAsset = _assets.FindByFingerPrint(toAddAsset.FingerPrint);

          if (lookupAsset != null && lookupAsset.PolicyId == toAddAsset.PolicyId && lookupAsset.Id > 0) {
              toAddAsset.Id = lookupAsset.Id;
          }

          return _assets.AddAsset(toAddAsset);
        }

        public string AsFormatedQuantity(AmountModel amount) {
          if (amount == null) return "Null Amount";
          var quantity = amount.Quantity.AsLong();
          var asset = _assets.FindByUnit(amount.Unit);
          if (asset == null || asset.Decimals == 0) return $"No Asset:{amount.Quantity}";      
          
          decimal adjustedQuantity = quantity / (decimal)Math.Pow(10, asset.Decimals);
          string fmtDecimals = $"0:F{asset.Decimals}";
          return string.Format("{"+fmtDecimals+"}", adjustedQuantity);
        }

  }

    public static class AssetExt {
      public static async Task SaveAssetsAsync(this Assets assets, string filePath) {
        List<AssetEntity> assetList = assets.Values.OrderBy(x => x.Id).ToList();
        byte[] data = MessagePackSerializer.Serialize(assetList);
        await File.WriteAllBytesAsync(filePath, data);
      }

      public static async Task<Assets> LoadAssetsAsync(string filePath) {
        byte[] data = await File.ReadAllBytesAsync(filePath);
        List<AssetEntity> assetList = MessagePackSerializer.Deserialize<List<AssetEntity>>(data);
        return assetList.ToAssets();
      }

      public static Assets ToAssets(this List<AssetEntity> assetList) {
        var assets = new Assets();
        foreach (var asset in assetList) {
          assets.AddAsset(asset);
        }
        var lovelace = assets.FindByUnit("lovelace");
        if (lovelace == null) assets.AddAsset(AssetExt.LoveLace());
        return assets;
      }

      public static AssetEntity LoveLace() {
        return new AssetEntity() {
          Asset = "lovelace",
          PolicyId = "e2a7c17c3e4e4d4e",
          AssetName = "Lovelace".AsUtf8ToHex(),
          FingerPrint = "PrimaryNativeADA",
          Quantity = "45000000000000000",
          Decimals = 6,
          Name = "Cardano ADA",
          Ticker = "ADA",
          Url = "https://cardano.org/",
          Logo = ""
        };
    }

      public static AssetEntity ToAssetEntry(this Blockfrost.Api.AssetResponse asset) {
        AssetEntity assetEntity = new AssetEntity() {
          Asset = asset.Asset1,
          PolicyId = asset.Policy_id,
          AssetName = asset.Asset_name ?? "nul Asset_Name",
          FingerPrint = asset.Fingerprint,
          Quantity = asset.Quantity,
          Decimals = asset.Metadata?.Decimals ?? 0,
          Name = asset?.Metadata?.Name ?? "",
          Ticker = asset?.Metadata?.Ticker ?? "",
          Url = asset?.Metadata?.Url ?? "",
          Logo = asset?.Metadata?.Logo ?? ""
        };
        return assetEntity;
      }

      public static AssetEntity FindByUnit(this Assets assets, string unit) {
        return assets.Values.FirstOrDefault(a => a.Asset == unit);
      }

      public static AssetEntity FindByTicker(this Assets assets, string ticker) {        
        return assets.Values.FirstOrDefault(a => a.Ticker == ticker);
      }

      public static AssetEntity FindByFingerPrint(this Assets assets, string fingerPrint) {
        return assets.Values.FirstOrDefault(a => a.FingerPrint == fingerPrint);
      }

      public static string AsHexToUtf8(this string hex) {
        if (hex.Length % 2 != 0) return "";
        try { 
          byte[] bytes = new byte[hex.Length / 2];
          for (int i = 0; i < hex.Length; i += 2)
            bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);

          return Encoding.UTF8.GetString(bytes).SanitizeUnit();
        } catch { 
          return "";
        }
      }

      public static string AsUtf8ToHex(this string utf8) {
        byte[] bytes = Encoding.UTF8.GetBytes(utf8);
        StringBuilder hex = new StringBuilder(bytes.Length * 2);

        foreach (byte b in bytes)
          hex.AppendFormat("{0:X2}", b); // Lowercase hex; use {0:X2} for uppercase

        return hex.ToString();
      }



  }

}
