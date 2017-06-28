using System;

namespace BUAnalytics{

	public class BTScore: BUDocument{
		
		//Fields

		public double Score = 0;

		//Methods

		public BTScore(double score): base(){
			this.Score = score;
		}

		public void Commit(){

			//Add document fields
			if (Score != null){ this.Contents["Score"] = Score; }

			//Add common fields
			if (BUPlayer.Current != null){ this.Contents["UserId"] = BUPlayer.Current.UserId; }
			if (BUSession.Current != null){ this.Contents["SessionId"] = BUSession.Current.SessionId; }

			//Create collection if non existant and add document
			string collection = "Scores";
			if (!BUCollectionManager.Instance.Collections.ContainsKey(collection)) {
				BUCollectionManager.Instance.Create(new string[]{ collection });
			}
			BUCollectionManager.Instance.Collections[collection].Add(this);
		}
	}
}