﻿namespace ProyectoFinalDAMAgil.Models.Admin
{
    public class AsignaturaModel
    {
        public int IdAsignatura { get; set; }

        public string NombreAsignatura { get; set; }

        public int Curso { get; set; }

        public override string ToString()
        {
            return "IdAsignatura: "+IdAsignatura+" NombreAsignatura: "+NombreAsignatura+" Curso: "+Curso;
        }
    }
}
