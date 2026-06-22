using CepHub.Models.DTOs;

namespace CepHub.Utils
{
    public class Logger
    {
        private readonly string _caminho;
        public Logger()
        {
            var dataDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");

            Directory.CreateDirectory(dataDirectory);

            _caminho = Path.Combine(dataDirectory, "RegistroLogs.txt");
        }

        public void GravarLog(ViaCepDto endereco)
        {
            string log = $"[{DateTime.Now:dd-MM-yyyy HH:mm:ss}] " + $"{endereco.Cep} | {endereco.Logradouro}, {endereco.Bairro}, {endereco.Localidade} - {endereco.Uf} " +
                                $"(Ibge: {endereco.Ibge} | Gia: {endereco.Gia} | Ddd: {endereco.Ddd} | Siafi: {endereco.Siafi})";

            using var streamWriter = new StreamWriter(_caminho, append: true);
            streamWriter.WriteLine(log);
        }

        public IEnumerable<string> ObterLogs()
        {
            if (!File.Exists(_caminho))
                return Enumerable.Empty<string>();
            return File.ReadAllLines(_caminho);
        }
    }
}