using System;
using System.Collections.Generic;

namespace BUAnalytics{

	public class BUScreen : BUTemplate {

		public string Collection;

		public DateTime? Started = DateTime.UtcNow;
		public DateTime? Ended;

		public string Name;

		public BUScreen(string name, string collection) {
			Name = name;
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
			this.AddRange (new Dictionary<string, object> () {
				{ "started", Started },
				{ "ended", Ended ?? DateTime.UtcNow },
				{ "length", (Ended ?? DateTime.UtcNow) - Started }
			});

			//Add optional fields
			if (Name != null){ Add("name", Name); }

			base.Upload(Collection ?? "Screens");
		}
	}

}