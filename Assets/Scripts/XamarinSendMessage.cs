using System.Runtime.InteropServices;
using UnityEngine;

public static class XamarinSendMessage
{
    /// <summary>
    /// Calls a method to Xamarin Android.
    /// </summary>
    /// <param name="methodName">Specifies which method to call.</param>
    /// <param name="args">An array of parameters passed to the method.</param>
    public static void SendAndroid(string methodName, params object[] args)
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        var unityClass = new AndroidJavaClass("com.company.product.OverrideUnityActivity");
        var unityActivity = unityClass.GetStatic<AndroidJavaObject>("instance");

        unityActivity.Call(methodName, args);
#else
        Debug.LogError("Error: Method isn't called from an Android device");
#endif
    }
    
    // Calls methods to Xamarin iOS.
    [DllImport("__Internal")]
    public static extern void receiveFinish();
    // Other methods
}