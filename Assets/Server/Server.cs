using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Server
{
    public class Server: IServer
    {
        private const string ServerUrl = "";

        private static string GetResponse(string func, Dictionary<string, string> requestParams)
        {
            var additionalUri = func + "/";
            if (requestParams != null)
            {
                foreach (var key in requestParams.Keys)
                {
                    additionalUri += WWW.EscapeURL(key) + "=" + WWW.EscapeURL(requestParams[key]) + "&";
                }

                additionalUri.Remove(additionalUri.Length - 1);
            }
            using (var www = UnityWebRequest.Get(ServerUrl+additionalUri))
            {
                www.SendWebRequest();
                while(!www.isDone){}

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                    throw new Exception(www.error);
                }
                

                // Show results as text
                Debug.Log(www.downloadHandler.text);
                return www.downloadHandler.text;
            }
        }
        
        private static IEnumerator PostRequest(string func, Dictionary<string, string> requestParams)
        {
            var additionalUri = func + "/";
            var form = new WWWForm();
            if (requestParams != null)
            {
                foreach (var key in requestParams.Keys)
                {
                    form.AddField(key, requestParams[key]);
                }
            }
            using (var www = UnityWebRequest.Post(ServerUrl+additionalUri, form ))
            {
                yield return www.SendWebRequest();

                if (!www.isNetworkError && !www.isHttpError) yield break;
                Debug.Log(www.error);
                throw new Exception(www.error);
            }
        }
        
        public IEnumerable<User> GetUsers()
        {
            var data = GetResponse("user/all", null);
            return JsonUtility.FromJson<List<User>>(data);
        }

        public IEnumerator UpdateLocation(User user)
        {
            var requestParams = new Dictionary<string, string>()
            {
                {"UserId", user.Id.ToString()},
                {"Location", JsonUtility.ToJson(user.Location)}
            };
            yield return PostRequest("user/location", requestParams);
        }

        public bool TryNeutralize(User self, User target)
        {
            var requestParams = new Dictionary<string, string>()
            {
                {"UserId", self.Id.ToString()},
                {"TargetId", target.Id.ToString()}
            };
            return bool.Parse(GetResponse("user/neutralize", requestParams));
        }
    }
}