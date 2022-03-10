using RestSharp;

namespace eRede.Service
{
    internal class CaptureTransactionService : AbstractTransactionService
    {
        public CaptureTransactionService(Store store, Transaction transaction) : base(store, transaction, null)
        {
        }

        public TransactionResponse Execute()
        {
            return base.Execute(Method.Put);
        }
    }
}