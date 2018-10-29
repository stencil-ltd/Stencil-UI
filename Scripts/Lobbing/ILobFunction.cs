using System;
using System.Collections;
using UnityEngine;

namespace Lobbing
{
    public interface ILobFunction
    {
        IEnumerator Lob(Lob lob, Transform origin, Transform target);
    }
}