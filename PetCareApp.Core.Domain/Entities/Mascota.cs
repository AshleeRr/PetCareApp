﻿namespace PetCareApp.Core.Domain.Entities;

public class Mascota
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int Edad { get; set; }
    public decimal Peso { get; set; }
    public bool EstaCastrado { get; set; }
    public int DueñoId { get; set; }
    public int TipoMascotaId { get; set; }

    public Dueño? Dueño { get; set; }

    public ICollection<MascotaPruebasMedica> MascotaPruebasMedicas { get; set; } = new List<MascotaPruebasMedica>();

    public TipoMascota TipoMascota { get; set; }
}
