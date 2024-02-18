using System;
using System.Collections.Generic;

namespace Acortador_Web_App.Models;

public partial class Acortador
{
    public string Id { get; set; } = null!;

    public string Link { get; set; } = null!;

    public DateTime? Lasttime { get; set; }

    public string? UserId { get; set; }

    public virtual Usuario? User { get; set; }
}
