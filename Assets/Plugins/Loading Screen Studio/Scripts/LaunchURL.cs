using UnityEngine;

namespace Michsky.LSS
{
    public class LaunchURL : MonoBehaviour
    {
        public string URL;

        public void urlLinkOrWeb()
        {
            Application.OpenURL(URL);
        }
    }
}