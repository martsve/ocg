using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class CardViewModel
    {
        public string Colors { get; set; } = "";
        public string Name { get; set; } = "";
        public string Cost { get; set; } = "";

        public string BG { get; set; } = "";
        public string Border { get; set; } = "";
        public string Type { get; set; } = "";
        public string Rarity { get; set; } = "";
        public string SetCode { get; set; } = "";
        public string Text { get; set; } = "";
        public string Power { get; set; } = "";
        public string Thoughness { get; set; } = "";
        public string Number { get; set; } = "";
        public string Artist { get; set; } = "";
        public string RarityCode { get; set; } = "";

        public string PTPadding => Power.Length > 0 ? "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" : "";
    }
}