﻿namespace RecommendationNetwork.DTOs
{
    public class PurchaseIdDetailRequest
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int PurchaseId { get; set; }
        public float Quantity { get; set; }
        public float PriceForOnePiece { get; set; }
    }
}