using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnipeIT_AssetTagTransporter
{
    public class Asset
    {
        public string AssetName {  get; set; }
        public string AssetTag { get; set; }

        public Asset(string assetName, string assetTag) 
        {
            AssetName = assetName;
            AssetTag = assetTag;
        }
    }
}
