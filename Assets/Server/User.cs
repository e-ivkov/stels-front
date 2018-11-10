using System;
using UnityEngine;

namespace Server
{
    [Serializable]
    public class User
    {
        public int Id;
        public string Name;
        public Location Location;
        public int Score;
        public bool Alive;
        
        [SerializeField]
        private string Photo_url;
        private Texture2D _photo = null;
        
        private bool _cacheEnabled = true;

        public User(int id, string name, Location location, int score, bool alive, string photoUrl)
        {
            Id = id;
            Name = name;
            Location = location;
            Score = score;
            Alive = alive;
            Photo_url = photoUrl;
        }

        public Texture2D Photo
        {
            get
            {
                if(_cacheEnabled && _photo != null)
                    return _photo;
                using (WWW www = new WWW(Photo_url))
                {
                    // Wait for download to complete
                    while(!www.isDone){}

                    // assign texture
                    _photo = www.texture;
                    return _photo;
                }
            }
        }
    }
}