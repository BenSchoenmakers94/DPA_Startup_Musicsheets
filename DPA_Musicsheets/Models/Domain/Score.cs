using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models.Domain
{
    public class Score
    {
        public Score()
        {
            this.staffsInScore = new List<Staff>();
        }

        public List<Staff> staffsInScore { get; private set; }

        public void addStaff(Staff staffToAdd)
        {
            staffsInScore.Add(staffToAdd);
        }
    }
}
