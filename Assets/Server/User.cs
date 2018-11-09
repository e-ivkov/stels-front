using System;
using UnityEngine;

namespace Server
{
    [Serializable]
    public class User
    {
        public int Id;
        public string Name;
        public Vector2 Location;
        public int Score;
        public bool Alive;
        
        [SerializeField]
        private string PhotoUrl;
        private Texture2D _photo = null;
        
        private bool _cacheEnabled = true;

        public User(int id, string name, Vector2 location, int score, bool alive, string photoUrl)
        {
            Id = id;
            Name = name;
            Location = location;
            Score = score;
            Alive = alive;
            PhotoUrl = photoUrl;
        }

        public Texture2D Photo
        {
            get
            {
                if(_cacheEnabled && _photo != null)
                    return _photo;
                using (WWW www = new WWW(PhotoUrl))
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