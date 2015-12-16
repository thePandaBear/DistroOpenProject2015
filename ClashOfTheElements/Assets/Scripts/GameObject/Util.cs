﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UnityEngine;

namespace Assets.Scripts
{
	public static class Util
	{
		public static XMLParser parseXML()
		{
			// create new xml parser
			XMLParser xmlParser = new XMLParser();
			
			// load the level file from the resources
			TextAsset textAsset = Resources.Load("levelProperties") as TextAsset;
			
			// get text from file
			String text = textAsset.text;
			
			// parse text using xDocument
			XDocument xDocument = XDocument.Parse(text);
			
			// get elements from xDocument
			XElement xElement = xDocument.Element("Elements");
			
			// get path legs
			var legs = xElement.Element("PathLegs").Elements("Leg");
			
			// parse each leg seperately
			foreach (var leg in legs) {
				
				// get the x value of this leg
				float xPosition = float.Parse(leg.Attribute("X").Value);
				
				// get the y value of this leg
				float yPosition = float.Parse(leg.Attribute("Y").Value);
				
				// create vector from the two values
				Vector2 vectorLeg = new Vector2(xPosition, yPosition);
				
				// add vector to the path list
				xmlParser.pathList.Add(vectorLeg);
			}
			Debug.Log ("All legs added");
			
			// get rounds
			var waves = xElement.Element("Waves").Elements("Wave");
			
			// parse each wave seperately
			foreach (var wave in waves) {
				
				// get number of enemies in current round
				int numberOfEnemies = int.Parse(wave.Attribute("nOfEnemies").Value);
				
				// create a new round
				Wave newWave = new Wave() {
					nOfEnemies = numberOfEnemies,
				};
				
				// add round to round list
				xmlParser.roundList.Add (newWave);
			}
			
			Debug.Log ("All waves added");
			
			
			// get waypoints
			var waypoints = xElement.Element("Waypoints").Elements("Waypoint");
			
			// parse each waypoint seperately
			foreach (var waypoint in waypoints) {
				
				// get the x value of this waypoint
				float xPosition = float.Parse(waypoint.Attribute("X").Value);
				
				// get the y value of this waypoint
				float yPosition = float.Parse(waypoint.Attribute("Y").Value);
				
				// create vector from the two values
				Vector2 vectorWaypoint = new Vector2(xPosition, yPosition);
				
				// add vector to the waypoint list
				xmlParser.waypointList.Add(vectorWaypoint);
			}
			
			Debug.Log ("All waypoints added");
			Debug.Log ("Nr of Waypoints" + xmlParser.waypointList.Count.ToString ());
			
			// get castle element
			XElement castle = xElement.Element("Castle");
			
			// get x value of the castle
			float xCastle = float.Parse (castle.Attribute ("X").Value);
			
			// get y value of the castle
			float yCastle = float.Parse(castle.Attribute("Y").Value);
			
			// create vector from the two values
			Vector2 vectorCastle = new Vector2 (xCastle, yCastle);
			
			// set castleposition in parser object                         
			xmlParser.castlePosition = vectorCastle;
			
			// get miscellaneous element
			XElement misc = xElement.Element("Misc");
			
			// get money value
			int gold = int.Parse (misc.Attribute ("gold").Value);
			
			// set money value in parser object
			xmlParser.gold = gold;
			
			// return the parser object
			Debug.Log ("Returning parsed object");
			return xmlParser;
		}
	}
}