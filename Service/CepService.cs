using Agendado.Application.DTOs;
using Agendado.Interface.Service;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Agendado.Service
{
    public class CepService : ICepService
    {
        private readonly HttpClient _httpClient;

        public CepService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<DadosViaCepResponse?> VerificaCepAsync(string cep)
        {
            string cepLimpo = Regex.Replace(cep, @"\D", "");

            if (cepLimpo.Length != 8) return null;

            var response = await _httpClient.GetAsync($"https://viacep.com.br/ws/{cepLimpo}/json");

            if (!response.IsSuccessStatusCode) return null;

            var jsonString = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var dados = JsonSerializer.Deserialize<DadosViaCepResponse>(jsonString, options);
            
            if(dados == null) return null;

            if(dados.Erro) throw new Exception($"Erro: {dados.Erro}") ;

            return dados;
        }
    }
}
