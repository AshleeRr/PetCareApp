﻿namespace PetCareApp.Core.Domain.Entities;

public class Receta
{
    public int Id { get; set; }

    public DateTime Fecha { get; set; }

    public string? Observaciones { get; set; }

    public ICollection<RecetaMedicamento> RecetaMedicamentos { get; set; } = new List<RecetaMedicamento>();
}
