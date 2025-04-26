using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blockfrost.Api.Models;

namespace AdaPositions.Core.Entities {
  public class UsageMetrics : ConcurrentDictionary<long, MetricsResponse> {
    public UsageMetrics() : base() { }

  }

  public static class UsageMetricsExt {
    public static UsageMetrics ToUsageMetrics(this MetricsResponseCollection metrics) {
      var usageMetrics = new UsageMetrics();
      foreach (var metric in metrics) {
        usageMetrics[metric.Time] = metric;                      
      }
      return usageMetrics;
    }
  }
}
