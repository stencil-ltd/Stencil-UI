using System.Collections;
using Currencies;
using Util;

namespace Lobbing
{
    public static class LobberExtensions
    {
        public class LobOptions
        {
            public bool PreStaged;
            public bool StageOnly;
            public LobOverrides Overrides;

            public static implicit operator LobOverrides(LobOptions options) => options.Overrides;
            public static implicit operator LobOptions(LobOverrides overrides) => new LobOptions
            {
                Overrides = overrides
            };
        }
        
        public static IEnumerator LobCurrency(this Lobber lobber, Currency currency, ulong amount, LobOptions options = null)
        {
            if (options?.PreStaged != true)
                currency.Stage(amount).AndSave();
            var overrides = options?.Overrides ?? new LobOverrides();
            overrides.OnEachComplete = lob =>
            {
                if (options?.StageOnly == true)
                    currency.Stage(lob.Amount);
                else
                    currency.Commit(lob.Amount);
            };
            yield return Objects.StartCoroutine(lobber.LobMany(amount, overrides));
        }
        
        public static IEnumerator LobCurrencySingle(this Lobber lobber, Currency currency, ulong amount, LobOptions options = null)
        {
            if (options?.PreStaged != true)
                currency.Stage(amount).AndSave();
            var overrides = options?.Overrides ?? new LobOverrides();
            overrides.OnEachComplete = lob => currency.Commit(lob.Amount);
            yield return Objects.StartCoroutine(lobber.LobSingle(amount, false, overrides));
        }
    }
}