using UnityEngine;

namespace Hotfix.framework
{
	public static class Hotfix
	{
        public static T Instantiate<T>(T original) where T : Object
        {
            return GameObject.Instantiate<T>(original);
        }

    }
}