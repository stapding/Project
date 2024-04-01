using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class Family
    {
        public string ID_family { get; set; } 
        public List<string> MembersID { get; set; }
        public Family()
        {
            MembersID = new List<string>();
            string uuid = Guid.NewGuid().ToString();
            ID_family = uuid;
        }
    }
}
