using System;
using System.Collections.Generic;

namespace CharitySale.Api.Entities;

public class Sale
{
    public int Id { get; set; }
    public DateTime SaleDate { get; set; }
    public decimal TotalAmount { get; set; }
    
    public ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
}
