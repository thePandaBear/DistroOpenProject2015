using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts {
	
	public class XMLParser {
		
		// local variables to parse the xml file
		public int money;
		public List<Wave> roundList;
		public List<Vector2> pathList;
		public List<Vector2> waypointList;
		public Vector2 castlePosition;
		
		public XMLParser() {
			pathList = new List<Vector2>();
			waypointList = new List<Vector2>();
			roundList = new List<Wave>();
		}
	}
	
	public class Wave {
		public int nOfEnemies { get; set; }
	}
}
