using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts {
	
	public class XMLParser {
		
		// local variables to parse the xml file
		public int gold;
		public List<Vector2> waypointList;
		public Vector2 castlePosition;
		
		public XMLParser() {
			waypointList = new List<Vector2>();
		}
	}
	
	public class Wave {
		public int nOfEnemies { get; set; }
	}
}
