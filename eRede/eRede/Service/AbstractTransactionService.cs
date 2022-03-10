using eRede.Service.Error;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

namespace eRede.Service
{
    internal abstract class AbstractTransactionService
    {
        private readonly Store store;
        private readonly Transaction transaction;
        private readonly string userAgent;

        internal AbstractTransactionService(Store store, Transaction transaction, string userAgent)
        {
            this.store = store;
            this.transaction = transaction;
            this.userAgent = userAgent;
        }

        public string tid { get; set; }

        protected virtual string getUri()
        {
            return store.environment.Endpoint("transactions");
        }

        public TransactionResponse Execute(Method method = Method.Post)
        {
            var request = new RestRequest { Method = method, RequestFormat = DataFormat.Json };

            request.AddJsonBody(transaction);

            return sendRequest(request);
        }

        protected TransactionResponse sendRequest(RestRequest request)
        {
            var client = new RestClient(new RestClientOptions(getUri())
            {
                //UserAgent = eRede.UserAgent
                UserAgent = string.IsNullOrEmpty(userAgent) ? eRede.UserAgent : userAgent,
            })
            {
                Authenticator = new HttpBasicAuthenticator(store.filliation, store.token)
            };

            request.AddHeader("Transaction-Response", "brand-return-opened");

            var response = client.ExecuteAsync(request).Result;
            var status = (int)response.StatusCode;

            if (status < 200 || status >= 400)
            {
                RedeError error = JsonConvert.DeserializeObject<RedeError>(response.Content);
                RedeException exception = new RedeException
                {
                    error = error
                };

                throw exception;
            }

            return JsonConvert.DeserializeObject<TransactionResponse>(response.Content);
        }
    }
}