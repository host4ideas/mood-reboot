using Microsoft.Azure.CognitiveServices.ContentModerator.Models;
using Microsoft.Azure.CognitiveServices.ContentModerator;
using MoodReboot.Models;

namespace MoodReboot.Services
{
    public class ServiceContentModerator
    {
        private readonly ContentModeratorClient client;

        public ServiceContentModerator(IConfiguration configuration)
        {
            string key = configuration.GetValue<string>("AzureKeys:ContentModeratorKey");
            string endpoint = configuration.GetValue<string>("AzureKeys:ContentModeratorEndpoint");

            this.client = this.Authenticate(key, endpoint);
        }

        // Instantiate client objects with your endpoint and key
        public ContentModeratorClient Authenticate(string key, string endpoint)
        {
            ContentModeratorClient client = new ContentModeratorClient(new ApiKeyServiceClientCredentials(key));
            client.Endpoint = endpoint;

            return client;
        }

        /*
         * IMAGE MODERATION
         * This example moderates images from URLs.
         */
        public List<EvaluationData> ModerateImages(string urlFile)
        {
            // Create an object to store the image moderation results.
            List<EvaluationData> evaluationData = new List<EvaluationData>();

            using (this.client)
            {
                // Read image URLs from the input file and evaluate each one.
                using (StreamReader inputReader = new StreamReader(urlFile))
                {
                    while (!inputReader.EndOfStream)
                    {
                        string? line = inputReader.ReadLine()?.Trim();

                        if (line != null && line != string.Empty)
                        {
                            var imageUrl = new BodyModel("URL", line.Trim());

                            var imageData = new EvaluationData
                            {
                                ImageUrl = imageUrl.Value,

                                // Evaluate for adult and racy content.
                                ImageModeration =
                            client.ImageModeration.EvaluateUrlInput("application/json", imageUrl, true)
                            };
                            Thread.Sleep(1000);

                            // Detect and extract text.
                            imageData.TextDetection =
                                client.ImageModeration.OCRUrlInput("eng", "application/json", imageUrl, true);
                            Thread.Sleep(1000);

                            // Detect faces.
                            imageData.FaceDetection =
                                client.ImageModeration.FindFacesUrlInput("application/json", imageUrl, true);
                            Thread.Sleep(1000);

                            // Add results to Evaluation object
                            evaluationData.Add(imageData);
                        }
                    }
                }
            }
            //// Save the moderation results to a file.
            //using (StreamWriter outputWriter = new StreamWriter(outputFile, false))
            //{
            //    outputWriter.WriteLine(JsonConvert.SerializeObject(
            //        evaluationData, Formatting.Indented));

            //    outputWriter.Flush();
            //    outputWriter.Close();
            //}
            //Console.WriteLine();
            //Console.WriteLine("Image moderation results written to output file: " + outputFile);
            //Console.WriteLine();

            return evaluationData;
        }
    }
}
