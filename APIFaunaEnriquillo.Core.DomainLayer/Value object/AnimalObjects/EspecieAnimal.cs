using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIFaunaEnriquillo.Core.DomainLayer.Value_object.AnimalObjects
{
    public class EspecieAnimal
    {
        public string Value { get; }
        public EspecieAnimal(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("La especie no puede estar vacío.");

            value = value.Trim();

            if (value.Length > 100)
                throw new ArgumentException("La especie no puede tener más de 100 caracteres.");

            if (!EsEspecieValido(value))
                throw new ArgumentException("La especie contiene caracteres inválidos.");

            Value = FormatearEspecie(value);
        }

        private bool EsEspecieValido(string especie)
        {
            foreach (char c in especie)
            {
                if (!char.IsLetter(c) && c != ' ' && c != '-')
                    return false;
            }
            return true;
        }

        private string FormatearEspecie(string especie)
        {
            if (string.IsNullOrEmpty(especie)) return especie;

            return char.ToUpper(especie[0]) + especie.Substring(1).ToLower();
        }

        public override bool Equals(object? obj)
        {
            if (obj is EspecieAnimal other)
            {
                return Value == other.Value;
            }
            return false;
        }

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString() => Value;


    }
}
