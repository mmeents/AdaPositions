using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaPositions.Core.Entities.Operations;
using MessagePack;

namespace AdaPositions.Core.Entities.Assets
{

    [MessagePackObject]
    public class AssetEntity : Item
    {
        public AssetEntity() { }
        public AssetEntity(long id) : base(id) { }

        [Key(1)]
        public string Asset { get; set; } = "";

        [Key(2)]
        public string PolicyId { get; set; } = "";

        [Key(3)]
        public string AssetName { get; set; } = "";

        [Key(4)]
        public string FingerPrint { get; set; } = "";

        [Key(5)]
        public string Quantity { get; set; } = "";

        [Key(6)]
        public int Decimals { get; set; } = 0;

        [Key(7)]
        public string Name { get; set; } = "";

        [Key(8)]
        public string Ticker { get; set; } = "";

        [Key(9)]
        public string Desc { get; set; } = "";

        [Key(10)]
        public string Url { get; set; } = "";

        [Key(11)]
        public string Logo { get; set; } = "";
    }

}
