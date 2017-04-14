using System;

namespace AppraisalSystem.Models
{
    public class UniqueNumbers
    {
        
        public static string GeneratePassword()
        {
            return String.Format("{0:d6}", (DateTime.Now.Ticks / 60) % 6000000000);
        }
    }
}