using System;
using System.Collections.Generic;

namespace DoAnFlutterAPI.Entities;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int OrderId { get; set; }

    public string? ZaloPayOrderId { get; set; }

    public string? ZaloTransId { get; set; }

    public decimal Amount { get; set; }

    public string Method { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime PaidAt { get; set; }

    public string? WebhookPayload { get; set; }

    public virtual Order Order { get; set; } = null!;
}
