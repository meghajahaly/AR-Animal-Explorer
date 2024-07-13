using System;
using System.IO;
using Google.Cloud.TextToSpeech.V1;
using Google.Apis.Auth.OAuth2;
using Grpc.Auth;
using UnityEngine;

public class GoogleTextToSpeech : MonoBehaviour
{
    private TextToSpeechClient client;

    void Start()
    {
        // Load the credentials from the JSON key file
        string credentialsPath = Path.Combine(Application.dataPath, "ar-animal-explorer-e19a8a7731d6.json");
        GoogleCredential googleCredential;
        using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            googleCredential = GoogleCredential.FromStream(stream).CreateScoped(TextToSpeechClient.DefaultScopes);
        }

        // Create the client using the credentials
        client = new TextToSpeechClientBuilder
        {
            ChannelCredentials = googleCredential.ToChannelCredentials()
        }.Build();
    }

    public void SynthesizeSpeech(string text)
    {
        var input = new SynthesisInput
        {
            Text = text
        };

        var voiceSelection = new VoiceSelectionParams
        {
            LanguageCode = "en-US",
            SsmlGender = SsmlVoiceGender.Female
        };

        var audioConfig = new AudioConfig
        {
            AudioEncoding = AudioEncoding.Mp3
        };

        var response = client.SynthesizeSpeech(input, voiceSelection, audioConfig);

        // Save the audio to a file or play it directly
        string outputPath = Path.Combine(Application.dataPath, "speech.mp3");
        using (var output = File.Create(outputPath))
        {
            response.AudioContent.WriteTo(output);
            Debug.Log("Audio content written to file: " + outputPath);
        }
    }
}
