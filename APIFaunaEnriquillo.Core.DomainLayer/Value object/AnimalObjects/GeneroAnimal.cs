using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIFaunaEnriquillo.Core.DomainLayer.Value_object.AnimalObjects
{
    public class GeneroAnimal
    {
        public string Value { get;}
        public GeneroAnimal(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("El genero no puede estar vacío.");

            value = value.Trim();

            if (value.Length > 100)
                throw new ArgumentException("El genero no puede tener más de 100 caracteres.");

            if (!EsGeneroValido(value))
                throw new ArgumentException("El genero contiene caracteres inválidos.");

            Value = FormatearGenero(value);
        }



        private bool EsGeneroValido(string genero)
        {
            foreach (char c in genero)
            {
                if (!char.IsLetter(c) && c != ' ' && c != '-')
                    return false;
            }
            return true;
        }

        private string FormatearGenero(string genero)
        {
            if (string.IsNullOrEmpty(genero)) return genero;

            return char.ToUpper(genero[0]) + genero.Substring(1).ToLower();
        }

        public override bool Equals(object? obj)
        {
            if (obj is GeneroAnimal other)
            {
                return Value == other.Value;
            }
            return false;
        }

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString() => Value;



    }
}
