using System;
using System.Collections.Generic;

namespace DoAnFlutterAPI.Entities;

public partial class TabletConfig
{
    public int ConfigId { get; set; }

    public string DeviceId { get; set; } = null!;

    public string Role { get; set; } = null!;

    public int? TableId { get; set; }

    public DateTime SetupAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Table? Table { get; set; }
}
