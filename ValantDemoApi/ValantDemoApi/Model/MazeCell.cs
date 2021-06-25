using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ValantDemoApi.Model
{
    public class MazeCell
    {
      public char Symbol { get; set; }
      public int Row { get; set; }
      public int Col { get; set; }
      public bool North { get; set; }
      public bool West { get; set; }
      public bool East { get; set; }
      public bool South { get; set; }
      public bool currentLocation { get; set;}

    }
}
