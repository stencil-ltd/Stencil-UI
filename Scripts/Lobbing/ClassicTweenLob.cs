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
            
            // Obsolete. Use the new enum.
            if (lob.Style.Elastic)
            {
                lt.setEaseInBack()
                    .setScale(lob.Style.Elasticity);
            }
            
            switch (lob.Style.Easing)
            {
                case LobEasing.In:
                    lt.setEaseInCubic();
                    break;
                case LobEasing.Out:
                    lt.setEaseOutCubic();
                    break;
                case LobEasing.InOut:
                    lt.setEaseInOutCubic();
                    break;
                case LobEasing.ElasticIn:
                    lt.setEaseInBack();
                    break;
                case LobEasing.ElasticOut:
                    lt.setEaseOutBack();
                    break;
                case LobEasing.ElasticInOut:
                    lt.setEaseInOutBack();
                    break;
            }
            
            yield return new WaitForSeconds(lob.Style.Duration);
        }
    }
}