using System;

namespace AppraisalSystem.Models
{
    public class UniqueNumber
    {
        public static string GenerateUniqueNumber()
        {
            string number = "OB" + String.Format("{0:d9}", (DateTime.Now.Ticks / 10) % 1000000000);
            return number;
        }
    }
}