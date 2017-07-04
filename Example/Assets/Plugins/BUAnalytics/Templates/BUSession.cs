using System;
using System.Collections.Generic;

namespace BUAnalytics{

	public class BUSession: BUDocument{

		public string Collection;

		public static BUSession Current;

		public string SessionId = BUID.Instance.Generate();
		public string UserId = BUID.Instance.Generate();

		public DateTime? Started = DateTime.UtcNow;
		public DateTime? Ended;

		public string Ip;
		public string Device;
		public string System;
		public string Version;

		public BUSession(string collection) {
			Collection = collection;
		}

		public void Start(){
			Started = DateTime.UtcNow;
		}

		public void End(){
			Ended = DateTime.UtcNow;
		}

		public void Upload(){
			
			//Add required fields
			this.AddRange(new Dictionary<string, object>(){
				{ "sessionId", SessionId },
				{ "started", Started },
				{ "ended", Ended ?? DateTime.UtcNow },
				{ "length", (Ended ?? DateTime.UtcNow) - Started }
			});

			//Add optional fields
			if (UserId != null){ Add("userId", UserId); }
			if (Ip != null){ Add("ip", Ip); }
			if (Device != null){ Add("device", Device); }
			if (System != null){ Add("system", System); }
			if (Version != null){ Add("version", Version); }

			//Add to collection manager
			BUCollectionManager.Instance.Add(Collection ?? "Sessions", document: this);

			//Remove current if self
			if (BUSession.Current == this){
				BUSession.Current = null;
			}
		}
	}
}