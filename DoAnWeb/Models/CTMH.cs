using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAnWeb.Models
{
    public class CTMH
    {
        public MATHANG Mh { get; set; }
        public List<THONGSOKYTHUAT> tskts { get; set; }
        public List<BINHLUAN> BLs { get; set; }
        public List<DANHGIA> DGs { get; set; }
        public List<ANHMINHHOA> AMHs { get; set; }
    }
}