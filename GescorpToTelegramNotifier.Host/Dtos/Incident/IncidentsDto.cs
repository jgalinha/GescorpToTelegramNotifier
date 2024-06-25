using Newtonsoft.Json;

namespace GescorpToTelegramNotifier.Host.Dtos.Incident {
    public record IncidentsDetails(
        [property: JsonProperty("id")] string id,
        [property: JsonProperty("numero")] string numero,
        [property: JsonProperty("numero_cdos")] string numero_cdos,
        [property: JsonProperty("morada")] string morada,
        [property: JsonProperty("localidade_morada")] string localidade_morada,
        [property: JsonProperty("classificacao")] string classificacao,
        [property: JsonProperty("desc_classificacao")] string desc_classificacao,
        [property: JsonProperty("n_bombeiros")] string n_bombeiros,
        [property: JsonProperty("n_viaturas")] int n_viaturas,
        [property: JsonProperty("viaturas")] IReadOnlyList<List<string>> viaturas,
        [property: JsonProperty("n_doentes")] string n_doentes,
        [property: JsonProperty("data_hora_alerta")] string data_hora_alerta,
        [property: JsonProperty("data_hora_fim")] string data_hora_fim,
        [property: JsonProperty("hora_alerta")] string hora_alerta,
        [property: JsonProperty("hora_fim")] string hora_fim,
        [property: JsonProperty("finalizada")] string finalizada,
        [property: JsonProperty("sado_latitude_gps")] string sado_latitude_gps,
        [property: JsonProperty("sado_longitude_gps")] string sado_longitude_gps,
        [property: JsonProperty("ult_posit")] object ult_posit,
        [property: JsonProperty("cor_estado")] string cor_estado,
        [property: JsonProperty("estado")] string estado
    );

    public record IncidentsDto(
        [property: JsonProperty("incident")] IReadOnlyList<IncidentsDetails> incident
    );

}
