using System.Collections;

namespace BUAnalytics{
	
	public class BUTemplate: BUDocument {

		public string UserId = BUUser.Current != null ? BUUser.Current.UserId : null;
		public string SessionId = BUSession.Current != null ? BUSession.Current.UserId : null;

		public void Upload(string collection){

			//Add optional linking fields
			if (UserId != null){ Add("userId", UserId); }
			if (SessionId != null){ Add("sessionId", SessionId); }

			//Add to collection manager
			BUCollectionManager.Instance.Add(collection, this);
		}
		
	}
	
}