using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GraphColouring
{
    public class GraphMap 
    {
        public int ID { get; private set; }
        public List<int> chlidren = new List<int>();
        public Color mapColor = new Color();
        public String mapColorString = "unknown";

        public GraphMap(int id)
        {
            ID = id;
        }
        //public GraphMap() { 
        //    chlidren = new List<int>();
        //    mapColor = new Color();
        //}
    }
}
