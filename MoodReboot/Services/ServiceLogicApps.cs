﻿using Newtonsoft.Json;
using NugetMoodReboot.Helpers;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;

namespace MvcLogicApps.Services
{
    public class ServiceLogicApps
    {
        private readonly MediaTypeWithQualityHeaderValue Header;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string UrlMail;
        private readonly HelperMail helperMail;

        public ServiceLogicApps(IHttpClientFactory httpClientFactory, IConfiguration configuration, HelperMail helperMail)
        {
            this.Header =
                new MediaTypeWithQualityHeaderValue("application/json");
            this._httpClientFactory = httpClientFactory;
            this.UrlMail = configuration.GetValue<string>("MailSettings:UrlMail");
            this.helperMail = helperMail;
        }

        public async Task SendMailAsync
            (string email, string asunto, string mensaje)
        {
            var model = new
            {
                email,
                asunto,
                mensaje
            };

            using HttpClient client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(this.Header);
            string json = JsonConvert.SerializeObject(model);
            StringContent content = new(json, Encoding.UTF8, "application/json");
            await client.PostAsync(this.UrlMail, content);
        }

        public async Task SendMailAsync(string para, string asunto, string mensaje, string baseUrl)
        {
            string nuevoEmail = this.helperMail.BuildMailTemplate(asunto, mensaje, baseUrl);
            var model = new
            {
                para,
                asunto,
                nuevoEmail
            };

            using HttpClient client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(this.Header);
            string json = JsonConvert.SerializeObject(model);
            StringContent content = new(json, Encoding.UTF8, "application/json");
            await client.PostAsync(this.UrlMail, content);
        }

        public async Task SendMailAsync(string para, string asunto, string mensaje, List<MailLink> links, string baseUrl)
        {
            string nuevoEmail = this.helperMail.BuildMailTemplate(asunto, mensaje, baseUrl, links);
            var model = new
            {
                para,
                asunto,
                nuevoEmail
            };

            using HttpClient client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(this.Header);
            string json = JsonConvert.SerializeObject(model);
            StringContent content = new(json, Encoding.UTF8, "application/json");
            await client.PostAsync(this.UrlMail, content);
        }
    }
}