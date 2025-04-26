using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPositions.Core.Entities {

    public static class SettingCol
    {
        public static string BlockFrostApiKey { get { return "BlockFrostApiKey"; } }
        public static string ApiKeyVerified { get { return "ApiKeyVerified"; } }
    }

    public class Settings : ConcurrentDictionary<string, SettingProperty> {
    private readonly object _lock = new object();

    public Settings() : base() { }

    public Settings(ICollection<SettingProperty> asList) : base() {
      AsList = asList;
    }

    public virtual Boolean Contains(String key) {
      try {
        return base.ContainsKey(key);
      } catch {
        return false;
      }
    }

    public virtual new SettingProperty this[string key] {
      get {
        lock (_lock) {
          if (!Contains(key)) base[key] = new SettingProperty() { Key = key, Value = "" };
          return base[key];
        }
      }
      set {
        lock (_lock) {
          if (value != null) {
            base[key] = value;
          } else {
            if (Contains(key)) {
              _ = base.TryRemove(key, out _);
            }
          }
        }
      }
    }

    public virtual void Remove(string key) {
      lock (_lock) {
        if (Contains(key)) {
          _ = base.TryRemove(key, out _);
        }
      }
    }

    public ICollection<SettingProperty> AsList {
      get {
        lock (_lock) {
          return base.Values.ToList();
        }
      }
      set {
        if (value == null) throw new ArgumentNullException(nameof(value));
        lock (_lock) {
          base.Clear();
          foreach (var x in value) {
            this[x.Key] = x;
          }
        }
      }
    }

    public Settings Clone() {      
        var clone = new Settings(this.AsList);
        return clone;      
    }
  }

}
