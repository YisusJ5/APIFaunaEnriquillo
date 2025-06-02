using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIFaunaEnriquillo.Core.DomainLayer.Value_object.AnimalObjects;

namespace APIFaunaEnriquillo.Core.DomainLayer.Value_object.PlantObjects
{
    public class SubEspeciePlant
    {

        public string Value { get; }

        public SubEspeciePlant(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("La sub especie no puede estar vacío.");

            value = value.Trim();

            if (value.Length > 100)
                throw new ArgumentException("La sub especie no puede tener más de 100 caracteres.");

            if (!EsSubEspecieValido(value))
                throw new ArgumentException("La sub especie contiene caracteres inválidos.");

            Value = FormatearSubEspecie(value);

        }

        private bool EsSubEspecieValido(string subEspecie)
        {
            foreach (char c in subEspecie)
            {
                if (!char.IsLetter(c) && c != ' ' && c != '-')
                    return false;
            }
            return true;
        }

        private string FormatearSubEspecie(string subEspecie)
        {
            if (string.IsNullOrEmpty(subEspecie)) return subEspecie;

            return char.ToUpper(subEspecie[0]) + subEspecie.Substring(1).ToLower();
        }

        public override bool Equals(object? obj)
        {
            if (obj is SubEspeciePlant other)
            {
                return Value == other.Value;
            }
            return false;
        }

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString() => Value;

    }
}
