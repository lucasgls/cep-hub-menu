using CepHub.Models.DTOs;
using CepHub.Utils;
using System.Text.Json;

namespace CepHub.Services
{
    public class CepService
    {
        private const string UrlViaCep = "https://viacep.com.br/ws/{0}/json/";

        private readonly HttpClient _httpClient;
        private readonly Logger _logger;
        private readonly CepNormalizer _normalizer;

        public CepService(HttpClient httpClient, Logger logger, CepNormalizer normalizer)
        {
            _httpClient = httpClient;
            _logger = logger;
            _normalizer = normalizer;
        }

        public async Task<ResponseEnderecoDto> ConsultarCepAsync(string cep)
        {
            var cepFormatado = _normalizer.Normalizar(cep);
            var url = string.Format(UrlViaCep, cepFormatado);

            var json = await ObterJsonAsync(url);

            var enderecoViaCep = Desserializar(json);

            var endereco = Transformar(enderecoViaCep);

            _logger.GravarLog(enderecoViaCep); 

            return endereco;
        }

        private async Task<string> ObterJsonAsync(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException e)
            {
                throw new HttpRequestException("Erro ao consultar CEP.", e);
            }
            catch (TaskCanceledException e)
            {
                throw new Exception("A requisição para consultar o CEP foi cancelada.", e);
            }
        }

        private ViaCepDto Desserializar(string json)
        {
            //desculpa a gambiarra dani
            json = json.Replace("\"erro\": \"true\"", "\"erro\": true");

            var endereco = JsonSerializer.Deserialize<ViaCepDto>(json);

            ValidarResposta(endereco);

            return endereco!;
        }

        private void ValidarResposta(ViaCepDto? endereco)
        {
            if (endereco == null)
                throw new ArgumentException("Resposta inválida.");

            if (endereco.Erro == true)
                throw new ArgumentException("CEP inválido.");
        }

        private ResponseEnderecoDto Transformar(ViaCepDto endereco)
        {
            return new ResponseEnderecoDto
            {
                Cep = endereco.Cep ?? string.Empty,
                Logradouro = endereco.Logradouro ?? string.Empty,
                Bairro = endereco.Bairro ?? string.Empty,
                Localidade = endereco.Localidade ?? string.Empty,
                Uf = endereco.Uf ?? string.Empty
            };
        }
    }
}