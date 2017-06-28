using System;
using System.Collections.Generic;

namespace BUAnalytics{

	public class BUSession: BUDocument{

		public static BUSession Current;

		//Fields

		public string SessionId = Guid.NewGuid().ToString();
		public DateTime Start = DateTime.UtcNow;
		public DateTime End;

		//Methods

		public BUSession(): base(){
			BUSession.Current = this;
		}

		public void Commit(){

			//End session
			End = DateTime.UtcNow;

			//Add document fields
			if (SessionId != null){ this.Contents["SessionId"] = SessionId; }
			if (Start != null){ this.Contents["Start"] = Start; }
			if (End != null){ this.Contents["End"] = End; }

			//Add common fields
			if (BUPlayer.Current != null){ this.Contents["UserId"] = BUPlayer.Current.UserId; }

			//Create collection if non existant and add document
			string collection = "Sessions";
			if (!BUCollectionManager.Instance.Collections.ContainsKey(collection)) {
				BUCollectionManager.Instance.Create(new string[]{ collection });
			}
			BUCollectionManager.Instance.Collections[collection].Add(this);

			//Reset session
			BUPlayer.Current = null;
		}
	}
}