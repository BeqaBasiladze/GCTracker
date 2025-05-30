﻿using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using ParcelTracker.Api.Models;


namespace ParcelTracker.Api.Services
{
    public class GoogleSheetsService
    {
        private readonly SheetsService _sheetsService;
        private readonly string _spreadsheetId = "1AbUTUQnGf03ooEBDOtsT6yDKOihXY9bMg200HcymBeM";
        private readonly string _sheetName = "Ireland Parcels";

        public GoogleSheetsService()
        {
            //var credentialsPath = Path.Combine(AppContext.BaseDirectory, "google-credentials.json");
            //Console.WriteLine($"[DEBUG] Download key from: {credentialsPath}");

            //if (!File.Exists(credentialsPath))
            //{
            //    Console.WriteLine("[ERROR] Файл google-credentials.json not found!");
            //    throw new FileNotFoundException("google-credentials.json not found!", credentialsPath);
            //}

            //var credential = GoogleCredential
            //    .FromFile(credentialsPath)
            //    .CreateScoped(SheetsService.Scope.SpreadsheetsReadonly);

            //_sheetsService = new SheetsService(new BaseClientService.Initializer
            //{
            //    HttpClientInitializer = credential,
            //    ApplicationName = "ParcelTracker"
            //});
            var credentialsPath = Path.Combine(AppContext.BaseDirectory, "google-credentials.json");
            Console.WriteLine($"[DEBUG] Looking for credentials at: {credentialsPath}");

            if (!File.Exists(credentialsPath))
            {
                Console.WriteLine("[DEBUG] google-credentials.json not found, creating from env...");

                var envJson = Environment.GetEnvironmentVariable("GOOGLE_CREDENTIALS_JSON");
                if (string.IsNullOrWhiteSpace(envJson))
                    throw new Exception("GOOGLE_CREDENTIALS_JSON env variable not set!");

                File.WriteAllText(credentialsPath, envJson);
                Console.WriteLine("[DEBUG] google-credentials.json created from environment.");
            }

            var credential = GoogleCredential
                .FromFile("/etc/secrets/parceltracker-key.json")
                .CreateScoped(SheetsService.Scope.SpreadsheetsReadonly);

            _sheetsService = new SheetsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "ParcelTracker"
            });
        }
        public TrackingInfo? GetTrackingInfo(string trackingNumber)
        {
            Console.WriteLine($"[DEBUG] Tracking number searching : {trackingNumber}");

            var range = $"{_sheetName}!A2:B";
            var request = _sheetsService.Spreadsheets.Values.Get(_spreadsheetId, range);
            var response = request.Execute();

            foreach (var row in response.Values)
            {
                var number = row[0]?.ToString()?.Trim();
                if(string.Equals(number, trackingNumber?.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    return new TrackingInfo
                    {
                        TrackingNumber = number,
                        Status = row.Count > 1 ? row[1]?.ToString() : "Not exist"
                    };
                }
            }
            Console.WriteLine("[DEBUG] Tracking number not found.");
            return null;
        }
    }
}
