using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using WebApiHelpers.Contracts;

namespace WebApiHelpers.ReCaptcha
{
    public sealed class RecaptchaValidationService: IRecaptchaValidationService
    {
        readonly HttpClient _backChannel; 
        readonly RecaptchaOptions _options;
        readonly IStringLocalizer _loc;

        public RecaptchaValidationService(IOptions<RecaptchaOptions> options, ISharedResource sr)
        {
            options.CheckArgumentNull(nameof(options));
            sr.CheckArgumentNull(nameof(sr));
             
            _options = options.Value;
            _options.ResponseValidationEndpoint.CheckMandatoryOption(nameof(_options.ResponseValidationEndpoint));            
            _options.SecretKey.CheckMandatoryOption(nameof(_options.SecretKey));
            _options.ValidationMessage.CheckMandatoryOption(nameof(_options.ValidationMessage));
            _loc = sr.Localizer;
            _backChannel = new HttpClient(_options.BackchannelHttpHandler ?? new HttpClientHandler())
            {
                Timeout = _options.BackchannelTimeout
            };
        }

        public string ValidationMessage => _loc[ _options.ValidationMessage ].Value;

        public async Task ValidateResponseAsync(string response, string remoteIp)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _options.ResponseValidationEndpoint);
            var paramaters = new Dictionary<string, string>();
            paramaters["secret"] = _options.SecretKey;
            paramaters["response"] = response;
            paramaters["remoteip"] = remoteIp;
            request.Content = new FormUrlEncodedContent(paramaters);

            var resp = await _backChannel.SendAsync(request);
            resp.EnsureSuccessStatusCode();

            var responseText = await resp.Content.ReadAsStringAsync();

            var validationResponse = JsonConvert.DeserializeObject<RecaptchaValidationResponse>(responseText);

            if (!validationResponse.Success)
            {
                bool invalidResponse;
                throw new RecaptchaValidationException(GetErrrorMessage(validationResponse, out invalidResponse), invalidResponse);
            }
        }

        private string GetErrrorMessage (RecaptchaValidationResponse validationResponse, out bool invalidResponse)
        {
            var errorList = new List<string>();
            invalidResponse = false;

            if (validationResponse.ErrorCodes != null)
            {
                foreach (var error in validationResponse.ErrorCodes)
                {
                    switch (error)
                    {
                        case "missing-input-secret":
                            errorList.Add(_loc["The secret parameter is missing"].Value);
                            break;
                        case "invalid-input-secret":
                            errorList.Add(_loc["The secret parameter is invalid or malformed"].Value);
                            break;
                        case "missing-input-response":
                            errorList.Add(_loc["The response parameter is missing"].Value);
                            invalidResponse = true;
                            break;
                        case "invalid-input-response":
                            errorList.Add(_loc["The response parameter is invalid or malformed"].Value);
                            invalidResponse = true;
                            break;
                        default:
                            errorList.Add(_loc["Unknown Google ReCaptcha error '{0}'", error]);
                            break;
                    }
                }
            }
            else
            {
                return _loc["Unspecified Google ReCaptcha remote server error"];
            }

            return string.Join(Environment.NewLine, errorList);
        }
    }
}
