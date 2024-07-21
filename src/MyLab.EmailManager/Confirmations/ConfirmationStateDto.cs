using System.Text.Json.Serialization;

#if CLIENTPROJ
namespace MyLab.EmailManager.Client.Confirmations
#else
namespace MyLab.EmailManager.Confirmations
#endif
{
    public class ConfirmationStateDto
    {
        [JsonPropertyName("confirmed")]
        public bool Confirmed { get; set; }
        [JsonPropertyName("step")]
        public ConfirmationStateStep Step { get; set; }
    }
}
