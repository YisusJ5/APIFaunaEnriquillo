using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIFaunaEnriquillo.Core.DomainLayer.Value_object.AnimalObjects;

namespace APIFaunaEnriquillo.Core.DomainLayer.Value_object.PlantObjects
{
    public class OrdenPlant
    {
        public string Value { get;}
        public OrdenPlant(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("El orden no puede estar vacío.");

            value = value.Trim();

            if (value.Length > 200)
                throw new ArgumentException("El orden no puede tener más de 200 caracteres.");

            if (!EsOrdenValido(value))
                throw new ArgumentException("El orden contiene caracteres inválidos.");

            Value = FormatearOrden(value);

        }

        private bool EsOrdenValido(string subEspecie)
        {
            foreach (char c in subEspecie)
            {
                if (!char.IsLetter(c) && c != ' ' && c != '-')
                    return false;
            }
            return true;
        }

        private string FormatearOrden(string subEspecie)
        {
            if (string.IsNullOrEmpty(subEspecie)) return subEspecie;

            return char.ToUpper(subEspecie[0]) + subEspecie.Substring(1).ToLower();
        }

        public override bool Equals(object? obj)
        {
            if (obj is OrdenPlant other)
            {
                return Value == other.Value;
            }
            return false;
        }

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString() => Value;


    }
}
