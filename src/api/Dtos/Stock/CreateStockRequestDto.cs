using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace api.Dtos
{
    public class CreateStockRequestDto
    {
        [Required]
        [MaxLength(5, ErrorMessage = "Max length is 7")]
        [MinLength(3, ErrorMessage = "Min length is 3")]
        public string Symbol { get; set; } = string.Empty;

        [Required]
        [MaxLength(100, ErrorMessage = "Max length is 100")]
        [MinLength(3, ErrorMessage = "Min length is 3")]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100, ErrorMessage = "Max length is 100")]
        [MinLength(3, ErrorMessage = "Min length is 3")]
        public string Industry { get; set; } = string.Empty;
        [Required]        
        public decimal Purchase { get; set; }
        public decimal Dividend { get; set; }
        public long MarketCap { get; set; }
        public IFormFile Image { get; set; }
    }
}