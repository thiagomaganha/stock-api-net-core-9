using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Dtos
{
    public class StockDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(5, ErrorMessage = "Stock Symbol Max length is 5")]
        [MinLength(3, ErrorMessage = "Stock Symbol Min length is 3")]
        public string Symbol { get; set; } = string.Empty;

        [Required]
        [MaxLength(100, ErrorMessage = "Company Name Max length is 100")]
        [MinLength(3, ErrorMessage = "Company Name Min length is 3")]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100, ErrorMessage = "Industry Max length is 100")]
        [MinLength(3, ErrorMessage = "Industry Min length is 3")]
        public string Industry { get; set; } = string.Empty;
        [Required]
        public decimal Purchase { get; set; }
        public decimal Dividend { get; set; }
        public long MarketCap { get; set; }
        public List<CommentDto> Comments { get; set; } = [];
    }
}