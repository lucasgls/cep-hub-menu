using System.Text.RegularExpressions;

namespace CepHub.Utils
{
    public class CepNormalizer
    {
        private static readonly Regex _regexNaoDigitos = new Regex(@"\D", RegexOptions.Compiled);
        public string Normalizar(string cep)
        { 
            if (string.IsNullOrWhiteSpace(cep))
                throw new ArgumentException("O CEP não pode ser nulo, conter espacos em branco ou vazio.");

            string cepFormatado = _regexNaoDigitos.Replace(cep, "");

            if(cepFormatado.Length != 8)
                throw new ArgumentException("O CEP deve conter exatamente 8 dígitos numéricos.");

            return cepFormatado;
        } 
    }
}