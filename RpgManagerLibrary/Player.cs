using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgManagerLibrary
{
    public class Player
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }

        public override string ToString()
        {
            return $"{LastName.ToUpperInvariant()}, {FirstName}";
        }
    }
}
