using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Domain.Entities
{
    public class PropertyImage
    {
        public int IdPropertyImage { get; set; }
        public byte[] File { get; set; } 
        public bool Enabled { get; set; } = true;

        public int IdProperty { get; set; }
        public Property Property { get; set; }
    }
}
