using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MiniApi.Models
{
    public class SampleModel
    {
        /// <summary>
        /// Calculate result
        /// </summary>
        public int Result { get; set; }

        /// <summary>
        /// Calculate formula
        /// </summary>
        public string Formula { get; set; }

        public async Task AddAsync(uint num)
        {
            if (!string.IsNullOrWhiteSpace(Formula))
            {
                Formula += " + ";
            }
            Formula += num.ToString();
            Result += (int)num;
            await Task.Delay(0);
        }

        public async Task SubtractAsync(uint num)
        {
            if (!string.IsNullOrWhiteSpace(Formula))
            {
                Formula += " - ";
            }
            Formula += num.ToString();
            Result -= (int)num;
            await Task.Delay(0);
        }

        public async Task MultiplyAsync(uint num)
        {
            if (Formula.Contains(" "))
            {
                Formula = "(" + Formula + ")";
            }
            if (!string.IsNullOrWhiteSpace(Formula))
            {
                Formula += " × ";
            }
            Formula += num.ToString();
            Result *= (int)num;
            await Task.Delay(0);
        }

        public async Task DivideAsync(uint num)
        {
            if (Formula.Contains(" "))
            {
                Formula = "(" + Formula + ")";
            }
            if (!string.IsNullOrWhiteSpace(Formula))
            {
                Formula += " ÷ ";
            }
            Formula += num.ToString();
            Result /= (int)num;
            await Task.Delay(0);
        }
    }
}