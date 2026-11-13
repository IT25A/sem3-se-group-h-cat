using UnityEngine;

namespace Unity.U2D.Welcome
{
    [CreateAssetMenu(fileName = "Welcome2DScript", menuName = "Welcome/Welcome2DScript")]
    class Welcome2DScript : ScriptableObject
    {
        // Öffnet die offizielle Unity-2D-Übersichtsseite im Browser
        public static void Open2DLandingPage()
        {
            Application.OpenURL("https://unity.com/features/2d");
        }
    }
}
