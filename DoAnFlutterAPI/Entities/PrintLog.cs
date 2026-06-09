using System;
using System.Collections.Generic;

namespace DoAnFlutterAPI.Entities;

public partial class PrintLog
{
    public int PrintLogId { get; set; }

    public int OrderId { get; set; }

    public string? PrinterIp { get; set; }

    public bool Success { get; set; }

    public string? ErrorMsg { get; set; }

    public DateTime PrintedAt { get; set; }

    public virtual Order Order { get; set; } = null!;
}
