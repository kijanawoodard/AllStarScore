using Raven.Client;
using Raven.Client.Listeners;

namespace AllStarScore.Scoring.Infrastructure.RavenQueryListeners
{
    public class NoStaleQueriesAllowedAsOfNow : IDocumentQueryListener
    {
        public void BeforeQueryExecuted(IDocumentQueryCustomization queryCustomization)
        {
            queryCustomization.WaitForNonStaleResultsAsOfNow();
        }
    }
}