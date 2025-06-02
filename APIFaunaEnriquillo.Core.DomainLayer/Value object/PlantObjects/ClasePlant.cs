using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIFaunaEnriquillo.Core.DomainLayer.Value_object.AnimalObjects;

namespace APIFaunaEnriquillo.Core.DomainLayer.Value_object.PlantObjects
{
    public class ClasePlant
    {

        public string Value { get; }

        public ClasePlant(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("La clase no puede estar vacío.");

            value = value.Trim();

            if (value.Length > 100)
                throw new ArgumentException("La clase no puede tener más de 100 caracteres.");

            if (!EsClaseValido(value))
                throw new ArgumentException("La clase contiene caracteres inválidos.");

            Value = FormatearClase(value);

        }

        private bool EsClaseValido(string familia)
        {
            foreach (char c in familia)
            {
                if (!char.IsLetter(c) && c != ' ' && c != '-')
                    return false;
            }
            return true;
        }

        private string FormatearClase(string clase)
        {
            if (string.IsNullOrEmpty(clase)) return clase;

            return char.ToUpper(clase[0]) + clase.Substring(1).ToLower();
        }

        public override bool Equals(object? obj)
        {
            if (obj is ClasePlant other)
            {
                return Value == other.Value;
            }
            return false;
        }

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString() => Value;

    }
}
