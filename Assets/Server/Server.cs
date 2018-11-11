using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;
using UnityEngine.Networking;

namespace Server
{
    public class Server: IServer
    {
        private const string ServerUrl = "http://142.93.172.157:88";

        private string _token;

        public string Token
        {
            get { return _token; }
        }

        public struct Login
        {
            public string token;
        }

        public struct UserList
        {
            public List<User> Users;
        }

        public Server(string username, string password)
        {
            var body = new Dictionary<string, string>()
            {
                {"username", username},
                {"password", password}
            };
            _token = JsonUtility.FromJson<Login>(PostRequest("/login/",body,null)).token;
        }

        private static string GetResponse(string func, Dictionary<string, string> requestParams,
            Dictionary<string, string> headers)
        {
            var additionalUri = func;
            if (requestParams != null)
            {
                foreach (var key in requestParams.Keys)
                {
                    additionalUri += WWW.EscapeURL(key) + "=" + WWW.EscapeURL(requestParams[key]) + "&";
                }

                additionalUri.Remove(additionalUri.Length - 1);
            }

            using (var www = UnityWebRequest.Get(ServerUrl + additionalUri))
            {
                if (headers != null)
                {
                    foreach (var key in headers.Keys)
                    {
                        www.SetRequestHeader(key, headers[key]);
                    }
                }

                www.SendWebRequest();
                while (!www.isDone)
                {
                }

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

        private static string PostRequest(string func, Dictionary<string, string> requestParams,
            Dictionary<string, string> headers, bool ignoreErrors=false)
        {
            var additionalUri = func;
            var form = new WWWForm();
            if (requestParams != null)
            {
                foreach (var key in requestParams.Keys)
                {
                    form.AddField(key, requestParams[key]);
                }
            }

            using (var www = UnityWebRequest.Post(ServerUrl + additionalUri, form))
            {
                if (headers != null)
                {
                    foreach (var key in headers.Keys)
                    {
                        www.SetRequestHeader(key, headers[key]);
                    }
                }

                www.SendWebRequest();
                while (!www.isDone){}
                if ((!www.isNetworkError && !www.isHttpError) || ignoreErrors) return www.downloadHandler.text;
                Debug.Log(www.error);
                throw new Exception(www.error);
            }
        }

        public IEnumerable<User> GetUsers()
        {
            var data = GetResponse("/game/players/", null,
                new Dictionary<string, string>() {{"Authorization", "Token " + _token}});
            return JsonUtility.FromJson<UserList>(data).Users;
        }

        public void UpdateLocation(Location myLocation)
        {
            var requestParams = new Dictionary<string, string>()
            {
                {"latitude", myLocation.latitude.ToString()},
                {"longitude", myLocation.longitude.ToString()},
            };
            PostRequest("/game/geolocation/my/", requestParams,
                new Dictionary<string, string>() {{"Authorization", "Token " + _token}});
        }

        public string TryNeutralize(User target)
        {
            var requestParams = new Dictionary<string, string>()
            {
                {"id", target.Id.ToString()}
            };
            return PostRequest("/game/kill/", requestParams,
                new Dictionary<string, string>() {{"Authorization", "Token " + _token}}, ignoreErrors:true);
        }
    }
}