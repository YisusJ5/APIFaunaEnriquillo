using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIFaunaEnriquillo.Core.DomainLayer.Value_object.AnimalObjects
{
    public class FamiliaAnimal
    {
        public string Value { get;}
        public FamiliaAnimal(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("La familia no puede estar vacío.");

            value = value.Trim();

            if (value.Length > 200)
                throw new ArgumentException("La familia no puede tener más de 200 caracteres.");

            if (!EsFamiliaValido(value))
                throw new ArgumentException("La familia contiene caracteres inválidos.");

            Value = FormatearFamilia(value);

        }
        private bool EsFamiliaValido(string familia)
        {
            foreach (char c in familia)
            {
                if (!char.IsLetter(c) && c != ' ' && c != '-')
                    return false;
            }
            return true;
        }

        private string FormatearFamilia(string familia)
        {
            if (string.IsNullOrEmpty(familia)) return familia;

            return char.ToUpper(familia[0]) + familia.Substring(1).ToLower();
        }

        public override bool Equals(object? obj)
        {
            if (obj is FamiliaAnimal other)
            {
                return Value == other.Value;
            }
            return false;
        }

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString() => Value;


    }
}
