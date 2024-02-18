using System;
using System.Collections.Generic;

namespace Acortador_Web_App.Models;

public partial class Usuario
{
    public string Id { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool IsAdministrador { get; set; } = false;

    public virtual ICollection<Acortador> Acortadors { get; set; } = new List<Acortador>();
}
