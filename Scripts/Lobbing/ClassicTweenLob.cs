using System.Collections;
using UnityEngine;

namespace Lobbing
{
    public class ClassicTweenLob : ILobFunction
    {      
        public IEnumerator Lob(Lob lob, Transform origin, Transform target)
        {
            var obj = lob.Projectile;
            var targetPos = obj.transform.parent.InverseTransformPoint(target.position);
            var lt = LeanTween.moveLocal(obj, targetPos, lob.Style.Duration);
            if (lob.Style.Elastic)
            {
                lt.setEaseInBack()
                    .setScale(lob.Style.Elasticity);
            }
            yield return new WaitForSeconds(lob.Style.Duration);
        }
    }
}