using System.Collections;
using Currencies;
using Util;

namespace Lobbing
{
    public static class LobberExtensions
    {
        public static IEnumerator LobCurrency(this Lobber lobber, Currency currency, long amount, LobOverrides overrides = null)
        {
            currency.Stage(amount).AndSave();
            overrides = overrides ?? new LobOverrides();
            overrides.OnEachComplete += (sender, lob) => currency.Commit(lob.Amount);
            yield return Objects.StartCoroutine(lobber.LobMany(amount, overrides));
            
        }
    }
}