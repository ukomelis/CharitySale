﻿namespace CharitySale.Api.Entities;

public class Sale
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime SaleDate { get; set; }
    public decimal TotalAmount { get; set; }
    
    public ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
}
