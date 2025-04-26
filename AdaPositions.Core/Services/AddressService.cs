using AdaPositions.Core.Entities;
using AdaPositions.Core.Entities.Operations;
using AdaPositions.Core.Extensions;
using AdaPositions.Core.Interfaces;
using Blockfrost.Api;
using Blockfrost.Api.Models;
using MessagePack;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPositions.Core.Services {
  public class AddressService {
        
    private readonly BlockfrostService _blockfrostService;
    private readonly AssetsService _assetsService;
    private readonly ILogMsg _form1;
    private TreeNode? _StakesNode = null;
    public TreeNode? StakesNode { get { return _StakesNode; } set { _StakesNode = value; } }
    private TreeNode? _AssetsNode = null;
    public TreeNode? AssetsNode { get { return _AssetsNode; } set { _AssetsNode = value; } }

    public LimitedOps Operations;
     
    public AddressService( BlockfrostService blockfrostService, ILogMsg form1, AssetsService assetsService ) {
      _form1 = form1;
      _blockfrostService = blockfrostService;
      _assetsService = assetsService;
      Package = new StakeAddressPackage();
      Operations = new LimitedOps(this, _form1);

      if (File.Exists(FileExts.StakeAddressFileName)) {
        LoadStakeAddressPackage();
      }
    }

    public StakeAddressPackage Package { get; set; }

    public StakeAddresses Stakes {
      get { return new StakeAddresses(this, Package.StakeAddresses); }
      set { Package.StakeAddresses = value.AsList; }
    }

    public Addresses AddressDict {
      get { return new Addresses(this, Package.Addresses); }
      set { Package.Addresses = value.AsList; }
    }

    public Amounts AddressAmounts { 
      get { return new Amounts(Package.AddressAmounts);}
      set { Package.AddressAmounts = value.AsList; }
    }

    public void LoadStakeAddressPackage() {
      Task.Run(async () => await LoadStakeAddressPackageAsync().ConfigureAwait(false)).GetAwaiter().GetResult();
    }

    public async Task LoadStakeAddressPackageAsync() {
      if (File.Exists(FileExts.StakeAddressFileName)) {
        var mark = DateTime.Now;
        var encoded = await FileExts.StakeAddressFileName.ReadAllTextAsync();
        var decoded = Convert.FromBase64String(encoded);
        this.Package = MessagePackSerializer.Deserialize<StakeAddressPackage>(decoded);        
        var finish = DateTime.Now;
        var diff = (finish - mark).TotalMilliseconds;
        _form1.LogMsg($"{DateTime.Now} {diff}ms loaded: {FileExts.StakeAddressFileName}");
      } else {
        _form1.LogMsg($"{DateTime.Now} ms skipped(no file): {FileExts.StakeAddressFileName}");
      }
    }

    public void SaveStakeAddressPackage() {
      Task.Run(async () => await SaveStakeAddressPackageAsync().ConfigureAwait(false)).GetAwaiter().GetResult();
    }

    public async Task SaveStakeAddressPackageAsync() {
      var mark = DateTime.Now;
      var encoded = MessagePackSerializer.Serialize(Package);
      var decoded = Convert.ToBase64String(encoded);
      await decoded.WriteAllTextAsync(FileExts.StakeAddressFileName).ConfigureAwait(false);
      var finish = DateTime.Now;
      var diff = (finish - mark).TotalMilliseconds;
      _form1.LogMsg($"{DateTime.Now} {diff}ms saved: {FileExts.StakeAddressFileName}");
    }

    public IEnumerable<AddressModel> GetAddressOfStake(long id) {
      Addresses addresses = this.AddressDict;
      IEnumerable<AddressModel> addrList = addresses.Select(x => x.Value).Where(x => x.StakeAddressModelId == id);
      return addrList;
    }

    public IEnumerable<AmountModel> GetAmountsOfAddress(long id) {
      Amounts amounts = this.AddressAmounts;
      IEnumerable<AmountModel> amountList = amounts.Select(x => x.Value).Where(x => x.AddressModelId == id);
      return amountList;
    }

    public IEnumerable<AmountModel> GetAmountOfAddress(long id, string unit) {
      Amounts amounts = this.AddressAmounts;
      IEnumerable<AmountModel> amountList = amounts.Select(x => x.Value).Where(x => ((x.AddressModelId == id) && (x.Unit == unit)));
      return amountList;
    }


    public void AddAddress(AddressModel NewAddress) {

      var addresses = new Addresses(this, Package.Addresses);      
      if (NewAddress.Id == 0) {
        NewAddress.Id = addresses.GetNextId();
      }
      addresses.Add(NewAddress);
      Package.Addresses = addresses.AsList;

    }

    public void RemoveAddress(AddressModel address) {
      var addresses = new Addresses(this, Package.Addresses);
      var allAmounts = new Amounts(Package.AddressAmounts);

      var am = addresses.AddressModel(address.Address);
      if (am == null) return;      
      am.AddressAmounts.ToList().ForEach(x => {        
        allAmounts.Remove(x.Value.Id);
      });
      addresses.Remove(am.Id);

      Package.AddressAmounts = allAmounts.AsList;
      Package.Addresses = addresses.AsList;  
      
    }

    public void RemoveStakeAddress(string stakeAddr) {

      var stakeAddresses = new StakeAddresses(this, Package.StakeAddresses);
      var stake = stakeAddresses.StakeAddressModel(stakeAddr);
      if (stake == null) return;

      stake.Addresses!.ToList().ForEach(x => {
        RemoveAddress(x.Value);       
      });      
      stakeAddresses.Remove(stake.Id);

      Package.StakeAddresses = stakeAddresses.AsList;
      SaveStakeAddressPackage();

    }

    public void AddStakeAddress(AddressContentResponse stakeAddress) {
      var stakeAddresses = new StakeAddresses( this, Package.StakeAddresses);     
      var NewlyAddedStake = new StakeAddressModel() {
        StakeAddress = stakeAddress.StakeAddress,
        Id = stakeAddresses.GetNextId()
      };
      stakeAddresses.Add( NewlyAddedStake);
      Package.StakeAddresses = stakeAddresses.AsList;
            
      AddressModel NewAddress = new AddressModel() {
        Address = stakeAddress.Address,
        StakeAddressModelId = NewlyAddedStake.Id       
      };
      AddAddress(NewAddress);    
      

      SaveStakeAddressPackage();
    }

    public void LoadTvExploreItems( TreeView TreeViewComponent, bool IncludeEmptyAddr) {
      TreeViewComponent.Nodes.Clear();
      StakeAddresses result = Stakes;
      _StakesNode = new TreeNode("Stakes", (int)II.RootFolder, (int)II.RootFolder);
      _AssetsNode = new TreeNode("Assets", (int)II.RootFolder, (int)II.RootFolder);
      TreeViewComponent.Nodes.Add(_StakesNode);
      Stakes.ToList().ForEach(x => {   
        var tnStake = new TreeNode(x.Value.StakeAddress, (int)II.StakeAddress, (int)II.StakeAddress);
        _StakesNode.Nodes.Add(tnStake);
        x.Value.Addresses!.ToList().ForEach(y => {
          if (IncludeEmptyAddr || y.Value.AddressAmounts.Any()) {
            var tnAddress = new TreeNode(y.Value.Address, (int)II.Address, (int)II.Address);
            tnStake.Nodes.Add(tnAddress);
          }          
        });
      });
      TreeViewComponent.Nodes.Add(_AssetsNode);
      _assetsService.Assets.ToList().ForEach(x => {
        var asset = x.Value;
        string assetName = asset.FingerPrint;
        var tnAsset = new TreeNode(assetName, (int)II.Asset, (int)II.Asset);
        _AssetsNode.Nodes.Add(tnAsset);
      });
    }

    public void LoadvrStakes( DataGridView theGrid) { 
      theGrid.Rows.Clear();
      theGrid.Columns.Clear();

      int stakaddrColIndex = theGrid.Columns.Add("StakeAddress", "strOpParam1 Address");
      theGrid.Columns[stakaddrColIndex].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      Stakes.ToList().ForEach(x => {
        theGrid.Rows.Add(x.Value.StakeAddress);
      });
    }

    public bool GetAddAddressByStakeFromBlockfrost(string stakeAddress, TreeNode? stakeNode, int pageNumber) {
      
      var resp = Task.Run( async () => await _blockfrostService.GetAccountAddressesAsync(stakeAddress, pageNumber).ConfigureAwait(false)).GetAwaiter().GetResult();
      bool hasMore = resp.Count == 100;
      if (resp != null) {
        var stake = Stakes.StakeAddressModel(stakeAddress);
        if (stake != null) {
          
          foreach (var address in resp) {
            
            if( stake.Addresses.AddressModel(address.Address) == null) { 
                AddressModel NewAddress = new AddressModel() {
                  Address = address.Address,
                  StakeAddressModelId = stake.Id
                };
              AddAddress(NewAddress);
              stakeNode?.Nodes.Add(new TreeNode(address.Address, 2, 2));
            }
          }

          SaveStakeAddressPackage();

        }        
      }
      return hasMore;
    }

    public void GetAddAddressAmountsFromBlockfrost(AddressModel address) {

      var resp = Task.Run(async () => await _blockfrostService.GetAddressDetailsAsync(address.Address).ConfigureAwait(false)).GetAwaiter().GetResult();

      if (resp != null) {
        var amountList = resp.Amount.ToAmountList();
        var amounts = this.AddressAmounts;
        
        foreach (var amount in amountList) {
          
          IEnumerable<AmountModel> amounts2 = amounts.Select(x => x.Value)
            .Where(x => ((x.AddressModelId == address.Id) && (x.Unit == amount.Unit)));
          var anAmount = amounts2.FirstOrDefault();

          if (anAmount == null) {
            anAmount = new AmountModel() {
              AddressModelId = address.Id,
              Quantity = amount.Quantity,
              Unit = amount.Unit
            };            
            amounts.Add(anAmount);            
          } else {
            anAmount.Quantity = amount.Quantity;
            anAmount.DateUpdatedUtc = DateTime.UtcNow;
          }          

        }
        this.AddressAmounts = amounts;
        SaveStakeAddressPackage();
      }
    }

    public void GetAssetDetailsFromBlockfrost(string unit) {
      if (string.IsNullOrEmpty(unit)) return;
      var resp = Task.Run(async () => await _blockfrostService.GetAssetsAsync(unit).ConfigureAwait(false)).GetAwaiter().GetResult();
            
      if (resp != null) {
        var assets = _assetsService.Assets;
        var respAsset = resp.ToAssetEntry();
        var asset = assets.FindByUnit(unit);
        if (asset != null) {
          asset.Asset = respAsset.Asset;
          asset.PolicyId = respAsset.PolicyId;
          asset.AssetName = respAsset.AssetName;
          asset.FingerPrint = respAsset.FingerPrint;
          asset.Quantity = respAsset.Quantity;
          asset.Decimals = respAsset.Decimals;
          asset.Name = respAsset.Name;
          asset.Ticker = respAsset.Ticker;
          asset.Url = respAsset.Url;
          asset.Logo = respAsset.Logo;
          assets[asset.Id] = asset;
        } else {
          asset = _assetsService.AddAsset(resp);
        }        
        _assetsService.Assets = assets;
        _assetsService.SaveAssets();
      }
    }

    public void RemoveAddressFromStake(TreeNode addressNode) {
      var address = addressNode.Text;
      var stakeNode = addressNode.Parent;
      var stake = Stakes.StakeAddressModel(stakeNode.Text);
      if (stake != null) {
        var addr = stake.Addresses.AddressModel(address);
        if (addr != null) {
          RemoveAddress(addr);
          stakeNode.Nodes.Remove(addressNode);
          SaveStakeAddressPackage();
        }
      }
    }

    public string RateLimitString { get { return _blockfrostService.LastToday.ToShortDateString() + " " +
        "Calls: " + _blockfrostService.LastCallsCount; } } 
    public void GetRateLimits() { 
      Task.Run(async () => await _blockfrostService.CaptureMetrics().ConfigureAwait(false)).GetAwaiter().GetResult();            
    }

    public void DoNextToDo() {
      if (_blockfrostService.IsOverDailyMaxCalls) {
        _form1.LogMsg("Over daily max calls");
        return;
      }
      var nextToDo = Operations.OpSchedule.Pop();
      if (nextToDo != null) {
        this.Operations.AddOp(new Entities.Operation( this.Operations, nextToDo));
      }

      _form1.DrawStats("unused");
    }

    public void ScheduleToDo(Ot Optype, string? strOpParam1, AddressModel? address, int pageNumber) {
      if (Operations.OpSchedule.ToList().Any(x => x.Value.Optype == Optype && x.Value.StrOpParam1 == strOpParam1)) {
        return;
      }
      var nextToDo = Operations.OpSchedule.AddToSchedule(Optype, strOpParam1, address, pageNumber);      
    }

  }

  public enum II {
    RootFolder = 0,
    StakeAddress = 1,
    Address = 2,
    Amount = 3,
    Asset = 4
  }

}
