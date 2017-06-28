using System;
using System.Collections.Generic;

namespace BUAnalytics{
	
	public class BUPlayer: BUDocument{
		
		public static BUPlayer Current;

		//Fields

		public string UserId = Guid.NewGuid().ToString();
		public string Name;

		//Methods

		public BUPlayer(): base(){
			BUPlayer.Current = this;
		}

		public BUPlayer(string name): base(){
			BUPlayer.Current = this;
			this.Name = name;
		}

		public void Commit(){

			//Add document fields
			if (UserId != null){ this.Contents["UserId"] = UserId; }
			if (Name != null){ this.Contents["Name"] = Name; }

			//Create collection if non existant and add document
			string collection = "Players";
			if (!BUCollectionManager.Instance.Collections.ContainsKey(collection)) {
				BUCollectionManager.Instance.Create(new string[]{ collection });
			}
			BUCollectionManager.Instance.Collections[collection].Add(this);

			//Reset player
			BUPlayer.Current = null;
		}
	}
}