using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAnWeb.Models
{
    public class CTMH
    {
        public CTMH()
        {
        }

        public CTMH(string mAMH, byte sOLUONG, int tHANHTIEN)
        {
            MAMH = mAMH;
            SOLUONG = sOLUONG;
            THANHTIEN = tHANHTIEN;
        }

        public string MAMH { get; set; }
        public byte SOLUONG { get; set; }
        public int THANHTIEN { get; set; }
    }
}