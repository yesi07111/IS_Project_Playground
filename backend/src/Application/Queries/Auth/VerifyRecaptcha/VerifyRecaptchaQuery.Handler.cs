using FastEndpoints;
using Playground.Application.Responses;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;

namespace Playground.Application.Queries.Auth.VerifyRecaptcha
{
    public class VerifyRecaptchaQueryHandler : CommandHandler<VerifyRecaptchaQuery, RecaptchaVerificationResponse>
    {
        private readonly string _secretKey;
        private readonly string _siteKey;
        private readonly IHttpClientFactory _httpClientFactory;

        public VerifyRecaptchaQueryHandler(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _secretKey = configuration["Recaptcha:SecretKey"]!;
            _siteKey = configuration["Recaptcha:SiteKey"]!;
            _httpClientFactory = httpClientFactory;
        }
        public override async Task<RecaptchaVerificationResponse> ExecuteAsync(VerifyRecaptchaQuery query, CancellationToken ct = default)
        {
            var client = _httpClientFactory.CreateClient();

            // Crear el objeto con el token y siteKey
            var objeto = new
            {
                @event = new
                {
                    token = query.Token,
                    siteKey = _siteKey
                }
            };

            // Serializar el objeto a JSON con la primera letra en minúscula
            var jsonContent = JsonConvert.SerializeObject(objeto, new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            });

            // Configurar el contenido de la solicitud
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                // Enviar solicitud a la API de reCAPTCHA
                var response = await client.PostAsync(
                    $"https://recaptchaenterprise.googleapis.com/v1/projects/playgroundis-071-1737150449660/assessments?key=AIzaSyBRhebC0It3Pvipt1fgMmPE7IK75B-7p4c",
                    content
                );

                if (!response.IsSuccessStatusCode)
                {
                    return new RecaptchaVerificationResponse(false, response.Content.ToString()!);
                }

                var jsonString = await response.Content.ReadAsStringAsync(ct);

                var jsonData = JObject.Parse(jsonString);

                // Verificar si el token es válido
                var tokenProperties = jsonData["tokenProperties"];
                if (tokenProperties?["valid"]?.Value<bool>() == true)
                {
                    // Verificar hostname y acción
                    var hostname = tokenProperties["hostname"]?.Value<string>();
                    var action = tokenProperties["action"]?.Value<string>();

                    if (hostname == "localhost" && action == "register")
                    {
                        // Verificar puntuación en riskAnalysis
                        var score = jsonData["riskAnalysis"]?["score"]?.Value<double>() ?? 0.0;

                        if (score >= 0.6) // Ajusta el umbral según tus necesidades
                        {
                            return new RecaptchaVerificationResponse(true, "CAPTCHA verificado con éxito.");
                        }
                        else
                        {
                            return new RecaptchaVerificationResponse(false, "La puntuación de CAPTCHA es demasiado baja.");
                        }
                    }
                    else
                    {
                        return new RecaptchaVerificationResponse(false, "Hostname o acción no válidos.");
                    }
                }
                else
                {
                    var invalidReason = tokenProperties?["invalidReason"]?.ToString() ?? "Sin razón de invalidez.";
                    return new RecaptchaVerificationResponse(false, "Verificación de CAPTCHA fallida.");
                }
            }
            catch (HttpRequestException httpEx)
            {
                return new RecaptchaVerificationResponse(false, $"Error HTTP: {httpEx.Message}");
            }
            catch (Exception ex)
            {
                return new RecaptchaVerificationResponse(false, $"Error inesperado: {ex.Message}");
            }
        }
    }



    public class Event
    {
        public string token { get; set; }
        public string siteKey { get; set; }

        public Event(string token, string sitekey)
        {
            this.token = token;
            this.siteKey = sitekey;
        }
    }

    public class Objeto
    {
        public Event Event { get; set; }

        public Objeto(Event evento)
        {
            Event = evento;
        }
    }
}
