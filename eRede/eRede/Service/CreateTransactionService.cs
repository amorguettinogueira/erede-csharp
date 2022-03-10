namespace eRede.Service
{
    internal class CreateTransactionService : AbstractTransactionService
    {
        public CreateTransactionService(Store store, Transaction transaction, string userAgent) : base(store, transaction, userAgent)
        {
        }
    }
}