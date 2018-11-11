using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Server
{
	
	public interface IServer
	{
		IEnumerable<User> GetUsers();
		void UpdateLocation(Location myLocation);
		string TryNeutralize(User target);
	}
}
