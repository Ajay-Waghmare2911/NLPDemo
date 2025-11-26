using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLPDemo.Database
{
    public class tblVillageInfo
    {
        public int Id { get; set; }

        public string VillageName { get; set; } = string.Empty;

        public int TotalPopulation { get; set; }

        public int Male { get; set; }

        public int Female { get; set; }

        public int Households { get; set; }

        public double LiteracyRate { get; set; }

        public int SC { get; set; }

        public int ST { get; set; }
    }
}
