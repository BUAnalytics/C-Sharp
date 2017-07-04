using System;
using System.Collections.Generic;

namespace BUAnalytics{

	public class BUQuestion: BUTemplate{

		public string Collection;

		public DateTime? Started = DateTime.UtcNow;
		public DateTime? Ended;

		public string Name;
		public string Question;
		public string Answer;
		public bool? Correct;

		public BUQuestion(string name, string collection) {
			Name = name;
			Collection = collection;
		}

		public void Ask(string question){
			Question = question;
			Started = DateTime.UtcNow;
		}

		public void Respond(string answer, bool correct){
			Answer = answer;
			Correct = correct;
			Ended = DateTime.UtcNow;
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
			if (Question != null){ Add("question", Question); }
			if (Answer != null){ Add("answer", Answer); }
			if (Correct != null){ Add("correct", Correct); }

			base.Upload(Collection ?? "Questions");
		}
	}

}