using AdaPositions.Core.Entities;
using AdaPositions.Core.Extensions;
using AdaPositions.Core.Interfaces;
using Blockfrost.Api;
using Blockfrost.Api.Extensions;
using Blockfrost.Api.Models;
using Blockfrost.Api.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPositions.Core.Services {
  public class BlockfrostService {

    private readonly ILogMsg _logMsg;
    private readonly ServiceProvider BlockFrostServiceProvider;
    private readonly string _network = "mainnet";
    private string _blockFrostApiKey;

    private IAssetService? _assetService = null;
    private IAccountsService? _accountsService = null;
    private IAddressesService? _addressService = null;
    private IMetricsService? _metricsService = null;    
    
    private UsageMetrics _usageMetrics = new UsageMetrics();
    private long _lastMetricTime = 0;
    private long _lastAccountCalls = 0;
    public DateTime LastToday { get{
      if (_lastMetricTime != 0) {
        return DateTimeOffset.FromUnixTimeSeconds(_lastMetricTime).DateTime;
      }
      return DateTime.Now;
    } }
    public long LastCallsCount { get { return _lastAccountCalls; } }
    public long DailyMaxCalls { get { return 50000; } }
    public bool IsOverDailyMaxCalls { get { return _lastAccountCalls > DailyMaxCalls; } }
    public IAssetService? AssetService {
      get {
        if (_assetService == null) {
          _assetService = BlockFrostServiceProvider.GetService<IAssetService>()!;
        }
        return _assetService;
      }
    }

    public IAccountsService? AccountsService { get {
        if (_accountsService == null) {
          _accountsService = BlockFrostServiceProvider.GetService<IAccountsService>()!;
        }
        return _accountsService;
    } }
    public IAddressesService? AddressService { get { 
        if (_addressService == null){
          _addressService = BlockFrostServiceProvider.GetService<IAddressesService>(); 
        }
        return _addressService;
    }}
    public IMetricsService? MetricsService {
      get {
        if (_metricsService == null) {
          _metricsService = BlockFrostServiceProvider.GetService<IMetricsService>();
        }
        return _metricsService;
      }
    }

    public BlockfrostService( string blockFrostApiKey, ILogMsg ErrorOut) {
      _logMsg = ErrorOut;
      _blockFrostApiKey = blockFrostApiKey;
      BlockFrostServiceProvider = new ServiceCollection().AddBlockfrost(_network, blockFrostApiKey).BuildServiceProvider();      
    }

    public async Task<int> CaptureMetrics() {
      try {
        var metrics = await MetricsService!.GetMetricsAsync();
        var latestMetric = metrics.OrderByDescending(metric => metric.Time).FirstOrDefault();
        if (latestMetric != null && latestMetric.Time > _lastMetricTime) {
          _lastMetricTime = latestMetric.Time;
          _lastAccountCalls = latestMetric.Calls;
        }
        _usageMetrics = metrics.ToUsageMetrics();
      } catch (Exception ex) {
        _logMsg.LogMsg(ex.Message);
        return 0;
      }      
      return 1;
    }

    public async Task<AddressContentResponse?> GetAddressDetailsAsync(string address) {
      try {     
        _lastAccountCalls++;        
        AddressContentResponse ACR = await AddressService!.GetAddressesAsync(address);             
        
        return ACR;

      } catch (Exception ex) {
        _logMsg.LogMsg(ex.Message);
      }
      return null;
    }

    public async Task<AccountAddressesContentResponseCollection?> GetAccountAddressesAsync(string stakeAddress, int pageNumber) {
      try {
        _lastAccountCalls++;
        AccountAddressesContentResponseCollection ACR = await AccountsService!.GetAddressesAsync(stakeAddress, 100, pageNumber, ESortOrder.Asc );
        return ACR;
      } catch (Exception ex) {
        _logMsg.LogMsg(ex.Message);
      }
      return null;
    }

    public async Task<Blockfrost.Api.AssetResponse?> GetAssetsAsync(string asset) {
      try {
        _lastAccountCalls++;
        var AR = await AssetService!.AssetsAsync(asset);
        return AR;
      } catch (Exception ex) {
        _logMsg.LogMsg(ex.Message);
      }
      return null;
    }

    

    public void Dispose() {
      BlockFrostServiceProvider.Dispose();
    }

  }
}
