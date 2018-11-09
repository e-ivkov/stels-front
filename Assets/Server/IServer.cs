using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Server
{
	
	public interface IServer
	{
		IEnumerable<User> GetUsers();
		IEnumerator UpdateLocation(User user);
		bool TryNeutralize(User self, User target);
	}
}
