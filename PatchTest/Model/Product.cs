using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatchTest.Model
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int AvailableInStock { get; set; }
        public bool IsHighlighted { get; set; }
    }
}
