using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DoAnWeb.Models
{
    public class PlaceContext : DbContext
    {
        public DbSet<THANHPHO> THANHPHOs { get; set; }
        public DbSet<QUANHUYEN> QUANHUYENs { get; set; }
    }
}