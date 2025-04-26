using Blockfrost.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Blockfrost.Api;
using static System.Net.Mime.MediaTypeNames;
using AdaPositions.Core.Entities.Assets;
using System.Xml.Linq;

namespace AdaPositions.Core.Extensions {
  public static class BlockfrostExts {  
    public static List<Amount> ToAmountList(this object amount) {
      if (amount == null) {
        return new List<Amount>();
      }
      var list = JsonSerializer.Deserialize<List<Amount>>($"{amount}");
      return list ?? new List<Amount>();
    }

    public static System.Drawing.Image? ToImage(this string base64String) {
      if (string.IsNullOrEmpty(base64String))
        return null;

      try {
        byte[] imageBytes = Convert.FromBase64String(base64String);
        using (var ms = new MemoryStream(imageBytes)) {
          return System.Drawing.Image.FromStream(ms);
        }
      } catch (FormatException) {        
        return null;
      }
    }



  }
}
