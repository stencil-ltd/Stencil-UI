using System;
using System.Linq;
using Analytics;
using CustomOrder;
using UI;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR

#endif

namespace State.Active
{
    [ExecutionOrder(-20)]
    public class ActiveEventSystem : MonoBehaviour
    {
        public bool IsRoot;
        
        private RegisterableBehaviour[] _registers;
        
        private void Awake()
        {
            Debug.Log("ActiveEventSystem Awake");
            if (IsRoot)
                _registers = Resources.FindObjectsOfTypeAll<RegisterableBehaviour>();
            else
                _registers = GetComponentsInChildren<RegisterableBehaviour>(true);
#if UNITY_EDITOR
            _registers = _registers.Where(arg => !EditorUtility.IsPersistent(arg))
                .ToArray();
#endif
            if (Application.isPlaying)
            {
                foreach (var res in _registers)
                {
                    try
                    {
                        res.Register();
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                    res.Registered = true;
                }

                foreach (var res in _registers)
                {
                    try
                    {
                        res.DidRegister();
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
            }
        }

        private void OnDestroy()
        {
            if (Application.isPlaying)
            {
                foreach (var res in _registers)
                {
                    try
                    {
                        res.WillUnregister();
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
                foreach (var res in _registers)
                {
                    try
                    {
                        res.Unregister();
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                    res.Unregistered = true;
                }
            }            
        }

        public void Check() 
        {
            foreach (var res in _registers)
                (res as ActiveManager)?.Check();   
        }
    }
}