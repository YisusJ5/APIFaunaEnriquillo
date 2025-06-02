using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIFaunaEnriquillo.Core.DomainLayer.Value_object.AnimalObjects
{
    public class FiloAnimal
    {
        public string Value { get;}
        public FiloAnimal(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("El filo no puede estar vacío.");

            value = value.Trim();

            if (value.Length > 250)
                throw new ArgumentException("El filo no puede tener más de 250 caracteres.");

            if (!EsFiloValido(value))
                throw new ArgumentException("El filo contiene caracteres inválidos.");

            Value = FormatearFilo(value);
        }

        private bool EsFiloValido(string filo)
        {
            foreach (char c in filo)
            {
                if (!char.IsLetter(c) && c != ' ' && c != '-')
                    return false;
            }
            return true;
        }

        private string FormatearFilo(string filo)
        {
            if (string.IsNullOrEmpty(filo)) return filo;

            return char.ToUpper(filo[0]) + filo.Substring(1).ToLower();
        }

        public override bool Equals(object? obj)
        {
            if (obj is FiloAnimal other)
            {
                return Value == other.Value;
            }
            return false;
        }

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString() => Value;

    }
}
