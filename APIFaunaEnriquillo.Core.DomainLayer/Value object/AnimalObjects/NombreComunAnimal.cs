using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIFaunaEnriquillo.Core.DomainLayer.Value_object.AnimalObjects
{
    public class NombreComunAnimal
    {
        private NombreComunAnimal nombreComun;

        public string Value { get; }

            public NombreComunAnimal(string value)
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El nombre común no puede estar vacío.");

                value = value.Trim();

                if (value.Length > 250)
                    throw new ArgumentException("El nombre común no puede tener más de 250 caracteres.");

                if (!EsNombreValido(value))
                    throw new ArgumentException("El nombre común contiene caracteres inválidos.");

                Value = FormatearNombre(value);

        }

        public NombreComunAnimal(NombreComunAnimal nombreComun)
        {
            this.nombreComun = nombreComun;
        }

        private bool EsNombreValido(string nombre)
            {
                foreach (char c in nombre)
                {
                    if (!char.IsLetter(c) && c != ' ' && c != '-')
                        return false;
                }
                return true;
            }

            private string FormatearNombre(string nombre)
            {
                if (string.IsNullOrEmpty(nombre)) return nombre;

                return char.ToUpper(nombre[0]) + nombre.Substring(1).ToLower();
            }


            public override bool Equals(object? obj)
            {
                if (obj is NombreComunAnimal other)
                {
                    return Value == other.Value;
                }
                return false;
            }

            public override int GetHashCode() => Value.GetHashCode();

            public override string ToString() => Value;
        

    }
}
