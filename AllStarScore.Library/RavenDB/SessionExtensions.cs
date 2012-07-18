using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client;
using Raven.Client.Document;

namespace AllStarScore.Library.RavenDB
{
    public static class SessionExtensions
    {
        public static IEnumerable<T> LoadStartingWith<T>(this IDocumentSession session,
                                                         string keyPrefix, int start = 0, int pageSize = 5000)
        {
            var inMemorySession = session as InMemoryDocumentSessionOperations;
            if (inMemorySession == null)
            {
                throw new InvalidOperationException(
                    "LoadStartingWith(..) only works on InMemoryDocumentSessionOperations");
            }

            var targetType = Raven.Client.Document.ReflectionUtil.GetFullNameWithoutVersionInformation(typeof(T)); //https://github.com/ravendb/ravendb/blob/master/Raven.Client.Lightweight/Document/ReflectionUtil.cs
            
            var result = 
                session
                    .Advanced.DatabaseCommands.StartsWith(keyPrefix, start, pageSize)
                    .Where(x => x.Key.IndexOf("/revisions/", StringComparison.Ordinal) == -1)
                    .Where(x => x.Metadata["Raven-Clr-Type"].ToString() == targetType)
                    .Select(inMemorySession.TrackEntity<T>)
                    .ToList();

            return result;
        }
    }
}