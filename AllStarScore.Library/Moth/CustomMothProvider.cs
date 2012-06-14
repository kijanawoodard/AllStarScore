using System;
using Moth.Core;
using Moth.Core.Providers;

namespace AllStarScore.Library.Moth
{
    public class CustomMothProvider : AspNetCacheProvider
    {
        public override void Store(string key, object o, TimeSpan duration)
        {
            /* tah dah */
        }

        public override IOutputCacheRestrictions Enable
        {
            get
            {
                return new OutputCacheRestrictions()
                       {
                           PageOutput = true,
                           CssTidy = false,
                           ScriptMinification = false,
                           CssPreprocessing = false
                       };
            }
        }
    }
}