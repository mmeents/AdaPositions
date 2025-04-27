using AdaPositions.Core.Services;
using AdaPositions.Core.Extensions;
using AdaPositions.Core.Interfaces;
using AdaPositions.Core.Entities;
using Blockfrost.Api;
using System.Net;
using AdaPositions.Core.Entities.Assets;

namespace AdaPositions {
  public partial class Form1 : Form, ILogMsg {

    private SettingsService _settingsService;
    private BlockfrostService? _blockfrostService;
    private AssetsService? _assetsService;
    private Core.Services.AddressService? _addressService;

    private bool _keyVerified = false;
    private bool KeyVerified {
      get {
        return _keyVerified;
      }
      set {
        _keyVerified = value;
        if (_keyVerified) {
          tbApiKey.Enabled = false;
          btnVerifyApiKey.Text = "Verified Unlock";
        } else {
          tbApiKey.Enabled = true;
          btnVerifyApiKey.Text = "Verify Api Key";
        }
      }
    }

    private bool _processing = false;
    private bool Processing {
      get {
        return _processing;
      }
      set {
        _processing = value;
        if (_processing) {
          if (scMain.Panel2Collapsed) scMain.Panel2Collapsed = false;
          CountDown = MaxCountDown;
          if (!timer1.Enabled) timer1.Enabled = true;
          if (!btnProcessStop.Enabled) btnProcessStop.Enabled = true;
        } else {
          if (!scMain.Panel2Collapsed) scMain.Panel2Collapsed = true;
          if (timer1.Enabled) timer1.Enabled = false;
          if (!btnProcessStop.Enabled) btnProcessStop.Enabled = false;
        }
      }
    }

    private int CountDown = 0;
    private int MaxCountDown { get { return trackBar1.Value; } }


    delegate void SetLogMsgCallback(string msg);
    public void LogMsg(string msg) {
      if (this.tbLog.InvokeRequired) {
        SetLogMsgCallback d = new(LogMsg);
        this.BeginInvoke(d, new object[] { msg });
      } else {
        if (!tbLog.Visible) tbLog.Visible = true;
        this.tbLog.Text = msg + Environment.NewLine + tbLog.Text;
      }
    }

    public void DrawStats(string msg) {
      if (this.lbRateTotals.InvokeRequired) {
        SetLogMsgCallback d = new(DrawStats);
        this.BeginInvoke(d, new object[] { msg });
      } else {

        this.lbRateTotals.Text = _addressService!.RateLimitString;
      }
    }

    public Form1() {
      InitializeComponent();
      if (!scMain.Panel2Collapsed) scMain.Panel2Collapsed = true;
      _settingsService = new SettingsService(FileExts.SettingsFileName, this);
      if (File.Exists(FileExts.SettingsFileName)) {
        tbApiKey.Text = _settingsService.Settings[SettingCol.BlockFrostApiKey].Value;
        var aVerified = _settingsService.Settings[SettingCol.ApiKeyVerified].Value;
        KeyVerified = aVerified == "True";
      } else {
        KeyVerified = false;
      }
      if (KeyVerified) {
        _blockfrostService = new BlockfrostService(tbApiKey.Text, this);
        _assetsService = new AssetsService(_blockfrostService);
        _addressService = new Core.Services.AddressService(_blockfrostService, this, _assetsService);
        LoadtvExplore();
      }
      Processing = false;
    }

    private void btnVerifyApiKey_Click(object sender, EventArgs e) {
      if (KeyVerified) {
        KeyVerified = false;
      } else {
        bool check = false;
        if (tbApiKey.Text.Length > 0) {
          var aBlockfrostService = new BlockfrostService(tbApiKey.Text, this);
          int callResult = Task.Run(async () => await aBlockfrostService.CaptureMetrics().ConfigureAwait(false)).GetAwaiter().GetResult();
          aBlockfrostService.Dispose();
          check = callResult == 1;
        }
        if (check) {
          Settings settings = _settingsService.Settings;
          settings[SettingCol.BlockFrostApiKey].Value = tbApiKey.Text;
          settings[SettingCol.ApiKeyVerified].Value = "True";
          _settingsService.Settings = settings;
          _settingsService.Save();
          _blockfrostService = new BlockfrostService(tbApiKey.Text, this);
          _assetsService = new AssetsService(_blockfrostService);
          _addressService = new Core.Services.AddressService(_blockfrostService, this, _assetsService);
          LoadtvExplore();
          KeyVerified = true;
        } else {
          KeyVerified = false;
        }
      }
    }

    private void tcMain_Selecting(object sender, TabControlCancelEventArgs e) {
      if (KeyVerified && e.TabPage?.Name == tpExplore.Name && e.Action == TabControlAction.Selecting) {

        LoadtvExplore();
        if (scMain.Panel2Collapsed) scMain.Panel2Collapsed = false;
        _addressService!.ScheduleToDo(Ot.UpdateRateLimits, null, null, 1);
        Processing = true;
        trackBar1_ValueChanged(sender, e);

      } else {
        if (!_keyVerified && e.TabPage?.Name == tpExplore.Name) {
          e.Cancel = true;
          MessageBox.Show("Please verify your API Key first.");
        }
      }
    }

    private void LoadtvExplore() {
      _addressService?.LoadvrStakes(vrStakes);
      _addressService?.LoadTvExploreItems(tvExplore, cbShowEmptyAddr.Checked);
    }

    private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
      if (Processing) {
        Processing = false;
      }
      if (_blockfrostService != null) {
        _blockfrostService.Dispose();
      }
    }

    private void timer1_Tick(object sender, EventArgs e) {
      if (Processing) {
        timer1.Enabled = false;
        try {

          if (CountDown >= 1) {
            CountDown = CountDown - 1;
          } else {
            CountDown = MaxCountDown;
            _addressService!.DoNextToDo();
            Processing = _addressService?.HasNextToDo() ?? false;
          }

          lbRateTotals.Text = (Convert.ToDecimal(CountDown / 10.0)).AsStr1P()
            + $" ToDo: {_addressService?.Operations.OpSchedule.Count() ?? 0} "
            + $" Doing: {_addressService!.Operations.Count()} "
            + $"{_addressService?.RateLimitString} ";

        } catch (Exception ex) {
          LogMsg(ex.Message);
          Processing = false;
        } finally {
          timer1.Enabled = Processing;
        }
      }
    }

    private void btnPlay_Click(object sender, EventArgs e) {
      Processing = true;
    }

    private void btnProcessStop_Click(object sender, EventArgs e) {
      if (Processing) {
        Processing = false;
        btnProcessStop.Text = "Play";
        btnProcessStop.BackColor = Color.Green;
      } else {
        Processing = true;
        btnProcessStop.Text = "Stop";
        btnProcessStop.BackColor = Color.Red;
      }
    }

    private void tpSetup_Click(object sender, EventArgs e) {

    }

    private void btnLookupAddress_Click(object sender, EventArgs e) {

      if (KeyVerified) {
        if (tbAddress.Text.Length > 0) {
          var aAddress = Task.Run(async () => await _blockfrostService!.GetAddressDetailsAsync(tbAddress.Text).ConfigureAwait(false)).GetAwaiter().GetResult();
          if (aAddress != null) {
            lbLookup1.Text = $"strOpParam1: {aAddress.StakeAddress}";
            lbLookup2.Text = $"Address: {aAddress.Address}";
          }
        }
      }
    }

    private void btnAddStake_Click(object sender, EventArgs e) {
      if (KeyVerified) {

        var aAddress = Task.Run(async () => await _blockfrostService.GetAddressDetailsAsync(tbAddress.Text).ConfigureAwait(false)).GetAwaiter().GetResult();

        if (aAddress != null) {

          _addressService!.AddStakeAddress(aAddress);

          LoadtvExplore();
        }
      }
    }

    private void label3_Click(object sender, EventArgs e) {

    }

    private void splitExplore_Panel1_Resize(object sender, EventArgs e) {
      tvExplore.Height = splitExplore.Panel1.Height - 60;
      tbMain.Height = splitExplore.Panel1.Height - 60;
    }

    private void cmsTvExplore_Opening(object sender, System.ComponentModel.CancelEventArgs e) {
      TreeNode selectedNode = tvExplore.SelectedNode;
      if (selectedNode == null) return;
      if (selectedNode.ImageIndex == (int)II.RootFolder) {
        if (_addressService?.StakesNode == selectedNode) {
          miGetAddressesForStake.Visible = true;
          miResyncStakeAddr.Visible = true;
          miResyncTotals.Visible = true;
          miRemove.Visible = false;
          miResyncAssets.Visible = false;
        } else if (_addressService?.AssetsNode == selectedNode) {
          miGetAddressesForStake.Visible = false;
          miResyncStakeAddr.Visible = false;
          miResyncTotals.Visible = false;
          miRemove.Visible = false;
          miResyncAssets.Visible = true;
        }
      } else if (selectedNode.ImageIndex == (int)II.StakeAddress) {
        miGetAddressesForStake.Visible = true;
        miResyncStakeAddr.Visible = true;
        miResyncTotals.Visible = true;
        miRemove.Visible = false;
        miResyncAssets.Visible = true;

      } else if (selectedNode.ImageIndex == (int)II.Address) {
        miGetAddressesForStake.Visible = false;
        miResyncStakeAddr.Visible = false;
        miResyncTotals.Visible = true;
        miRemove.Visible = true;
        miResyncAssets.Visible = true;
      }

    }

    private void miGetAddressesForStake_Click(object sender, EventArgs e) {
      TreeNode selectedNode = tvExplore.SelectedNode;
      if (selectedNode == null) return;
      if (selectedNode.ImageIndex == (int)II.RootFolder) {
        if (selectedNode == _addressService!.StakesNode) {
          _addressService!.Stakes.Values.ToList().ForEach(x => {
            _addressService!.ScheduleToDo(Ot.GetStakeAddresses, x.StakeAddress, null, 1);
          });
        } 
      } else if (selectedNode.ImageIndex == (int)II.StakeAddress) {
        var stakeAddress = selectedNode.Text;
        var aStake = _addressService!.Stakes.StakeAddressModel(stakeAddress);
        if (aStake != null) {
          _addressService.ScheduleToDo(Ot.GetStakeAddresses, aStake.StakeAddress, null, 1);
          if (!Processing) Processing = true;
        }
      }
    }

    private void miRemove_Click(object sender, EventArgs e) {
      TreeNode selectedNode = tvExplore.SelectedNode;
      if (selectedNode == null) return;
      if (selectedNode.ImageIndex == (int)II.Address) {
        _addressService!.RemoveAddressFromStake(selectedNode);
      }
    }

    private void trackBar1_ValueChanged(object sender, EventArgs e) {
      try {
        lbTrackBarValue.Text = "Every " + (Convert.ToDecimal((trackBar1.Value) / 10.001)).AsStr1P() + " sec or " +
          $"{Convert.ToDecimal(1.0 / ((trackBar1.Value) / 10.001)).AsStr1P()} /sec or " +
          $"{Convert.ToDecimal((60.0) / ((trackBar1.Value) / 10.001)).AsStr1P()} /min or so ";
      } catch (Exception ex) {
        LogMsg(ex.Message);
      }
    }

    private void miResyncStakeAddr_Click(object sender, EventArgs e) {
      TreeNode selectedNode = tvExplore.SelectedNode;
      if (selectedNode == null) return;
      if (selectedNode.ImageIndex == (int)II.StakeAddress) {
        var stakeAddrModel = _addressService!.Stakes.StakeAddressModel(selectedNode.Text);
        if (stakeAddrModel == null) return;
        foreach (var aAddress in stakeAddrModel.Addresses!.Values) {
          _addressService!.ScheduleToDo(Ot.GetAddressAmounts, stakeAddrModel.StakeAddress, aAddress, 1);
        }
        if (!Processing) Processing = true;
      }
    }
    private void miResyncTotals_Click(object sender, EventArgs e) {  
      TreeNode selectedNode = tvExplore.SelectedNode;
      if (selectedNode == null) return;
      if (selectedNode.ImageIndex == (int)II.RootFolder) { 
        if (selectedNode == _addressService!.StakesNode) {
          foreach (var aStake in _addressService.Stakes.Values) {
            foreach (var aAddress in aStake.Addresses!.Values) {
              _addressService!.ScheduleToDo(Ot.GetAddressAmounts, aStake.StakeAddress, aAddress, 1);
            }
          }
        }
        if (!Processing) Processing = true;
      } else if (selectedNode.ImageIndex == (int)II.StakeAddress) {
        var stakeAddrModel = _addressService!.Stakes.StakeAddressModel(selectedNode.Text);
        if (stakeAddrModel == null) return;
        foreach (var aAddress in stakeAddrModel.Addresses!.Values) {
          _addressService!.ScheduleToDo(Ot.GetAddressAmounts, stakeAddrModel.StakeAddress, aAddress, 1);
        }
        if (!Processing) Processing = true;
      } else if (selectedNode.ImageIndex == (int)II.Address) {
        var stakeAddrModel = _addressService!.Stakes.StakeAddressModel(selectedNode.Parent.Text);
        if (stakeAddrModel == null) return;
        var aAddress = stakeAddrModel.Addresses!.AddressModel(selectedNode.Text);
        if (aAddress != null) {
          _addressService!.ScheduleToDo(Ot.GetAddressAmounts, stakeAddrModel.StakeAddress, aAddress, 1);
          if (!Processing) Processing = true;
        }
      }

    }

    private void miResyncAssets_Click(object sender, EventArgs e) {
      TreeNode selectedNode = tvExplore.SelectedNode;
      if (selectedNode == null) return;
      if (selectedNode.ImageIndex == (int)II.RootFolder) {

        foreach (var aAsset in _assetsService!.Assets.Values) {
          if (aAsset.Quantity.AsLong() > 0) {
            var asset = _assetsService!.Assets.FindByUnit(aAsset.AssetName);
            if (asset == null) {
              _addressService!.ScheduleToDo(Ot.GetAssetDetails, aAsset.PolicyId + aAsset.AssetName, null, 1);
            }
          }
        }

      }
      if (selectedNode.ImageIndex == (int)II.StakeAddress) {
        var stakeAddrModel = _addressService!.Stakes.StakeAddressModel(selectedNode.Text);
        if (stakeAddrModel == null) return;
        foreach (var aAddress in stakeAddrModel.Addresses!.Values) {
          foreach (var aAmount in aAddress.AddressAmounts) {
            var asset = _assetsService!.Assets.FindByUnit(aAmount.Value.Unit);
            if (asset == null) {            
              _addressService!.ScheduleToDo(Ot.GetAssetDetails, aAmount.Value.Unit, aAddress, 1);
            }
          }
        }
      } else if (selectedNode.ImageIndex == (int)II.Address) {
        var stakeAddrModel = _addressService!.Stakes.StakeAddressModel(selectedNode.Parent.Text);
        if (stakeAddrModel == null) return;
        var aAddress = stakeAddrModel.Addresses!.AddressModel(selectedNode.Text);
        if (aAddress == null) return;
        foreach (var aAmount in aAddress.AddressAmounts) {
          var asset = _assetsService!.Assets.FindByUnit(aAmount.Value.Unit);
          if (asset == null) {
            _addressService!.ScheduleToDo(Ot.GetAssetDetails, aAmount.Value.Unit, aAddress, 1);
          }          
        }
      }
      if (!Processing) Processing = true;
    }

    private void tvExplore_AfterSelect(object sender, TreeViewEventArgs e) {
      TreeNode selectedNode = tvExplore.SelectedNode;
      if (selectedNode == null) return;
      if (selectedNode.ImageIndex == (int)II.RootFolder) {
        tbMain.Text = AllStakesTotals();
      } else if (selectedNode.ImageIndex == (int)II.StakeAddress) {
        var stakeAddrModel = _addressService!.Stakes.StakeAddressModel(selectedNode.Text);
        if (stakeAddrModel == null) return;
        tbMain.Text = StakeTotals(stakeAddrModel);
      } else if (selectedNode.ImageIndex == (int)II.Address) {
        var stakeAddrModel = _addressService!.Stakes.StakeAddressModel(selectedNode.Parent.Text);
        if (stakeAddrModel == null) return;
        var aAddress = stakeAddrModel.Addresses!.AddressModel(selectedNode.Text);
        if (aAddress == null) return;
        tbMain.Text = AddressTotals(aAddress);
      } else if (selectedNode.ImageIndex == (int)II.Asset) {
        tbMain.Text = AssetDetails(selectedNode.Text);
      }
    }

    private string AllStakesTotals() {
      string aStr = "";
      Amounts amountTotals = new Amounts();
      Addresses uniqueAddress = new Addresses(_addressService!);
      aStr = aStr + $"{ss.Nl}All Stakes Totals:{ss.Nl}";
      _addressService!.Stakes.Values.ToList().ForEach( stake => {      
        stake.Addresses!.Values.ToList().ForEach(x => {
          try {
            x.AddressAmounts!.Values.ToList().ForEach(y => {
              try {
                if (y.Quantity.AsLong() > 0) {
                  amountTotals.Add(y);
                  if (!uniqueAddress.ContainsKey(x.Id)) {
                    uniqueAddress[x.Id] = x;
                  }
                }
              } catch (Exception ex) {
                LogMsg(ex.Message);
              }
            });
          } catch (Exception ex) {
            LogMsg(ex.Message);
          }          
        });
      });
      foreach (var addrId in uniqueAddress.Keys) {
        aStr = aStr + $"Address: {uniqueAddress[addrId].Address}{ss.Nl}";
      }

      aStr = aStr + $"{ss.Nl}Total Ada: {amountTotals.GetTotalAda()}{ss.Nl}";

      foreach (var y in amountTotals.Values) {
        try {
          if (y.Quantity.AsLong() > 0 && y.Unit != "lovelace") {
            var asset = _assetsService!.Assets.FindByUnit(y.Unit);
            var unit = y?.Unit ?? "Missing Unit";
            var quantity = _assetsService!.AsFormatedQuantity(y);
            if (asset != null) {
              unit = asset.AssetName.AsHexToUtf8();
              aStr = aStr + $"{ss.Nl} Unit: {unit}{ss.Nl} Quantity:{quantity}{ss.Nl}";
            } else {
              aStr = aStr + $"{ss.Nl}Asset missing{ss.Nl} Unit: {unit}{ss.Nl} Quantity:{quantity}{ss.Nl}";
            }
          }
        } catch (Exception ex) {
          LogMsg(ex.Message);
        }        
      }
      return aStr;      
    }

    private string StakeTotals(StakeAddressModel stake) {
      string aStr = "";
      Amounts amountTotals = new Amounts();
      Addresses uniqueAddress = new Addresses(_addressService!);
      aStr = aStr + $"{ss.Nl}StakeAddress: {stake.StakeAddress}{ss.Nl}";
      stake.Addresses!.Values.ToList().ForEach(x => { 
        x.AddressAmounts!.Values.ToList().ForEach(y => {
          try {
            if (y.Quantity.AsLong() > 0) {
              amountTotals.Add(y);
              if (!uniqueAddress.ContainsKey(x.Id)) {
                uniqueAddress[x.Id] = x;
              }
            }
          } catch (Exception ex) {
            LogMsg(ex.Message);
          }                    
        });
      });
      foreach(var addrId in uniqueAddress.Keys ) {
        aStr = aStr + $"Address: {uniqueAddress[addrId].Address}{ss.Nl}";
      }
      aStr = aStr + $"{ss.Nl}Total Ada: {amountTotals.GetTotalAda()}{ss.Nl}";      
      foreach(var y in amountTotals.Values) { 
        try {
          if (y.Quantity.AsLong() > 0 && y.Unit != "lovelace") {
            var asset = _assetsService!.Assets.FindByUnit(y.Unit);
            var unit = y.Unit;
            var quantity = _assetsService!.AsFormatedQuantity(y);
            if (asset != null) {
              unit = asset.AssetName.AsHexToUtf8();
              aStr = aStr + $"{ss.Nl} Unit: {unit}{ss.Nl} Quantity:{quantity}{ss.Nl}";
            } else {
              aStr = aStr + $"{ss.Nl}Asset missing{ss.Nl} Unit: {unit}{ss.Nl} Quantity:{quantity}{ss.Nl}";
            }
          }
        } catch (Exception ex) {
          LogMsg(ex.Message);
        }        
      }
      return aStr;
    }

    private string AddressTotals(AddressModel address) {
      string aStr = "";
      aStr = aStr + $"Address: {address.Address}{ss.Nl}";
      Amounts amountTotals = new Amounts();
      address.AddressAmounts!.Values.ToList().ForEach(x => {
        try {
          if (x.Quantity.AsLong() > 0) {
            amountTotals.Add(x);
          }
        } catch (Exception ex) {
          LogMsg(ex.Message);
        }        
      });
      aStr = aStr + $"{ss.Nl}Total Ada: {amountTotals.GetTotalAda()}{ss.Nl}";
      foreach (var y in amountTotals.Values) {
        try {
          if (y.Quantity.AsLong() > 0 && y.Unit != "lovelace") {
            var asset = _assetsService!.Assets.FindByUnit(y.Unit);
            var unit = y.Unit;
            var quantity = _assetsService!.AsFormatedQuantity(y);
            if (asset != null) {
              unit = asset.AssetName.AsHexToUtf8();
              aStr = aStr + $"{ss.Nl} Unit: {unit}{ss.Nl} Quantity:{quantity}{ss.Nl}";
            } else {
              aStr = aStr + $"{ss.Nl}Asset missing{ss.Nl} Unit: {unit}{ss.Nl} Quantity:{quantity}{ss.Nl}";
            }
          }
        } catch (Exception ex) {
          LogMsg(ex.Message);
        }        
      }
      return aStr;
    }

    private string AssetDetails(string assetFingerprint) {
      Assets assets = _assetsService!.Assets;
      var asset = assets.FindByFingerPrint(assetFingerprint);
      if (asset == null) return " Asset not found. ";
      return $"{ss.Nl}  Asset: {asset.AssetName}{ss.Nl}" +
        $"  PolicyId: {asset.PolicyId}{ss.Nl}" +
        $"  AssetName: {asset.AssetName.AsHexToUtf8()} {asset.AssetName}{ss.Nl}" +
        $"  FingerPrint: {asset.FingerPrint}{ss.Nl}" +
        $"  Quantity: {asset.Quantity}{ss.Nl}" +
        $"  Decimals: {asset.Decimals}{ss.Nl}" +
        $"  Name: {asset.Name}{ss.Nl}" +
        $"  Ticker: {asset.Ticker}{ss.Nl}" +
        $"  Url: {asset.Url}{ss.Nl} {ss.Nl}";
    }

    private void vrStakes_SelectionChanged(object sender, EventArgs e) {
      btnRemoveStake.Enabled = ((vrStakes.RowCount > 0) && (vrStakes.SelectedRows.Count > 0));
    }

    private void btnRemoveStake_Click(object sender, EventArgs e) {
      vrStakes.SelectedRows.Cast<DataGridViewRow>().ToList().ForEach(x => {
        string? stakeAddress = x.Cells[0].Value.ToString();
        if (stakeAddress != null) _addressService!.RemoveStakeAddress(stakeAddress);
      });
      LoadtvExplore();
    }

    private void cbShowEmptyAddr_CheckedChanged(object sender, EventArgs e) {
      LoadtvExplore();
    }


  }

  public static class ss { 
    public static string Nl { get { return Environment.NewLine; } }
    
  }

}