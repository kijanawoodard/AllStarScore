using Raven.Client;
using Raven.Client.Listeners;

namespace AllStarScore.Scoring.Specs
{
    public class NoStaleQueriesAllowed : IDocumentQueryListener
    {
        public void BeforeQueryExecuted(IDocumentQueryCustomization queryCustomization)
        {
            queryCustomization.WaitForNonStaleResults();
        }
    }
}